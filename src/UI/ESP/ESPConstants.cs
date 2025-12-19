/*
 * Lone EFT DMA Radar
 * MIT License - Copyright (c) 2025 Lone DMA
 */

namespace LoneEftDmaRadar.UI.ESP
{
    /// <summary>
    /// Constants for ESP rendering and window management.
    /// </summary>
    public static class ESPConstants
    {
        #region Window Defaults

        /// <summary>
        /// Default ESP window width.
        /// </summary>
        public const double DefaultWindowWidth = 400;

        /// <summary>
        /// Default ESP window height.
        /// </summary>
        public const double DefaultWindowHeight = 300;

        #endregion

        #region Rendering

        /// <summary>
        /// High-frequency timer interval in milliseconds (~250 FPS max).
        /// </summary>
        public const int HighFrequencyTimerIntervalMs = 4;

        /// <summary>
        /// Notification display duration in milliseconds.
        /// </summary>
        public const int NotificationDurationMs = 2000;

        /// <summary>
        /// Notification fade-in duration in milliseconds.
        /// </summary>
        public const int NotificationFadeInMs = 200;

        /// <summary>
        /// Notification fade-out duration in milliseconds.
        /// </summary>
        public const int NotificationFadeOutMs = 300;

        #endregion

        #region Skeleton/Box Rendering

        /// <summary>
        /// Default skeleton line thickness.
        /// </summary>
        public const float SkeletonStrokeWidth = 1.5f;

        /// <summary>
        /// Default box line thickness.
        /// </summary>
        public const float BoxStrokeWidth = 1.0f;

        /// <summary>
        /// Default crosshair line thickness.
        /// </summary>
        public const float CrosshairStrokeWidth = 1.5f;

        /// <summary>
        /// Minimum crosshair length.
        /// </summary>
        public const float MinCrosshairLength = 2f;

        #endregion

        #region Bounding Box

        /// <summary>
        /// Bounding box X/Z extent from player center.
        /// </summary>
        public const float BoundingBoxExtentXZ = 0.4f;

        /// <summary>
        /// Bounding box height.
        /// </summary>
        public const float BoundingBoxHeight = 1.75f;

        /// <summary>
        /// Bounding box padding.
        /// </summary>
        public const float BoundingBoxPadding = 2f;

        /// <summary>
        /// Minimum bounding box dimension.
        /// </summary>
        public const float MinBoundingBoxDimension = 1f;

        #endregion

        #region Head Circle

        /// <summary>
        /// Head circle offset above head bone.
        /// </summary>
        public const float HeadCircleOffset = 0.18f;

        /// <summary>
        /// Head circle radius multiplier.
        /// </summary>
        public const float HeadCircleRadiusMultiplier = 0.65f;

        /// <summary>
        /// Minimum head circle radius.
        /// </summary>
        public const float MinHeadCircleRadius = 2f;

        /// <summary>
        /// Maximum head circle radius.
        /// </summary>
        public const float MaxHeadCircleRadius = 12f;

        #endregion

        #region Marker Scaling

        /// <summary>
        /// Base marker radius before scaling.
        /// </summary>
        public const float BaseMarkerRadius = 3f;

        /// <summary>
        /// Minimum marker radius after scaling.
        /// </summary>
        public const float MinMarkerRadius = 2f;

        /// <summary>
        /// Maximum marker radius after scaling.
        /// </summary>
        public const float MaxMarkerRadius = 15f;

        /// <summary>
        /// Reference distance for 1.0x scale calculation.
        /// </summary>
        public const float ScaleReferenceDistance = 10f;

        /// <summary>
        /// Minimum scale factor.
        /// </summary>
        public const float MinScaleFactor = 0.3f;

        /// <summary>
        /// Maximum scale factor.
        /// </summary>
        public const float MaxScaleFactor = 3f;

        /// <summary>
        /// Scale threshold for using medium vs small text.
        /// </summary>
        public const float MediumTextScaleThreshold = 1.5f;

        #endregion

        #region Distance Scaling

        /// <summary>
        /// Reference distance for skeleton thickness scaling.
        /// </summary>
        public const float SkeletonScaleReferenceDistance = 50f;

        /// <summary>
        /// Minimum skeleton scale distance.
        /// </summary>
        public const float MinSkeletonScaleDistance = 5f;

        /// <summary>
        /// Minimum skeleton scale factor.
        /// </summary>
        public const float MinSkeletonScaleFactor = 0.5f;

        /// <summary>
        /// Maximum skeleton scale factor.
        /// </summary>
        public const float MaxSkeletonScaleFactor = 2.5f;

        #endregion

        #region Text Positioning

        /// <summary>
        /// Text offset from marker.
        /// </summary>
        public const float TextOffsetFromMarker = 4f;

        /// <summary>
        /// Text padding above/below bounding box.
        /// </summary>
        public const int TextPadding = 6;

        /// <summary>
        /// Nearest player info Y padding from bottom.
        /// </summary>
        public const float NearestPlayerInfoPadding = 150f;

        /// <summary>
        /// Notification padding from edge.
        /// </summary>
        public const float NotificationPadding = 20f;

        /// <summary>
        /// Notification background padding.
        /// </summary>
        public const float NotificationBgPadding = 14f;

        #endregion

        #region Screen Margins

        /// <summary>
        /// Screen margin for clipping checks.
        /// </summary>
        public const float ScreenMargin = 200f;

        /// <summary>
        /// Bounding box screen margin for clipping.
        /// </summary>
        public const float BoundingBoxScreenMargin = 50f;

        #endregion

        #region DeviceAimbot

        /// <summary>
        /// DeviceAimbot target line thickness.
        /// </summary>
        public const float DeviceAimbotLineThickness = 2f;

        /// <summary>
        /// Minimum FOV circle radius.
        /// </summary>
        public const float MinFovCircleRadius = 5f;

        #endregion

        #region Debug Overlay

        /// <summary>
        /// Debug overlay X position.
        /// </summary>
        public const float DebugOverlayX = 10f;

        /// <summary>
        /// Debug overlay starting Y position.
        /// </summary>
        public const float DebugOverlayY = 40f;

        /// <summary>
        /// Debug overlay line height.
        /// </summary>
        public const float DebugOverlayLineHeight = 16f;

        /// <summary>
        /// FPS display X position.
        /// </summary>
        public const float FpsDisplayX = 10f;

        /// <summary>
        /// FPS display Y position.
        /// </summary>
        public const float FpsDisplayY = 10f;

        #endregion
    }
}
