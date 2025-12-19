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

using LoneEftDmaRadar.Misc;
using LoneEftDmaRadar.Tarkov.GameWorld.Player;
using LoneEftDmaRadar.Tarkov.Unity;
using LoneEftDmaRadar.UI.Radar.Maps;
using LoneEftDmaRadar.UI.Skia;
using SkiaSharp;
using System.Text.Json.Serialization;

namespace LoneEftDmaRadar.Tarkov.GameWorld.Hazards
{
    /// <summary>
    /// Generic implementation of a world hazard loaded from TarkovDev data.
    /// </summary>
    public class GenericWorldHazard : IWorldHazard
    {
        #region Fields

        private Vector3 _position;

        #endregion

        #region Properties

        /// <summary>
        /// Type/description of the hazard.
        /// </summary>
        [JsonPropertyName("hazardType")]
        public string HazardType { get; set; }

        /// <summary>
        /// Position of the hazard in world coordinates.
        /// </summary>
        [JsonPropertyName("position")]
        public Vector3 Position 
        { 
            get => _position; 
            set => _position = value; 
        }

        /// <summary>
        /// Cached position for mouseover detection.
        /// </summary>
        [JsonIgnore]
        public Vector2 MouseoverPosition { get; set; }

        /// <summary>
        /// IWorldEntity Position implementation.
        /// </summary>
        [JsonIgnore]
        ref readonly Vector3 IWorldEntity.Position => ref _position;

        #endregion

        #region Drawing

        /// <summary>
        /// Draws the hazard marker on the radar.
        /// </summary>
        public void Draw(SKCanvas canvas, EftMapParams mapParams, LocalPlayer localPlayer)
        {
            if (!App.Config.UI.ShowHazards)
                return;

            var hazardZoomedPos = Position.ToMapPos(mapParams.Map).ToZoomedPos(mapParams);
            MouseoverPosition = hazardZoomedPos.AsVector2();
            hazardZoomedPos.DrawHazardMarker(canvas);
        }

        /// <summary>
        /// Draws the hazard info when moused over.
        /// </summary>
        public void DrawMouseover(SKCanvas canvas, EftMapParams mapParams, LocalPlayer localPlayer)
        {
            var hazardName = string.IsNullOrEmpty(HazardType) ? "Unknown" : HazardType;
            Position.ToMapPos(mapParams.Map).ToZoomedPos(mapParams)
                .DrawMouseoverText(canvas, $"Hazard: {hazardName}");
        }

        #endregion
    }
}
