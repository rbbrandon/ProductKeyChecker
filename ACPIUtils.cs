using System;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;

namespace ProductKeyChecker
{
    // based on: https://gist.github.com/lwalthert/fe52f7fa98b4ea491345a0518750baa9
    /// <summary>
    /// Contains functions for interacting with the system's firmware.
    /// </summary>
    internal static class ACPIUtils
    {
        /// <summary>
        /// Retrieve an array of ACPI table headers from the local system's firmware.
        /// </summary>
        /// <returns>A string array of ACPI table headers, or an empty string array in the event of failure.</returns>
        private static string[] GetACPITableHeaders()
        {
            string[] acpiTableHeaders = new string[0];

            // Get the size of the array required to read the list of ACPI table headers:
            uint uStructSize = NativeMethods.EnumSystemFirmwareTables(Constants.ACPI, IntPtr.Zero, 0);

            if (uStructSize != 0)
            {
                int iStructSize = Convert.ToInt32(uStructSize);
                IntPtr structPtr = Marshal.AllocHGlobal(iStructSize);

                if (structPtr != null)
                {
                    byte[] buffer = new byte[uStructSize];

                    // Read the list of ACPI table headers and store into memory:
                    NativeMethods.EnumSystemFirmwareTables(Constants.ACPI, structPtr, uStructSize);
                    Marshal.Copy(structPtr, buffer, 0, iStructSize);
                    Marshal.FreeHGlobal(structPtr);

                    // Get ACPI Table Headers as a string:
                    string acpiTableHeadersString = Encoding.ASCII.GetString(buffer);

                    // Split the ACPI Table Header string into a string array of headers (4 characters per header):
                    // Source: https://stackoverflow.com/a/4133751
                    double partSize = 4;
                    int k = 0;
                    var output = acpiTableHeadersString
                        .ToLookup(c => Math.Floor(k++ / partSize))
                        .Select(e => new String(e.ToArray()));

                    acpiTableHeaders = output.ToArray();
                }
            }

            return acpiTableHeaders;
        }

        /// <summary>
        /// Read the embedded (OEM) Windows 8-10 product key from the local system's firmware.
        /// </summary>
        /// <returns>The embedded Windows product key, or an empty string if this function fails.</returns>
        public static string GetOEMKey()
        {
            string key = string.Empty;

            // If there is not an MSDM table in the list of ACPI headers, return an empty string.
            // NOTE: The "MSDM" ("Microsoft Data Management") ACPI table is the table that stores OEM Windows product keys.
            if (!GetACPITableHeaders().Contains("MSDM"))
                return key;

            // Get the size of the array required to read the MSDM ACPI table:
            uint uStructSize = NativeMethods.GetSystemFirmwareTable(Constants.ACPI, Constants.MSDM, IntPtr.Zero, 0);

            if (uStructSize != 0)
            {
                int iStructSize = Convert.ToInt32(uStructSize);
                IntPtr structPtr = Marshal.AllocHGlobal(iStructSize);

                if (structPtr != null)
                {
                    byte[] buffer = new byte[uStructSize];

                    // Read the list of MSDM table and store into memory:
                    NativeMethods.GetSystemFirmwareTable(Constants.ACPI, Constants.MSDM, structPtr, uStructSize);
                    Marshal.Copy(structPtr, buffer, 0, iStructSize);
                    Marshal.FreeHGlobal(structPtr);

                    // Extract the OEM key out of the MSDM table:
                    var msdm = new EfiAcpiMsdm(buffer);
                    key = msdm.ProductKey;
                }
            }

            return key;
        }
    }
}
