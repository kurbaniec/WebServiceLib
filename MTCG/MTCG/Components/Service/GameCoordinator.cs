using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MTCG.Battles.Basis;
using MTCG.Battles.Logging;
using MTCG.Battles.Player;
using MTCG.Cards.Factory;
using MTCG.Components.DataManagement.DB;
using Newtonsoft.Json;
using WebService_Lib.Attributes;

namespace MTCG.Components.Service
{
    /// <summary>
    /// Game Coordinator that is responsible for matchmaking and the processing
    /// of battles.
    /// </summary>
    [Component]
    public class GameCoordinator
    {
        [Autowired] 
        private readonly PostgresDatabase db = null!;

        private readonly ConcurrentQueue<IPlayer> playerPool;
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

        /// <summary>
        /// Matchmaking loop. When two players are in the player pool
        /// a new match will be processed.
        /// </summary>
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

        /// <summary>
        /// Used to process a battle.
        /// </summary>
        /// <param name="playerA"></param>
        /// <param name="playerB"></param>
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

        /// <summary>
        /// Used by users to enter matchmaking and play a game.
        /// </summary>
        /// <param name="username"></param>
        /// <returns>
        /// Returns battle log or error message in form of a dictionary.
        /// </returns>
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

        /// <summary>
        /// Used to stop the game coordinator.
        /// </summary>
        public void Stop()
        {
            playerPool.Clear();
            listening = false;
            tokenSource.Cancel();
            foreach (var task in tasks.Values)
            {
                if (task.IsCompleted) continue;
                try
                {
                    task.Wait(500);
                }
                catch (Exception)
                {
                    // ignored
                    // Prevent TaskCanceledException
                }
            }
            tasks.Clear();
            autoStart.Join(1000);
        }
        
        /// <summary>
        /// Used to restart the game coordinator.
        /// </summary>
        public void ReStart()
        {
            if (autoStart.IsAlive) return;
            autoStart = new Thread(Run);
            autoStart.Start();
        }
    }
}