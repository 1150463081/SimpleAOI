using System;
using System.Collections.Generic;
using System.Text;
using System.Collections.Concurrent;
using AOICell;

namespace AOIServer
{
    public class BattleStage
    {
        ConcurrentDictionary<int, RoleEntity> roleDict = new ConcurrentDictionary<int, RoleEntity>();
        AOIMgr AOIMgr;

        private ConcurrentQueue<RoleEntity> exitQueue = new ConcurrentQueue<RoleEntity>();
        private ConcurrentQueue<RoleEntity> enterQueue = new ConcurrentQueue<RoleEntity>();
        private ConcurrentQueue<RoleEntity> moveQueue = new ConcurrentQueue<RoleEntity>();

        public void Init()
        {
            AOIMgr = new AOIMgr(new AOICfg());
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
                role.OnUpdateStage(this);
            }
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
    }
}
