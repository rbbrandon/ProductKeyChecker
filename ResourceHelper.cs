using System;
using System.IO;
using System.Reflection;

namespace ProductKeyChecker
{
    /// <summary>
    /// Contains methods to work with embedded resources.
    /// </summary>
    public static class ResourceHelper
    {
        private const string RESOURCE_BASE = "ProductKeyChecker.Resources.";


        /// <summary>
        /// Gets the contents of an embedded text resource.
        /// </summary>
        /// <param name="fileName">The name of the embedded file to read.</param>
        /// <returns>A string containing the contents of the specified fileName.</returns>
        public static string GetResourceTextFile(string fileName)
        {
            // Path.GetFileName(path);
            string result = string.Empty;

            Assembly assembly = Assembly.GetExecutingAssembly();
            string resourceName = RESOURCE_BASE + fileName;

            using (Stream stream = assembly.GetManifestResourceStream(resourceName))
            using (StreamReader reader = new StreamReader(stream))
                result = reader.ReadToEnd();

            return result;
        }

        /// <summary>
        /// Extracts an embedded resource file with the same name as the destination file to the specified destination.
        /// </summary>
        /// <param name="destination">The full file name to the extracted file.</param>
        public static void ExtractResourceBinaryFile(string destination)
        {
            string fileName = Path.GetFileName(destination);
            ExtractResourceBinaryFile(destination, fileName);
        }

        /// <summary>
        /// Extracts an embedded resource file to the specified destination.
        /// </summary>
        /// <param name="destination">The full file name to the extracted file.</param>
        /// <param name="resourceFileName">The name of the embedded resource file to extract.</param>
        public static void ExtractResourceBinaryFile(string destination, string resourceFileName)
        {
            Assembly assembly = Assembly.GetExecutingAssembly();
            string resourceName = RESOURCE_BASE + resourceFileName;

            if (File.Exists(destination))
                File.Delete(destination);

            if (!File.Exists(destination))
            {
                Stream manifestResourceStream = assembly.GetManifestResourceStream(resourceName);
                int count = Convert.ToInt32(manifestResourceStream.Length);

                using (BinaryReader binaryReader = new BinaryReader(manifestResourceStream))
                    File.WriteAllBytes(destination, binaryReader.ReadBytes(count));
            }
        }
    }
}
