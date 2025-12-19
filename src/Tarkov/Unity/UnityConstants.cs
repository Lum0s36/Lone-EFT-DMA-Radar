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

namespace LoneEftDmaRadar.Tarkov.Unity
{
    /// <summary>
    /// Constants for Unity structures, collections, and transforms.
    /// </summary>
    public static class UnityConstants
    {
        #region Collection Limits

        /// <summary>
        /// Maximum count for Unity collections (sanity check).
        /// </summary>
        public const int MaxCollectionCount = 16384;

        /// <summary>
        /// Maximum transform index (sanity check).
        /// </summary>
        public const int MaxTransformIndex = 128000;

        /// <summary>
        /// Maximum iterations for transform hierarchy traversal.
        /// </summary>
        public const int MaxTransformIterations = 4000;

        #endregion

        #region UnityList Offsets

        /// <summary>
        /// Offset to count field in Unity List.
        /// </summary>
        public const uint ListCountOffset = 0x18;

        /// <summary>
        /// Offset to array pointer in Unity List.
        /// </summary>
        public const uint ListArrayOffset = 0x10;

        /// <summary>
        /// Offset to first element in Unity List array.
        /// </summary>
        public const uint ListArrayStartOffset = 0x20;

        #endregion

        #region UnityArray Offsets

        /// <summary>
        /// Offset to count field in Unity Array.
        /// </summary>
        public const uint ArrayCountOffset = 0x18;

        /// <summary>
        /// Offset to first element in Unity Array.
        /// </summary>
        public const uint ArrayBaseOffset = 0x20;

        #endregion

        #region UnityDictionary Offsets

        /// <summary>
        /// Offset to count field in Unity Dictionary.
        /// </summary>
        public const uint DictionaryCountOffset = 0x40;

        /// <summary>
        /// Offset to entries pointer in Unity Dictionary.
        /// </summary>
        public const uint DictionaryEntriesOffset = 0x18;

        /// <summary>
        /// Offset to first entry in Unity Dictionary entries array.
        /// </summary>
        public const uint DictionaryEntriesStartOffset = 0x20;

        #endregion

        #region UnityHashSet Offsets

        /// <summary>
        /// Offset to count field in Unity HashSet.
        /// </summary>
        public const uint HashSetCountOffset = 0x30;

        /// <summary>
        /// Offset to slots pointer in Unity HashSet.
        /// </summary>
        public const uint HashSetSlotsOffset = 0x18;

        /// <summary>
        /// Offset to first slot in Unity HashSet slots array.
        /// </summary>
        public const uint HashSetSlotsStartOffset = 0x20;

        #endregion

        #region DynamicArray Offsets

        /// <summary>
        /// Offset to base pointer in Dynamic Array.
        /// </summary>
        public const uint DynamicArrayBaseOffset = 0x10;

        /// <summary>
        /// Offset to size field in Dynamic Array.
        /// </summary>
        public const uint DynamicArraySizeOffset = 0x18;

        /// <summary>
        /// Stride between elements in Dynamic Array.
        /// </summary>
        public const uint DynamicArrayStride = 0x10;

        #endregion

        #region String Reading

        /// <summary>
        /// Default string read buffer size.
        /// </summary>
        public const int DefaultStringBufferSize = 128;

        /// <summary>
        /// Maximum name length for game objects.
        /// </summary>
        public const int MaxGameObjectNameLength = 128;

        #endregion

        #region Direction Vectors

        /// <summary>
        /// Standard left direction vector.
        /// </summary>
        public static readonly Vector3 Left = new(-1, 0, 0);

        /// <summary>
        /// Standard right direction vector.
        /// </summary>
        public static readonly Vector3 Right = new(1, 0, 0);

        /// <summary>
        /// Standard up direction vector.
        /// </summary>
        public static readonly Vector3 Up = new(0, 1, 0);

        /// <summary>
        /// Standard down direction vector.
        /// </summary>
        public static readonly Vector3 Down = new(0, -1, 0);

        /// <summary>
        /// Standard forward direction vector.
        /// </summary>
        public static readonly Vector3 Forward = new(0, 0, 1);

        #endregion

        #region Signature Scanning

        /// <summary>
        /// RVA offset within signature for AllCameras.
        /// </summary>
        public const uint AllCamerasSignatureRvaOffset = 3;

        /// <summary>
        /// Instruction length offset for AllCameras signature.
        /// </summary>
        public const uint AllCamerasSignatureInstructionLength = 7;

        #endregion
    }
}
