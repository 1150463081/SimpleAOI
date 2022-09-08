using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AOICell
{
    public class AOICell
    {
        public int XIndex { get; private set; }
        public int ZIndex { get; private set; }
        public bool IsCalcuBoundary { get; private set; } = false;
        private AOIMgr mgr;
        private AOICell[] AOIGround = null;
        public CellUpdateData CellUpdateData { get; private set; }
        public HashSet<AOIEntity> HoldEntity { get; private set; }
        //新进入的实体，待实体存量增删完成后再加入HoldEntity
        public HashSet<AOIEntity> EnterEntity { get; private set; }

        public AOICell(int xIndex, int zIndex, AOIMgr mgr)
        {
            XIndex = xIndex;
            ZIndex = zIndex;
            this.mgr = mgr;
            CellUpdateData = new CellUpdateData();
            HoldEntity = new HashSet<AOIEntity>();
            EnterEntity = new HashSet<AOIEntity>();
        }
        public void EnterCell(AOIEntity entity)
        {
            if (!EnterEntity.Add(entity))
            {
                this.Error($"EnterEntity Add Err:{entity.EntityId}");
                return;
            }   
            if (entity.MoveType == EMoveType.TransferEnter)
            {
                entity.SetAroundCell(AOIGround);
                for (int i = 0; i < AOIGround.Length; i++)
                {
                    AOIGround[i].AddCellOperate(ECellOperate.EntityEnter, entity);
                }
            }
            else if (entity.MoveType == EMoveType.MoveCross)
            {

            }
            else
            {

            }
        }
        public void MoveCell(AOIEntity entity)
        {

        }
        public void ExitCell(AOIEntity entity)
        {
            for (int i = 0; i < AOIGround.Length; i++)
            {
                AOIGround[i].AddCellOperate(ECellOperate.EntityExit, entity);
            }
            HoldEntity.Remove(entity);
        }
        public void AddCellOperate(ECellOperate cellOperate, AOIEntity entity)
        {
            switch (cellOperate)
            {
                case ECellOperate.EntityEnter:
                    CellUpdateData.enterList.Add(new EnterData(entity.EntityId, entity.XIndex, entity.ZIndex));
                    break;
                case ECellOperate.EntityMove:
                    CellUpdateData.moveList.Add(new MoveData(entity.EntityId, entity.XIndex, entity.ZIndex));
                    break;
                case ECellOperate.EntityExit:
                    CellUpdateData.exitList.Add(new ExitData(entity.EntityId));
                    break;
            }
        }
        //计算边界
        public void CalcuBoundary()
        {
            if (IsCalcuBoundary)
                return;
            AOIGround = new AOICell[9];
            int index = 0;
            for (int i = XIndex - 2; i <= XIndex + 2; i++)
            {
                for (int j = ZIndex - 2; j <= ZIndex + 2; j++)
                {
                    if (!mgr.HasCell(i, j))
                    {
                        mgr.CreateCell(i, j);
                    }
                    if (i > XIndex - 2 &&
                        i < XIndex + 2 &&
                        j > ZIndex - 2 &&
                        j < ZIndex + 2)
                    {
                        AOIGround[index++] = mgr.GetOrCreateCell(i, j);
                    }
                }
            }
            IsCalcuBoundary = true;
        }
        //合并当前格所有的操作指令，告知客户端
        public void CalcCellOpCombine()
        {
            if (!CellUpdateData.IsEmpty)
            {
                mgr.CellEntityOpCombineEvent(this, CellUpdateData);
                CellUpdateData.Clear();
            }
        }
    }

    public enum ECellOperate
    {
        EntityEnter,
        EntityMove,
        EntityExit,
    }

}