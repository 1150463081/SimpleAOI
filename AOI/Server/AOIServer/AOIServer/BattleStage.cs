using System;
using System.Collections.Generic;
using System.Text;
using System.Collections.Concurrent;
using AOICell;
using AOICellProtocol;

namespace AOIServer
{
    public class BattleStage
    {
        ConcurrentDictionary<int, RoleEntity> roleDict = new ConcurrentDictionary<int, RoleEntity>();
        AOIMgr AOIMgr;

        //将收到的操作数据收集起来，每一帧批量操作
        private ConcurrentQueue<RoleEntity> exitQueue = new ConcurrentQueue<RoleEntity>();
        private ConcurrentQueue<RoleEntity> enterQueue = new ConcurrentQueue<RoleEntity>();
        private ConcurrentQueue<RoleEntity> moveQueue = new ConcurrentQueue<RoleEntity>();

        public void Init()
        {
            AOIMgr = new AOIMgr();
            AOIMgr.EntityViewChangeEvent += EntityValueChangeHandler;
            AOIMgr.CellEntityOpCombineEvent += CellEntityOpCombineHandler;
        }
        public void Tick()
        {
            while (exitQueue.TryDequeue(out var role))
            {
                AOIMgr.ExitCell(role.AOIEntity);
                if (roleDict.TryRemove(role.RoleId, out var r))
                {
                    role.OnExitStage(this);
                }
                else
                {
                    this.Error($"{role.RoleId}is not exist...");
                }
            }
            while (enterQueue.TryDequeue(out var role))
            {
                var aoiEntity = AOIMgr.EnterCell(role.RoleId, role.TargetPos.X, role.TargetPos.Z);
                role.SetAOIEntity(aoiEntity);
                if (roleDict.TryAdd(role.RoleId, role))
                {
                    role.OnEnterStage(this);
                }
                else
                {
                    this.Error($"{role.RoleId}is  exist...");
                }
            }
            while (moveQueue.TryDequeue(out var role))
            {
                AOIMgr.UpdatePos(role.AOIEntity, role.TargetPos.X, role.TargetPos.Z);
            }
            AOIMgr.CalcuateAOIUpdate();
        }
        public void Destroy()
        {
            roleDict.Clear();
            enterQueue.Clear();
            exitQueue.Clear();
            moveQueue.Clear();
        }


        public void EnterStage(RoleEntity role)
        {
            if (!roleDict.ContainsKey(role.RoleId))
            {
                enterQueue.Enqueue(role);
            }
            else
            {
                this.Error($"{role.RoleId}is exist...");
            }
        }
        public void UpdateStage(RoleEntity role)
        {
            if (roleDict.ContainsKey(role.RoleId))
            {
                moveQueue.Enqueue(role);
            }
            else
            {
                this.Error($"{role.RoleId}is not exist...");
            }
        }
        public void ExitStage(RoleEntity role)
        {
            if (roleDict.ContainsKey(role.RoleId))
            {
                exitQueue.Enqueue(role);
            }
            else
            {
                this.Error($"{role.RoleId}is not exist...");
            }
        }
        public bool TryGetRole(int roleId,out RoleEntity role)
        {
            if(roleDict.TryGetValue(roleId,out role))
            {
                return true;
            }
            return false;
        }

        private void EntityValueChangeHandler(AOIEntity entity,CellUpdateData cellUpdateData)
        {
            Pkg_S2CUpdateAOI pkg = new Pkg_S2CUpdateAOI()
            {
                enterList = new List<EnterMsg>(),
                exitList = new List<ExitMsg>(),
                operateCode = OperateCode.S2CUpdateAOI,
            };
            if (cellUpdateData.enterList.Count > 0)
            {
                for (int i = 0; i < cellUpdateData.enterList.Count; i++)
                {
                    pkg.enterList.Add(new EnterMsg()
                    {
                        entitiyId = cellUpdateData.enterList[i].Id,
                        PosX = cellUpdateData.enterList[i].x,
                        PosZ = cellUpdateData.enterList[i].z
                    });
                }
            }
            if (cellUpdateData.exitList.Count > 0)
            {
                for (int i = 0; i < cellUpdateData.exitList.Count; i++)
                {
                    pkg.exitList.Add(new ExitMsg()
                    {
                        entitiyId = cellUpdateData.enterList[i].Id,
                    });
                }
            }
            if(roleDict.TryGetValue(entity.EntityId,out var role))
            {
                role.OnUpdateStage(pkg);
                this.Log($"ValueChange:{pkg.enterList.Count}");
            }

        }
        private void CellEntityOpCombineHandler(AOICell.AOICell cell,CellUpdateData cellUpdateData)
        {
            Pkg_S2CUpdateAOI pkg = new Pkg_S2CUpdateAOI()
            {
                enterList = new List<EnterMsg>(),
                exitList = new List<ExitMsg>(),
                operateCode = OperateCode.S2CUpdateAOI,
            };
            if (cellUpdateData.enterList.Count > 0)
            {
                for (int i = 0; i < cellUpdateData.enterList.Count; i++)
                {
                    pkg.enterList.Add(new EnterMsg()
                    {
                        entitiyId = cellUpdateData.enterList[i].Id,
                        PosX = cellUpdateData.enterList[i].x,
                        PosZ = cellUpdateData.enterList[i].z
                    });
                }
            }
            if (cellUpdateData.exitList.Count > 0)
            {
                for (int i = 0; i < cellUpdateData.exitList.Count; i++)
                {
                    pkg.exitList.Add(new ExitMsg()
                    {
                        entitiyId = cellUpdateData.enterList[i].Id,
                    });
                }
            }

            foreach (var entity in cell.HoldEntity)
            {
                if (roleDict.TryGetValue(entity.EntityId, out var role))
                {
                    role.OnUpdateStage(pkg);
                    this.Log($"OpCombine:{pkg.enterList.Count}");
                }
            }
        }


    }
}
