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
        


        public AOICell(int xIndex, int zIndex, AOIMgr mgr)
        {
            XIndex = xIndex;
            ZIndex = zIndex;
            this.mgr = mgr;
        }
        public void EnterCell(AOIEntity entity)
        {

        }
        public void MoveCell(AOIEntity entity)
        {

        }
        //计算边界
        public void CalcuBoundary()
        {
            if (IsCalcuBoundary)
                return;
            AOIGround = new AOICell[9];
            int index = 0;
            for (int i = XIndex-2; i <= XIndex+2; i++)
            {
                for (int j = ZIndex-2; j <= ZIndex+2; j++)
                {
                    if(!mgr.HasCell(i,j))
                    {
                        mgr.CreateCell(i, j);
                    }
                    if(i>XIndex-2&&
                        i<XIndex+2&&
                        j>ZIndex-2&&
                        j < ZIndex + 2)
                    {
                        AOIGround[index++] = mgr.GetOrCreateCell(i, j);
                    }
                }
            }
            IsCalcuBoundary = true;
        }
    }
}
