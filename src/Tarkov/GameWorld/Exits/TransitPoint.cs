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

using LoneEftDmaRadar.Tarkov.GameWorld.Player;
using LoneEftDmaRadar.Tarkov.Unity;
using LoneEftDmaRadar.UI.Radar.Maps;
using LoneEftDmaRadar.UI.Skia;

namespace LoneEftDmaRadar.Tarkov.GameWorld.Exits
{
    /// <summary>
    /// Represents a transit point for traveling between maps.
    /// </summary>
    public sealed class TransitPoint : IExitPoint, IWorldEntity, IMapEntity, IMouseoverEntity
    {
        #region Constructor

        public TransitPoint(TarkovDataManager.TransitElement transit)
        {
            Description = transit.Description;
            _position = transit.Position.AsVector3();
        }

        #endregion

        #region Properties

        /// <summary>
        /// Description/name of the transit point.
        /// </summary>
        public string Description { get; }

        private readonly Vector3 _position;
        
        /// <summary>
        /// World position of the transit point.
        /// </summary>
        public ref readonly Vector3 Position => ref _position;
        
        /// <summary>
        /// Screen position for mouseover detection.
        /// </summary>
        public Vector2 MouseoverPosition { get; set; }

        #endregion

        #region Drawing

        /// <summary>
        /// Draws the transit point on the radar map.
        /// </summary>
        public void Draw(SKCanvas canvas, EftMapParams mapParams, LocalPlayer localPlayer)
        {
            var heightDiff = Position.Y - localPlayer.Position.Y;
            var paint = SKPaints.PaintExfilTransit;
            var point = Position.ToMapPos(mapParams.Map).ToZoomedPos(mapParams);
            
            MouseoverPosition = new Vector2(point.X, point.Y);
            ExitPointRenderer.DrawMarker(canvas, point, paint, heightDiff);
        }

        /// <summary>
        /// Draws the mouseover tooltip for this transit point.
        /// </summary>
        public void DrawMouseover(SKCanvas canvas, EftMapParams mapParams, LocalPlayer localPlayer)
        {
            Position.ToMapPos(mapParams.Map).ToZoomedPos(mapParams).DrawMouseoverText(canvas, Description);
        }

        #endregion
    }
}
