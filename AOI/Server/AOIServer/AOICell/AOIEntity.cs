using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PEUtils;

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
        public ECrossDirType ECrossDirType { get; private set; }

        private CellUpdateData entityUpdateData;
        //存量视野九宫格,用于第一进格时获取周围格的信息
        private AOICell[] aroundCell = null;
        private List<AOICell> cellAddView = new List<AOICell>();
        private List<AOICell> cellRemoveView = new List<AOICell>();

        public AOIEntity(int entityId, AOIMgr mgr)
        {
            EntityId = entityId;
            this.mgr = mgr;
            entityUpdateData = new CellUpdateData();
        }

        public void UpdatePos(float x, float z, EMoveType moveType = EMoveType.None)
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

                if (MoveType != EMoveType.TransferEnter && MoveType != EMoveType.TransferOut)
                {
                    moveType = EMoveType.MoveCross;
                    #region 穿越边界朝向判断
                    if (XIndex < XLastIndex)
                    {
                        if (ZIndex == ZLastIndex)
                        {
                            ECrossDirType = ECrossDirType.Left;
                        }
                        else if (ZIndex < ZLastIndex)
                        {
                            ECrossDirType = ECrossDirType.LeftDown;
                        }
                        else
                        {
                            ECrossDirType = ECrossDirType.LeftUp;
                        }
                    }
                    else if (XIndex > XLastIndex)
                    {
                        if (ZIndex == ZLastIndex)
                        {
                            ECrossDirType = ECrossDirType.Right;
                        }
                        else if (ZIndex < ZLastIndex)
                        {
                            ECrossDirType = ECrossDirType.RightDown;
                        }
                        else
                        {
                            ECrossDirType = ECrossDirType.RightUp;
                        }
                    }
                    else
                    {
                        if (ZIndex < ZLastIndex)
                        {
                            ECrossDirType = ECrossDirType.Down;
                        }
                        else
                        {
                            ECrossDirType = ECrossDirType.Up;
                        }
                    }
                    #endregion
                }
                PELog.ColorLog(LogColor.Green, $"MoveCross:{_cellKey}");
                mgr.MoveCrossCell(this);
            }
            else
            {
                PELog.ColorLog(LogColor.Yellow, "MoveInside");
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
                        PELog.ColorLog(LogColor.Magenta, "存量视野变化");
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

        public void AddAroundCellView(AOICell[] around)
        {
            aroundCell = around;
        }
        public void AddCellView(AOICell cell)
        {
            cellAddView.Add(cell);
        }
        public void RemoveCellView(AOICell cell)
        {
            cellRemoveView.Add(cell);
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
    //穿越边界时的方向
    public enum ECrossDirType
    {
        None,
        Up,
        Down,
        Right,
        Left,
        LeftUp,
        RightUp,
        LeftDown,
        RightDown,
    }
}
