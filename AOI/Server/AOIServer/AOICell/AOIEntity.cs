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

        private AOICell[] aroundCell = null;

        public AOIEntity(int entityId,AOIMgr mgr)
        {
            EntityId = entityId;
            this.mgr = mgr;
        }
        
        public void UpdatePos(float x,float z,EMoveType moveType= EMoveType.None)
        {
            PosX = x;
            PosZ = z;

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

        public void SetAroundCell(AOICell[] around)
        {
            aroundCell = around;
        }
    }
    public enum EMoveType
    {
        None,
        TransferEnter,
        TransferOut,
        MoveInside,
        MoveCross
    }
}
