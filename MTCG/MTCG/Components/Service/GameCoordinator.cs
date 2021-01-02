using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MTCG.Battles.Basis;
using MTCG.Battles.Logging;
using MTCG.Battles.Player;
using MTCG.Cards.Basis;
using MTCG.Cards.Factory;
using MTCG.Components.DataManagement.DB;
using Newtonsoft.Json;
using WebService_Lib.Attributes;

namespace MTCG.Components.Service
{
    [Component]
    public class GameCoordinator
    {
        [Autowired] 
        private PostgresDatabase db = null!;

        private ConcurrentQueue<IPlayer> playerPool;
        private bool listening;
        private readonly ConcurrentDictionary<string, Task> tasks;
        private readonly CancellationTokenSource tokenSource;
        private Thread autoStart;

        public GameCoordinator()
        {
            playerPool = new ConcurrentQueue<IPlayer>();
            tasks = new ConcurrentDictionary<string, Task>();
            listening = true;
            tokenSource = new CancellationTokenSource();
            autoStart = new Thread(Run);
            autoStart.Start();
        }

        public void ReStart()
        {
            if (autoStart.IsAlive) return;
            autoStart = new Thread(Run);
            autoStart.Start();
        }

        private void Run()
        {
            while (listening)
            {
                try
                {
                    if (playerPool.Count >= 2)
                    {
                        // Get players from queue
                        IPlayer? playerA = null, playerB = null;
                        while (playerA == null) playerPool.TryDequeue(out playerA);
                        while (playerB == null) playerPool.TryDequeue(out playerB);
                        // Check if players are different 
                        if (playerA.Username == playerB.Username)
                        {
                            Dictionary<string, object> error = new Dictionary<string, object>()
                            {
                                {"error", "Cannot battle oneself!"}
                            };
                            playerA.BattleResult = error;
                            playerB.BattleResult = error;
                        }
                        else
                        {
                            // Get token & GUID and start battle processing
                            var token = tokenSource.Token;
                            var id = Guid.NewGuid().ToString();
                            var task = Task.Run(() => Process(playerA, playerB), token);
                            tasks[id] = task;
                            // Remove task from collection when finished
                            task.ContinueWith(t =>
                            {
                                tasks.TryRemove(id, out t!);
                            }, token);
                        }
                    } else Thread.Sleep(15);
                }
                catch (Exception)
                {
                    // ignored
                }
            }
        }

        private void Process(IPlayer playerA, IPlayer playerB)
        {
            // Get loggers
            IBattleLog battleLog = new BattleLog();
            var (playerLogA, playerLogB) = battleLog.GetPlayerLogs(playerA.Username, playerB.Username);
            // Get cards
            var rawDeckA = db.GetUserDeck(playerA.Username);
            var rawDeckB = db.GetUserDeck(playerB.Username);
            // Convert card data schema to functional card
            var deckA = rawDeckA.Select(
                rawCard => CardFactory.Print(rawCard.Name, rawCard.Damage, playerLogA)).
                    Where(card => card != null).ToList()!;
            var deckB = rawDeckB.Select(
                rawCard => CardFactory.Print(rawCard.Name, rawCard.Damage, playerLogB)).
                    Where(card => card != null).ToList()!;
            playerA.AddDeck(deckA!);
            playerB.AddDeck(deckB!);
            // Let's get ready to rumble
            var battle = new Battle(playerA, playerB, battleLog);
            var result = battle.ProcessBattle();
            var log = (JsonConvert.SerializeObject(result.Log) is var json && json is {}) ? json : "";
            var draw = result.Draw;
            var winner = !draw ? result.Winner : "";
            var looser = !draw ? result.Looser : "";
            // Update stats and add battle to history
            for (var i = 0; i < 10; i++)
            {
                // Check if update was successful
                if (db.AddBattleResultModifyEloAndGiveCoins(
                    playerA.Username, playerB.Username, log, draw, winner, looser
                ))
                {
                    playerA.BattleResult = result.Log;
                    playerB.BattleResult = result.Log;
                    return;
                }
                // If not try again...
                Thread.Sleep(25);
            }
            // Could not update stats... therefore invalid game
            Dictionary<string, object> error = new Dictionary<string, object>()
            {
                {"error", "Cannot battle oneself!"}
            };
            playerA.BattleResult = error;
            playerB.BattleResult = error;
            
        }

        public Dictionary<string, object>? Play(string username)
        {
            var deck = db.GetUserDeck(username);
            if (deck.Count == 0) return null;
            IPlayer player = new Player(username);
            playerPool.Enqueue(player);
            while (player.BattleResult is null)
            {
                Thread.Sleep(30);
            }
            return player.BattleResult;
        }
        
    }
}