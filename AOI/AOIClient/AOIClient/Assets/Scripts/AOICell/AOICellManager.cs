using System.Collections.Generic;
using AOICellProtocol;
using UnityEngine;

namespace AOIClient
{
    public class AOICellManager:MonoSingleton<AOICellManager>
    {
        private GameObject cellPrefab;
        private Transform cellRoot;
        private Dictionary<string, CellEntity> cellDict;
        protected override void OnInit()
        {
            cellPrefab = Resources.Load<GameObject>("Plane");
            cellRoot = (new GameObject("CellRoot")).transform;
            NetManager.Instance.AddNetMsgLisener(OperateCode.S2CUpdateAOI, UpdateAOIHandler);
            NetManager.Instance.AddNetMsgLisener(OperateCode.S2CCreateCell, CreateCellHandler);
            NetManager.Instance.AddNetMsgLisener(OperateCode.S2CInitExistCell, InitExistCellHandler);
        }
        protected override void OnTermination()
        {
            NetManager.Instance.RemoveNetMsgLisener(OperateCode.S2CUpdateAOI, UpdateAOIHandler);
            NetManager.Instance.RemoveNetMsgLisener(OperateCode.S2CCreateCell, CreateCellHandler);
            NetManager.Instance.RemoveNetMsgLisener(OperateCode.S2CInitExistCell, InitExistCellHandler);
        }
        private void CreateCell(int xIndex,int zIndex)
        {
            this.LogBlue($"创建宫格：{xIndex},{zIndex}");
            var go = Instantiate(cellPrefab, cellRoot);
            go.transform.localScale = go.transform.localScale * 0.99f;
            var cellSize = RoleManager.Instance.LoginPkg.cellSize;
            go.transform.position = new Vector3((xIndex + 0.5f) * cellSize, 0, (zIndex + 0.5f) * cellSize);
            TextMesh textMesh = go.transform.Find("TextMesh").GetComponent<TextMesh>();
            textMesh.text = $"{xIndex}:{zIndex}";
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
                    Debug.Log($"{moveMsg.entitiyId}移动:({moveMsg.PosX},{moveMsg.PosZ})");
                    if (moveMsg.entitiyId != RoleManager.Instance.MyRoleId)//不考虑玩家自身
                    {
                        var role = RoleManager.Instance.GetRole(moveMsg.entitiyId);
                        role.UpdatePos(moveMsg.PosX, moveMsg.PosZ);
                    }
                }
            }
        }
        private void CreateCellHandler(Pkg pkg)
        {
            var data = pkg as Pkg_S2CCreateCell;
            CreateCell(data.xIndex, data.zIndex);
        }
        private void InitExistCellHandler(Pkg pkg)
        {
            var data = pkg as Pkg_S2CInitExistCell;
            for (int i = 0; i < data.xArr.Count; i++)
            {
                CreateCell(data.xArr[i], data.zArr[i]);
            }
        }

    }
}
