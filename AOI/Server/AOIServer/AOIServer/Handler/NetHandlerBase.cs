using System;
using System.Collections.Generic;
using System.Text;
using AOICellProtocol;

namespace AOIServer
{
    public abstract class NetHandlerBase<T>
        where T : Pkg
    {
        protected abstract OperateCode OperateCode { get; }
        public void OnRecive(NetPkg netPkg)
        {
            T pkg = netPkg.pkg as T;
            Handler(netPkg.session, pkg);
        }
        public abstract void Handler(ServerSession session, T pkg);
        public NetHandlerBase()
        {
            ServerRoot.Instance.AddHandler(OperateCode, OnRecive);
        }
    }
}
