using System;
using System.Collections.Generic;
using System.Text;
using AOICellProtocol;
using PEUtils;
using AOICell;

namespace AOIServer
{
    public class LoginHandler : NetHandlerBase<Pkg_C2SLogin>
    {
        protected override OperateCode OperateCode => OperateCode.C2SLogin;

        public override void Handler(ServerSession session, Pkg_C2SLogin pkg)
        {
            PELog.Log($"处理登录消息,用户名{pkg.usernName}");
            int roleId = ServerRoot.Instance.SpawnClientId();
            RoleEntity roleEntity = new RoleEntity(roleId, session,EDriveType.Client);
            ServerRoot.Instance.BattleStage.EnterStage(roleEntity);
            session.SendMsg(new Pkg_S2CLogin()
            {
                operateCode = OperateCode.S2CLogin,
                roleId = roleId,
                moveSpeed = AOICfg.moveSpeed,
                cellSize = AOICfg.cellSize
            });
        }
    }
}
