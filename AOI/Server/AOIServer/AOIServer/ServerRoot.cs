using PEUtils;
using System;
using System.Collections.Generic;
using System.Text;
using PENet;
using AOICellProtocol;
using System.Collections.Concurrent;

namespace AOIServer
{
    public partial class ServerRoot
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
        ConcurrentQueue<NetPkg> pkgQueue = new ConcurrentQueue<NetPkg>();
        Dictionary<OperateCode, Action<NetPkg>> handlerDict = new Dictionary<OperateCode, Action<NetPkg>>();
        BattleStage battleStage;
        public void Init()
        {
            asyncNet.StartAsServer("127.0.0.1",0415);
            battleStage = new BattleStage();
            InitHandler();
        }
        public void Tick()
        {
            battleStage?.Tick();
            while (!pkgQueue.IsEmpty)
            {
                if(pkgQueue.TryDequeue(out var netPkg))
                {

                }
                else
                {
                    PELog.Error("Dequeue Queue Error!!!");
                }
            }
        }
        public void Destroy()
        {
            battleStage.Destroy();
        }
        public void AddNetPkg(NetPkg netPkg)
        {
            pkgQueue.Enqueue(netPkg);
        }
        public void AddHandler(OperateCode operateCode,Action<NetPkg> handler)
        {
            handlerDict[operateCode] = handler;
        }

        private void InitHandler()
        {
            new LoginHandler();
        }
    }
}
