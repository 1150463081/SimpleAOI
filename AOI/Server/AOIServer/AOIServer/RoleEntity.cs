using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text;
using AOICell;

namespace AOIServer
{
    public enum ERoleState
    {
        None,
        OnLine,
        OffLine,
        Trusteeship,//托管
    }
    public class RoleEntity
    {
        public int RoleId { get; private set; }
        public ERoleState RoleState { get; private set; }
        public AOIEntity AOIEntity { get; private set; }
        public Vector3 TargetPos { get; private set; }

        public RoleEntity(int roleId)
        {
            RoleId = roleId;
            RoleState = ERoleState.OnLine;
        }
        public void SetAOIEntity(AOIEntity entity)
        {
            AOIEntity = entity;
        }
        public void OnEnterStage(BattleStage stage)
        {

        }
        public void OnUpdateStage(BattleStage stage)
        {

        }
        public void OnExitStage(BattleStage stage)
        {
            AOIEntity = null;
        }
    }
}
