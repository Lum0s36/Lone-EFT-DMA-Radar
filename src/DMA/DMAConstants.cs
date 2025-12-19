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

namespace LoneEftDmaRadar.DMA
{
    /// <summary>
    /// Constants for DMA memory operations and timing.
    /// </summary>
    public static class DMAConstants
    {
        #region Process

        /// <summary>
        /// Target game process name.
        /// </summary>
        public const string GameProcessName = "EscapeFromTarkov.exe";

        /// <summary>
        /// Unity player DLL name.
        /// </summary>
        public const string UnityPlayerDll = "UnityPlayer.dll";

        #endregion

        #region Memory Limits

        /// <summary>
        /// Maximum single read size (in bytes).
        /// </summary>
        public const uint MaxReadSize = 0x1000u * 1500u;

        /// <summary>
        /// Page size for memory alignment.
        /// </summary>
        public const ulong PageSize = 0x1000ul;

        /// <summary>
        /// Maximum string read size.
        /// </summary>
        public const int MaxStringReadSize = 0x1000;

        /// <summary>
        /// Minimum valid virtual address.
        /// </summary>
        public const ulong MinValidVirtualAddress = 0x10000;

        #endregion

        #region Timing

        /// <summary>
        /// Memory partial refresh interval in milliseconds.
        /// </summary>
        public const int MemoryRefreshIntervalMs = 300;

        /// <summary>
        /// TLB partial refresh interval in seconds.
        /// </summary>
        public const int TlbRefreshIntervalSeconds = 2;

        /// <summary>
        /// Game loop sleep time in milliseconds.
        /// </summary>
        public const int GameLoopSleepMs = 133;

        /// <summary>
        /// Startup delay after finding MainWindow in milliseconds.
        /// </summary>
        public const int StartupCheckDelayMs = 1;

        /// <summary>
        /// Process check retry delay in milliseconds.
        /// </summary>
        public const int ProcessCheckRetryDelayMs = 150;

        /// <summary>
        /// Number of retries for process running check.
        /// </summary>
        public const int ProcessCheckRetryCount = 5;

        /// <summary>
        /// Sleep after process stopped in milliseconds.
        /// </summary>
        public const int ProcessStoppedSleepMs = 1000;

        /// <summary>
        /// Sleep after raid stopped in milliseconds.
        /// </summary>
        public const int RaidStoppedSleepMs = 100;

        #endregion

        #region Camera Initialization

        /// <summary>
        /// Initial delay before first camera attempt in seconds.
        /// </summary>
        public const int CameraInitialDelaySeconds = 5;

        /// <summary>
        /// Camera retry interval in seconds.
        /// </summary>
        public const int CameraRetryIntervalSeconds = 3;

        /// <summary>
        /// Camera initialization timeout in seconds.
        /// </summary>
        public const int CameraInitTimeoutSeconds = 15;

        #endregion

        #region Memory Read Verification

        /// <summary>
        /// Spin wait iterations between verification reads.
        /// </summary>
        public const int VerificationSpinWait = 5;

        #endregion

        #region Unicode String

        /// <summary>
        /// Offset for Unicode string data.
        /// </summary>
        public const uint UnicodeStringOffset = 0x14;

        #endregion

        #region Input Manager

        /// <summary>
        /// Input polling interval in milliseconds.
        /// </summary>
        public const int InputPollingIntervalMs = 12;

        /// <summary>
        /// Bit mask for GetAsyncKeyState key down check.
        /// </summary>
        public const int KeyDownBitMask = 0x8000;

        #endregion
    }
}
