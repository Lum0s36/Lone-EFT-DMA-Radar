/*
 * Lone EFT DMA Radar
 * MIT License - Copyright (c) 2025 Lone DMA
 */

namespace LoneEftDmaRadar.UI.Misc
{
    /// <summary>
    /// Constants for Device Aimbot and related features.
    /// </summary>
    public static class DeviceAimbotConstants
    {
        #region Timing

        /// <summary>
        /// Main aimbot loop sleep time in milliseconds (~125Hz).
        /// </summary>
        public const int MainLoopSleepMs = 8;

        /// <summary>
        /// Sleep time when not in raid.
        /// </summary>
        public const int NotInRaidSleepMs = 100;

        /// <summary>
        /// Sleep time when LocalPlayer is null.
        /// </summary>
        public const int NoPlayerSleepMs = 50;

        /// <summary>
        /// Sleep time when device is disconnected.
        /// </summary>
        public const int DisconnectedSleepMs = 250;

        /// <summary>
        /// Sleep time when not engaged.
        /// </summary>
        public const int NotEngagedSleepMs = 10;

        /// <summary>
        /// Sleep time when no fireport.
        /// </summary>
        public const int NoFireportSleepMs = 16;

        /// <summary>
        /// Sleep time after error.
        /// </summary>
        public const int ErrorSleepMs = 100;

        #endregion

        #region Targeting

        /// <summary>
        /// Maximum slots count for sanity check.
        /// </summary>
        public const int MaxSlotsCount = 100;

        /// <summary>
        /// Minimum velocity for target prediction (m/s).
        /// </summary>
        public const float MinVelocityForPrediction = 0.5f;

        /// <summary>
        /// Debug player count limit.
        /// </summary>
        public const int DebugPlayerLimit = 3;

        #endregion

        #region Memory Aim

        /// <summary>
        /// Maximum radians for gun angle clamping (~20 degrees).
        /// </summary>
        public const float MaxGunAngleRadians = 0.35f;

        /// <summary>
        /// Default aim intensity (1.0 = no modification).
        /// </summary>
        public const float DefaultAimIntensity = 1.0f;

        /// <summary>
        /// Fixed Y component for shot direction write.
        /// </summary>
        public const float ShotDirectionYComponent = -1.0f;

        #endregion

        #region Debug Overlay

        /// <summary>
        /// Debug overlay background alpha.
        /// </summary>
        public const byte DebugBackgroundAlpha = 180;

        /// <summary>
        /// Debug overlay X position.
        /// </summary>
        public const float DebugOverlayX = 10f;

        /// <summary>
        /// Debug overlay Y start position.
        /// </summary>
        public const float DebugOverlayY = 30f;

        /// <summary>
        /// Debug overlay line height.
        /// </summary>
        public const float DebugLineHeight = 18f;

        /// <summary>
        /// Debug overlay padding.
        /// </summary>
        public const float DebugOverlayPadding = 5f;

        /// <summary>
        /// Debug overlay extra width.
        /// </summary>
        public const float DebugOverlayExtraWidth = 25f;

        /// <summary>
        /// Debug overlay extra height.
        /// </summary>
        public const float DebugOverlayExtraHeight = 20f;

        /// <summary>
        /// Debug shadow offset.
        /// </summary>
        public const float DebugShadowOffset = 1.5f;

        /// <summary>
        /// Debug shadow stroke width.
        /// </summary>
        public const float DebugShadowStrokeWidth = 3f;

        /// <summary>
        /// Initial debug lines capacity.
        /// </summary>
        public const int DebugLinesCapacity = 64;

        #endregion

        #region Fallback Ballistics

        /// <summary>
        /// Fallback bullet mass in grams (7.62-ish).
        /// </summary>
        public const float FallbackBulletMassGrams = 8.0f;

        /// <summary>
        /// Fallback bullet diameter in millimeters.
        /// </summary>
        public const float FallbackBulletDiameterMm = 7.6f;

        /// <summary>
        /// Fallback ballistic coefficient.
        /// </summary>
        public const float FallbackBallisticCoefficient = 0.35f;

        /// <summary>
        /// Fallback bullet speed in m/s.
        /// </summary>
        public const float FallbackBulletSpeed = 800f;

        #endregion
    }
}
