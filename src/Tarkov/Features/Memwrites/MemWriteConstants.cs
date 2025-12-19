/*
 * Lone EFT DMA Radar
 * MIT License - Copyright (c) 2025 Lone DMA
 */

namespace LoneEftDmaRadar.Tarkov.Features.MemWrites
{
    /// <summary>
    /// Constants for memory write features.
    /// </summary>
    public static class MemWriteConstants
    {
        #region NoRecoil

        /// <summary>
        /// Delay between NoRecoil applications in milliseconds.
        /// </summary>
        public const int NoRecoilDelayMs = 50;

        /// <summary>
        /// Default intensity value (no modification).
        /// </summary>
        public const float DefaultIntensity = 1.0f;

        /// <summary>
        /// Minimum valid breath intensity value.
        /// </summary>
        public const float MinBreathIntensity = 0f;

        /// <summary>
        /// Maximum valid breath intensity value.
        /// </summary>
        public const float MaxBreathIntensity = 5f;

        /// <summary>
        /// Threshold below which mask is minimized for NoRecoil.
        /// </summary>
        public const float MinimalMaskThreshold = 0.15f;

        /// <summary>
        /// Tolerance for comparing float values.
        /// </summary>
        public const float FloatComparisonTolerance = 0.001f;

        #endregion

        #region InfiniteStamina

        /// <summary>
        /// Delay between InfiniteStamina applications in seconds.
        /// </summary>
        public const int InfiniteStaminaDelaySeconds = 1;

        /// <summary>
        /// Maximum stamina value.
        /// </summary>
        public const float MaxStamina = 100f;

        /// <summary>
        /// Maximum oxygen value.
        /// </summary>
        public const float MaxOxygen = 350f;

        /// <summary>
        /// Threshold percentage below which stamina/oxygen is refilled.
        /// </summary>
        public const float RefillThreshold = 0.33f;

        /// <summary>
        /// Maximum valid stamina value for validation.
        /// </summary>
        public const float MaxValidStamina = 500f;

        /// <summary>
        /// Maximum valid oxygen value for validation.
        /// </summary>
        public const float MaxValidOxygen = 1000f;

        /// <summary>
        /// Initial stamina write batch capacity.
        /// </summary>
        public const int StaminaWriteBatchInitialCapacity = 4;

        #endregion

        #region MemoryAim

        /// <summary>
        /// Delay between MemoryAim applications in milliseconds.
        /// </summary>
        public const int MemoryAimDelayMs = 16;

        #endregion
    }
}
