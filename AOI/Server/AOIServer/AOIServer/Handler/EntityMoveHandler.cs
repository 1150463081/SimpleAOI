using AOICellProtocol;
using System;
using System.Collections.Generic;
using System.Text;

namespace AOIServer
{
    public class EntityMoveHandler : NetHandlerBase<Pkg_C2SEntityMove>
    {
        protected override OperateCode OperateCode => OperateCode.C2SEntityMove;

        public override void Handler(ServerSession session, Pkg_C2SEntityMove pkg)
        {
            if(ServerRoot.Instance.BattleStage.TryGetRole(pkg.entityId,out var role))
            {
                role.TargetPos = new System.Numerics.Vector3(pkg.posX, 0, pkg.posY);
                ServerRoot.Instance.BattleStage.UpdateStage(role);
            }
        }
    }
}
