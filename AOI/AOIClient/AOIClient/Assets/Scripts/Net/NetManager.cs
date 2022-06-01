using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AOICellProtocol;
using PENet;

namespace AOIClient
{
    public class NetManager:MonoSingleton<NetManager>
    {
        private AsyncNet<ClientSession, Pkg> asyncNet;
        protected override void OnInit()
        {
            asyncNet = new AsyncNet<ClientSession, Pkg>();
            asyncNet.StartAsClient("127.0.0.1", 0415);
        }

        public void SendMsg(Pkg pkg)
        {
            asyncNet.session.SendMsg(pkg);
        }
    }
}
