using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PEUtils;

namespace AOICell
{ 
    //核心思路，存量增删，增量叠加
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
        //某个实体穿越了边界 
        public void MoveCrossCell(AOIEntity entity)
        {
            var cell = GetOrCreateCell(entity.XIndex,entity.ZIndex);
            if(!cell.IsCalcuBoundary)
            {
                cell.CalcuBoundary();
            }
            cell.EnterCell(entity);
        }
        //边界内移动
        public void MoveInsideCell(AOIEntity entity)
        {
            if (HasCell(entity.XIndex, entity.ZIndex))
            {
                var cell = GetOrCreateCell(entity.XIndex, entity.ZIndex);
                cell.MoveCell(entity);
            }
            else
            {
                PELog.Error($"不存在地图块[{entity.XIndex},{entity.ZIndex}]");
            }
        }

        public AOICell GetOrCreateCell(int xIndex, int zIndex)
        {
            var key = GetCellKey(xIndex, zIndex);
            if (!cellDict.ContainsKey(key))
            {
                CreateCell(xIndex, zIndex);
            }
            return cellDict[key];
        }
        public bool HasCell(int xIndex,int zIndex)
        {
            var key = GetCellKey(xIndex, zIndex);
            return cellDict.ContainsKey(key);
        }
        public void CreateCell(int xIndex, int zIndex)
        {
            var key = GetCellKey(xIndex, zIndex);
            if (!cellDict.TryGetValue(key, out var cell))
            {
                cell = new AOICell(xIndex, zIndex, this);
                cellDict[key] = cell;
            }
        }
        public string GetCellKey(int xIndex,int zIndex)
        {
            return $"{xIndex},{zIndex}";
        }
    }
}
