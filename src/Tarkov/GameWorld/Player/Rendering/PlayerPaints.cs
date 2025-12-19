/*
 * Lone EFT DMA Radar
 * MIT License - Copyright (c) 2025 Lone DMA
 */

using LoneEftDmaRadar.Tarkov.GameWorld.Player.Helpers;
using LoneEftDmaRadar.UI.Radar.ViewModels;
using LoneEftDmaRadar.UI.Skia;
using SkiaSharp;

namespace LoneEftDmaRadar.Tarkov.GameWorld.Player.Rendering
{
    /// <summary>
    /// Provides paint selection for player rendering based on player type and state.
    /// Extracted from AbstractPlayer to centralize paint logic.
    /// </summary>
    public static class PlayerPaints
    {
        /// <summary>
        /// Paint pair for fill and text rendering.
        /// </summary>
        public struct PaintPair
        {
            public SKPaint Fill { get; set; }
            public SKPaint Text { get; set; }

            public PaintPair(SKPaint fill, SKPaint text)
            {
                Fill = fill;
                Text = text;
            }
        }

        /// <summary>
        /// Gets the appropriate paints for a player based on their type and state.
        /// </summary>
        public static PaintPair GetPaints(AbstractPlayer player)
        {
            if (player.IsFocused)
                return new PaintPair(SKPaints.PaintFocused, SKPaints.TextFocused);

            if (player is LocalPlayer)
                return new PaintPair(SKPaints.PaintLocalPlayer, SKPaints.TextLocalPlayer);

            return player.Type switch
            {
                PlayerType.Teammate => new PaintPair(SKPaints.PaintTeammate, SKPaints.TextTeammate),
                PlayerType.PMC => GetPmcPaints(player),
                PlayerType.AIScav => new PaintPair(SKPaints.PaintScav, SKPaints.TextScav),
                PlayerType.AIRaider => new PaintPair(SKPaints.PaintRaider, SKPaints.TextRaider),
                PlayerType.AIBoss => new PaintPair(SKPaints.PaintBoss, SKPaints.TextBoss),
                PlayerType.PScav => new PaintPair(SKPaints.PaintPScav, SKPaints.TextPScav),
                PlayerType.SpecialPlayer => new PaintPair(SKPaints.PaintWatchlist, SKPaints.TextWatchlist),
                PlayerType.Streamer => new PaintPair(SKPaints.PaintStreamer, SKPaints.TextStreamer),
                _ => new PaintPair(SKPaints.PaintPMC, SKPaints.TextPMC)
            };
        }

        private static PaintPair GetPmcPaints(AbstractPlayer player)
        {
            return player.PlayerSide switch
            {
                Enums.EPlayerSide.Bear => new PaintPair(SKPaints.PaintPMCBear, SKPaints.TextPMCBear),
                Enums.EPlayerSide.Usec => new PaintPair(SKPaints.PaintPMCUsec, SKPaints.TextPMCUsec),
                _ => new PaintPair(SKPaints.PaintPMC, SKPaints.TextPMC)
            };
        }
    }
}
