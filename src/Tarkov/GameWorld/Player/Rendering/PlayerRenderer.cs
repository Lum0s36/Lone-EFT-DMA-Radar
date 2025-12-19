/*
 * Lone EFT DMA Radar
 * MIT License - Copyright (c) 2025 Lone DMA
 */

using Collections.Pooled;
using LoneEftDmaRadar.Misc;
using LoneEftDmaRadar.Tarkov.GameWorld.Player.Helpers;
using LoneEftDmaRadar.UI.Radar.Maps;
using LoneEftDmaRadar.UI.Radar.ViewModels;
using LoneEftDmaRadar.UI.Skia;
using LoneEftDmaRadar.Web.TarkovDev.Data;
using SkiaSharp;

namespace LoneEftDmaRadar.Tarkov.GameWorld.Player.Rendering
{
    /// <summary>
    /// Handles rendering of player markers on the radar map.
    /// Extracted from AbstractPlayer to separate rendering concerns from game logic.
    /// </summary>
    public static class PlayerRenderer
    {
        #region Cached Paths

        private static readonly SKPath _playerPill = CreatePlayerPillPath();
        private static readonly SKPath _deathMarker = CreateDeathMarkerPath();

        private static SKPath CreatePlayerPillPath()
        {
            float halfHeight = PlayerConstants.PillRadius * PlayerConstants.PillHalfHeightMultiplier;
            float noseX = PlayerConstants.PillLength / 2f + PlayerConstants.PillRadius * PlayerConstants.PillNoseXMultiplier;

            var path = new SKPath();

            // Rounded back (left side)
            var backRect = new SKRect(
                -PlayerConstants.PillLength / 2f,
                -halfHeight,
                -PlayerConstants.PillLength / 2f + PlayerConstants.PillRadius * 2f,
                halfHeight);
            path.AddArc(backRect, 90, 180);

            // Pointed nose (right side) using bezier curves
            float backFrontX = -PlayerConstants.PillLength / 2f + PlayerConstants.PillRadius;

            float c1X = backFrontX + PlayerConstants.PillRadius * PlayerConstants.BezierC1XMultiplier;
            float c2X = noseX - PlayerConstants.PillRadius * PlayerConstants.BezierC2XMultiplier;
            float c1Y = -halfHeight * PlayerConstants.BezierC1YMultiplier;
            float c2Y = -halfHeight * PlayerConstants.BezierC2YMultiplier;

            path.CubicTo(c1X, c1Y, c2X, c2Y, noseX, 0f);
            path.CubicTo(c2X, -c2Y, c1X, -c1Y, backFrontX, halfHeight);

            path.Close();
            return path;
        }

        private static SKPath CreateDeathMarkerPath()
        {
            var path = new SKPath();

            path.MoveTo(-PlayerConstants.DeathMarkerLength, PlayerConstants.DeathMarkerLength);
            path.LineTo(PlayerConstants.DeathMarkerLength, -PlayerConstants.DeathMarkerLength);
            path.MoveTo(-PlayerConstants.DeathMarkerLength, -PlayerConstants.DeathMarkerLength);
            path.LineTo(PlayerConstants.DeathMarkerLength, PlayerConstants.DeathMarkerLength);

            return path;
        }

        #endregion

        #region Public Draw Methods

        /// <summary>
        /// Draws a player on the radar map.
        /// </summary>
        public static void Draw(AbstractPlayer player, SKCanvas canvas, EftMapParams mapParams, LocalPlayer localPlayer)
        {
            var point = player.Position.ToMapPos(mapParams.Map).ToZoomedPos(mapParams);
            player.MouseoverPosition = new Vector2(point.X, point.Y);

            if (!player.IsAlive)
            {
                DrawDeathMarker(canvas, point);
                return;
            }

            DrawPlayerPill(player, canvas, localPlayer, point);

            if (player == localPlayer)
                return;

            DrawPlayerInfo(player, canvas, localPlayer, point);
        }

        /// <summary>
        /// Draws mouseover tooltip for a player.
        /// </summary>
        public static void DrawMouseover(AbstractPlayer player, SKCanvas canvas, EftMapParams mapParams, LocalPlayer localPlayer)
        {
            if (player == localPlayer)
                return;

            using var lines = new PooledList<string>();
            var name = App.Config.UI.HideNames && player.IsHuman ? "<Hidden>" : player.Name;

            string health = null;
            if (player is ObservedPlayer observed)
            {
                health = observed.HealthStatus is Enums.ETagStatus.Healthy
                    ? null
                    : $" ({observed.HealthStatus})";
            }

            // Streamer notice
            if (player is ObservedPlayer obs && obs.IsStreaming)
                lines.Add("[LIVE TTV - Double Click]");

            // Alerts
            string alert = player.Alerts?.Trim();
            if (!string.IsNullOrEmpty(alert))
                lines.Add(alert);

            // Player info based on state
            if (player.IsHostileActive)
            {
                lines.Add($"{name}{health} {player.AccountID}".Trim());
                var faction = player.PlayerSide.ToString();
                string g = player.GroupID != -1 ? $" G:{player.GroupID} " : null;
                lines.Add($"{faction}{g}");
            }
            else if (!player.IsAlive)
            {
                lines.Add($"{player.Type}:{name}");
                if (player.GroupID != -1)
                    lines.Add($"G:{player.GroupID} ");
            }
            else if (player.IsAIActive)
            {
                lines.Add(name);
            }

            // Equipment info
            if (player is ObservedPlayer obs2 && obs2.Equipment.Items is IReadOnlyDictionary<string, TarkovMarketItem> equipment)
            {
                lines.Add($"Value: {Utilities.FormatNumberKM(obs2.Equipment.Value)}");
                foreach (var item in equipment.OrderBy(e => e.Key))
                {
                    lines.Add($"{item.Key.Substring(0, 5)}: {item.Value.ShortName}");
                }
            }

            player.Position.ToMapPos(mapParams.Map).ToZoomedPos(mapParams).DrawMouseoverText(canvas, lines.Span);
        }

        #endregion

        #region Private Draw Methods

        private static void DrawPlayerPill(AbstractPlayer player, SKCanvas canvas, LocalPlayer localPlayer, SKPoint point)
        {
            float noseX = PlayerConstants.PillLength / 2f + PlayerConstants.PillRadius * PlayerConstants.PillNoseXMultiplier;

            var paints = PlayerPaints.GetPaints(player);
            if (player != localPlayer && RadarViewModel.MouseoverGroup is int grp && grp == player.GroupID)
                paints.Fill = SKPaints.PaintMouseoverGroup;

            float scale = PlayerConstants.PillBaseScale * App.Config.UI.UIScale;

            canvas.Save();
            canvas.Translate(point.X, point.Y);
            canvas.Scale(scale, scale);
            canvas.RotateDegrees(player.MapRotation);

            SKPaints.ShapeOutline.StrokeWidth = paints.Fill.StrokeWidth * PlayerConstants.OutlineWidthMultiplier;

            // Draw pill shape
            canvas.DrawPath(_playerPill, SKPaints.ShapeOutline);
            canvas.DrawPath(_playerPill, paints.Fill);

            // Calculate aimline length
            int aimlineLength = CalculateAimlineLength(player, localPlayer);

            if (aimlineLength > 0)
            {
                canvas.DrawLine(noseX, 0, noseX + aimlineLength, 0, SKPaints.ShapeOutline);
                canvas.DrawLine(noseX, 0, noseX + aimlineLength, 0, paints.Fill);
            }

            canvas.Restore();
        }

        private static int CalculateAimlineLength(AbstractPlayer player, LocalPlayer localPlayer)
        {
            if (player == localPlayer)
            {
                return App.Config.UI.AimLineLength == 0 
                    ? PlayerConstants.UnlimitedAimlineLength 
                    : App.Config.UI.AimLineLength;
            }

            if (player.IsFriendly && App.Config.UI.TeammateAimlines)
            {
                if (App.Config.UI.TeammateAimlineLength == 0)
                {
                    return App.Config.UI.AimLineLength == 0 
                        ? PlayerConstants.UnlimitedAimlineLength 
                        : App.Config.UI.AimLineLength;
                }
                return App.Config.UI.TeammateAimlineLength;
            }

            if (!player.IsFriendly &&
                !(player.IsAI && !App.Config.UI.AIAimlines) &&
                player.IsFacingTarget(localPlayer, App.Config.UI.MaxDistance))
            {
                // Hostile facing friendly - high alert, extended aimline
                return PlayerConstants.UnlimitedAimlineLength;
            }

            return 0;
        }

        private static void DrawDeathMarker(SKCanvas canvas, SKPoint point)
        {
            float scale = App.Config.UI.UIScale;

            canvas.Save();
            canvas.Translate(point.X, point.Y);
            canvas.Scale(scale, scale);
            canvas.DrawPath(_deathMarker, SKPaints.PaintDeathMarker);
            canvas.Restore();
        }

        private static void DrawPlayerInfo(AbstractPlayer player, SKCanvas canvas, LocalPlayer localPlayer, SKPoint point)
        {
            var height = player.Position.Y - localPlayer.Position.Y;
            var dist = Vector3.Distance(localPlayer.Position, player.Position);

            using var lines = new PooledList<string>();

            // AI wishlist items
            if (player.IsAI && player is ObservedPlayer obs && obs.Equipment?.Items is not null)
            {
                foreach (var itemLabel in GetWishlistItemLabels(obs))
                {
                    lines.Add(itemLabel);
                }
            }

            // Player name and info
            if (!App.Config.UI.HideNames)
            {
                string name = player.IsError ? "ERROR" : player.Name;
                string health = null;
                string level = null;

                if (player is ObservedPlayer observed)
                {
                    health = observed.HealthStatus is Enums.ETagStatus.Healthy
                        ? null
                        : $" ({observed.HealthStatus})";
                    if (observed.Profile?.Level is int levelResult)
                        level = $"L{levelResult}:";
                }

                lines.Add($"{level}{name}{health}");
                lines.Add($"{height:n0},{dist:n0}");
            }
            else
            {
                var line = $"{height:n0},{dist:n0}";
                if (player.IsError)
                    line = "ERROR";
                lines.Add(line);
            }

            DrawPlayerText(player, canvas, point, lines);
        }

        private static void DrawPlayerText(AbstractPlayer player, SKCanvas canvas, SKPoint point, IList<string> lines)
        {
            var paints = PlayerPaints.GetPaints(player);
            if (RadarViewModel.MouseoverGroup is int grp && grp == player.GroupID)
                paints.Text = SKPaints.TextMouseoverGroup;

            point.Offset(PlayerConstants.TextOffsetX * App.Config.UI.UIScale, 0);

            foreach (var line in lines)
            {
                if (string.IsNullOrEmpty(line?.Trim()))
                    continue;

                // Use wishlist color for lines starting with "!!"
                var textPaint = line.StartsWith("!!") ? SKPaints.TextWishlistItem : paints.Text;

                canvas.DrawText(line, point, SKTextAlign.Left, SKFonts.UIRegular, SKPaints.TextOutline);
                canvas.DrawText(line, point, SKTextAlign.Left, SKFonts.UIRegular, textPaint);

                point.Offset(0, SKFonts.UIRegular.Spacing);
            }
        }

        private static IEnumerable<string> GetWishlistItemLabels(ObservedPlayer player)
        {
            if (player.Equipment?.Items is null)
                yield break;

            foreach (var item in player.Equipment.Items.Values)
            {
                if (LocalPlayer.WishlistItems.Contains(item.BsgId))
                {
                    yield return $"!! {item.ShortName}";
                }
            }
        }

        #endregion
    }
}
