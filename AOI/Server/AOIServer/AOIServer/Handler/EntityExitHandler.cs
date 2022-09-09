using System;
using System.Collections.Generic;
using System.Text;
using AOICellProtocol;

namespace AOIServer
{
    public class EntityExitHandler : NetHandlerBase<Pkg_C2SEntityExit>
    {
        protected override OperateCode OperateCode => OperateCode.C2SEntiyExit;

        public override void Handler(ServerSession session, Pkg_C2SEntityExit pkg)
        {
            if (ServerRoot.Instance.BattleStage.TryGetRole(pkg.entityId, out var role))
            {
                ServerRoot.Instance.BattleStage.ExitStage(role);
            }
        }
    }
}
