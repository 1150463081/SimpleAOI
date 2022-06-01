using System;
using System.Collections.Generic;
using System.Text;
using AOICellProtocol;
using PEUtils;

namespace AOIServer
{
    public class LoginHandler : NetHandlerBase<Pkg_C2SLogin>
    {
        protected override OperateCode OperateCode => OperateCode.Login;

        public override void Handler(ServerSession session, Pkg_C2SLogin pkg)
        {
            PELog.Log("处理登录消息");
        }
    }
}
