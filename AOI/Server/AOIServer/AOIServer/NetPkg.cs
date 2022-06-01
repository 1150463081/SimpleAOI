using System;
using System.Collections.Generic;
using System.Text;
using AOICellProtocol;

namespace AOIServer
{
    public class NetPkg
    {
        public ServerSession session;
        public Pkg pkg;
        public NetPkg(ServerSession session,Pkg pkg)
        {
            this.session = session;
            this.pkg = pkg;
        }
    }
}
