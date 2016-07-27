using System.Diagnostics;
using System.Runtime.InteropServices;

namespace Replace.Common.Certification
{
    [DebuggerDisplay("{_farmID} -> {_contentID}")]
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public class FarmContent : ICertificationRow
    {
        #region Fields

        [MarshalAs(UnmanagedType.U1)]
        private byte _farmID;

        [MarshalAs(UnmanagedType.U1)]
        private byte _contentID;

        [MarshalAs(UnmanagedType.ByValArray, ArraySubType = UnmanagedType.U1, SizeConst = 4)]
        public byte[] unkBuffer; //RUNTIME

        #endregion Fields

        #region Properties

        public byte FarmID { get { return _farmID; } private set { _farmID = value; } }
        public byte ContentID { get { return _contentID; } private set { _contentID = value; } }

        #endregion Properties
    }
}