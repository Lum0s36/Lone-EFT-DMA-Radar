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

using LoneEftDmaRadar.UI.Skia;

namespace LoneEftDmaRadar.Tarkov.GameWorld.Exits
{
    /// <summary>
    /// Constants and shared rendering logic for exit points (Exfils and Transits).
    /// </summary>
    public static class ExitPointRenderer
    {
        #region Constants

        /// <summary>
        /// Height difference threshold for determining if exit is above/below player.
        /// </summary>
        public const float HeightThreshold = 1.85f;

        /// <summary>
        /// Size of the directional arrow indicator.
        /// </summary>
        public const float ArrowSize = 6.5f;

        /// <summary>
        /// Base size of the circle indicator when exit is level with player.
        /// </summary>
        public const float BaseCircleSize = 4.75f;

        /// <summary>
        /// Outline stroke width for exit markers.
        /// </summary>
        public const float OutlineStrokeWidth = 2f;

        #endregion

        #region Rendering

        /// <summary>
        /// Draws an exit point marker on the radar map.
        /// </summary>
        /// <param name="canvas">The SkiaSharp canvas to draw on.</param>
        /// <param name="point">The screen position to draw at.</param>
        /// <param name="paint">The paint to use for the marker.</param>
        /// <param name="heightDiff">Height difference between exit and player.</param>
        public static void DrawMarker(SKCanvas canvas, SKPoint point, SKPaint paint, float heightDiff)
        {
            SKPaints.ShapeOutline.StrokeWidth = OutlineStrokeWidth;

            if (heightDiff > HeightThreshold)
            {
                // Exit is above player
                DrawUpArrow(canvas, point, paint);
            }
            else if (heightDiff < -HeightThreshold)
            {
                // Exit is below player
                DrawDownArrow(canvas, point, paint);
            }
            else
            {
                // Exit is level with player
                DrawCircle(canvas, point, paint);
            }
        }

        private static void DrawUpArrow(SKCanvas canvas, SKPoint point, SKPaint paint)
        {
            using var path = point.GetUpArrow(ArrowSize);
            canvas.DrawPath(path, SKPaints.ShapeOutline);
            canvas.DrawPath(path, paint);
        }

        private static void DrawDownArrow(SKCanvas canvas, SKPoint point, SKPaint paint)
        {
            using var path = point.GetDownArrow(ArrowSize);
            canvas.DrawPath(path, SKPaints.ShapeOutline);
            canvas.DrawPath(path, paint);
        }

        private static void DrawCircle(SKCanvas canvas, SKPoint point, SKPaint paint)
        {
            float size = BaseCircleSize * App.Config.UI.UIScale;
            canvas.DrawCircle(point, size, SKPaints.ShapeOutline);
            canvas.DrawCircle(point, size, paint);
        }

        #endregion
    }
}
