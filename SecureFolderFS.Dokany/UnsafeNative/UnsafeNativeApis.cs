using System.Runtime.InteropServices;

namespace SecureFolderFS.Dokany.UnsafeNative
{
    internal static class UnsafeNativeApis
    {
        [DllImport("dokan1.dll", ExactSpelling = true)]
        [return: MarshalAs(UnmanagedType.I8)]
        public static extern long DokanNtStatusFromWin32([In][MarshalAs(UnmanagedType.U4)] uint Error);

        [DllImport("dokan1.dll", ExactSpelling = true)] // TODO: When v2 releases, change to dokan2.dll
        [return: MarshalAs(UnmanagedType.U8)]
        public static extern ulong DokanVersion();

        [DllImport("dokan1.dll", ExactSpelling = true)]
        [return: MarshalAs(UnmanagedType.U8)]
        public static extern ulong DokanDriverVersion();
    }
}
