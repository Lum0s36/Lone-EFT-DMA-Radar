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

namespace LoneEftDmaRadar.Tarkov.GameWorld.Loot
{
    /// <summary>
    /// Constants for loot rendering and processing.
    /// </summary>
    public static class LootConstants
    {
        #region Rendering

        /// <summary>
        /// Height difference threshold for determining if loot is above/below player.
        /// </summary>
        public const float HeightThreshold = 1.45f;

        /// <summary>
        /// Size of directional arrow for loot markers.
        /// </summary>
        public const float ArrowSize = 5f;

        /// <summary>
        /// Base size of circle marker for loot.
        /// </summary>
        public const float BaseCircleSize = 5f;

        /// <summary>
        /// Outline stroke width for loot markers.
        /// </summary>
        public const float OutlineStrokeWidth = 2f;

        /// <summary>
        /// Horizontal offset for loot label text.
        /// </summary>
        public const float LabelOffsetX = 7f;

        /// <summary>
        /// Vertical offset for loot label text.
        /// </summary>
        public const float LabelOffsetY = 3f;

        /// <summary>
        /// Stroke width for custom filter paints.
        /// </summary>
        public const float FilterPaintStrokeWidth = 3f;

        #endregion

        #region Memory Reading

        /// <summary>
        /// Maximum length for reading class names from memory.
        /// </summary>
        public const int MaxClassNameLength = 64;

        /// <summary>
        /// Maximum length for reading class names during scatter read.
        /// </summary>
        public const int MaxClassNameReadLength = 64;

        /// <summary>
        /// Maximum length for reading object names from memory.
        /// </summary>
        public const int MaxObjectNameLength = 64;

        /// <summary>
        /// Maximum length for reading object names during scatter read.
        /// </summary>
        public const int MaxObjectNameReadLength = 64;

        /// <summary>
        /// Maximum length for reading short names from memory.
        /// </summary>
        public const int MaxShortNameLength = 128;

        /// <summary>
        /// Offset to transform internal pointer in components array.
        /// </summary>
        public const uint ComponentsTransformOffset = 0x8;

        #endregion

        #region Loot Classification

        /// <summary>
        /// Class name identifier for corpse loot.
        /// </summary>
        public const string CorpseClassName = "Corpse";

        /// <summary>
        /// Class name identifier for loose loot items.
        /// </summary>
        public const string LooseLootClassName = "ObservedLootItem";

        /// <summary>
        /// Class name identifier for lootable containers.
        /// </summary>
        public const string ContainerClassName = "LootableContainer";

        /// <summary>
        /// Object name identifier for airdrops.
        /// </summary>
        public const string AirdropObjectName = "loot_collider";

        /// <summary>
        /// Object name pattern to skip (script objects).
        /// </summary>
        public const string SkipObjectNamePattern = "script";

        #endregion
    }
}
