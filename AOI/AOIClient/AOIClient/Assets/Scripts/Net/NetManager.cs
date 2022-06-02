using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AOICellProtocol;
using PENet;
using System.Collections.Concurrent;
using PEUtils;

namespace AOIClient
{
    public class NetManager:MonoSingleton<NetManager>
    {
        public string SessionId { get; private set; }

        private AsyncNet<ClientSession, Pkg> asyncNet;
        private ConcurrentQueue<Pkg> pkgQueue = new ConcurrentQueue<Pkg>();
        private Dictionary<OperateCode, Action<Pkg>> netHandlerDict = new Dictionary<OperateCode, Action<Pkg>>();
        private void Update()
        {
            if (!pkgQueue.IsEmpty)
            {
                if(pkgQueue.TryDequeue(out var pkg))
                {
                    if(netHandlerDict.TryGetValue(pkg.operateCode,out var handler))
                    {
                        PELog.Log("触发");
                        handler?.Invoke(pkg);
                    }
                }
            }
        }
        protected override void OnInit()
        {
            AddNetMsgLisener(OperateCode.S2CConnect, p =>
            {
                SessionId = (p as Pkg_S2CConnect).sessionId;
                PELog.Log("SessionId：{0}", SessionId);
            });
            asyncNet = new AsyncNet<ClientSession, Pkg>();
            asyncNet.StartAsClient("127.0.0.1", 0415);
        }

        public void SendMsg(Pkg pkg)
        {
            asyncNet.session.SendMsg(pkg);
        }
        public void AddNetPkg(Pkg pkg)
        {
            pkgQueue.Enqueue(pkg);
        }
        public void AddNetMsgLisener(OperateCode operate,Action<Pkg> action)
        {
            if(netHandlerDict.TryGetValue(operate, out var handler))
            {
                handler += action;
            }
            else
            {
                netHandlerDict.Add(operate, action);
            }
        }
        public void RemoveNetMsgLisener(OperateCode operate, Action<Pkg> action)
        {
            if (netHandlerDict.TryGetValue(operate, out var handler))
            {
                try
                {
                    handler -= action;
                }
                catch(Exception e)
                {
                    PELog.Error(e.ToString());
                }
            }
            else
            {
                PELog.Error("未注册{0}，无法移除监听", operate);
            }
        }
    }
}
