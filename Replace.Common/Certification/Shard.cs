using System.Runtime.InteropServices;

namespace Replace.Common.Certification
{
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public class Shard : ICertificationRow
    {
        #region Fields

        [MarshalAs(UnmanagedType.U2)]
        private short _id;

        [MarshalAs(UnmanagedType.U1)]
        private byte _farmID;

        [MarshalAs(UnmanagedType.U1)]
        private byte _contentID;

        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 32)]
        private string _name;

        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 256)]
        public string _dbConfig;

        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 256)]
        private string _logDBConfig;

        [MarshalAs(UnmanagedType.U2)]
        private short _maxUser;

        [MarshalAs(UnmanagedType.U2)]
        private short _shardManagerID;

        [MarshalAs(UnmanagedType.ByValArray, ArraySubType = UnmanagedType.U1, SizeConst = 7)]
        public byte[] unkBuffer; //RUNTIME

        #endregion Fields

        #region Properties

        public short ID { get { return _id; } private set { _id = value; } }
        public byte FarmID { get { return _farmID; } private set { _farmID = value; } }
        public byte ContentID { get { return _contentID; } private set { _contentID = value; } }
        public string Name { get { return _name; } private set { _name = value; } }
        public string DBConfig { get { return _dbConfig; } private set { _dbConfig = value; } }
        public string LogDBConfig { get { return _logDBConfig; } private set { _logDBConfig = value; } }
        public short MaxUser { get { return _maxUser; } private set { _maxUser = value; } }
        public short ShardManagerID { get { return _shardManagerID; } private set { _shardManagerID = value; } }

        #endregion Properties

        #region Methods

        public void UpdateName(string name)
        {
            this.Name = name;
        }

        public void UpdateMaxUser(short maxUser)
        {
            this.MaxUser = maxUser;
        }

        public override string ToString()
        {
            return $"{_id} [{_name}]";
        }

        #endregion Methods
    }
}