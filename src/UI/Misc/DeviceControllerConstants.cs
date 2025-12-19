/*
 * Lone EFT DMA Radar
 * MIT License - Copyright (c) 2025 Lone DMA
 */

namespace LoneEftDmaRadar.UI.Misc
{
    /// <summary>
    /// Constants for serial device communication (KMBox/Makcu).
    /// </summary>
    public static class DeviceControllerConstants
    {
        #region Makcu Device Identity

        /// <summary>
        /// Friendly name for Makcu device.
        /// </summary>
        public const string MakcuFriendlyName = "USB-Enhanced-SERIAL CH343";

        /// <summary>
        /// Vendor ID for Makcu device.
        /// </summary>
        public const string MakcuVid = "1A86";

        /// <summary>
        /// Product ID for Makcu device.
        /// </summary>
        public const string MakcuPid = "55D3";

        /// <summary>
        /// Serial fragment for Makcu identification.
        /// </summary>
        public const string MakcuSerialFragment = "58A6074578";

        /// <summary>
        /// Expected signature response from Makcu device.
        /// </summary>
        public const string MakcuExpectSignature = "km.MAKCU";

        /// <summary>
        /// All valid Makcu-compatible signature responses.
        /// </summary>
        public static readonly string[] MakcuSignatures = new[]
        {
            MakcuExpectSignature,
            "km.net",
            "kmbox.net",
            "km.kmboxnet",
            "kmnet"
        };

        #endregion

        #region Generic Serial Identity

        /// <summary>
        /// Generic serial friendly name fallback patterns.
        /// </summary>
        public static readonly string[] GenericFriendlyFallbacks = new[]
        {
            "USB-SERIAL", "CH340", "CH341", "CH9102"
        };

        /// <summary>
        /// Product ID for CH340 serial adapter.
        /// </summary>
        public const string Ch340Pid = "7523";

        /// <summary>
        /// Product ID for CH9102 serial adapter.
        /// </summary>
        public const string Ch9102Pid = "5523";

        /// <summary>
        /// Friendly name for CH340 serial adapter.
        /// </summary>
        public const string Ch340FriendlyName = "USB-SERIAL CH340";

        /// <summary>
        /// Friendly name for CH9102 serial adapter.
        /// </summary>
        public const string Ch9102FriendlyName = "USB-SERIAL CH9102";

        #endregion

        #region Serial Communication

        /// <summary>
        /// Default baud rate for opening connection.
        /// </summary>
        public const int DefaultOpenBaud = 115200;

        /// <summary>
        /// High-speed baud rate for Makcu device.
        /// </summary>
        public const int HighBaud = 4000000;

        /// <summary>
        /// Default read timeout in milliseconds.
        /// </summary>
        public const int DefaultReadTimeout = 500;

        /// <summary>
        /// Default write timeout in milliseconds.
        /// </summary>
        public const int DefaultWriteTimeout = 500;

        /// <summary>
        /// Signature validation timeout in milliseconds.
        /// </summary>
        public const int SignatureValidationTimeout = 800;

        /// <summary>
        /// Delay after opening port before mode switch.
        /// </summary>
        public const int PortOpenDelayMs = 150;

        /// <summary>
        /// Delay after setting baud rate.
        /// </summary>
        public const int BaudChangeDelayMs = 50;

        /// <summary>
        /// Delay before starting listener after connect.
        /// </summary>
        public const int ListenerStartDelayMs = 500;

        /// <summary>
        /// Default milliseconds per smoothing segment.
        /// </summary>
        public const int MakcuSegmentMsDefault = 3;

        #endregion

        #region Change Command

        /// <summary>
        /// Command bytes to switch Makcu to high-speed mode.
        /// </summary>
        public static readonly byte[] ChangeModeCommand = { 0xDE, 0xAD, 0x05, 0x00, 0xA5, 0x00, 0x09, 0x3D, 0x00 };

        #endregion

        #region Button State

        /// <summary>
        /// Number of mouse buttons tracked.
        /// </summary>
        public const int MouseButtonCount = 5;

        /// <summary>
        /// Valid button state bytes.
        /// </summary>
        public static readonly HashSet<byte> ValidButtonBytes = new HashSet<byte>
        {
            0x00, 0x01, 0x02, 0x03, 0x04, 0x05, 0x06, 0x07,
            0x08, 0x09, 0x10, 0x11, 0x12, 0x13, 0x14, 0x15,
            0x16, 0x17, 0x19, 0x1F
        };

        #endregion

        #region Click Randomization

        /// <summary>
        /// Minimum random press time in milliseconds.
        /// </summary>
        public const int MinRandomPressTimeMs = 10;

        /// <summary>
        /// Maximum random press time in milliseconds.
        /// </summary>
        public const int MaxRandomPressTimeMs = 100;

        #endregion

        #region Reconnect

        /// <summary>
        /// Delay before reconnect attempt in milliseconds.
        /// </summary>
        public const int ReconnectDelayMs = 200;

        /// <summary>
        /// Delay when connection is lost before retry.
        /// </summary>
        public const int ConnectionLostRetryDelayMs = 250;

        /// <summary>
        /// Small delay after exception in button reader.
        /// </summary>
        public const int ButtonReaderExceptionDelayMs = 50;

        #endregion
    }
}
