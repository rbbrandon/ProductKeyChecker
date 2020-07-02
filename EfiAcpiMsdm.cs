using System;
using System.Text;

namespace ProductKeyChecker
{
	/// <summary>
	/// Class that defines the EFI ACPI MSDM table structure.
	/// </summary>
	public class EfiAcpiMsdm
	{
		/// <summary>
		/// EFI ACPI SDT Header information.
		/// </summary>
		public EfiAcpiSdtHeader Header;

		/// <summary>
		/// Software Licencing Version.
		/// </summary>
		public uint SlsVersion;

		/// <summary>
		/// Software Licencing Reserved.
		/// </summary>
		public uint SlsReserved;

		/// <summary>
		/// Software Licencing Data Type.
		/// </summary>
		public uint SlsDataType;

		/// <summary>
		/// Software Licencing Data Reserved.
		/// </summary>
		public uint SlsDataReserved;

		/// <summary>
		/// The number of characters in the ProductKey.
		/// </summary>
		public uint SlsDataLength;

		/// <summary>
		/// The embedded OEM Windows licence key.
		/// </summary>
		public string ProductKey;    // 30 characters

		/// <summary>
		/// Create a new EfiAcpiMsdm object, and initialise the data to that contained in a byte array.
		/// </summary>
		public EfiAcpiMsdm(byte[] data)
        {
			if (data.Length < 85)
				throw new ArgumentOutOfRangeException("data", "data[] is not long enough to contain the EFI ACPI MSDM table.");

			Encoding encoding = Encoding.GetEncoding(Constants.WINDOWS_1252);

			Header          = new EfiAcpiSdtHeader(data);
			SlsVersion      = BitConverter.ToUInt32(data, 36);
			SlsReserved     = BitConverter.ToUInt32(data, 40);
			SlsDataType     = BitConverter.ToUInt32(data, 44);
			SlsDataReserved = BitConverter.ToUInt32(data, 48);
			SlsDataLength   = BitConverter.ToUInt32(data, 52);
			ProductKey      = encoding.GetString(data, 56, (int)SlsDataLength);
		}
	}
}
