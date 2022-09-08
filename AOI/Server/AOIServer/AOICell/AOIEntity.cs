using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AOICell
{
    public class AOIEntity
    {
        public int EntityId { get; private set; }

        private AOIMgr mgr;

        public int XIndex { get; private set; }
        public int ZIndex { get; private set; }
        public int XLastIndex { get; private set; }
        public int ZLastIndex { get; private set; }
        public string CellKey { get; private set; } = "";
        public string LastCellKey { get; private set; } = "";
        public float PosX { get; private set; } 
        public float PosZ { get; private set; } 
        public EMoveType MoveType { get; private set; }

        private CellUpdateData entityUpdateData;
        //存量视野九宫格
        private AOICell[] aroundCell = null;

        public AOIEntity(int entityId,AOIMgr mgr)
        {
            EntityId = entityId;
            this.mgr = mgr;
            entityUpdateData = new CellUpdateData();
        }
        
        public void UpdatePos(float x,float z,EMoveType moveType= EMoveType.None)
        {
            PosX = x;
            PosZ = z;

            //计算所在格二维坐标
            int _xIndex = (int)Math.Floor(PosX / mgr.CellSize);
            int _zIndex = (int)Math.Floor(PosZ / mgr.CellSize);
            string _cellKey = $"{_xIndex},{_zIndex}";
            if (CellKey != _cellKey)
            {
                XLastIndex = XIndex;
                ZLastIndex = ZIndex;
                LastCellKey = CellKey;

                XIndex = _xIndex;
                ZIndex = _zIndex;
                CellKey = _cellKey;

                MoveType = moveType;

                if (MoveType != EMoveType.TransferEnter&& MoveType != EMoveType.TransferOut)
                {
                    moveType = EMoveType.MoveCross;
                }

                mgr.MoveCrossCell(this);
            }
            else
            {
                MoveType = EMoveType.MoveInside;
                mgr.MoveInsideCell(this);
            }
        }
        //对实体宫格视野存量进行增删变化
        public void CalculateViewChange()
        {
            if (aroundCell != null)
            {
                for (int i = 0; i < aroundCell.Length; i++)
                {
                    var set = aroundCell[i].HoldEntity;
                    foreach (var e in set)
                    {
                        entityUpdateData.enterList.Add(new EnterData(e.EntityId, e.PosX, e.PosZ));
                    }
                }
                if (!entityUpdateData.IsEmpty)
                {
                    mgr.EntityViewChangeEvent?.Invoke(this, entityUpdateData);
                    entityUpdateData.Clear();
                }
            }
            aroundCell = null;
        }

        public void SetAroundCell(AOICell[] around)
        {
            aroundCell = around;
        }
    }
    public enum EMoveType
    {
        None,
        TransferEnter,//传送进入
        TransferOut,//传送离开
        MoveInside,//在格子内移动
        MoveCross//跨格移动
    }
}
