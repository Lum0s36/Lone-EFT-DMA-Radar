/*
 * Lone EFT DMA Radar
 * Brought to you by Lone (Lone DMA)
 * 
 * Quest Helper: Credit to LONE for the foundational implementation
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
using LoneEftDmaRadar.UI.Misc;

namespace LoneEftDmaRadar.Tarkov.GameWorld.Quests
{
    /// <summary>
    /// Manages quest tracking and condition monitoring for the local player.
    /// Coordinates between memory reading and condition filtering.
    /// </summary>
    public sealed class QuestManager
    {
        #region Fields

        private readonly ulong _profile;
        private readonly QuestMemoryReader _memoryReader;
        private DateTime _lastRefresh = DateTime.MinValue;

        private readonly ConcurrentDictionary<string, QuestEntry> _quests = new(StringComparer.OrdinalIgnoreCase);
        private readonly ConcurrentDictionary<string, byte> _items = new(StringComparer.OrdinalIgnoreCase);
        private readonly ConcurrentDictionary<string, QuestLocation> _locations = new(StringComparer.OrdinalIgnoreCase);

        /// <summary>
        /// Cached condition counters from Profile.TaskConditionCounters
        /// </summary>
        private IReadOnlyDictionary<string, (int CurrentCount, int TargetCount)> _conditionCounters = 
            new Dictionary<string, (int, int)>();

        #endregion

        #region Properties

        /// <summary>
        /// All current quests.
        /// </summary>
        public IReadOnlyDictionary<string, QuestEntry> Quests => _quests;

        /// <summary>
        /// All item BSG ID's that we need to pickup.
        /// </summary>
        public IReadOnlyDictionary<string, byte> ItemConditions => _items;

        /// <summary>
        /// All locations that we need to visit.
        /// </summary>
        public IReadOnlyDictionary<string, QuestLocation> LocationConditions => _locations;

        #endregion

        #region Constructor

        public QuestManager(ulong profile)
        {
            _profile = profile;
            _memoryReader = new QuestMemoryReader(profile);
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Refresh quest data from memory.
        /// </summary>
        public void Refresh(CancellationToken ct)
        {
            try
            {
                if (DateTime.UtcNow - _lastRefresh < QuestConstants.RefreshInterval)
                    return;
                _lastRefresh = DateTime.UtcNow;

                using var masterQuests = new PooledSet<string>(StringComparer.OrdinalIgnoreCase);
                using var masterItems = new PooledSet<string>(StringComparer.OrdinalIgnoreCase);
                using var masterLocations = new PooledSet<string>(StringComparer.OrdinalIgnoreCase);

                // Read condition counters from memory
                _conditionCounters = _memoryReader.ReadConditionCounters();

                // Read quest data
                var questsData = Memory.ReadPtr(_profile + Offsets.Profile.QuestsData);
                using var questsDataList = UnityList<ulong>.Create(questsData, false);

                foreach (var qDataEntry in questsDataList)
                {
                    ct.ThrowIfCancellationRequested();
                    ProcessQuestEntry(qDataEntry, masterQuests, masterItems, masterLocations);
                }

                RemoveStaleEntries(masterQuests, masterItems, masterLocations);
            }
            catch (OperationCanceledException) { throw; }
            catch (Exception ex)
            {
                DebugLogger.LogDebug($"[QuestManager] CRITICAL ERROR: {ex}");
            }
        }

        /// <summary>
        /// Check if an item ID is required for an active quest.
        /// </summary>
        public bool IsQuestItem(string itemId)
        {
            if (string.IsNullOrEmpty(itemId))
                return false;
            return _items.ContainsKey(itemId);
        }

        /// <summary>
        /// Get objective progress for a specific quest and objective.
        /// </summary>
        public (bool isCompleted, int currentCount) GetObjectiveProgress(string questId, string objectiveId)
        {
            if (string.IsNullOrEmpty(questId) || string.IsNullOrEmpty(objectiveId))
                return (false, 0);

            if (_quests.TryGetValue(questId, out var quest))
            {
                var isCompleted = quest.IsObjectiveCompleted(objectiveId);
                var currentCount = quest.GetObjectiveProgress(objectiveId);
                return (isCompleted, currentCount);
            }

            return (false, 0);
        }

        #endregion

        #region Quest Processing

        private void ProcessQuestEntry(
            ulong qDataEntry,
            PooledSet<string> masterQuests,
            PooledSet<string> masterItems,
            PooledSet<string> masterLocations)
        {
            try
            {
                var qStatus = Memory.ReadValue<int>(qDataEntry + Offsets.QuestStatusData.Status);
                if (qStatus != QuestConstants.QuestStatusStarted)
                    return;

                var qIdPtr = Memory.ReadPtr(qDataEntry + Offsets.QuestStatusData.Id);
                var qId = Memory.ReadUnicodeString(qIdPtr, QuestConstants.MaxQuestIdLength, false);

                if (string.IsNullOrEmpty(qId) || !TarkovDataManager.TaskData.TryGetValue(qId, out var task))
                    return;

                masterQuests.Add(qId);
                var questEntry = _quests.GetOrAdd(qId, id => new QuestEntry(id));

                // Read completed conditions
                using var completedConditions = new PooledList<string>();
                var completedConditionsPtr = Memory.ReadPtr(qDataEntry + Offsets.QuestStatusData.CompletedConditions);

                if (completedConditionsPtr != 0)
                {
                    try
                    {
                        _memoryReader.ReadCompletedConditionsHashSet(completedConditionsPtr, completedConditions);
                    }
                    catch (Exception ex)
                    {
                        DebugLogger.LogDebug($"[QuestManager] Error reading CompletedConditions for {qId}: {ex.Message}");
                    }
                }

                questEntry.UpdateCompletedConditions(completedConditions);
                
                // Log completed conditions for debugging
                if (completedConditions.Count > 0)
                {
                    DebugLogger.LogDebug($"[QuestManager] Quest '{task.Name}': Found {completedConditions.Count} completed conditions");
                }
                else
                {
                    // Check if the HashSet count was non-zero but reading failed
                    DebugLogger.LogDebug($"[QuestManager] Quest '{task.Name}': No completed conditions found (ptr: 0x{completedConditionsPtr:X})");
                }
                
                UpdateQuestConditionCounters(questEntry, task, completedConditions);

                // Skip blacklisted quests for filtering
                if (App.Config.QuestHelper.BlacklistedQuests.ContainsKey(qId))
                    return;

                // Build completed set for filtering
                using var completedSet = new PooledSet<string>(StringComparer.OrdinalIgnoreCase);
                foreach (var c in completedConditions)
                    completedSet.Add(c);

                // Filter conditions
                QuestConditionFilter.FilterConditions(
                    task, qId, completedSet, 
                    masterItems, masterLocations, 
                    _items, _locations);
            }
            catch
            {
                // Skip invalid quest entries
            }
        }

        private void UpdateQuestConditionCounters(
            QuestEntry questEntry, 
            TarkovDataManager.TaskElement task, 
            PooledList<string> completedConditions)
        {
            if (task.Objectives == null)
                return;

            using var counters = new PooledList<KeyValuePair<string, (int CurrentCount, int TargetCount)>>();

            foreach (var obj in task.Objectives)
            {
                if (string.IsNullOrEmpty(obj.Id))
                    continue;

                if (_conditionCounters.TryGetValue(obj.Id, out var count))
                {
                    int targetCount = count.TargetCount > 0 ? count.TargetCount : obj.Count;
                    counters.Add(new KeyValuePair<string, (int, int)>(obj.Id, (count.CurrentCount, targetCount)));

                    DebugLogger.LogDebug($"[QuestManager] Matched objective '{obj.Id}' ({obj.Description?.Substring(0, Math.Min(30, obj.Description?.Length ?? 0))}...) = {count.CurrentCount}/{targetCount}");
                }
                else if (questEntry.CompletedConditions.Contains(obj.Id))
                {
                    // Objective is completed but has no counter in memory
                    // Use questEntry.CompletedConditions (HashSet with OrdinalIgnoreCase) for reliable comparison
                    // For objectives with Count > 0, set counter to (Count, Count)
                    // For objectives without Count (e.g. Visit, Mark, Extract, FindQuestItem), set to (1, 1) to indicate completion
                    int targetCount = obj.Count > 0 ? obj.Count : 1;
                    counters.Add(new KeyValuePair<string, (int, int)>(obj.Id, (targetCount, targetCount)));
                    
                    DebugLogger.LogDebug($"[QuestManager] Objective '{obj.Id}' marked as DONE (in CompletedConditions)");
                }
            }

            if (counters.Count > 0)
            {
                DebugLogger.LogDebug($"[QuestManager] Quest '{task.Name}': Updating {counters.Count} counters from memory");
            }

            questEntry.UpdateConditionCounters(counters);
        }

        private void RemoveStaleEntries(
            PooledSet<string> masterQuests,
            PooledSet<string> masterItems,
            PooledSet<string> masterLocations)
        {
            foreach (var oldQuest in _quests)
            {
                if (!masterQuests.Contains(oldQuest.Key))
                    _quests.TryRemove(oldQuest.Key, out _);
            }
            foreach (var oldItem in _items)
            {
                if (!masterItems.Contains(oldItem.Key))
                    _items.TryRemove(oldItem.Key, out _);
            }
            foreach (var oldLoc in _locations.Keys)
            {
                if (!masterLocations.Contains(oldLoc))
                    _locations.TryRemove(oldLoc, out _);
            }
        }

        #endregion
    }
}
