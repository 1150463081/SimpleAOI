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

        #region GroundCell
        public AOICell Up => mgr.CellDict[$"{XIndex},{ZIndex + 1}"];
        public AOICell Down => mgr.CellDict[$"{XIndex},{ZIndex - 1}"];
        public AOICell Left => mgr.CellDict[$"{XIndex - 1},{ZIndex}"];
        public AOICell Right => mgr.CellDict[$"{XIndex + 1},{ZIndex}"];
        public AOICell LeftUp => mgr.CellDict[$"{XIndex - 1},{ZIndex + 1}"];
        public AOICell RightUp => mgr.CellDict[$"{XIndex + 1},{ZIndex + 1}"];
        public AOICell LeftDown => mgr.CellDict[$"{XIndex - 1},{ZIndex - 1}"];
        public AOICell RightDown => mgr.CellDict[$"{XIndex + 1},{ZIndex - 1}"];
        #endregion


        private AOICell[] AOIGround = null;
        //穿越边界时视野内添加的宫格
        private Dictionary<ECrossDirType, AOICell[]> CrossAddCell = new Dictionary<ECrossDirType, AOICell[]>();
        //穿越边界时视野内移除的宫格
        private Dictionary<ECrossDirType, AOICell[]> CrossRemoveCell = new Dictionary<ECrossDirType, AOICell[]>();
        //穿越边界时视野内未变的宫格
        private Dictionary<ECrossDirType, AOICell[]> CrossNoChangeCell = new Dictionary<ECrossDirType, AOICell[]>();


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
                entity.AddAroundCellView(AOIGround);
                for (int i = 0; i < AOIGround.Length; i++)
                {
                    AOIGround[i].AddCellOperate(ECellOperate.EntityEnter, entity);
                }
            }
            else if (entity.MoveType == EMoveType.MoveCross)
            {
                CrossMove(entity.ECrossDirType, entity);
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

        void CrossMove(ECrossDirType crossDir, AOIEntity entity)
        {
            if (crossDir == ECrossDirType.None)
            {
                return;
            }
            var removeArr = CrossRemoveCell[crossDir];
            var addArr = CrossAddCell[crossDir];
            var noChangeArr = CrossNoChangeCell[crossDir];
            for (int i = 0; i < removeArr.Length; i++)
            {
                entity.RemoveCellView(removeArr[i]);
                removeArr[i].AddCellOperate(ECellOperate.EntityExit, entity);
            }
            for (int i = 0; i < addArr.Length; i++)
            {
                entity.AddCellView(addArr[i]);
                addArr[i].AddCellOperate(ECellOperate.EntityEnter, entity);
            }
            for (int i = 0; i < noChangeArr.Length; i++)
            {
                noChangeArr[i].AddCellOperate(ECellOperate.EntityMove, entity);
            }
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
            //up
            {
                CrossRemoveCell[ECrossDirType.Up] = new AOICell[3];
                CrossRemoveCell[ECrossDirType.Up][0] = Down.LeftDown;
                CrossRemoveCell[ECrossDirType.Up][1] = Down.Down;
                CrossRemoveCell[ECrossDirType.Up][2] = Down.RightDown;

                CrossAddCell[ECrossDirType.Up] = new AOICell[3];
                CrossAddCell[ECrossDirType.Up][0] = LeftUp;
                CrossAddCell[ECrossDirType.Up][1] = Up;
                CrossAddCell[ECrossDirType.Up][2] = RightUp;

                CrossNoChangeCell[ECrossDirType.Up] = new AOICell[6];
                CrossNoChangeCell[ECrossDirType.Up][0] = Left;
                CrossNoChangeCell[ECrossDirType.Up][1] = this;
                CrossNoChangeCell[ECrossDirType.Up][2] = Right;
                CrossNoChangeCell[ECrossDirType.Up][3] = LeftDown;
                CrossNoChangeCell[ECrossDirType.Up][4] = Down;
                CrossNoChangeCell[ECrossDirType.Up][5] = RightDown;
            }
            //Down
            {
                CrossRemoveCell[ECrossDirType.Down] = new AOICell[3];
                CrossRemoveCell[ECrossDirType.Down][0] = Up.LeftUp;
                CrossRemoveCell[ECrossDirType.Down][1] = Up.Up;
                CrossRemoveCell[ECrossDirType.Down][2] = Up.RightUp;

                CrossAddCell[ECrossDirType.Down] = new AOICell[3];
                CrossAddCell[ECrossDirType.Down][0] = LeftDown;
                CrossAddCell[ECrossDirType.Down][1] = Down;
                CrossAddCell[ECrossDirType.Down][2] = RightDown;

                CrossNoChangeCell[ECrossDirType.Down] = new AOICell[6];
                CrossNoChangeCell[ECrossDirType.Down][0] = Left;
                CrossNoChangeCell[ECrossDirType.Down][1] = this;
                CrossNoChangeCell[ECrossDirType.Down][2] = Right;
                CrossNoChangeCell[ECrossDirType.Down][3] = LeftUp;
                CrossNoChangeCell[ECrossDirType.Down][4] = Up;
                CrossNoChangeCell[ECrossDirType.Down][5] = RightUp;
            }
            //Left
            {
                CrossRemoveCell[ECrossDirType.Left] = new AOICell[3];
                CrossRemoveCell[ECrossDirType.Left][0] = Right.RightUp;
                CrossRemoveCell[ECrossDirType.Left][1] = Right.Right;
                CrossRemoveCell[ECrossDirType.Left][2] = Right.RightDown;

                CrossAddCell[ECrossDirType.Left] = new AOICell[3];
                CrossAddCell[ECrossDirType.Left][0] = LeftUp;
                CrossAddCell[ECrossDirType.Left][1] = Left;
                CrossAddCell[ECrossDirType.Left][2] = LeftDown;

                CrossNoChangeCell[ECrossDirType.Left] = new AOICell[6];
                CrossNoChangeCell[ECrossDirType.Left][0] = Up;
                CrossNoChangeCell[ECrossDirType.Left][1] = this;
                CrossNoChangeCell[ECrossDirType.Left][2] = Down;
                CrossNoChangeCell[ECrossDirType.Left][3] = RightUp;
                CrossNoChangeCell[ECrossDirType.Left][4] = Right;
                CrossNoChangeCell[ECrossDirType.Left][5] = RightDown;
            }
            //Right
            {
                CrossRemoveCell[ECrossDirType.Right] = new AOICell[3];
                CrossRemoveCell[ECrossDirType.Right][0] = Left.LeftUp;
                CrossRemoveCell[ECrossDirType.Right][1] = Left.Left;
                CrossRemoveCell[ECrossDirType.Right][2] = Left.LeftDown;

                CrossAddCell[ECrossDirType.Right] = new AOICell[3];
                CrossAddCell[ECrossDirType.Right][0] = RightUp;
                CrossAddCell[ECrossDirType.Right][1] = Right;
                CrossAddCell[ECrossDirType.Right][2] = RightDown;

                CrossNoChangeCell[ECrossDirType.Right] = new AOICell[6];
                CrossNoChangeCell[ECrossDirType.Right][0] = Up;
                CrossNoChangeCell[ECrossDirType.Right][1] = this;
                CrossNoChangeCell[ECrossDirType.Right][2] = Down;
                CrossNoChangeCell[ECrossDirType.Right][3] = RightUp;
                CrossNoChangeCell[ECrossDirType.Right][4] = Right;
                CrossNoChangeCell[ECrossDirType.Right][5] = RightDown;
            }
            //LeftUp
            {
                CrossRemoveCell[ECrossDirType.LeftUp] = new AOICell[5];
                CrossRemoveCell[ECrossDirType.LeftUp][0] = Right.Right;
                CrossRemoveCell[ECrossDirType.LeftUp][1] = Right.RightDown;
                CrossRemoveCell[ECrossDirType.LeftUp][2] = RightDown.RightDown;
                CrossRemoveCell[ECrossDirType.LeftUp][3] = Down.RightDown;
                CrossRemoveCell[ECrossDirType.LeftUp][4] = Down.Down;

                CrossAddCell[ECrossDirType.LeftUp] = new AOICell[5];
                CrossAddCell[ECrossDirType.LeftUp][0] = LeftUp;
                CrossAddCell[ECrossDirType.LeftUp][1] = Up;
                CrossAddCell[ECrossDirType.LeftUp][2] = RightUp;
                CrossAddCell[ECrossDirType.LeftUp][3] = Left;
                CrossAddCell[ECrossDirType.LeftUp][4] = LeftDown;

                CrossNoChangeCell[ECrossDirType.LeftUp] = new AOICell[4];
                CrossNoChangeCell[ECrossDirType.LeftUp][0] = this;
                CrossNoChangeCell[ECrossDirType.LeftUp][1] = Right;
                CrossNoChangeCell[ECrossDirType.LeftUp][2] = Down;
                CrossNoChangeCell[ECrossDirType.LeftUp][3] = RightDown;
            }
            //RightUp
            {
                CrossRemoveCell[ECrossDirType.RightUp] = new AOICell[5];
                CrossRemoveCell[ECrossDirType.RightUp][0] = Left.Left;
                CrossRemoveCell[ECrossDirType.RightUp][1] = Left.LeftDown;
                CrossRemoveCell[ECrossDirType.RightUp][2] = LeftDown.LeftDown;
                CrossRemoveCell[ECrossDirType.RightUp][3] = Down.LeftDown;
                CrossRemoveCell[ECrossDirType.RightUp][4] = Down.Down;

                CrossAddCell[ECrossDirType.RightUp] = new AOICell[5];
                CrossAddCell[ECrossDirType.RightUp][0] = LeftUp;
                CrossAddCell[ECrossDirType.RightUp][1] = Up;
                CrossAddCell[ECrossDirType.RightUp][2] = RightUp;
                CrossAddCell[ECrossDirType.RightUp][3] = Right;
                CrossAddCell[ECrossDirType.RightUp][4] = RightDown;

                CrossNoChangeCell[ECrossDirType.RightUp] = new AOICell[4];
                CrossNoChangeCell[ECrossDirType.RightUp][0] = Left;
                CrossNoChangeCell[ECrossDirType.RightUp][1] = this;
                CrossNoChangeCell[ECrossDirType.RightUp][2] = LeftDown;
                CrossNoChangeCell[ECrossDirType.RightUp][3] = Down;
            }
            //leftDown
            {
                CrossRemoveCell[ECrossDirType.LeftDown] = new AOICell[5];
                CrossRemoveCell[ECrossDirType.LeftDown][0] = Up.Up;
                CrossRemoveCell[ECrossDirType.LeftDown][1] = Up.RightUp;
                CrossRemoveCell[ECrossDirType.LeftDown][2] = RightUp.RightUp;
                CrossRemoveCell[ECrossDirType.LeftDown][3] = Right.RightUp;
                CrossRemoveCell[ECrossDirType.LeftDown][4] = Right.Right;

                CrossAddCell[ECrossDirType.LeftDown] = new AOICell[5];
                CrossAddCell[ECrossDirType.LeftDown][0] = LeftUp;
                CrossAddCell[ECrossDirType.LeftDown][1] = Left;
                CrossAddCell[ECrossDirType.LeftDown][2] = LeftDown;
                CrossAddCell[ECrossDirType.LeftDown][3] = Down;
                CrossAddCell[ECrossDirType.LeftDown][4] = RightDown;

                CrossNoChangeCell[ECrossDirType.LeftDown] = new AOICell[4];
                CrossNoChangeCell[ECrossDirType.LeftDown][0] = Up;
                CrossNoChangeCell[ECrossDirType.LeftDown][1] = RightUp;
                CrossNoChangeCell[ECrossDirType.LeftDown][2] = this;
                CrossNoChangeCell[ECrossDirType.LeftDown][3] = Right;
            }
            //RightDown
            {
                CrossRemoveCell[ECrossDirType.RightDown] = new AOICell[5];
                CrossRemoveCell[ECrossDirType.RightDown][0] = LeftUp.LeftUp;
                CrossRemoveCell[ECrossDirType.RightDown][1] = LeftUp.Up;
                CrossRemoveCell[ECrossDirType.RightDown][2] = Up.Up;
                CrossRemoveCell[ECrossDirType.RightDown][3] = Left.LeftUp;
                CrossRemoveCell[ECrossDirType.RightDown][4] = Left.Left;

                CrossAddCell[ECrossDirType.RightDown] = new AOICell[5];
                CrossAddCell[ECrossDirType.RightDown][0] = RightUp;
                CrossAddCell[ECrossDirType.RightDown][1] = Right;
                CrossAddCell[ECrossDirType.RightDown][2] = RightDown;
                CrossAddCell[ECrossDirType.RightDown][3] = Down;
                CrossAddCell[ECrossDirType.RightDown][4] = LeftDown;

                CrossNoChangeCell[ECrossDirType.RightDown] = new AOICell[4];
                CrossNoChangeCell[ECrossDirType.RightDown][0] = LeftUp;
                CrossNoChangeCell[ECrossDirType.RightDown][1] = Up;
                CrossNoChangeCell[ECrossDirType.RightDown][2] = Left;
                CrossNoChangeCell[ECrossDirType.RightDown][3] = this;
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