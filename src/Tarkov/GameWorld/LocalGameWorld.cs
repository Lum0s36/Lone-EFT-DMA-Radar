/*
 * Lone EFT DMA Radar
 * Brought to you by Lone (Lone DMA)
 * 
MIT License

Copyright (c) 2025 Lone DMA

Permission is hereby granted, free of charge, to any person obtaining a copy
of this software and associated documentation files (the "Software"), to deal
in the Software without restriction, including without limitation the rights
to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
copies of the Software, and to permit persons to whom the Software is
furnished to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in all
copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
SOFTWARE.
 *
*/

using LoneEftDmaRadar.DMA;
using LoneEftDmaRadar.Misc;
using LoneEftDmaRadar.Misc.Workers;
using LoneEftDmaRadar.Tarkov.Features.MemWrites;
using LoneEftDmaRadar.Tarkov.GameWorld.Exits;
using LoneEftDmaRadar.Tarkov.GameWorld.Explosives;
using LoneEftDmaRadar.Tarkov.GameWorld.Hazards;
using LoneEftDmaRadar.Tarkov.GameWorld.Loot;
using LoneEftDmaRadar.Tarkov.GameWorld.Player;
using LoneEftDmaRadar.Tarkov.GameWorld.Quests;
using LoneEftDmaRadar.Tarkov.Unity.Structures;
using LoneEftDmaRadar.UI.Misc;
using VmmSharpEx.Options;

namespace LoneEftDmaRadar.Tarkov.GameWorld
{
    /// <summary>
    /// Class containing Game (Raid) instance.
    /// Manages all game world subsystems including players, loot, exits, explosives, and quests.
    /// IDisposable.
    /// </summary>
    public sealed class LocalGameWorld : IDisposable
    {
        #region Fields / Properties / Constructors

        public static implicit operator ulong(LocalGameWorld x) => x.Base;

        /// <summary>
        /// LocalGameWorld Address.
        /// </summary>
        private ulong Base { get; }

        // Player Management
        private readonly RegisteredPlayers _rgtPlayers;
        
        // Game World Managers
        private readonly ExitManager _exfilManager;
        private readonly ExplosivesManager _explosivesManager;
        private readonly MemWritesManager _memWritesManager;
        private readonly QuestManager _questManager;
        
        // Worker Threads
        private readonly WorkerThread _realtimeWorker;
        private readonly WorkerThread _slowWorker;
        private readonly WorkerThread _explosivesWorker;
        private readonly WorkerThread _memWritesWorker;

        /// <summary>
        /// Map ID of Current Map.
        /// </summary>
        public string MapID { get; }

        /// <summary>
        /// Indicates if the raid is currently active.
        /// </summary>
        public bool InRaid => !_disposed;
        
        /// <summary>
        /// All registered players in the game world.
        /// </summary>
        public IReadOnlyCollection<AbstractPlayer> Players => _rgtPlayers;
        
        /// <summary>
        /// All active explosives (grenades, tripwires) in the game world.
        /// </summary>
        public IReadOnlyCollection<IExplosiveItem> Explosives => _explosivesManager;
        
        /// <summary>
        /// All exit points (exfils, transits) in the game world.
        /// </summary>
        public IReadOnlyCollection<IExitPoint> Exits => _exfilManager;
        
        /// <summary>
        /// The local player (radar user).
        /// </summary>
        public LocalPlayer LocalPlayer => _rgtPlayers?.LocalPlayer;
        
        /// <summary>
        /// Loot manager for tracking items in the game world.
        /// </summary>
        public LootManager Loot { get; }
        
        /// <summary>
        /// Quest manager for tracking active quests and zones.
        /// </summary>
        public QuestManager QuestManager => _questManager;

        /// <summary>
        /// World hazards (minefields, radiation zones, etc.) for the current map.
        /// </summary>
        public IReadOnlyList<IWorldHazard> Hazards { get; }

        private LocalGameWorld() { }

        /// <summary>
        /// Game Constructor. Only called internally via CreateGameInstance.
        /// </summary>
        private LocalGameWorld(ulong localGameWorld, string mapID)
        {
            try
            {
                Base = localGameWorld;
                MapID = mapID;
                
                // Initialize Worker Threads
                _realtimeWorker = CreateRealtimeWorker();
                _slowWorker = CreateSlowWorker();
                _explosivesWorker = CreateExplosivesWorker();
                
                // Initialize Player Registry
                var rgtPlayersAddr = Memory.ReadPtr(localGameWorld + Offsets.GameWorld.RegisteredPlayers, false);
                _rgtPlayers = new RegisteredPlayers(rgtPlayersAddr, this);
                ArgumentOutOfRangeException.ThrowIfLessThan(_rgtPlayers.GetPlayerCount(), 1, nameof(_rgtPlayers));
                
                // Initialize Game World Managers
                Loot = new LootManager(localGameWorld);
                _exfilManager = new ExitManager(localGameWorld, mapID, _rgtPlayers.LocalPlayer);
                _explosivesManager = new ExplosivesManager(localGameWorld);
                _memWritesManager = new MemWritesManager();
                
                // Initialize Quest Manager (requires LocalPlayer's Profile)
                var profilePtr = _rgtPlayers.LocalPlayer?.Profile ?? 0;
                _questManager = new QuestManager(profilePtr);
                
                // Load static map data
                Hazards = LoadHazards(mapID);
                
                // Initialize MemWrites Worker (after managers are ready)
                _memWritesWorker = CreateMemWritesWorker();
            }
            catch
            {
                Dispose();
                throw;
            }
        }

        #endregion

        #region Worker Thread Factory Methods

        private WorkerThread CreateRealtimeWorker()
        {
            var worker = new WorkerThread
            {
                Name = GameWorldConstants.RealtimeWorkerName,
                ThreadPriority = ThreadPriority.AboveNormal,
                SleepDuration = GameWorldConstants.RealtimeWorkerInterval,
                SleepMode = WorkerThreadSleepMode.DynamicSleep
            };
            worker.PerformWork += OnRealtimeWorkerTick;
            return worker;
        }

        private WorkerThread CreateSlowWorker()
        {
            var worker = new WorkerThread
            {
                Name = GameWorldConstants.SlowWorkerName,
                ThreadPriority = ThreadPriority.BelowNormal,
                SleepDuration = GameWorldConstants.SlowWorkerInterval
            };
            worker.PerformWork += OnSlowWorkerTick;
            return worker;
        }

        private WorkerThread CreateExplosivesWorker()
        {
            var worker = new WorkerThread
            {
                Name = GameWorldConstants.ExplosivesWorkerName,
                SleepDuration = GameWorldConstants.ExplosivesWorkerInterval,
                SleepMode = WorkerThreadSleepMode.DynamicSleep
            };
            worker.PerformWork += OnExplosivesWorkerTick;
            return worker;
        }

        private WorkerThread CreateMemWritesWorker()
        {
            var worker = new WorkerThread
            {
                Name = GameWorldConstants.MemWritesWorkerName,
                ThreadPriority = ThreadPriority.Normal,
                SleepDuration = GameWorldConstants.MemWritesWorkerInterval
            };
            worker.PerformWork += OnMemWritesWorkerTick;
            return worker;
        }

        #endregion

        #region Static Factory & Initialization

        /// <summary>
        /// Loads hazard data for the specified map from TarkovDataManager.
        /// </summary>
        private static List<IWorldHazard> LoadHazards(string mapId)
        {
            var hazards = new List<IWorldHazard>();
            
            if (TarkovDataManager.MapData?.TryGetValue(mapId, out var mapData) != true || mapData?.Hazards == null)
                return hazards;

            foreach (var hazard in mapData.Hazards)
            {
                hazards.Add(new GenericWorldHazard
                {
                    HazardType = hazard.HazardType,
                    Position = hazard.Position?.AsVector3() ?? Vector3.Zero
                });
            }
            
            return hazards;
        }

        /// <summary>
        /// Start all game world worker threads.
        /// </summary>
        public void Start()
        {
            _memWritesManager?.OnRaidStart();
            _realtimeWorker.Start();
            _slowWorker.Start();
            _explosivesWorker.Start();
            _memWritesWorker.Start();
        }

        /// <summary>
        /// Blocks until a LocalGameWorld singleton instance can be instantiated.
        /// Polls for raid start and returns when successful.
        /// </summary>
        public static LocalGameWorld CreateGameInstance(CancellationToken ct)
        {
            while (true)
            {
                ct.ThrowIfCancellationRequested();
                ResourceJanitor.Run();
                Memory.ThrowIfProcessNotRunning();
                
                try
                {
                    var instance = TryCreateGameWorld(ct);
                    DebugLogger.LogDebug("Raid has started!");
                    return instance;
                }
                catch (OperationCanceledException)
                {
                    throw;
                }
                catch (Exception ex) when (IsExpectedNotInRaidError(ex))
                {
                    // Expected when not in raid - silently continue polling
                }
                catch (Exception ex)
                {
                    DebugLogger.LogDebug($"ERROR Instantiating Game Instance: {ex}");
                }
                finally
                {
                    Thread.Sleep(GameWorldConstants.RaidPollingInterval);
                }
            }
        }

        private static bool IsExpectedNotInRaidError(Exception ex)
        {
            return ex.InnerException?.Message?.Contains("GameWorld not found") == true;
        }

        private static LocalGameWorld TryCreateGameWorld(CancellationToken ct)
        {
            try
            {
                var localGameWorld = GameObjectManager.Get().GetGameWorld(ct, out string map);
                if (localGameWorld == 0) 
                    throw new InvalidOperationException("GameWorld Address is 0");
                    
                return new LocalGameWorld(localGameWorld, map);
            }
            catch (OperationCanceledException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException("ERROR Getting LocalGameWorld", ex);
            }
        }

        #endregion

        #region Main Game Loop

        /// <summary>
        /// Main game loop executed by memory worker thread.
        /// Refreshes/updates player list and performs player allocations.
        /// </summary>
        public void Refresh()
        {
            try
            {
                ThrowIfRaidEnded();
                TryAllocateBtrIfSupported();
                _rgtPlayers.Refresh();
            }
            catch (OperationCanceledException ex)
            {
                DebugLogger.LogDebug(ex.Message);
                Dispose();
            }
            catch (Exception ex)
            {
                DebugLogger.LogDebug($"CRITICAL ERROR - Raid ended due to unhandled exception: {ex}");
                throw;
            }
        }

        private void TryAllocateBtrIfSupported()
        {
            if (GameWorldConstants.IsBtrSupportedMap(MapID))
                TryAllocateBTR();
        }

        #endregion

        #region Raid State Validation

        /// <summary>
        /// Throws an exception if the current raid instance has ended.
        /// </summary>
        /// <exception cref="OperationCanceledException">Thrown when raid has ended.</exception>
        private void ThrowIfRaidEnded()
        {
            for (int i = 0; i < GameWorldConstants.RaidEndCheckRetries; i++)
            {
                try
                {
                    if (IsRaidActive())
                        return;
                }
                catch
                {
                    Thread.Sleep(GameWorldConstants.RaidEndCheckDelayMs);
                }
            }
            
            throw new OperationCanceledException("Raid has ended!");
        }

        /// <summary>
        /// Checks if the current raid is active and LocalPlayer is valid.
        /// </summary>
        /// <returns>True if raid is active, otherwise false.</returns>
        private bool IsRaidActive()
        {
            try
            {
                var mainPlayer = Memory.ReadPtr(this + Offsets.GameWorld.MainPlayer, false);
                ArgumentOutOfRangeException.ThrowIfNotEqual(mainPlayer, _rgtPlayers.LocalPlayer, nameof(mainPlayer));
                return _rgtPlayers.GetPlayerCount() > 0;
            }
            catch
            {
                return false;
            }
        }

        #endregion

        #region Worker Thread Handlers

        /// <summary>
        /// Realtime worker: Updates player positions and camera (~125Hz).
        /// </summary>
        private void OnRealtimeWorkerTick(object sender, WorkerThreadArgs e)
        {
            var players = _rgtPlayers.Where(x => x.IsActive && x.IsAlive);
            var localPlayer = LocalPlayer;
            
            if (!players.Any())
            {
                Thread.Sleep(1);
                return;
            }

            using var scatter = Memory.CreateScatter(VmmFlags.NOCACHE);
            
            if (MemDMA.CameraManager != null && localPlayer != null)
            {
                MemDMA.CameraManager.OnRealtimeLoop(scatter, localPlayer);
            }
            
            foreach (var player in players)
            {
                player.OnRealtimeLoop(scatter);
            }
            
            scatter.Execute();
        }

        /// <summary>
        /// Slow worker: Updates loot, equipment, quests, exfils (~20Hz).
        /// Note: Individual operations may take several seconds.
        /// </summary>
        private void OnSlowWorkerTick(object sender, WorkerThreadArgs e)
        {
            var ct = e.CancellationToken;
            
            ValidatePlayerTransforms();
            RefreshLoot(ct);
            RefreshEquipment();
            RefreshWishlist();
            RefreshQuests(ct);
            RefreshExfils();
        }

        /// <summary>
        /// Explosives worker: Updates grenades and tripwires (~60Hz).
        /// </summary>
        private void OnExplosivesWorkerTick(object sender, WorkerThreadArgs e)
        {
            _explosivesManager.Refresh(e.CancellationToken);
        }

        /// <summary>
        /// MemWrites worker: Applies memory write features (~10Hz).
        /// </summary>
        private void OnMemWritesWorkerTick(object sender, WorkerThreadArgs e)
        {
            try
            {
                if (!App.Config.MemWrites.Enabled)
                {
                    Thread.Sleep(GameWorldConstants.MemWritesWorkerInterval);
                    return;
                }

                var localPlayer = LocalPlayer;
                if (localPlayer is null)
                    return;

                _memWritesManager.Apply(localPlayer);
            }
            catch (Exception ex)
            {
                DebugLogger.LogDebug($"[MemWritesWorker] Error: {ex}");
            }
        }

        #endregion

        #region Slow Worker Sub-Operations

        private void RefreshLoot(CancellationToken ct)
        {
            Loot.Refresh(ct);
        }

        private void RefreshExfils()
        {
            try
            {
                _exfilManager?.Refresh();
            }
            catch (Exception ex)
            {
                DebugLogger.LogDebug($"[ExitManager] ERROR Refreshing: {ex}");
            }
        }

        private void RefreshQuests(CancellationToken ct)
        {
            if (!App.Config.QuestHelper.Enabled)
                return;

            try
            {
                _questManager?.Refresh(ct);
            }
            catch (Exception ex)
            {
                DebugLogger.LogDebug($"[QuestManager] ERROR Refreshing: {ex}");
            }
        }

        private void RefreshWishlist()
        {
            if (!App.Config.Loot.ShowWishlistedRadar)
                return;

            try
            {
                LocalPlayer?.RefreshWishlist();
            }
            catch (Exception ex)
            {
                DebugLogger.LogDebug($"[Wishlist] ERROR Refreshing: {ex}");
            }
        }

        private void RefreshEquipment()
        {
            var allPlayers = _rgtPlayers.OfType<ObservedPlayer>();
            foreach (var player in allPlayers)
            {
                player.Equipment.Refresh();
            }
        }

        /// <summary>
        /// Validates player transforms for anomalies.
        /// </summary>
        public void ValidatePlayerTransforms()
        {
            try
            {
                var players = _rgtPlayers
                    .Where(x => x.IsActive && x.IsAlive && x is not BtrPlayer);
                    
                if (!players.Any())
                    return;

                using var map = Memory.CreateScatterMap();
                var round1 = map.AddRound();
                var round2 = map.AddRound();
                
                foreach (var player in players)
                {
                    player.OnValidateTransforms(round1, round2);
                }
                
                map.Execute();
            }
            catch (Exception ex)
            {
                DebugLogger.LogDebug($"CRITICAL ERROR - ValidatePlayerTransforms Loop FAILED: {ex}");
            }
        }

        #endregion

        #region BTR Vehicle

        /// <summary>
        /// Checks if there is a bot attached to the BTR turret and allocates the player instance.
        /// </summary>
        public void TryAllocateBTR()
        {
            try
            {
                if (_rgtPlayers.Any(p => p is BtrPlayer))
                    return;

                var btrController = Memory.ReadPtr(this + Offsets.GameWorld.BtrController);
                var btrView = Memory.ReadPtr(btrController + Offsets.BtrController.BtrView);
                var btrTurretView = Memory.ReadPtr(btrView + Offsets.BTRView.turret);
                var btrOperator = Memory.ReadPtr(btrTurretView + Offsets.BTRTurretView._bot);
                
                _rgtPlayers.TryAllocateBTR(btrView, btrOperator);
            }
            catch (Exception ex)
            {
                DebugLogger.LogDebug($"ERROR Allocating BTR: {ex}");
            }
        }

        #endregion

        #region IDisposable

        private bool _disposed;

        public void Dispose()
        {
            if (Interlocked.Exchange(ref _disposed, true))
                return;
                
            _realtimeWorker?.Dispose();
            _slowWorker?.Dispose();
            _explosivesWorker?.Dispose();
            _memWritesWorker?.Dispose();
        }

        #endregion
    }
}
