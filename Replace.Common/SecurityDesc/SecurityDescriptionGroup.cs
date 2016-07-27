using System.Runtime.InteropServices;

namespace Replace.Common.SecurityDesc
{
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public class SecurityDescriptionGroup : Unmanaged.IMarshalled
    {
        [MarshalAs(UnmanagedType.U1)]
        public byte nID;

        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 32)]
        public string szName;

        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 256)]
        public string szDesc;
    }
}