using System.Runtime.InteropServices;

namespace Replace.Common.Certification
{
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public class ServerBody : ICertificationRow
    {
        #region Fields

        [MarshalAs(UnmanagedType.U2)]
        private short _id;

        [MarshalAs(UnmanagedType.U1)]
        private byte _divisionID;

        [MarshalAs(UnmanagedType.U1)]
        private byte _farmID;

        [MarshalAs(UnmanagedType.U2)]
        private short _shardID;

        [MarshalAs(UnmanagedType.U4)]
        private int _machineID;

        [MarshalAs(UnmanagedType.U1)]
        private byte _moduleID;

        [MarshalAs(UnmanagedType.U1)]
        private byte _moduleType;

        [MarshalAs(UnmanagedType.U2)]
        private short _certifierID;

        [MarshalAs(UnmanagedType.U2)]
        private short _bindPort;

        [MarshalAs(UnmanagedType.U4)]
        private ServerBodyState _state; //RUNTIME

        [MarshalAs(UnmanagedType.ByValArray, ArraySubType = UnmanagedType.U1, SizeConst = 20)]
        public byte[] unkBuffer; //RUNTIME

        #endregion Fields

        #region Properties

        public short ID { get { return _id; } private set { _id = value; } }
        public byte DivisionID { get { return _divisionID; } private set { _divisionID = value; } }
        public byte FarmID { get { return _farmID; } private set { _farmID = value; } }
        public short ShardID { get { return _shardID; } private set { _shardID = value; } }
        public int MachineID { get { return _machineID; } private set { _machineID = value; } }
        public byte ModuleID { get { return _moduleID; } private set { _moduleID = value; } }
        public byte ModuleType { get { return _moduleType; } private set { _moduleType = value; } }
        public short CertifierID { get { return _certifierID; } private set { _certifierID = value; } }
        public short BindPort { get { return _bindPort; } private set { _bindPort = value; } }
        public ServerBodyState State { get { return _state; } set { _state = value; } }

        #endregion Properties
    }
}