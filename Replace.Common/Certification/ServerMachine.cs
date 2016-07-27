using System.Diagnostics;
using System.Runtime.InteropServices;

namespace Replace.Common.Certification
{
    [DebuggerDisplay("{_id} - {_name}")]
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public class ServerMachine : ICertificationRow
    {
        #region Fields

        [MarshalAs(UnmanagedType.U4)]
        private int _id;

        [MarshalAs(UnmanagedType.U1)]
        private byte _divisionID;

        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 32)]
        private string _name;

        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 16)]
        private string _publicIP;

        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 16)]
        private string _privateIP;

        [MarshalAs(UnmanagedType.U2)]
        private short _machineManagerID; //RUNTIME

        #endregion Fields

        #region Properties

        public int ID { get { return _id; } private set { _id = value; } }
        public byte DivisionID { get { return _divisionID; } private set { _divisionID = value; } }
        public string Name { get { return _name; } private set { _name = value; } }
        public string PublicIP { get { return _publicIP; } private set { _publicIP = value; } }
        public string PrivateIP { get { return _privateIP; } private set { _privateIP = value; } }
        public short MachineManagerID { get { return _machineManagerID; } set { _machineManagerID = value; } }

        #endregion Properties

        #region Methods

        public string GetIP(ServerCordBindType bindType)
        {
            return (bindType == ServerCordBindType.Public) ? _publicIP : _privateIP;
        }

        #endregion Methods
    }
}