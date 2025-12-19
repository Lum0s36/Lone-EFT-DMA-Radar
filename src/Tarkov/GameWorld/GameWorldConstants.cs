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

namespace LoneEftDmaRadar.Tarkov.GameWorld
{
    /// <summary>
    /// Constants for LocalGameWorld worker threads, timing, and configuration.
    /// Centralizes all magic numbers and hardcoded values for easier maintenance.
    /// </summary>
    public static class GameWorldConstants
    {
        #region Worker Thread Configuration

        /// <summary>
        /// Realtime worker interval (~125Hz) - Used for player position updates.
        /// </summary>
        public static readonly TimeSpan RealtimeWorkerInterval = TimeSpan.FromMilliseconds(8);

        /// <summary>
        /// Slow worker interval (~20Hz) - Used for loot, equipment, quests, exfils.
        /// </summary>
        public static readonly TimeSpan SlowWorkerInterval = TimeSpan.FromMilliseconds(50);

        /// <summary>
        /// Explosives worker interval (~60Hz) - Used for grenade/tripwire tracking.
        /// </summary>
        public static readonly TimeSpan ExplosivesWorkerInterval = TimeSpan.FromMilliseconds(16);

        /// <summary>
        /// MemWrites worker interval (~10Hz) - Used for memory write features.
        /// </summary>
        public static readonly TimeSpan MemWritesWorkerInterval = TimeSpan.FromMilliseconds(100);

        #endregion

        #region Raid Validation

        /// <summary>
        /// Number of retry attempts when checking if raid has ended.
        /// </summary>
        public const int RaidEndCheckRetries = 5;

        /// <summary>
        /// Delay between raid end check retries in milliseconds.
        /// </summary>
        public const int RaidEndCheckDelayMs = 10;

        /// <summary>
        /// Polling interval when waiting for raid to start.
        /// </summary>
        public static readonly TimeSpan RaidPollingInterval = TimeSpan.FromMilliseconds(1000);

        #endregion

        #region Map Configuration

        /// <summary>
        /// Maps that support the BTR vehicle.
        /// </summary>
        public static readonly string[] BtrSupportedMaps = new[]
        {
            "tarkovstreets",
            "woods"
        };

        /// <summary>
        /// Checks if the specified map supports BTR vehicles.
        /// </summary>
        /// <param name="mapId">The map identifier to check.</param>
        /// <returns>True if BTR is supported on this map.</returns>
        public static bool IsBtrSupportedMap(string mapId)
        {
            if (string.IsNullOrEmpty(mapId))
                return false;

            foreach (var map in BtrSupportedMaps)
            {
                if (mapId.Equals(map, StringComparison.OrdinalIgnoreCase))
                    return true;
            }
            return false;
        }

        #endregion

        #region Worker Thread Names

        public const string RealtimeWorkerName = "Realtime Worker";
        public const string SlowWorkerName = "Slow Worker";
        public const string ExplosivesWorkerName = "Explosives Worker";
        public const string MemWritesWorkerName = "MemWrites Worker";

        #endregion

        #region Player Registration

        /// <summary>
        /// Maximum expected player count for sanity check.
        /// </summary>
        public const int MaxPlayerCount = 256;

        #endregion
    }
}
