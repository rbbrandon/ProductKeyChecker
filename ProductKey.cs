using System;
using System.IO;
using System.Runtime.InteropServices;
using System.Xml;

namespace ProductKeyChecker
{
    /// <summary>
    /// A class that is used for handling Windows product keys.
    /// </summary>
    internal class ProductKey
    {
        /// <summary>
        /// The edition of Windows to test for.
        /// </summary>
        public enum Edition
        {
            Win8,
            Win10
        }

        /// <summary>
        /// The Windows product key saved in this object.
        /// </summary>
        public string Key
        {
            get { return _ProductKey; }
            set { _ProductKey = value.ToUpper(); }
        }
        private string _ProductKey;

        /// <summary>
        /// Creates a new ProductKey object.
        /// </summary>
        /// <param name="productKey">The Windows product key (including dashes).</param>
        public ProductKey(string productKey)
        {
            Key = productKey;
        }

        /// <summary>
        /// Get the product description for the key in this ProductKey object.
        /// </summary>
        /// <param name="edition">Windows edition to check the key against to try to retrieve its product description.</param>
        /// <returns>Windows product description for the version of Windows that the stored key is for, or an error message.</returns>
        public string GetProductDescription(Edition edition)
        {
            string result = string.Empty;

            // Initialise array to hold DigitalProductId2 structure.
            byte[] productId2 = new byte[50];
            // Initialise array to hold DigitalProductId3 structure.
            byte[] productId3 = new byte[164];
            // Initialise array to hold DigitalProductId4 structure.
            byte[] productId4 = new byte[1272];

            // Initialise non-calculated values of the above arrays.
            // "uiSize"
            productId2[0] = 50;
            // "uiSize"
            productId3[0] = 164;
            // "uiSize" (both of these byte values together give a uint value of 1272)
            productId4[0] = 248;
            productId4[1] = 4;

            // Allocate memory for the above structures and obtain their pointers.
            IntPtr productId2Ptr = Marshal.AllocHGlobal(50);
            IntPtr productId3Ptr = Marshal.AllocHGlobal(164);
            IntPtr productId4Ptr = Marshal.AllocHGlobal(1272);

            string localAppData = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
            string pkeyconfigFile = string.Empty;
            string pidGenXFilePath = localAppData + (Environment.Is64BitOperatingSystem ? @"\pidgenx64.dll" : @"\pidgenx86.dll");

            try
            {
                // Select the pkeyconfig.xrm-ms file for the Windows edition specified.
                if (edition == Edition.Win10)
                    pkeyconfigFile = localAppData + @"\Win10-pkeyconfig.xrm-ms";
                else if (edition == Edition.Win8)
                    pkeyconfigFile = localAppData + @"\Win8-pkeyconfig.xrm-ms";

                // Write the pkeyconfig.xrm-ms file to disk.
                File.WriteAllText(pkeyconfigFile, ResourceHelper.GetResourceTextFile(Path.GetFileName(pkeyconfigFile)));

                // Copy the DigitalProductId2-4 arrays to the memory addresses for use by the PidGenX function.
                Marshal.Copy(productId2, 0, productId2Ptr, 50);
                Marshal.Copy(productId3, 0, productId3Ptr, 164);
                Marshal.Copy(productId4, 0, productId4Ptr, 1272);
                IntPtr hModule = IntPtr.Zero;

                // Extract the PidGenX.dll file for use.
                ResourceHelper.ExtractResourceBinaryFile(pidGenXFilePath);

                // Load the PidGenX.dll file into memory.
                hModule = NativeMethods.LoadLibrary(pidGenXFilePath);
                if (hModule == IntPtr.Zero)
                {
                    int errorCode = Marshal.GetLastWin32Error();
                    throw new Exception(string.Format("Failed to load library (ErrorCode: {0})", errorCode));
                }

                // Run the PigGenX function that will store its data in the location in memory for productId2Ptr, productId3Ptr, and productId4Ptr.
                uint pidGenXReturnCode = ((NativeMethods.PidGenX)Marshal.GetDelegateForFunctionPointer(NativeMethods.GetProcAddress(hModule, "PidGenX"), typeof(NativeMethods.PidGenX)))(_ProductKey, pkeyconfigFile, Constants.MPC, 0, productId2Ptr, productId3Ptr, productId4Ptr);
                
                if (pidGenXReturnCode == Constants.PGX_OK)
                {
                    // PID generation succeeded.
                    // Copy the data in memory from productId4Ptr into the productId4 array.
                    Marshal.Copy(productId4Ptr, productId4, 0, productId4.Length);

                    // Create a new DigitalProductId4 object from the data copied to productId4 for parsing.
                    DigitalProductId4 dpid4 = new DigitalProductId4(productId4);

                    // Obtain the product description for the PID generation's activation GUID from the pkeyconfig.xrm-ms file.
                    result = GetProductDescription(pkeyconfigFile, "{" + dpid4.ActivationId + "}");
                }
                else if (pidGenXReturnCode == Constants.PGX_MALFORMEDKEY)
                    result = "Invalid Arguments";
                else if (pidGenXReturnCode == Constants.PGX_INVALIDKEY)
                    result = "Invalid Key";
                else if (pidGenXReturnCode == Constants.PGX_PKEYMISSING)
                    result = "pkeyconfig.xrm.ms file not found";
                else
                    result = "Key not supported";

                // Free resources:
                Marshal.FreeHGlobal(productId2Ptr);
                Marshal.FreeHGlobal(productId3Ptr);
                Marshal.FreeHGlobal(productId4Ptr);
                NativeMethods.FreeLibrary(hModule);
            }
            catch (Exception)
            {
                result = "Edition not supported at present";
            }
            finally
            {
                // Cleanup files written to disk:
                if (File.Exists(pidGenXFilePath))
                    File.Delete(pidGenXFilePath);

                if (File.Exists(pkeyconfigFile))
                    File.Delete(pkeyconfigFile);
            }

            return result;
        }

        /// <summary>
        /// Gets a product description for a specified Windows activation GUID from a specified pkeyconfig.xrm-ms file.
        /// </summary>
        /// <param name="pkeyconfigFile">pkeyconfig.xrm-ms full file path.</param>
        /// <param name="activationGuid">Windows activation GUID for the desired product.</param>
        /// <returns>Product description for the version of Windows that the activationGuid belongs to.</returns>
        private static string GetProductDescription(string pkeyconfigFile, string activationGuid)
        {
            XmlDocument xmlDocument = new XmlDocument();
            xmlDocument.Load(pkeyconfigFile);
            Stream inStream = new MemoryStream(Convert.FromBase64String(xmlDocument.GetElementsByTagName("tm:infoBin")[0].InnerText));
            xmlDocument.Load(inStream);
            XmlNamespaceManager xmlNamespaceManager = new XmlNamespaceManager(xmlDocument.NameTable);
            xmlNamespaceManager.AddNamespace("pkc", "http://www.microsoft.com/DRM/PKEY/Configuration/2.0");
            XmlNode xmlNode = xmlDocument.SelectSingleNode("/pkc:ProductKeyConfiguration/pkc:Configurations/pkc:Configuration[pkc:ActConfigId='" + activationGuid + "']", xmlNamespaceManager);
            if (xmlNode == null)
                xmlNode = xmlDocument.SelectSingleNode("/pkc:ProductKeyConfiguration/pkc:Configurations/pkc:Configuration[pkc:ActConfigId='" + activationGuid.ToUpper() + "']", xmlNamespaceManager);

            string result;
            try
            {
                result = xmlNode.ChildNodes.Item(3).InnerText;
            }
            catch (Exception)
            {
                result = "Product Key Configuration invalid";
            }

            return result;
        }
    }
}
