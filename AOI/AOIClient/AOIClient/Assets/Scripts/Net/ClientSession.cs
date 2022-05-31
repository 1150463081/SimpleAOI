using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PENet;
using AOICellProtocol;
using PEUtils;

namespace AOIClient
{
    public class ClientSession : AsyncSession<Pkg>
    {
        protected override void OnConnected(bool result)
        {
            PELog.LogGreen("Client Connect{0}", result);
        }

        protected override void OnDisConnected()
        {
            PELog.LogYellow("Client DisConnect{0}");
        }

        protected override void OnReceiveMsg(Pkg msg)
        {
            PELog.Log("Recive new msg");
        }
    }
}
