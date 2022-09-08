using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AOICellProtocol;

namespace AOIClient
{
    public class AOICellManager:MonoSingleton<AOICellManager>
    {
        private Dictionary<string, CellEntity> cellDict;
        protected override void OnInit()
        {
            NetManager.Instance.AddNetMsgLisener(OperateCode.S2CUpdateAOI, UpdateAOIHandler);
        }
        protected override void OnTermination()
        {
            NetManager.Instance.RemoveNetMsgLisener(OperateCode.S2CUpdateAOI, UpdateAOIHandler);
        }

        private void UpdateAOIHandler(Pkg pkg)
        {
            var Data = pkg as Pkg_S2CUpdateAOI;
            if (Data.exitList != null && Data.exitList.Count > 0)
            {
                for (int i = 0; i < Data.exitList.Count; i++)
                {
                    ExitMsg exitMsg = Data.exitList[i];
                    RoleManager.Instance.RemoveRole(exitMsg.entitiyId);
                }
            }
            if (Data.enterList != null && Data.enterList.Count > 0)
            {
                for (int i = 0; i < Data.enterList.Count; i++)
                {
                    EnterMsg enterMsg = Data.enterList[i];
                    RoleManager.Instance.AddRole(enterMsg.entitiyId, enterMsg.PosX, enterMsg.PosZ);
                }
            }
            if (Data.moveList != null && Data.moveList.Count > 0)
            {
                for (int i = 0; i < Data.moveList.Count; i++)
                {
                    MoveMsg moveMsg = Data.moveList[i];
                    var role = RoleManager.Instance.GetRole(moveMsg.entitiyId);
                    role.UpdatePos(moveMsg.PosX, moveMsg.PosZ);
                }
            }
        }
    }
}
