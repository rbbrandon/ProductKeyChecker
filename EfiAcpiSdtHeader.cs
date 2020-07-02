using System;
using System.Text;

namespace ProductKeyChecker
{
	/// <summary>
	/// Class that defines the EFI ACPI SDT Header structure.
	/// </summary>
	public class EfiAcpiSdtHeader
    {
		/// <summary>
		/// ACPI table header signature. e.g. "MSDM".
		/// </summary>
		public string Signature;  // 4 characters

		/// <summary>
		/// Length, in bytes, of the entire table.
		/// </summary>
		public uint Length;

		/// <summary>
		/// 0x01
		/// </summary>
		public byte Revision;

		/// <summary>
		/// Checksum of the entire table.
		/// </summary>
		public byte Checksum;

		/// <summary>
		/// An OEM-supplied string that identifies the OEM.
		/// </summary>
		public string OemId;      // 6 characters

		/// <summary>
		/// optional motherboard/BIOS logical identifier.
		/// </summary>
		public string OemTableId; // 8 characters

		/// <summary>
		/// OEM revision number of the table for the supplied OEM Table ID.
		/// </summary>
		public uint OemRevision;

		/// <summary>
		/// Vendor ID of the utility that created the table.
		/// </summary>
		public string CreatorId;  // 4 characters

		/// <summary>
		/// Revision of the utility that created the table.
		/// </summary>
		public uint CreatorRevision;

		/// <summary>
		/// Create a new EfiAcpiSdtHeader object, and initialise the data to that contained in a byte array.
		/// </summary>
		public EfiAcpiSdtHeader(byte[] data)
        {
			if (data.Length < 36)
				throw new ArgumentOutOfRangeException("data", "data[] is not long enough to contain the EFI ACPI STD HEADER information.");

			Encoding encoding = Encoding.GetEncoding(Constants.WINDOWS_1252);

			Signature       = encoding.GetString(data, 0, 4);
			Length          = BitConverter.ToUInt32(data, 4);
			Revision        = data[8];
			Checksum        = data[9];
			OemId           = encoding.GetString(data, 10, 6);
			OemTableId      = encoding.GetString(data, 16, 8);
			OemRevision     = BitConverter.ToUInt32(data, 24);
			CreatorId       = encoding.GetString(data, 28, 4);
			CreatorRevision = BitConverter.ToUInt32(data, 32);
		}
	}
}
