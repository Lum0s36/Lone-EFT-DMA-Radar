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

namespace LoneEftDmaRadar.Tarkov.GameWorld.Quests
{
    /// <summary>
    /// Constants for quest processing and memory reading.
    /// </summary>
    public static class QuestConstants
    {
        #region Refresh Timing

        /// <summary>
        /// Minimum interval between quest data refreshes.
        /// </summary>
        public static readonly TimeSpan RefreshInterval = TimeSpan.FromSeconds(1);

        #endregion

        #region Quest Status

        /// <summary>
        /// Quest status value indicating the quest is started (in progress).
        /// </summary>
        public const int QuestStatusStarted = 2;

        #endregion

        #region Memory Limits

        /// <summary>
        /// Maximum length for reading quest IDs from memory.
        /// </summary>
        public const int MaxQuestIdLength = 64;

        /// <summary>
        /// Maximum length for reading condition IDs from memory.
        /// </summary>
        public const int MaxConditionIdLength = 64;

        /// <summary>
        /// Maximum length for reading MongoID strings.
        /// </summary>
        public const int MaxMongoIdLength = 128;

        /// <summary>
        /// Maximum number of slots to read from HashSet.
        /// </summary>
        public const int MaxHashSetSlots = 100;

        /// <summary>
        /// Maximum slots array length for HashSet.
        /// </summary>
        public const int MaxHashSetArrayLength = 200;

        /// <summary>
        /// Maximum number of entries to read from condition counters dictionary.
        /// </summary>
        public const int MaxConditionCounterEntries = 300;

        /// <summary>
        /// Maximum valid counter value.
        /// </summary>
        public const int MaxCounterValue = 10000;

        /// <summary>
        /// Maximum array length for condition counters dictionary.
        /// </summary>
        public const int MaxCounterArrayLength = 500;

        #endregion

        #region Memory Offsets (HashSet)

        /// <summary>
        /// Offset to _slots array in HashSet.
        /// </summary>
        public const uint HashSetSlotsOffset = 0x18;

        /// <summary>
        /// Offset to _count in HashSet.
        /// </summary>
        public const uint HashSetCountOffset = 0x3C;

        /// <summary>
        /// Offset to array length in Unity array header.
        /// </summary>
        public const uint ArrayLengthOffset = 0x18;

        /// <summary>
        /// Size of array header in Unity.
        /// </summary>
        public const uint ArrayHeaderSize = 0x20;

        /// <summary>
        /// Possible slot sizes in HashSet (for probing).
        /// </summary>
        public static readonly int[] HashSetSlotSizes = { 0x20, 0x28, 0x30 };

        /// <summary>
        /// Offset to MongoID within HashSet slot.
        /// </summary>
        public const uint SlotMongoIdOffset = 0x08;

        /// <summary>
        /// Offset to _stringId within MongoID.
        /// </summary>
        public const uint MongoIdStringOffset = 0x10;

        #endregion

        #region Memory Offsets (Dictionary)

        /// <summary>
        /// Offset to entries array in Dictionary.
        /// </summary>
        public const uint DictionaryEntriesOffset = 0x18;

        /// <summary>
        /// Offset to _count in Dictionary.
        /// </summary>
        public const uint DictionaryCountOffset = 0x40;

        /// <summary>
        /// Size of dictionary entry struct.
        /// </summary>
        public const int DictionaryEntrySize = 0x28;

        /// <summary>
        /// Offset to hashCode in dictionary entry.
        /// </summary>
        public const int EntryHashCodeOffset = 0x00;

        /// <summary>
        /// Offset to key in dictionary entry.
        /// </summary>
        public const int EntryKeyOffset = 0x08;

        /// <summary>
        /// Offset to value in dictionary entry.
        /// </summary>
        public const int EntryValueOffset = 0x20;

        #endregion

        #region Condition ID Validation

        /// <summary>
        /// Minimum valid condition ID length.
        /// </summary>
        public const int MinConditionIdLength = 10;

        /// <summary>
        /// Maximum valid condition ID length.
        /// </summary>
        public const int MaxValidConditionIdLength = 50;

        /// <summary>
        /// Minimum valid pointer value.
        /// </summary>
        public const ulong MinValidPointer = 0x10000;

        #endregion

        #region Default Values

        /// <summary>
        /// Default map ID when none is available.
        /// </summary>
        public const string DefaultMapId = "MAPDEFAULT";

        #endregion

        #region Quest Location Drawing

        /// <summary>
        /// Height difference threshold for up/down arrows on quest markers.
        /// </summary>
        public const float QuestMarkerHeightThreshold = 1.45f;

        /// <summary>
        /// Quest marker square base size.
        /// </summary>
        public const float QuestMarkerSquareSize = 8f;

        /// <summary>
        /// Quest marker outline stroke width.
        /// </summary>
        public const float QuestMarkerStrokeWidth = 2f;

        /// <summary>
        /// Maximum description length before truncation.
        /// </summary>
        public const int MaxDescriptionLength = 60;

        /// <summary>
        /// Truncated description suffix length to keep.
        /// </summary>
        public const int TruncatedDescriptionLength = 57;

        #endregion
    }
}
