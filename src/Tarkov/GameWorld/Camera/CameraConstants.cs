/*
 * Lone EFT DMA Radar
 * MIT License - Copyright (c) 2025 Lone DMA
 */

namespace LoneEftDmaRadar.Tarkov.GameWorld.Camera
{
    /// <summary>
    /// Constants for Camera Manager and viewport operations.
    /// </summary>
    public static class CameraConstants
    {
        #region Viewport

        /// <summary>
        /// Tolerance for viewport boundary checks.
        /// </summary>
        public const int ViewportTolerance = 800;

        /// <summary>
        /// Default fallback viewport width.
        /// </summary>
        public const int DefaultViewportWidth = 1920;

        /// <summary>
        /// Default fallback viewport height.
        /// </summary>
        public const int DefaultViewportHeight = 1080;

        #endregion

        #region Camera Search

        /// <summary>
        /// Maximum cameras to iterate when searching.
        /// </summary>
        public const int MaxCameraSearchCount = 100;

        /// <summary>
        /// Maximum camera name length to read.
        /// </summary>
        public const int MaxCameraNameLength = 64;

        /// <summary>
        /// Minimum camera name length for valid name.
        /// </summary>
        public const int MinCameraNameLength = 3;

        /// <summary>
        /// Camera list entry stride (pointer size).
        /// </summary>
        public const int CameraListStride = 0x8;

        /// <summary>
        /// Camera list count offset.
        /// </summary>
        public const int CameraListCountOffset = 0x8;

        /// <summary>
        /// Camera matrix components offset.
        /// </summary>
        public const uint CameraMatrixComponentsOffset = 0x18;

        #endregion

        #region View Matrix Validation

        /// <summary>
        /// Minimum W value for valid projection.
        /// </summary>
        public const float MinProjectionW = 0.098f;

        /// <summary>
        /// Minimum vector magnitude for valid matrix.
        /// </summary>
        public const float MinVectorMagnitude = 0.1f;

        /// <summary>
        /// Maximum vector magnitude for valid matrix.
        /// </summary>
        public const float MaxVectorMagnitude = 100.0f;

        /// <summary>
        /// Minimum absolute value for non-zero check.
        /// </summary>
        public const float MinNonZeroValue = 0.001f;

        #endregion

        #region Zoom

        /// <summary>
        /// Minimum zoom level to consider scoped.
        /// </summary>
        public const float MinScopeZoom = 1f;

        /// <summary>
        /// Maximum valid zoom level.
        /// </summary>
        public const float MaxValidZoom = 100f;

        /// <summary>
        /// Maximum valid selected scope index.
        /// </summary>
        public const int MaxSelectedScopeIndex = 10;

        /// <summary>
        /// Zoom array element size.
        /// </summary>
        public const uint ZoomElementSize = 0x4;

        #endregion

        #region Memory Validation

        /// <summary>
        /// Maximum valid pointer address (48-bit).
        /// </summary>
        public const ulong MaxValidPointer = 0x7FFFFFFFFFFF;

        #endregion
    }
}
