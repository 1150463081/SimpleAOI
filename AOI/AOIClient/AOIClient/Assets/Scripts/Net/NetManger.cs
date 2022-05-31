using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AOICellProtocol;
using PENet;

namespace AOIClient
{
    public class NetManger:MonoSingleton<NetManger>
    {
        private AsyncNet<ClientSession, Pkg> asyncNet;
        protected override void OnInit()
        {
            asyncNet = new AsyncNet<ClientSession, Pkg>();
            asyncNet.StartAsClient("127.0.0.1", 0415);
        }
    }
}
