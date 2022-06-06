using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AOICell
{
    public class AOICfg
    {
        public int cellSize = 20;
        public int cellCount = 100;
    }
    public class AOIMgr
    {
        public int CellSize => aoiCfg.cellSize;
        public int CellCount => aoiCfg.cellCount;

        private Dictionary<string, AOICell> cellDict;
        private List<AOIEntity> entityList;
        private AOICfg aoiCfg;
        public AOIMgr(AOICfg cfg)
        {
            aoiCfg = cfg;
            cellDict = new Dictionary<string, AOICell>();
            entityList = new List<AOIEntity>();
        }

        public AOIEntity EnterCell(int entityId,float x,float z)
        {
            return null;
        }
        public void ExitCell(AOIEntity entity)
        {

        }
        public void UpdatePos(int entityId, float x, float z)
        {

        }
    }
}
