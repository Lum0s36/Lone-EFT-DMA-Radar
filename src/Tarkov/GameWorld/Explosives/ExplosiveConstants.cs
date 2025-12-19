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

namespace LoneEftDmaRadar.Tarkov.GameWorld.Explosives
{
    /// <summary>
    /// Constants for explosive item rendering.
    /// </summary>
    public static class ExplosiveConstants
    {
        #region Rendering

        /// <summary>
        /// Base size of explosive marker circle.
        /// </summary>
        public const float MarkerSize = 5f;

        /// <summary>
        /// Additional stroke width for outline.
        /// </summary>
        public const float OutlineStrokeWidthAddition = 2f;

        #endregion

        #region Memory Reading

        /// <summary>
        /// Maximum length for reading type names from memory.
        /// </summary>
        public const int MaxTypeNameLength = 64;

        #endregion

        #region Type Identifiers

        /// <summary>
        /// Identifier for smoke grenade type.
        /// </summary>
        public const string SmokeGrenadeIdentifier = "SmokeGrenade";

        #endregion

        #region Grenade Offsets

        /// <summary>
        /// Offset to grenades list pointer from Grenades dictionary.
        /// </summary>
        public const uint GrenadesListOffset = 0x18;

        #endregion
    }
}
