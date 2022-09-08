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

        }
    }
}
