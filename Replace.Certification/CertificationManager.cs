using NLog;
using Replace.Certification.Config;
using Replace.Common;
using Replace.Common.Certification;
using Replace.Common.Database;
using Replace.Common.Security;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;

namespace Replace.Certification
{
    public class CertificationManager
    {
        private static Logger Logger = LogManager.GetLogger(nameof(CertificationManager));

        #region Fields

        private CertificationConfig _config;

        private SqlDatabase _database;

        private List<Content> _contentList;
        private List<Division> _divisionList;
        private List<Farm> _farmList;
        private List<FarmContent> _farmContentList;
        private List<Shard> _shardList;
        private List<ServerMachine> _serverMachineList;
        private List<ServerBody> _serverBodyList;
        private List<ServerCord> _serverCordList;
        private List<Module> _moduleList;

        #endregion Fields

        #region Properties

        public CertificationConfig Config => _config;

        public IReadOnlyList<Module> ModuleList => _moduleList;
        public IReadOnlyList<Content> ContentList => _contentList;
        public IReadOnlyList<Division> DivisionList => _divisionList;
        public IReadOnlyList<Farm> FarmList => _farmList;
        public IReadOnlyList<FarmContent> FarmContentList => _farmContentList;
        public IReadOnlyList<Shard> ShardList => _shardList;
        public IReadOnlyList<ServerMachine> ServerMachineList => _serverMachineList;
        public IReadOnlyList<ServerBody> ServerBodyList => _serverBodyList;
        public IReadOnlyList<ServerCord> ServerCordList => _serverCordList;

        public Module CertificationModule { get; set; }
        public ServerMachine CertificationMachine { get; set; }
        public ServerBody CertificationBody { get; set; }
        public IEnumerable<ServerCord> CertificationCords { get; set; }

        #endregion Properties

        #region Constructor

        public CertificationManager(CertificationConfig config)
        {
            _config = config;
            _database = new SqlDatabase();

            _moduleList = new List<Module>();
            _contentList = new List<Content>();
            _divisionList = new List<Division>();
            _farmList = new List<Farm>();
            _farmContentList = new List<FarmContent>();
            _shardList = new List<Shard>();
            _serverMachineList = new List<ServerMachine>();
            _serverBodyList = new List<ServerBody>();
            _serverCordList = new List<ServerCord>();
        }

        #endregion Constructor

        #region Load Methods

        public void Load(string certificationModuleName)
        {
            _database.Open(_config.CertificationConnectionString);

            SqlDataReader reader;

            if (_database.Execute("SELECT ID, Name FROM Module", out reader))
                this.Load(reader, _moduleList);

            if (_database.Execute("SELECT ID, Name FROM Content", out reader))
                this.Load(reader, _contentList);

            if (_database.Execute("SELECT ID, Name, DBConfig FROM Division", out reader))
                this.Load(reader, _divisionList);

            if (_database.Execute("SELECT ID, DivisionID, Name, DBConfig FROM Farm", out reader))
                this.Load(reader, _farmList);

            if (_database.Execute("SELECT FarmID, ContentID FROM FarmContent", out reader))
                this.Load(reader, _farmContentList);

            if (_database.Execute("SELECT ID, FarmID, ContentID, Name, DBConfig, LogDBConfig, MaxUser, ShardManagerID FROM Shard", out reader))
                this.Load(reader, _shardList);

            if (_database.Execute("SELECT ID, DivisionID, Name, PublicIP, PrivateIP FROM ServerMachine", out reader))
                this.Load(reader, _serverMachineList);

            if (_database.Execute("SELECT ID, DivisionID, FarmID, ShardID, MachineID, ModuleID, ModuleType, CertifierID, BindPort FROM ServerBody", out reader))
                this.Load(reader, _serverBodyList);

            if (_database.Execute("SELECT ID, ChildID, ParentID, BindType FROM ServerCord", out reader))
                this.Load(reader, _serverCordList);

            //Validation
            this.ValidateList(_moduleList, nameof(_moduleList));
            this.ValidateList(_contentList, nameof(_contentList));
            this.ValidateList(_divisionList, nameof(_divisionList));
            this.ValidateList(_farmList, nameof(_farmList));
            this.ValidateList(_farmContentList, nameof(_farmContentList));
            this.ValidateList(_shardList, nameof(_shardList));
            this.ValidateList(_serverMachineList, nameof(_serverMachineList));
            this.ValidateList(_serverBodyList, nameof(_serverBodyList));
            this.ValidateList(_serverCordList, nameof(_serverCordList));

            //GlobalManager relations.
            var globalManagerModule = _moduleList.Single(p => p.Name == "GlobalManager");
            foreach (var globalManagerBody in _serverBodyList.Where(p => p.ModuleID == globalManagerModule.ID))
            {
                _divisionList.Single(p => p.ID == globalManagerBody.DivisionID).GlobalManagerID = globalManagerBody.ID;
            }

            //MachineManager relations.
            var machineManagerModule = _moduleList.Single(p => p.Name == "MachineManager");
            foreach (var machineManagerBody in _serverBodyList.Where(p => p.ModuleID == machineManagerModule.ID))
            {
                _serverMachineList.Single(p => p.ID == machineManagerBody.MachineID).MachineManagerID = machineManagerBody.ID;
            }

            //Get certification relation.
            this.CertificationModule = _moduleList.Single(p => p.Name == certificationModuleName);
            this.CertificationBody = _serverBodyList.Single(p => p.ModuleID == this.CertificationModule.ID);
            this.CertificationMachine = _serverMachineList.Single(p => p.ID == this.CertificationBody.MachineID);
            this.CertificationCords = _serverCordList.Where(p => p.ParentID == this.CertificationBody.ID);

            //HOTFIX: Move CertificationBody at the end so GlobalMAnager does not think he's the Certification -.-
            _serverBodyList.Remove(this.CertificationBody);
            _serverBodyList.Add(this.CertificationBody);

            this.CertificationBody.State = ServerBodyState.Blue;
        }

        private void Load<T>(SqlDataReader reader, List<T> list) where T : ICertificationRow, new()
        {
            while (reader.Read())
            {
                var instance = new T();

                Helper.Load(instance, reader);

                list.Add(instance);
            }
            reader.Close();
        }

        private void ValidateList<T>(List<T> list, string name)
        {
            if (list.Count == 0)
                Logger.Fatal($"{Caller.GetMemberName()}: {name} is empty.");
        }

        #endregion Load Methods

        #region UpdateMethods

        public bool UpdateShardName(short shardID, string shardName)
        {
            var shard = _shardList.Find(p => p.ID == shardID);
            if (shard == null)
                return false;

            if (string.IsNullOrWhiteSpace(shardName) || shardName.Length > 32)
                return false;

            var sqlParams = new[]
            {
                new SqlParameter("@name", shardName),
                new SqlParameter("@id", shardID)
            };
            var result = _database.Execute($"UPDATE Shard SET Name = {sqlParams[0].ParameterName} WHERE ID = {sqlParams[1].ParameterName}", sqlParams);
            if (result)
                shard.UpdateName(shardName);

            return result;
        }

        public bool UpdateShardMaxUser(short shardID, short maxUser)
        {
            var shard = _shardList.Find(p => p.ID == shardID);
            if (shard == null)
                return false;

            if (maxUser <= 0)
                return false;

            var sqlParams = new[]
            {
                new SqlParameter("@maxUser", maxUser),
                new SqlParameter("@id", shardID)
            };
            var result = _database.Execute($"UPDATE Shard SET MaxUser = {sqlParams[0].ParameterName} WHERE ID = {sqlParams[1].ParameterName}", sqlParams);
            if (result)
                shard.UpdateMaxUser(maxUser);

            return result;
        }

        #endregion UpdateMethods

        #region WriteMethods

        public void WriteAcknowledge(Packet packet)
        {
            //packet.WriteByte(1); //result
            this.WriteList(packet, _moduleList);
            this.WriteList(packet, _contentList);
            this.WriteList(packet, _divisionList);
            this.WriteList(packet, _farmList);
            this.WriteList(packet, _farmContentList);
            this.WriteList(packet, _shardList);
            this.WriteList(packet, _serverMachineList);
            this.WriteList(packet, _serverBodyList);
            this.WriteList(packet, _serverCordList);

            packet.WriteBool(false);
            //packet.WriteBool(writeSecurityDesc);
            //if (writeSecurityDesc)
            //{
            //    this.WriteList(packet, _securityDescriptionGroups);
            //    this.WriteList(packet, _securityDescriptions);
            //    this.WriteList(packet, _securityDescriptionGroupAssigns);
            //}
        }

        public void WriteList<TStruct>(Packet packet, IList<TStruct> list) where TStruct : Unmanaged.IMarshalled
        {
            packet.WriteByte(0); //unkByte1
            foreach (TStruct structure in list)
            {
                packet.WriteByte(1);
                packet.WriteMarshalled(structure);
            }
            packet.WriteByte(2);
        }

        #endregion WriteMethods
    }
}