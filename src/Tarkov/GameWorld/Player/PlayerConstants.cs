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

namespace LoneEftDmaRadar.Tarkov.GameWorld.Player
{
    /// <summary>
    /// Constants for player rendering, validation, and processing.
    /// </summary>
    public static class PlayerConstants
    {
        #region Player Pill Rendering

        /// <summary>
        /// Length of the player pill marker.
        /// </summary>
        public const float PillLength = 9f;

        /// <summary>
        /// Radius of the player pill marker.
        /// </summary>
        public const float PillRadius = 3f;

        /// <summary>
        /// Half height multiplier for pill marker.
        /// </summary>
        public const float PillHalfHeightMultiplier = 0.85f;

        /// <summary>
        /// Nose X position multiplier for pill marker.
        /// </summary>
        public const float PillNoseXMultiplier = 0.18f;

        /// <summary>
        /// Death marker line length.
        /// </summary>
        public const float DeathMarkerLength = 6f;

        /// <summary>
        /// Base scale for player pill rendering.
        /// </summary>
        public const float PillBaseScale = 1.65f;

        /// <summary>
        /// Outline width multiplier for pill markers.
        /// </summary>
        public const float OutlineWidthMultiplier = 1.3f;

        /// <summary>
        /// Text offset X from player marker.
        /// </summary>
        public const float TextOffsetX = 9.5f;

        #endregion

        #region Memory Reading

        /// <summary>
        /// Maximum length for reading class names from memory.
        /// </summary>
        public const int MaxClassNameLength = 64;

        /// <summary>
        /// Maximum valid vertex count for skeleton transforms.
        /// </summary>
        public const int MaxVertexCount = 10000;

        /// <summary>
        /// Maximum skeleton index sanity check value.
        /// </summary>
        public const int MaxSkeletonIndex = 128000;

        #endregion

        #region Rotation Validation

        /// <summary>
        /// Maximum absolute rotation X value (yaw).
        /// </summary>
        public const float MaxRotationX = 360f;

        /// <summary>
        /// Maximum absolute rotation Y value (pitch).
        /// </summary>
        public const float MaxRotationY = 90f;

        #endregion

        #region Aim Calculation

        /// <summary>
        /// Default unlimited aimline length.
        /// </summary>
        public const int UnlimitedAimlineLength = 9999;

        /// <summary>
        /// Coefficient A for facing angle calculation.
        /// </summary>
        public const float FacingAngleCoeffA = 31.3573f;

        /// <summary>
        /// Coefficient B for facing angle calculation.
        /// </summary>
        public const float FacingAngleCoeffB = 3.51726f;

        /// <summary>
        /// Coefficient C for facing angle calculation.
        /// </summary>
        public const float FacingAngleCoeffC = 0.626957f;

        /// <summary>
        /// Coefficient D for facing angle calculation.
        /// </summary>
        public const float FacingAngleCoeffD = 15.6948f;

        /// <summary>
        /// Minimum angle degrees for facing calculation.
        /// </summary>
        public const float MinFacingAngle = 1f;

        /// <summary>
        /// Maximum angle degrees for facing calculation.
        /// </summary>
        public const float MaxFacingAngle = 179f;

        #endregion

        #region Class Names

        /// <summary>
        /// Class name for client player.
        /// </summary>
        public const string ClientPlayerClassName = "ClientPlayer";

        /// <summary>
        /// Class name for local player.
        /// </summary>
        public const string LocalPlayerClassName = "LocalPlayer";

        /// <summary>
        /// Default AI name.
        /// </summary>
        public const string DefaultAIName = "defaultAI";

        #endregion

        #region Bezier Curve Constants (Pill Shape)

        /// <summary>
        /// Control point 1 X multiplier.
        /// </summary>
        public const float BezierC1XMultiplier = 1.1f;

        /// <summary>
        /// Control point 2 X multiplier.
        /// </summary>
        public const float BezierC2XMultiplier = 0.28f;

        /// <summary>
        /// Control point 1 Y multiplier.
        /// </summary>
        public const float BezierC1YMultiplier = 0.55f;

        /// <summary>
        /// Control point 2 Y multiplier.
        /// </summary>
        public const float BezierC2YMultiplier = 0.3f;

        #endregion

        #region Wishlist Reading

        /// <summary>
        /// Maximum wishlist items to read.
        /// </summary>
        public const int MaxWishlistItems = 500;

        /// <summary>
        /// Maximum count threshold for wishlist validation.
        /// </summary>
        public const int MaxWishlistCount = 1000;

        /// <summary>
        /// Minimum item ID length for wishlist validation.
        /// </summary>
        public const int MinWishlistItemIdLength = 20;

        /// <summary>
        /// Maximum item ID length for wishlist validation.
        /// </summary>
        public const int MaxWishlistItemIdLength = 30;

        /// <summary>
        /// Maximum string length for wishlist item read.
        /// </summary>
        public const int WishlistItemStringLength = 64;

        /// <summary>
        /// Minimum valid pointer address for wishlist.
        /// </summary>
        public const ulong MinWishlistPointer = 0x10000;

        /// <summary>
        /// Dictionary count offset variant 1.
        /// </summary>
        public const uint DictionaryCountOffset1 = 0x20;

        /// <summary>
        /// Dictionary count offset variant 2.
        /// </summary>
        public const uint DictionaryCountOffset2 = 0x40;

        /// <summary>
        /// Dictionary entries pointer offset.
        /// </summary>
        public const uint DictionaryEntriesOffset = 0x18;

        /// <summary>
        /// Dictionary entries start offset (skip array header).
        /// </summary>
        public const uint DictionaryEntriesStartOffset = 0x20;

        /// <summary>
        /// MongoID _stringId offset within MongoID.
        /// </summary>
        public const uint MongoIdStringIdOffset = 0x10;

        #endregion

        #region Bone Structures

        /// <summary>
        /// Stride between bone transform pointers.
        /// </summary>
        public const uint BonePointerStride = 0x8;

        /// <summary>
        /// Transform internal offset.
        /// </summary>
        public const uint TransformInternalOffset = 0x10;

        #endregion

        #region Movement Context

        /// <summary>
        /// PoseLevel offset in MovementContext.
        /// </summary>
        public const uint MovementContextPoseLevelOffset = 0xD0;

        #endregion

        #region BTR

        /// <summary>
        /// BTR off-map initialization position X.
        /// </summary>
        public const float BtrOffMapPositionX = 9999f;

        /// <summary>
        /// BTR off-map initialization position Z.
        /// </summary>
        public const float BtrOffMapPositionZ = 9999f;

        #endregion

        #region Equipment Initialization

        /// <summary>
        /// Maximum retry attempts for equipment initialization.
        /// </summary>
        public const int EquipmentInitRetryCount = 3;

        /// <summary>
        /// Delay between equipment initialization retries in seconds.
        /// </summary>
        public const int EquipmentInitRetryDelaySeconds = 2;

        /// <summary>
        /// Minimum required slots count for valid equipment.
        /// </summary>
        public const int MinEquipmentSlotCount = 1;

        #endregion

        #region Sus Player Detection Thresholds

        /// <summary>
        /// Default K/D ratio when no data is available.
        /// </summary>
        public const float DefaultKDRatio = 5f;

        /// <summary>
        /// Default survival rate percentage when no data is available.
        /// </summary>
        public const float DefaultSurvivalRate = 50f;

        /// <summary>
        /// K/D ratio threshold for excessive kills (auto-focus).
        /// </summary>
        public const float ExcessiveKDThreshold = 15f;

        /// <summary>
        /// K/D ratio threshold for high kills.
        /// </summary>
        public const float HighKDThreshold = 10f;

        /// <summary>
        /// Maximum hours for new account detection on standard edition.
        /// </summary>
        public const int NewAccountMaxHours = 30;

        /// <summary>
        /// Survival rate threshold for very high survival (suspicious).
        /// </summary>
        public const float HighSurvivalRateThreshold = 65f;

        /// <summary>
        /// Survival rate threshold for low survival (K/D dropping check).
        /// </summary>
        public const float LowSurvivalRateThreshold = 35f;

        /// <summary>
        /// Survival rate threshold for very low survival.
        /// </summary>
        public const float VeryLowSurvivalRateThreshold = 25f;

        /// <summary>
        /// High hours threshold for K/D dropping detection.
        /// </summary>
        public const int HighHoursThreshold = 1000;

        /// <summary>
        /// Low hours threshold for high achievement suspicion.
        /// </summary>
        public const int LowHoursForHighAchievementThreshold = 100;

        /// <summary>
        /// Very high achievement level.
        /// </summary>
        public const int VeryHighAchievementLevel = 2;

        /// <summary>
        /// High achievement level.
        /// </summary>
        public const int HighAchievementLevel = 1;

        /// <summary>
        /// Seconds per hour for time conversion.
        /// </summary>
        public const float SecondsPerHour = 3600f;

        /// <summary>
        /// Percentage multiplier.
        /// </summary>
        public const float PercentageMultiplier = 100f;

        #endregion
    }
}
