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

using Collections.Pooled;
using LoneEftDmaRadar.Tarkov.Unity.Collections;
using LoneEftDmaRadar.UI.Loot;
using LoneEftDmaRadar.UI.Misc;

namespace LoneEftDmaRadar.Tarkov.GameWorld.Loot
{
    /// <summary>
    /// Manages loot discovery, tracking, and filtering for the game world.
    /// </summary>
    public sealed class LootManager
    {
        #region Fields/Properties/Constructor

        private readonly ulong _lgw;
        private readonly Lock _filterSync = new();
        private readonly ConcurrentDictionary<ulong, LootItem> _loot = new();

        /// <summary>
        /// All loot (with filter applied).
        /// </summary>
        public IReadOnlyList<LootItem> FilteredLoot { get; private set; }

        /// <summary>
        /// All unfiltered loot.
        /// </summary>
        public IEnumerable<LootItem> AllLoot => _loot.Values;

        /// <summary>
        /// All Static Containers on the map.
        /// </summary>
        public IEnumerable<StaticLootContainer> StaticContainers => _loot.Values.OfType<StaticLootContainer>();

        public LootManager(ulong localGameWorld)
        {
            _lgw = localGameWorld;
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Force a filter refresh.
        /// Thread Safe.
        /// </summary>
        public void RefreshFilter()
        {
            if (_filterSync.TryEnter())
            {
                try
                {
                    var filter = LootFilter.Create();
                    FilteredLoot = _loot.Values?
                        .Where(x => filter(x))
                        .OrderBy(x => x.Important)
                        .ThenBy(x => x?.Price ?? 0)
                        .ToList();
                }
                catch { }
                finally
                {
                    _filterSync.Exit();
                }
            }
        }

        /// <summary>
        /// Refreshes loot, only call from a memory thread (Non-GUI).
        /// </summary>
        public void Refresh(CancellationToken ct)
        {
            try
            {
                GetLoot(ct);
                RefreshFilter();
            }
            catch (OperationCanceledException)
            {
                throw;
            }
            catch (Exception ex)
            {
                DebugLogger.LogDebug($"CRITICAL ERROR - Failed to refresh loot: {ex}");
            }
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// Updates loot collection with fresh values from memory.
        /// </summary>
        private void GetLoot(CancellationToken ct)
        {
            var lootListAddr = Memory.ReadPtr(_lgw + Offsets.GameWorld.LootList);
            using var lootList = UnityList<ulong>.Create(addr: lootListAddr, useCache: true);

            // Remove loot no longer in the game world
            RemoveStaleLoot(lootList);

            // Update existing loot positions and states
            UpdateExistingLoot();

            // Discover new loot items using scatter reads
            DiscoverNewLoot(lootList, ct);

            // Sync corpses with dead players
            SyncCorpses();
        }

        /// <summary>
        /// Remove loot entries that are no longer present in the game world.
        /// </summary>
        private void RemoveStaleLoot(UnityList<ulong> lootList)
        {
            using var lootListHs = lootList.ToPooledSet();
            foreach (var existing in _loot.Keys)
            {
                if (!lootListHs.Contains(existing))
                {
                    _ = _loot.TryRemove(existing, out _);
                }
            }
        }

        /// <summary>
        /// Update positions and states for existing loot items.
        /// </summary>
        private void UpdateExistingLoot()
        {
            foreach (var item in _loot.Values)
            {
                item.UpdatePosition();
                if (item is StaticLootContainer container)
                {
                    container.UpdateSearchedStatus();
                }
            }
        }

        /// <summary>
        /// Discover new loot items using scatter reads.
        /// </summary>
        private void DiscoverNewLoot(UnityList<ulong> lootList, CancellationToken ct)
        {
            using var map = Memory.CreateScatterMap();
            var round1 = map.AddRound();
            var round2 = map.AddRound();
            var round3 = map.AddRound();
            var round4 = map.AddRound();

            var scatterReader = new LootScatterReader(_loot, ct);

            foreach (var lootBase in lootList)
            {
                ct.ThrowIfCancellationRequested();
                scatterReader.SetupScatterReads(lootBase, round1, round2, round3, round4, map);
            }

            map.Execute();
        }

        /// <summary>
        /// Synchronize corpse loot with dead players.
        /// </summary>
        private void SyncCorpses()
        {
            var deadPlayers = Memory.Players?
                .Where(x => x.Corpse is not null)?
                .ToList();

            foreach (var corpse in _loot.Values.OfType<LootCorpse>())
            {
                corpse.Sync(deadPlayers);
            }
        }

        #endregion
    }
}