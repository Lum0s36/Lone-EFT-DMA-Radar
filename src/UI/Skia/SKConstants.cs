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

namespace LoneEftDmaRadar.UI.Skia
{
    /// <summary>
    /// Constants for Skia rendering operations.
    /// </summary>
    public static class SKConstants
    {
        #region Stroke Widths

        /// <summary>
        /// Standard stroke width for player markers.
        /// </summary>
        public const float PlayerStrokeWidth = 1.66f;

        /// <summary>
        /// Standard stroke width for loot markers.
        /// </summary>
        public const float LootStrokeWidth = 0.25f;

        /// <summary>
        /// Stroke width for explosive markers.
        /// </summary>
        public const float ExplosiveStrokeWidth = 3f;

        /// <summary>
        /// Stroke width for death markers.
        /// </summary>
        public const float DeathMarkerStrokeWidth = 3f;

        /// <summary>
        /// Stroke width for text outlines.
        /// </summary>
        public const float TextOutlineStrokeWidth = 2f;

        /// <summary>
        /// Stroke width for connector lines between grouped players.
        /// </summary>
        public const float ConnectorStrokeWidth = 2.25f;

        /// <summary>
        /// Stroke width for hazard markers.
        /// </summary>
        public const float HazardStrokeWidth = 2f;

        /// <summary>
        /// Stroke width for quest zone markers.
        /// </summary>
        public const float QuestZoneStrokeWidth = 3f;

        /// <summary>
        /// Stroke width for ESP widget lines.
        /// </summary>
        public const float ESPStrokeWidth = 1f;

        #endregion

        #region Alpha Values

        /// <summary>
        /// Alpha value for transparent backer (190/255).
        /// </summary>
        public const byte TransparentBackerAlpha = 0xBE;

        /// <summary>
        /// Alpha value for bitmap alpha paint (127/255).
        /// </summary>
        public const byte BitmapAlphaValue = 127;

        /// <summary>
        /// Alpha value for connector group paint (60/255).
        /// </summary>
        public const byte ConnectorGroupAlpha = 60;

        /// <summary>
        /// Alpha value for closed exfil (128/255).
        /// </summary>
        public const byte ExfilClosedAlpha = 128;

        #endregion

        #region Text Sizes

        /// <summary>
        /// Text size for quest zone labels.
        /// </summary>
        public const float QuestZoneTextSize = 13f;

        #endregion

        #region Marker Sizes

        /// <summary>
        /// Base size for exfil markers.
        /// </summary>
        public const float ExfilMarkerSize = 6f;

        /// <summary>
        /// Base size for transit markers.
        /// </summary>
        public const float TransitMarkerSize = 5f;

        /// <summary>
        /// Base size for hazard markers.
        /// </summary>
        public const float HazardMarkerSize = 8f;

        /// <summary>
        /// Size for mouseover text background padding.
        /// </summary>
        public const float MouseoverPadding = 3f;

        #endregion

        #region Drawing Offsets

        /// <summary>
        /// Standard text offset X from marker.
        /// </summary>
        public const float TextOffsetX = 7f;

        /// <summary>
        /// Standard text offset Y from marker.
        /// </summary>
        public const float TextOffsetY = 3f;

        #endregion

        #region Color Hex Codes

        /// <summary>
        /// Hex color code for Raider entities.
        /// </summary>
        public const string RaiderColorHex = "ffc70f";

        /// <summary>
        /// Hex color code for Backpack loot.
        /// </summary>
        public const string BackpackColorHex = "00b02c";

        /// <summary>
        /// Hex color code for Container loot.
        /// </summary>
        public const string ContainerColorHex = "FFFFCC";

        #endregion

        #region Widget Constants

        /// <summary>
        /// Default widget border radius.
        /// </summary>
        public const float WidgetBorderRadius = 5f;

        /// <summary>
        /// Default widget padding.
        /// </summary>
        public const float WidgetPadding = 10f;

        /// <summary>
        /// Line spacing multiplier for widgets.
        /// </summary>
        public const float WidgetLineSpacingMultiplier = 1.2f;

        /// <summary>
        /// Widget title bar padding.
        /// </summary>
        public const float WidgetTitlePaddingMultiplier = 2.5f;

        /// <summary>
        /// Widget base font size.
        /// </summary>
        public const float WidgetBaseFontSize = 9f;

        /// <summary>
        /// Widget title bar base height.
        /// </summary>
        public const float WidgetTitleBarBaseHeight = 12.5f;

        /// <summary>
        /// Widget resize glyph base size.
        /// </summary>
        public const float WidgetResizeGlyphBaseSize = 10.5f;

        /// <summary>
        /// Widget minimum size.
        /// </summary>
        public const float WidgetMinSize = 16f;

        /// <summary>
        /// Widget background alpha value.
        /// </summary>
        public const byte WidgetBackgroundAlpha = 0xBE;

        /// <summary>
        /// Widget title bar stroke width.
        /// </summary>
        public const float WidgetTitleBarStrokeWidth = 0.5f;

        /// <summary>
        /// Widget button stroke width.
        /// </summary>
        public const float WidgetButtonStrokeWidth = 0.1f;

        /// <summary>
        /// Widget symbol stroke width.
        /// </summary>
        public const float WidgetSymbolStrokeWidth = 2f;

        #endregion

        #region Arrow/Marker Constants

        /// <summary>
        /// Default arrow size.
        /// </summary>
        public const float DefaultArrowSize = 6f;

        /// <summary>
        /// Mine marker base length.
        /// </summary>
        public const float MineMarkerLength = 3.5f;

        /// <summary>
        /// Hazard marker triangle size.
        /// </summary>
        public const float HazardMarkerTriangleSize = 6f;

        /// <summary>
        /// Hazard marker bottom multiplier.
        /// </summary>
        public const float HazardMarkerBottomMultiplier = 0.6f;

        #endregion

        #region Aimview Widget

        /// <summary>
        /// Head offset for head circle calculation (meters).
        /// </summary>
        public const float AimviewHeadOffset = 0.18f;

        /// <summary>
        /// Head circle radius multiplier.
        /// </summary>
        public const float AimviewHeadRadiusMultiplier = 0.65f;

        /// <summary>
        /// Minimum head circle radius.
        /// </summary>
        public const float AimviewHeadRadiusMin = 2f;

        /// <summary>
        /// Maximum head circle radius.
        /// </summary>
        public const float AimviewHeadRadiusMax = 12f;

        /// <summary>
        /// Reference distance for perspective scaling (1.0x scale).
        /// </summary>
        public const float AimviewReferenceDistance = 10f;

        /// <summary>
        /// Minimum perspective scale factor.
        /// </summary>
        public const float AimviewPerspectiveScaleMin = 0.3f;

        /// <summary>
        /// Maximum perspective scale factor.
        /// </summary>
        public const float AimviewPerspectiveScaleMax = 3f;

        /// <summary>
        /// Tolerance for edge projection in Aimview.
        /// </summary>
        public const float AimviewEdgeTolerance = 100f;

        /// <summary>
        /// Base font size multiplier for perspective scaling.
        /// </summary>
        public const float AimviewFontScaleMultiplier = 0.9f;

        /// <summary>
        /// Minimum font size for Aimview labels.
        /// </summary>
        public const float AimviewFontSizeMin = 8f;

        /// <summary>
        /// Maximum font size for Aimview labels.
        /// </summary>
        public const float AimviewFontSizeMax = 20f;

        /// <summary>
        /// Minimum marker radius.
        /// </summary>
        public const float AimviewMarkerRadiusMin = 2f;

        /// <summary>
        /// Maximum marker radius.
        /// </summary>
        public const float AimviewMarkerRadiusMax = 15f;

        /// <summary>
        /// Base marker radius.
        /// </summary>
        public const float AimviewMarkerRadiusBase = 3f;

        /// <summary>
        /// Label offset from marker center (X).
        /// </summary>
        public const float AimviewLabelOffsetX = 3f;

        /// <summary>
        /// Label offset from marker center (Y).
        /// </summary>
        public const float AimviewLabelOffsetY = 1f;

        /// <summary>
        /// Line spacing between items.
        /// </summary>
        public const float AimviewLineSpacing = 2f;

        /// <summary>
        /// Minimum distance for skeleton line scaling.
        /// </summary>
        public const float AimviewMinSkeletonDistance = 5f;

        /// <summary>
        /// Base skeleton distance for scaling calculation.
        /// </summary>
        public const float AimviewSkeletonDistanceBase = 50f;

        /// <summary>
        /// Minimum skeleton distance scale.
        /// </summary>
        public const float AimviewSkeletonScaleMin = 0.5f;

        /// <summary>
        /// Maximum skeleton distance scale.
        /// </summary>
        public const float AimviewSkeletonScaleMax = 2.5f;

        /// <summary>
        /// Base skeleton line thickness.
        /// </summary>
        public const float AimviewSkeletonLineThicknessBase = 1.5f;

        /// <summary>
        /// Minimum skeleton line thickness.
        /// </summary>
        public const float AimviewSkeletonLineThicknessMin = 0.5f;

        #endregion

        #region Quest Helper Widget

        /// <summary>
        /// Quest helper widget minimum width.
        /// </summary>
        public const float QuestWidgetMinWidth = 300f;

        /// <summary>
        /// Quest helper widget minimum height.
        /// </summary>
        public const float QuestWidgetMinHeight = 200f;

        /// <summary>
        /// Quest helper widget padding.
        /// </summary>
        public const float QuestWidgetPadding = 5f;

        /// <summary>
        /// Quest helper widget quest padding.
        /// </summary>
        public const float QuestWidgetQuestPadding = 8f;

        /// <summary>
        /// Quest helper widget objective indent.
        /// </summary>
        public const float QuestWidgetObjectiveIndent = 20f;

        /// <summary>
        /// Quest helper widget scrollbar width.
        /// </summary>
        public const float QuestWidgetScrollbarWidth = 6f;

        /// <summary>
        /// Quest helper widget minimum scrollbar thumb height.
        /// </summary>
        public const float QuestWidgetMinThumbHeight = 20f;

        /// <summary>
        /// Quest helper widget scrollbar thumb corner radius.
        /// </summary>
        public const float QuestWidgetThumbRadius = 3f;

        /// <summary>
        /// Quest helper widget scroll lines per wheel tick.
        /// </summary>
        public const float QuestWidgetScrollLinesPerTick = 3f;

        /// <summary>
        /// Quest helper widget mouse wheel divisor.
        /// </summary>
        public const float QuestWidgetMouseWheelDivisor = 120f;

        /// <summary>
        /// Quest helper widget title separator spacing.
        /// </summary>
        public const float QuestWidgetTitleSpacing = 2f;

        /// <summary>
        /// Quest helper widget objective spacing.
        /// </summary>
        public const float QuestWidgetObjectiveSpacing = 4f;

        /// <summary>
        /// Quest helper widget icon text spacing.
        /// </summary>
        public const float QuestWidgetIconSpacing = 5f;

        /// <summary>
        /// Quest helper widget scrollbar margin.
        /// </summary>
        public const float QuestWidgetScrollbarMargin = 2f;

        /// <summary>
        /// Quest helper widget scrollbar padding.
        /// </summary>
        public const float QuestWidgetScrollbarPadding = 10f;

        #endregion
    }
}
