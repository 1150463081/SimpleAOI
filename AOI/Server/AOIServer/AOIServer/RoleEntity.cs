using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text;
using AOICell;
using AOICellProtocol;

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
        public Vector3 TargetPos { get; set; }
        public ServerSession session { get; private set; }

        public RoleEntity(int roleId,ServerSession serverSession)
        {
            RoleId = roleId;
            RoleState = ERoleState.OnLine;
            this.session = serverSession;
        }
        public void SetAOIEntity(AOIEntity entity)
        {
            AOIEntity = entity;
        }
        public void OnEnterStage(BattleStage stage)
        {
            RoleState = ERoleState.OnLine;
        }
        public void OnUpdateStage(Pkg pkg)
        {
            if(RoleState== ERoleState.OnLine)
            {
                session?.SendMsg(pkg);
            }
        }
        public void OnExitStage(BattleStage stage)
        {
            RoleState = ERoleState.OffLine;
            AOIEntity = null;
        }
    }
}
