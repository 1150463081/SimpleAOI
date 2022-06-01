using System;
using System.Collections.Generic;
using System.Text;
using PENet;
using AOICellProtocol;
using PEUtils;

namespace AOIServer
{
    public class ServerSession : AsyncSession<Pkg>
    {
        protected override void OnConnected(bool result)
        {
            PELog.LogGreen("New Client Connect:{0}", result);
        }

        protected override void OnDisConnected()
        {
            PELog.LogYellow("Client DisConnected");
        }

        protected override void OnReceiveMsg(Pkg msg)
        {
            PELog.Log("Receive Msg:{0}",msg.operateCode);
            ServerRoot.Instance.AddNetPkg(new NetPkg(this, msg));
        }
    }
}
