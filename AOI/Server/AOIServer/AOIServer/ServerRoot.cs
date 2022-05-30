using PEUtils;
using System;
using System.Collections.Generic;
using System.Text;
using PENet;
using AOICellProtocol;

namespace AOIServer
{
    public class ServerRoot
    {
        private static ServerRoot instance;
        public static ServerRoot Instance
        {
            get {
                if (instance == null)
                {
                    instance = new ServerRoot();
                }
                return instance;
            }
        }
        AsyncNet<ServerSession, Pkg> asyncNet = new AsyncNet<ServerSession, Pkg>();
        BattleStage battleStage;
        public void Init()
        {
            asyncNet.StartAsServer("127.0.0.1",0415);
            battleStage = new BattleStage();
        }
        public void Tick()
        {
            battleStage?.Tick();
        }
        public void Destroy()
        {
            battleStage.Destroy();
        }
    }
}
