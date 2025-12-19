/*
 * Lone EFT DMA Radar
 * Brought to you by Lone (Lone DMA)
 * 
 * Exfil Status Tracking: Credit to Keegi
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
using SDK;
using System.Collections.Frozen;
using System.ComponentModel;

namespace LoneEftDmaRadar.Tarkov.GameWorld.Exits
{
    /// <summary>
    /// Represents an exfiltration point in the game world.
    /// Supports both static data and live memory updates for status tracking.
    /// </summary>
    public class Exfil : IExitPoint, IWorldEntity, IMapEntity, IMouseoverEntity
    {
        #region Constructors

        /// <summary>
        /// Creates an Exfil from static map data (fallback when no memory data available).
        /// </summary>
        public Exfil(TarkovDataManager.ExtractElement extract)
        {
            Name = extract.Name;
            _position = extract.Position.AsVector3();
            ExfilBase = 0;
        }

        /// <summary>
        /// Creates an Exfil from game memory with live status updates.
        /// </summary>
        public Exfil(ulong exfilAddr, string exfilName, string mapId, bool isPmc, Vector3 position)
        {
            ExfilBase = exfilAddr;
            _position = position;
            Name = ExfilNameLookup.GetFriendlyName(mapId, exfilName);
        }

        #endregion

        #region Properties

        /// <summary>
        /// Memory address of the exfil point (0 if from static data).
        /// </summary>
        public ulong ExfilBase { get; init; }

        /// <summary>
        /// Display name of the exfil point.
        /// </summary>
        public string Name { get; }

        /// <summary>
        /// Current status of the exfil (Open, Pending, or Closed).
        /// </summary>
        public EStatus Status { get; private set; } = EStatus.Open;

        private readonly Vector3 _position;
        
        /// <summary>
        /// World position of the exfil point.
        /// </summary>
        public ref readonly Vector3 Position => ref _position;
        
        /// <summary>
        /// Screen position for mouseover detection.
        /// </summary>
        public Vector2 MouseoverPosition { get; set; }

        #endregion

        #region Status Update

        /// <summary>
        /// Updates the exfil status from game memory.
        /// </summary>
        public void Update(Enums.EExfiltrationStatus status)
        {
            Status = status switch
            {
                Enums.EExfiltrationStatus.NotPresent => EStatus.Closed,
                Enums.EExfiltrationStatus.Hidden => EStatus.Closed,
                Enums.EExfiltrationStatus.UncompleteRequirements => EStatus.Pending,
                Enums.EExfiltrationStatus.Pending => EStatus.Pending,
                Enums.EExfiltrationStatus.AwaitsManualActivation => EStatus.Pending,
                Enums.EExfiltrationStatus.Countdown => EStatus.Open,
                Enums.EExfiltrationStatus.RegularMode => EStatus.Open,
                _ => EStatus.Closed // Default to Closed for unknown status values
            };
        }

        /// <summary>
        /// Updates the exfil status directly from raw int value.
        /// </summary>
        public void UpdateFromRaw(int rawStatus)
        {
            // Handle raw status value - EExfiltrationStatus is 1-indexed
            Status = rawStatus switch
            {
                1 => EStatus.Closed,  // NotPresent
                2 => EStatus.Pending, // UncompleteRequirements
                3 => EStatus.Open,    // Countdown
                4 => EStatus.Open,    // RegularMode
                5 => EStatus.Pending, // Pending
                6 => EStatus.Pending, // AwaitsManualActivation
                7 => EStatus.Closed,  // Hidden
                _ => EStatus.Closed   // Unknown - default to Closed
            };
        }

        #endregion

        #region Drawing

        /// <summary>
        /// Draws the exfil point on the radar map.
        /// </summary>
        public void Draw(SKCanvas canvas, EftMapParams mapParams, LocalPlayer localPlayer)
        {
            if (Status == EStatus.Closed)
                return;

            var heightDiff = Position.Y - localPlayer.Position.Y;
            var paint = GetStatusPaint();
            var point = Position.ToMapPos(mapParams.Map).ToZoomedPos(mapParams);
            
            MouseoverPosition = new Vector2(point.X, point.Y);
            ExitPointRenderer.DrawMarker(canvas, point, paint, heightDiff);
        }

        private SKPaint GetStatusPaint()
        {
            return Status switch
            {
                EStatus.Open => SKPaints.PaintExfilOpen,
                EStatus.Pending => SKPaints.PaintExfilPending,
                EStatus.Closed => SKPaints.PaintExfilClosed,
                _ => SKPaints.PaintExfilOpen
            };
        }

        /// <summary>
        /// Draws the mouseover tooltip for this exfil.
        /// </summary>
        public void DrawMouseover(SKCanvas canvas, EftMapParams mapParams, LocalPlayer localPlayer)
        {
            var exfilName = Name ?? "unknown";
            var statusText = GetStatusDisplayText();
            var text = $"{exfilName} ({statusText})";
            
            Position.ToMapPos(mapParams.Map).ToZoomedPos(mapParams).DrawMouseoverText(canvas, text);
        }

        private string GetStatusDisplayText()
        {
            return Status switch
            {
                EStatus.Open => "Open",
                EStatus.Pending => "Pending",
                EStatus.Closed => "Closed",
                _ => "Unknown"
            };
        }

        #endregion

        #region Status Enum

        /// <summary>
        /// Exfil status enumeration.
        /// </summary>
        public enum EStatus
        {
            [Description(nameof(Open))] Open,
            [Description(nameof(Pending))] Pending,
            [Description(nameof(Closed))] Closed
        }

        #endregion

        #region Legacy Compatibility

        /// <summary>
        /// Provides access to exfil name lookup for backward compatibility.
        /// Use ExfilNameLookup.MapExfilNames directly for new code.
        /// </summary>
        public static FrozenDictionary<string, FrozenDictionary<string, string>> ExfilNames => 
            ExfilNameLookup.MapExfilNames;

        #endregion
    }
}
