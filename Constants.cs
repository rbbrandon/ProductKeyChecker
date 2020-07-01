namespace ProductKeyChecker
{
    /// <summary>
    /// Contains constants for use within ProductKeyChecker utility.
    /// </summary>
    public static class Constants
    {
        /// <summary>
        /// ACPI firmware table provider signature.
        /// </summary>
        public const uint ACPI = 0x41435049;

        /// <summary>
        /// MSDM (Microsoft Data Management) ACPI table ID.
        /// </summary>
        public const uint MSDM = 0x4d44534d;

        /// <summary>
        /// "Windows-1252" encoding codepage.
        /// </summary>
        public const int WINDOWS_1252 = 0x4e4;

        /// <summary>
        /// "Microsoft Product Code" for use with PID Generation.
        /// </summary>
        public const string MPC = "00000";

        /// <summary>
        /// The key is valid.
        /// </summary>
        public const uint PGX_OK = 0x00000000;

        /// <summary>
        /// Could not find specified pkeyconfig file.
        /// </summary>
        public const uint PGX_PKEYMISSING = 0x80070002;

        /// <summary>
        /// The key is in invalid format.
        /// </summary>
        public const uint PGX_MALFORMEDKEY = 0x80070057;

        /// <summary>
        /// The key is invalid.
        /// </summary>
        public const uint PGX_INVALIDKEY = 0x8A010101;
    }
}
