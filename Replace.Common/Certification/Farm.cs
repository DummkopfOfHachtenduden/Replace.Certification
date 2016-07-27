using System.Diagnostics;
using System.Runtime.InteropServices;

namespace Replace.Common.Certification
{
    [DebuggerDisplay("{_id} - {_name}")]
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public class Farm : ICertificationRow
    {
        #region Fields

        [MarshalAs(UnmanagedType.U1)]
        public byte _id;

        [MarshalAs(UnmanagedType.U1)]
        public byte _divisionID;

        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 32)]
        public string _name;

        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 256)]
        public string _dbConfig;

        #endregion Fields

        #region Properties

        public byte ID { get { return _id; } private set { _id = value; } }
        public byte DivisionID { get { return _divisionID; } private set { _divisionID = value; } }
        public string Name { get { return _name; } private set { _name = value; } }
        public string DBConfig { get { return _dbConfig; } private set { _dbConfig = value; } }

        #endregion Properties

        public override string ToString()
        {
            return $"{_id} [{_name}]";
        }
    }
}