using System.Diagnostics;
using System.Runtime.InteropServices;

namespace Replace.Common.Certification
{
    [DebuggerDisplay("{_id} - {_name}")]
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public class Content : ICertificationRow
    {
        #region Fields

        [MarshalAs(UnmanagedType.U1)]
        private byte _id;

        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 64)]
        private string _name;

        #endregion Fields

        #region Properties

        public byte ID { get { return _id; } private set { _id = value; } }
        public string Name { get { return _name; } private set { _name = value; } }

        #endregion Properties

        public override string ToString()
        {
            return $"{_id} [{_name}]";
        }
    }
}