/*
 * Lone EFT DMA Radar
 * MIT License - Copyright (c) 2025 Lone DMA
 */

namespace LoneEftDmaRadar.UI.Misc
{
    /// <summary>
    /// Constants for firearm tracking and management.
    /// </summary>
    public static class FirearmConstants
    {
        #region Validation

        /// <summary>
        /// Maximum valid distance from player to fireport.
        /// Fireports further than this are likely invalid during weapon swaps.
        /// </summary>
        public const float MaxFireportDistanceFromPlayer = 100f;

        /// <summary>
        /// Expected length of BSG Item ID string.
        /// </summary>
        public const int BsgItemIdLength = 24;

        /// <summary>
        /// Maximum string length for reading item IDs.
        /// </summary>
        public const int MaxItemIdStringLength = 64;

        #endregion
    }
}
