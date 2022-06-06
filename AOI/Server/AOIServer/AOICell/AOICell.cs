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
        private AOIMgr mgr;

        public AOICell(int xIndex, int zIndex, AOIMgr mgr)
        {
            XIndex = xIndex;
            ZIndex = zIndex;
            this.mgr = mgr;
        }
    }
}
