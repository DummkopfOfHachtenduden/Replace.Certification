using System.Diagnostics;
using System.Runtime.InteropServices;

namespace Replace.Common.Certification
{
    [DebuggerDisplay("{_id} [{_childID} to {_parentID}] [{_bindType}]")]
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public class ServerCord : ICertificationRow
    {
        #region Fields

        [MarshalAs(UnmanagedType.U4)]
        private int _id;

        [MarshalAs(UnmanagedType.U2)]
        private short _childID;

        [MarshalAs(UnmanagedType.U2)]
        private short _parentID;

        [MarshalAs(UnmanagedType.U1)]
        private ServerCordBindType _bindType;

        [MarshalAs(UnmanagedType.U4)]
        private ServerCordState _state; //RUNTIME

        [MarshalAs(UnmanagedType.U4)]
        public uint unkUInt1; //RUNTIME

        #endregion Fields

        #region Properties

        public int ID { get { return _id; } private set { _id = value; } }
        public short ChildID { get { return _childID; } private set { _childID = value; } }
        public short ParentID { get { return _parentID; } private set { _parentID = value; } }
        public ServerCordBindType BindType { get { return _bindType; } private set { _bindType = value; } }
        public ServerCordState State { get { return _state; } set { _state = value; } }

        #endregion Properties

        public override string ToString()
        {
            return $"{nameof(ServerCord)}#{_id} [{_childID} -> {_parentID}] [{_bindType}]";
        }
    }
}