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
        public EDriveType EDriveType { get; private set; }

        public RoleEntity(int roleId, ServerSession serverSession, EDriveType eDriveType)
        {
            RoleId = roleId;
            RoleState = ERoleState.OnLine;
            this.session = serverSession;
            EDriveType = eDriveType;
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
            if (RoleState == ERoleState.OnLine)
            {
                SendMsg(pkg);
            }
        }
        public void OnUpdateStage(byte[] bytes)
        {
            if (RoleState == ERoleState.OnLine)
            {
                SendMsg(bytes);
            }
        }
        public void OnExitStage(BattleStage stage)
        {
            RoleState = ERoleState.OffLine;
            AOIEntity = null;
        }
        public void SendMsg(Pkg pkg)
        {
            session?.SendMsg(pkg);
        }
        public void SendMsg(byte[] bytes)
        {
            session?.SendMsg(bytes);
        }

        DateTime nextDirTime;
        DateTime lastMoveTime;
        Vector3 moveDir;
        bool isTurn = false;//是否立刻转向
        public void StartRandomMove()
        {
            nextDirTime = DateTime.Now;
            lastMoveTime = DateTime.Now;
            ServerRoot.Instance.BattleStage.TickEvent += RandomMove;
        }
        private void RandomMove()
        {
            if (DateTime.Now > nextDirTime||isTurn)
            {
                isTurn = false;
                Random random = new Random();
                //随机下次改变朝向的时间
                var seconds = random.Next(5, 20);
                nextDirTime = DateTime.Now.AddSeconds(seconds);
                //随机一个方向
                var rdx = random.Next(AOICfg.borderLeft, AOICfg.borderRight);
                var rdz = random.Next(AOICfg.borderDown, AOICfg.borderUp);
                moveDir = Vector3.Normalize(new Vector3(rdx, 0, rdz) - TargetPos);
            }
            float delta = (float)((DateTime.Now - lastMoveTime).TotalMilliseconds / 1000f);
            var targetPos = moveDir * delta * AOICfg.moveSpeed + TargetPos;
            lastMoveTime = DateTime.Now;
            //判断目标位置是否超出边界  
            if (targetPos.X < AOICfg.borderLeft ||
                targetPos.X > AOICfg.borderRight ||
                targetPos.Z < AOICfg.borderDown ||
                targetPos.Z > AOICfg.borderUp)
            {
                isTurn = true;
                return;
            }
            TargetPos = targetPos;
            ServerRoot.Instance.BattleStage.UpdateStage(this);
        }
    }
}
