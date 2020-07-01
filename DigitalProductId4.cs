using System;
using System.Text;

namespace ProductKeyChecker
{
    /// <summary>
    /// DigitalProductId4 is a structure used in PidGenX function as output. It contains a lot of data describing the key and system where the PID generation took place.
    /// </summary>
    public class DigitalProductId4
    {
        // Based of defined structure here: https://pidgenx.fandom.com/wiki/DigitalProductId4

        /// <summary>
        /// Size of the structure in bytes.
        /// </summary>
        public const uint Size = 1272;
        /// <summary>
        /// Major kernel version of system where generation took place.
        /// </summary>
        public ushort MajorVersion { get; private set; }
        /// <summary>
        /// Minor kernel version of system where generation took place.
        /// </summary>
        public ushort MinorVersion { get; private set; }
        /// <summary>
        /// Null-terminated string containing Advanced Product ID (used for Volume Activations Count checking).
        /// </summary>
        public string AdvancedPid { get; private set; }
        /// <summary>
        /// Null-terminated string containing Activation ID.
        /// </summary>
        public string ActivationId { get; private set; }
        /// <summary>
        /// Null-terminated string containing OEM ID.
        /// </summary>
        public string OemID { get; private set; }
        /// <summary>
        /// Null-terminated string containing Edition Type (name).
        /// </summary>
        public string EditionType { get; private set; }
        /// <summary>
        /// Unknown
        /// </summary>
        public byte IsUpgrade { get; private set; }
        /// <summary>
        /// Unknown
        /// </summary>
        public byte[] Reserved { get; private set; }
        /// <summary>
        /// Unknown
        /// </summary>
        public byte[] CDKey { get; private set; }
        /// <summary>
        /// Unknown
        /// </summary>
        public byte[] CDKey256Hash { get; private set; }
        /// <summary>
        /// Unknown
        /// </summary>
        public byte[] Hash256 { get; private set; }
        /// <summary>
        /// Null-terminated string containing Edition ID.
        /// </summary>
        public string EditionId { get; private set; }
        /// <summary>
        /// Null-terminated string containing Key Type (for example "Retail" or "OEM:SLP", etc.).
        /// </summary>
        public string KeyType { get; private set; }
        /// <summary>
        /// Null-terminated string containing EULA (for example "Retail" or "MSDN", etc.).
        /// </summary>
        public string EULA { get; private set; }

        /// <summary>
        /// Creates a new DigitalProductId4 object with data initialised from a byte array.
        /// </summary>
        /// <param name="data">A byte array of length 1272 (returned from PidGenX function).</param>
        public DigitalProductId4(byte[] data)
        {
            if (data.Length != Size)
                throw new ArgumentOutOfRangeException("data", "data[] must have a length of " + Size + " bytes.");

            MajorVersion = BitConverter.ToUInt16(data, 4);
            MinorVersion = BitConverter.ToUInt16(data, 6);
            AdvancedPid = Encoding.Unicode.GetString(data, 8, 128).TrimEnd('\0');
            ActivationId = Encoding.Unicode.GetString(data, 136, 128).TrimEnd('\0');
            OemID = Encoding.Unicode.GetString(data, 264, 16).TrimEnd('\0');
            EditionType = Encoding.Unicode.GetString(data, 280, 520).TrimEnd('\0');
            IsUpgrade = data[800];
            Reserved = new byte[7];
            Array.Copy(data, 801, Reserved, 0, 7);
            CDKey = new byte[16];
            Array.Copy(data, 808, CDKey, 0, 16);
            CDKey256Hash = new byte[32];
            Array.Copy(data, 824, CDKey256Hash, 0, 32);
            Hash256 = new byte[32];
            Array.Copy(data, 856, Hash256, 0, 32);
            EditionId = Encoding.Unicode.GetString(data, 888, 128).TrimEnd('\0');
            KeyType = Encoding.Unicode.GetString(data, 1016, 128).TrimEnd('\0');
            EULA = Encoding.Unicode.GetString(data, 1144, 128).TrimEnd('\0');
        }
    }
}
