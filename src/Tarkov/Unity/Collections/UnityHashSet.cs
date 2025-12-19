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
using LoneEftDmaRadar.DMA;

namespace LoneEftDmaRadar.Tarkov.Unity.Collections
{
    /// <summary>
    /// DMA Wrapper for a C# HashSet
    /// Must initialize before use. Must dispose after use.
    /// </summary>
    /// <typeparam name="T">Collection Type</typeparam>
    public sealed class UnityHashSet<T> : PooledMemory<UnityHashSet<T>.MemHashEntry>
        where T : unmanaged
    {
        /// <summary>Offset to count field. Use UnityConstants.HashSetCountOffset instead.</summary>
        public const uint CountOffset = UnityConstants.HashSetCountOffset;
        /// <summary>Offset to slots pointer. Use UnityConstants.HashSetSlotsOffset instead.</summary>
        public const uint ArrOffset = UnityConstants.HashSetSlotsOffset;
        /// <summary>Offset to first slot. Use UnityConstants.HashSetSlotsStartOffset instead.</summary>
        public const uint ArrStartOffset = UnityConstants.HashSetSlotsStartOffset;

        private UnityHashSet() : base(0) { }
        private UnityHashSet(int count) : base(count) { }

        /// <summary>
        /// Factory method to create a new <see cref="UnityHashSet{T}"/> instance from a memory address.
        /// </summary>
        /// <param name="addr"></param>
        /// <param name="useCache"></param>
        /// <returns></returns>
        public static UnityHashSet<T> Create(ulong addr, bool useCache = true)
        {
            var count = MemoryInterface.Memory.ReadValue<int>(addr + UnityConstants.HashSetCountOffset, useCache);
            ArgumentOutOfRangeException.ThrowIfGreaterThan(count, UnityConstants.MaxCollectionCount, nameof(count));
            var hs = new UnityHashSet<T>(count);
            try
            {
                if (count == 0)
                {
                    return hs;
                }
                var hashSetBase = MemoryInterface.Memory.ReadPtr(addr + UnityConstants.HashSetSlotsOffset, useCache) + UnityConstants.HashSetSlotsStartOffset;
                MemoryInterface.Memory.ReadSpan(hashSetBase, hs.Span, useCache);
                return hs;
            }
            catch
            {
                hs.Dispose();
                throw;
            }
        }

        // Pack = 1 to match Unity/Mono memory layout for HashSet entries
        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        public readonly struct MemHashEntry
        {
            public static implicit operator T(MemHashEntry x) => x.Value;

            private readonly int _hashCode;
            private readonly int _next;
            public readonly T Value;
        }
    }
}
