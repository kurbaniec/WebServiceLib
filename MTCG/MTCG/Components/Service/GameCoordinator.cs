using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using MTCG.Battles.Logging;
using MTCG.Battles.Player;
using MTCG.Components.DataManagement.DB;
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
                        IPlayer? playerA, playerB;
                        if (!playerPool.TryDequeue(out playerA) || !playerPool.TryDequeue(out playerB)) continue;
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
            IBattleLog battleLog = new BattleLog();
            IPlayerLog playerLogA, playerLogB;
            var playerLogs = battleLog.GetPlayerLogs(playerA.Username, playerB.Username);
        }

        private Dictionary<string, object>? Play(string username)
        {
            var deck = db.GetUserDeck(username);
            if (deck.Count == 0) return null;
            IPlayer player = new Player(username);
            playerPool.Enqueue(player);
            while (player.BattleResult is null)
            {
                Thread.Sleep(25);
            }
            return player.BattleResult;
        }
        
    }
}