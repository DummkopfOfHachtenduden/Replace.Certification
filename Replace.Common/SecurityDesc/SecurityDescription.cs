using System.Runtime.InteropServices;

namespace Replace.Common.SecurityDesc
{
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public class SecurityDescription : Unmanaged.IMarshalled
    {
        [MarshalAs(UnmanagedType.U4)]
        public uint nID;

        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 64)]
        public string szName;

        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 256)]
        public string szDesc;
    }
}