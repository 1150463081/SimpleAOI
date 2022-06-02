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
}
