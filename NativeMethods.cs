using System;
using System.Runtime.InteropServices;

namespace ProductKeyChecker
{
    /// <summary>
    /// Contains methods imported from external (Microsoft) DLL's.
    /// </summary>
    internal static class NativeMethods
    {
        /// <summary>
        /// Enumerates all system firmware tables of the specified type.
        /// </summary>
        /// <param name="FirmwareTableProviderSignature">The identifier of the firmware table provider to which the query is to be directed.</param>
        /// <param name="pFirmwareTableBuffer">A pointer to a buffer that receives the list of firmware tables. If this parameter is IntPtr.Zero, the return value is the required buffer size.</param>
        /// <param name="BufferSize">The size of the pFirmwareTableBuffer buffer, in bytes.</param>
        /// <returns>If the function succeeds, the return value is the number of bytes written to the buffer. If the function fails because the buffer is not large enough, the return value is the required buffer size, in bytes.</returns>
        [DllImport("kernel32.dll")]
        public static extern uint EnumSystemFirmwareTables(uint FirmwareTableProviderSignature, IntPtr pFirmwareTableBuffer, uint BufferSize);

        /// <summary>
        /// Retrieves the specified firmware table from the firmware table provider.
        /// </summary>
        /// <param name="FirmwareTableProviderSignature">The identifier of the firmware table provider to which the query is to be directed.</param>
        /// <param name="FirmwareTableID">The identifier of the firmware table.</param>
        /// <param name="pFirmwareTableBuffer">A pointer to a buffer that receives the requested firmware table. If this parameter is IntPtr.Zero, the return value is the required buffer size.</param>
        /// <param name="BufferSize">The size of the pFirmwareTableBuffer buffer, in bytes.</param>
        /// <returns>If the function succeeds, the return value is the number of bytes written to the buffer. If the function fails because the buffer is not large enough, the return value is the required buffer size, in bytes.</returns>
        [DllImport("kernel32.dll")]
        public static extern uint GetSystemFirmwareTable(uint FirmwareTableProviderSignature, uint FirmwareTableID, IntPtr pFirmwareTableBuffer, uint BufferSize);

        /// <summary>
        /// Loads the specified module into the address space of the calling process. The specified module may cause other modules to be loaded.
        /// </summary>
        /// <param name="libname">The name of the module. This can be either a library module (a .dll file) or an executable module (an .exe file).</param>
        /// <returns>If the function succeeds, the return value is a handle to the module. If the function fails, the return value is IntPtr.Zero. To get extended error information, call GetLastError.</returns>
        [DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        public static extern IntPtr LoadLibrary(string libname);

        /// <summary>
        /// Frees the loaded dynamic-link library (DLL) module and, if necessary, decrements its reference count. When the reference count reaches zero, the module is unloaded from the address space of the calling process and the handle is no longer valid.
        /// </summary>
        /// <param name="hLibModule">A handle to the loaded library module. The LoadLibrary, LoadLibraryEx, GetModuleHandle, or GetModuleHandleEx function returns this handle.</param>
        /// <returns>If the function succeeds, the return value is true. If the function fails, the return value is false. To get extended error information, call the GetLastError function.</returns>
        [DllImport("kernel32.dll", CharSet = CharSet.Auto)]
        public static extern bool FreeLibrary(IntPtr hLibModule);

        /// <summary>
        /// Retrieves the address of an exported function or variable from the specified dynamic-link library (DLL).
        /// </summary>
        /// <param name="hModule">A handle to the DLL module that contains the function or variable. The LoadLibrary, LoadLibraryEx, LoadPackagedLibrary, or GetModuleHandle function returns this handle.</param>
        /// <param name="lpProcName">The function or variable name.</param>
        /// <returns>If the function succeeds, the return value is the address of the exported function or variable. If the function fails, the return value is IntPtr.Zero. To get extended error information, call GetLastError.</returns>
        [DllImport("kernel32.dll")]
        public static extern IntPtr GetProcAddress(IntPtr hModule, string lpProcName);

        /// <summary>
        /// This function generates informations about specified product key based on specified config file.
        /// </summary>
        /// <param name="productKey">A null-terminated unicode string containing upper case Product Key with dashes.</param>
        /// <param name="configFile">A null-terminated unicode string containing the full path to a pkeyconfig.xrm-ms file.</param>
        /// <param name="mpc">A null-terminated unicode string containing first **five** characters for Product ID and Advanced Product ID.</param>
        /// <param name="oemId">Unknown parameter. Please use 0.</param>
        /// <param name="productId2">Pointer to DigitalProductId2 structure containing Product ID.</param>
        /// <param name="productId3">Pointer to DigitalProductId3 structure.</param>
        /// <param name="productId4">Pointer to DigitalProductId4 structure.</param>
        /// <returns>Function returns the result of PID generating process.</returns>
        [DllImport("pidgenx.dll", CharSet = CharSet.Unicode, EntryPoint = "PidGenX")]
        public static extern uint PidGenx(string productKey, string configFile, string mpc, int oemId, IntPtr productId2, IntPtr productId3, IntPtr productId4);

        /// <summary>
        /// This function generates informations about specified product key based on specified config file.
        /// </summary>
        /// <param name="productKey">A string containing upper case Product Key with dashes.</param>
        /// <param name="configFile">A string containing the full path to a pkeyconfig.xrm-ms file.</param>
        /// <param name="mpc">A string containing first **five** characters for Product ID and Advanced Product ID.</param>
        /// <param name="oemId">Unknown parameter. Please use 0.</param>
        /// <param name="productId2">Pointer to DigitalProductId2 structure containing Product ID.</param>
        /// <param name="productId3">Pointer to DigitalProductId3 structure.</param>
        /// <param name="productId4">Pointer to DigitalProductId4 structure.</param>
        /// <returns>Function returns the result of PID generating process.</returns>
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        public delegate uint PidGenX([MarshalAs(UnmanagedType.LPWStr)] string productKey, [MarshalAs(UnmanagedType.LPWStr)] string configFile, [MarshalAs(UnmanagedType.LPWStr)] string mpc, int oemId, IntPtr productId2, IntPtr productId3, IntPtr productId4);
    }
}
