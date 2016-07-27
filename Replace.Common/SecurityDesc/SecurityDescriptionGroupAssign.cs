using System.Runtime.InteropServices;

namespace Replace.Common.SecurityDesc
{
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct SecurityDescriptionGroupAssign : Unmanaged.IMarshalled
    {
        [MarshalAs(UnmanagedType.U1)]
        public byte nGroupID;

        [MarshalAs(UnmanagedType.U4)]
        public uint nDescriptionID;
    }
}