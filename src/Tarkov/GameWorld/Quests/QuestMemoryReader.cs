/*
 * Lone EFT DMA Radar
 * MIT License - Copyright (c) 2025 Lone DMA
 */

using Collections.Pooled;
using LoneEftDmaRadar.Tarkov.Unity.Structures;
using LoneEftDmaRadar.UI.Misc;

namespace LoneEftDmaRadar.Tarkov.GameWorld.Quests
{
    /// <summary>
    /// Handles low-level memory reading operations for quest data structures.
    /// Extracted from QuestManager to separate memory concerns from quest logic.
    /// </summary>
    internal sealed class QuestMemoryReader
    {
        private readonly ulong _profile;
        
        /// <summary>
        /// Cached TaskConditionCounter pointers for fast value updates.
        /// Key: condition ID, Value: (counterPtr, targetCount)
        /// </summary>
        private readonly ConcurrentDictionary<string, (ulong CounterPtr, int TargetCount)> _counterPointers = new(StringComparer.OrdinalIgnoreCase);
        private bool _countersInitialized = false;

        public QuestMemoryReader(ulong profile)
        {
            _profile = profile;
        }

        #region HashSet Reading

        /// <summary>
        /// Read completed conditions from HashSet&lt;MongoID&gt; manually.
        /// </summary>
        public void ReadCompletedConditionsHashSet(ulong hashSetPtr, PooledList<string> results)
        {
            // Read count first to know how many we expect
            var count = Memory.ReadValue<int>(hashSetPtr + QuestConstants.HashSetCountOffset);
            
            DebugLogger.LogDebug($"[QuestMemoryReader] HashSet count at 0x{hashSetPtr:X}: {count}");
            
            if (count <= 0 || count > QuestConstants.MaxHashSetSlots)
            {
                // Try alternative count offset (some Unity versions differ)
                var altCount = Memory.ReadValue<int>(hashSetPtr + 0x38);
                if (altCount > 0 && altCount <= QuestConstants.MaxHashSetSlots)
                {
                    count = altCount;
                    DebugLogger.LogDebug($"[QuestMemoryReader] Using alt count offset: {count}");
                }
                else
                {
                    DebugLogger.LogDebug($"[QuestMemoryReader] Invalid HashSet count: {count}");
                    return;
                }
            }

            var slotsArrayPtr = Memory.ReadPtr(hashSetPtr + QuestConstants.HashSetSlotsOffset);
            if (slotsArrayPtr == 0)
            {
                // Try alternative slots offset
                slotsArrayPtr = Memory.ReadPtr(hashSetPtr + 0x10);
                if (slotsArrayPtr == 0)
                {
                    DebugLogger.LogDebug("[QuestMemoryReader] Slots array pointer is null");
                    return;
                }
            }

            var slotsArrayLength = Memory.ReadValue<int>(slotsArrayPtr + QuestConstants.ArrayLengthOffset);
            if (slotsArrayLength <= 0 || slotsArrayLength > QuestConstants.MaxHashSetArrayLength)
            {
                DebugLogger.LogDebug($"[QuestMemoryReader] Invalid slots array length: {slotsArrayLength}");
                return;
            }

            DebugLogger.LogDebug($"[QuestMemoryReader] Slots array at 0x{slotsArrayPtr:X}, length: {slotsArrayLength}");

            // Try all possible slot sizes (primary)
            foreach (var slotSize in QuestConstants.HashSetSlotSizes)
            {
                if (TryReadSlotsWithSize(slotsArrayPtr, slotsArrayLength, slotSize, count, results))
                    return;
            }

            // If standard sizes didn't work, try fallback sizes
            foreach (var slotSize in QuestConstants.HashSetSlotSizesFallback)
            {
                if (TryReadSlotsWithSize(slotsArrayPtr, slotsArrayLength, slotSize, count, results))
                    return;
            }

            // Try reading MongoID directly without hashcode check
            if (TryReadSlotsDirectMongoId(slotsArrayPtr, slotsArrayLength, count, results))
                return;

            DebugLogger.LogDebug($"[QuestMemoryReader] Manual HashSet reading failed for all slot sizes. Expected {count} conditions.");
        }

        private static bool TryReadSlotsWithSize(ulong slotsArrayPtr, int slotsArrayLength, int slotSize, int expectedCount, PooledList<string> results)
        {
            results.Clear();
            var slotsStart = slotsArrayPtr + QuestConstants.ArrayHeaderSize;
            int foundCount = 0;

            for (int i = 0; i < slotsArrayLength && i < QuestConstants.MaxHashSetSlots; i++)
            {
                try
                {
                    var slotAddr = slotsStart + (ulong)(i * slotSize);
                    var hashCode = Memory.ReadValue<int>(slotAddr);

                    if (hashCode < 0)
                        continue;

                    var mongoIdOffset = slotAddr + QuestConstants.SlotMongoIdOffset;
                    var stringIdPtr = Memory.ReadPtr(mongoIdOffset + QuestConstants.MongoIdStringOffset);

                    if (stringIdPtr != 0 && stringIdPtr > QuestConstants.MinValidPointer)
                    {
                        var conditionId = Memory.ReadUnicodeString(stringIdPtr, QuestConstants.MaxConditionIdLength, true);
                        if (IsValidConditionId(conditionId))
                        {
                            results.Add(conditionId);
                            foundCount++;
                            DebugLogger.LogDebug($"[QuestMemoryReader] Found completed condition: {conditionId}");
                        }
                    }
                }
                catch
                {
                    // Skip invalid slots
                }
            }

            // Accept results if we found at least one condition, OR if we found most of the expected count
            if (foundCount > 0 && (foundCount >= expectedCount || foundCount >= (expectedCount + 1) / 2))
            {
                DebugLogger.LogDebug($"[QuestMemoryReader] Successfully read {foundCount}/{expectedCount} completed conditions with slot size 0x{slotSize:X}");
                return true;
            }

            return false;
        }

        /// <summary>
        /// Alternative reading method: try to read MongoID structures directly.
        /// Some HashSet implementations store MongoID at different offsets.
        /// </summary>
        private static bool TryReadSlotsDirectMongoId(ulong slotsArrayPtr, int slotsArrayLength, int expectedCount, PooledList<string> results)
        {
            results.Clear();
            var slotsStart = slotsArrayPtr + QuestConstants.ArrayHeaderSize;
            int foundCount = 0;

            // Try different MongoID offsets within each slot
            int[] mongoIdOffsets = { 0x00, 0x08, 0x10, 0x18 };
            int[] slotSizes = { 0x20, 0x28, 0x18 };

            foreach (var slotSize in slotSizes)
            {
                foreach (var mongoOffset in mongoIdOffsets)
                {
                    results.Clear();
                    foundCount = 0;

                    for (int i = 0; i < slotsArrayLength && i < QuestConstants.MaxHashSetSlots; i++)
                    {
                        try
                        {
                            var slotAddr = slotsStart + (ulong)(i * slotSize);
                            
                            // Try to read MongoID directly
                            var mongoId = Memory.ReadValue<MongoID>(slotAddr + (ulong)mongoOffset, false);
                            var conditionId = mongoId.ReadString(QuestConstants.MaxMongoIdLength, true);
                            
                            if (IsValidConditionId(conditionId))
                            {
                                results.Add(conditionId);
                                foundCount++;
                                DebugLogger.LogDebug($"[QuestMemoryReader] Direct MongoID read: {conditionId}");
                            }
                        }
                        catch
                        {
                            // Skip invalid entries
                        }
                    }

                    if (foundCount > 0 && foundCount >= (expectedCount + 1) / 2)
                    {
                        DebugLogger.LogDebug($"[QuestMemoryReader] Direct MongoID read success: {foundCount}/{expectedCount} (slotSize=0x{slotSize:X}, offset=0x{mongoOffset:X})");
                        return true;
                    }
                }
            }

            return false;
        }

        private static bool IsValidConditionId(string conditionId)
        {
            if (string.IsNullOrEmpty(conditionId))
                return false;
            
            // Valid condition IDs are typically 24 characters (MongoDB ObjectId hex string)
            // But some may vary, so we use a range
            if (conditionId.Length < QuestConstants.MinConditionIdLength || 
                conditionId.Length > QuestConstants.MaxValidConditionIdLength)
                return false;
            
            // Additional validation: condition IDs should be alphanumeric (hex characters for MongoDB ObjectId)
            // But some may have hyphens or other characters, so just check for printable ASCII
            foreach (char c in conditionId)
            {
                if (c < 0x20 || c > 0x7E)
                    return false;
            }
            
            return true;
        }

        #endregion

        #region Condition Counter Reading

        /// <summary>
        /// Read condition counters from the player profile.
        /// Returns a dictionary of condition ID to (currentCount, targetCount).
        /// </summary>
        public IReadOnlyDictionary<string, (int CurrentCount, int TargetCount)> ReadConditionCounters()
        {
            var result = new ConcurrentDictionary<string, (int CurrentCount, int TargetCount)>(StringComparer.OrdinalIgnoreCase);

            try
            {
                var countersPtr = Memory.ReadPtr(_profile + Offsets.Profile.TaskConditionCounters, false);
                if (countersPtr == 0)
                    return result;

                // Check if we need to reinitialize
                if (ShouldReinitializeCounters(countersPtr))
                {
                    _countersInitialized = false;
                }

                if (_countersInitialized && _counterPointers.Count > 0)
                {
                    RefreshCounterValuesFromCache(result);
                }
                else
                {
                    InitializeCounterPointers(countersPtr, result);
                }
            }
            catch (Exception ex)
            {
                DebugLogger.LogDebug($"[QuestMemoryReader] Error reading condition counters: {ex.Message}");
            }

            return result;
        }

        private bool ShouldReinitializeCounters(ulong countersPtr)
        {
            var countPtr = countersPtr + QuestConstants.DictionaryCountOffset;
            var currentCount = Memory.ReadValue<int>(countPtr, false);

            if (_countersInitialized && currentCount != _counterPointers.Count)
            {
                DebugLogger.LogDebug($"[QuestMemoryReader] Counter count changed ({_counterPointers.Count} -> {currentCount}), reinitializing...");
                return true;
            }

            return false;
        }

        private void RefreshCounterValuesFromCache(ConcurrentDictionary<string, (int CurrentCount, int TargetCount)> result)
        {
            int refreshedCount = 0;

            foreach (var kvp in _counterPointers)
            {
                try
                {
                    var conditionId = kvp.Key;
                    var (counterPtr, targetValue) = kvp.Value;

                    var currentValue = Memory.ReadValue<int>(counterPtr + Offsets.TaskConditionCounter.Value, false);

                    if (currentValue >= 0 && currentValue < QuestConstants.MaxCounterValue)
                    {
                        result[conditionId] = (currentValue, targetValue);
                        refreshedCount++;
                    }
                }
                catch
                {
                    // Counter may have become invalid - skip for now
                }
            }

            if (refreshedCount > 0)
            {
                DebugLogger.LogDebug($"[QuestMemoryReader] Refreshed {refreshedCount} counter values from cache");
            }
        }

        private void InitializeCounterPointers(ulong countersPtr, ConcurrentDictionary<string, (int CurrentCount, int TargetCount)> result)
        {
            var entriesPtr = Memory.ReadPtr(countersPtr + QuestConstants.DictionaryEntriesOffset, false);
            if (entriesPtr == 0)
            {
                DebugLogger.LogDebug("[QuestMemoryReader] Entries array pointer is null");
                return;
            }

            var arrayLength = Memory.ReadValue<int>(entriesPtr + QuestConstants.ArrayLengthOffset, false);

            if (arrayLength <= 0 || arrayLength > QuestConstants.MaxCounterArrayLength)
            {
                DebugLogger.LogDebug($"[QuestMemoryReader] Invalid array length: {arrayLength}");
                return;
            }

            _counterPointers.Clear();
            int foundCounters = 0;

            var entriesStart = entriesPtr + QuestConstants.ArrayHeaderSize;

            for (int i = 0; i < arrayLength && i < QuestConstants.MaxConditionCounterEntries; i++)
            {
                var counterData = TryReadCounterEntry(entriesStart, i);
                if (counterData.HasValue)
                {
                    var (conditionId, counterPtr, currentValue, targetValue) = counterData.Value;
                    _counterPointers[conditionId] = (counterPtr, targetValue);
                    result[conditionId] = (currentValue, targetValue);
                    foundCounters++;

                    if (foundCounters <= 5)
                    {
                        DebugLogger.LogDebug($"[QuestMemoryReader] Counter initialized: {conditionId} = {currentValue}/{targetValue}");
                    }
                }
            }

            if (foundCounters > 0)
            {
                _countersInitialized = true;
                DebugLogger.LogDebug($"[QuestMemoryReader] Initialized {foundCounters} counter pointers for fast refresh");
            }
        }

        private static (string conditionId, ulong counterPtr, int currentValue, int targetValue)? TryReadCounterEntry(ulong entriesStart, int index)
        {
            try
            {
                var entryAddr = entriesStart + (uint)(index * QuestConstants.DictionaryEntrySize);

                var hashCode = Memory.ReadValue<int>(entryAddr + QuestConstants.EntryHashCodeOffset, false);
                if (hashCode < 0)
                    return null;

                var mongoId = Memory.ReadValue<MongoID>(entryAddr + QuestConstants.EntryKeyOffset, false);
                var conditionId = mongoId.ReadString(QuestConstants.MaxMongoIdLength, false);

                if (string.IsNullOrEmpty(conditionId))
                    return null;

                var counterPtr = Memory.ReadPtr(entryAddr + QuestConstants.EntryValueOffset, false);
                if (counterPtr == 0)
                    return null;

                var currentValue = Memory.ReadValue<int>(counterPtr + Offsets.TaskConditionCounter.Value, false);

                int targetValue = ReadTargetValue(counterPtr);

                if (currentValue >= 0 && currentValue < QuestConstants.MaxCounterValue)
                {
                    return (conditionId, counterPtr, currentValue, targetValue);
                }
            }
            catch
            {
                // Skip invalid entries
            }

            return null;
        }

        private static int ReadTargetValue(ulong counterPtr)
        {
            try
            {
                var templatePtr = Memory.ReadPtr(counterPtr + Offsets.TaskConditionCounter.Template, false);
                if (templatePtr != 0)
                {
                    var floatValue = Memory.ReadValue<float>(templatePtr + Offsets.Condition.Value, false);
                    var targetValue = (int)floatValue;

                    if (targetValue >= 0 && targetValue <= QuestConstants.MaxCounterValue)
                        return targetValue;
                }
            }
            catch
            {
                // Fall through to return 0
            }

            return 0;
        }

        #endregion

        /// <summary>
        /// Reset cached state (call on raid end).
        /// </summary>
        public void Reset()
        {
            _counterPointers.Clear();
            _countersInitialized = false;
        }
    }
}
