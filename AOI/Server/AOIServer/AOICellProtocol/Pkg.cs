using PENet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AOICellProtocol
{
    [Serializable]
    public class Pkg: AsyncMsg
    {
        public OperateCode operateCode;
    }

    [Serializable]
    public class Pkg_S2CConnect : Pkg
    {
        public string sessionId;
    }
    [Serializable]
    public class Pkg_C2SLogin : Pkg
    {
        public string usernName;
        public string sessionId;
    }
    [Serializable]
    public class Pkg_S2CLogin : Pkg
    {
        public int roleId;
    }
    [Serializable]
    public class Pkg_S2CUpdateAOI : Pkg
    {
        public int type;
        public List<EnterMsg> enterList;
        public List<MoveMsg> moveList;
        public List<ExitMsg> exitList;
    }
    [Serializable]
    public class EnterMsg
    {
        public int entitiyId;
        public float PosX;
        public float PosZ;
    }
    [Serializable]
    public class MoveMsg
    {
        public int entitiyId;
        public float PosX;
        public float PosZ;
    }
    [Serializable]
    public class ExitMsg
    {
        public int entitiyId;
    }
    [Serializable]
    public class Pkg_C2SEntityMove : Pkg
    {
        public int entityId;
        public float posX;
        public float posY;
    }
}
