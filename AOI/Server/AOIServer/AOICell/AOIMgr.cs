using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PEUtils;

namespace AOICell
{ 
    //核心思路，存量增删(针对AOIEntity)，增量叠加(针对AOICell)
    public class AOICfg
    {
        public int cellSize = 20;
        public int cellCount = 100;
    }
    public class AOIMgr
    {
        public int CellSize => aoiCfg.cellSize;
        public int CellCount => aoiCfg.cellCount;
        public Action<AOIEntity, CellUpdateData> EntityViewChangeEvent;
        public Action<AOICell, CellUpdateData> CellEntityOpCombineEvent;

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
            AOIEntity entity = new AOIEntity(entityId, this);
            entity.UpdatePos(x, z, EMoveType.TransferEnter);
            entityList.Add(entity);
            return entity;
        }
        public void ExitCell(AOIEntity entity)
        {
            if(cellDict.TryGetValue(entity.CellKey,out var cell))
            {
                cell.ExitCell(entity);
                entityList.Remove(entity);
            }
        }
        public void UpdatePos(AOIEntity entity, float x, float z)
        {
            entity.UpdatePos(x, z);
        }
        public void CalcuateAOIUpdate()
        {
            //驱动实体视野更新,存量增删
            for (int i = 0; i < entityList.Count; i++)
            {
                entityList[i].CalculateViewChange();
            }
            //驱动每个宫格周围宫格操作更新
            foreach (var cell in cellDict.Values)
            {
                if (cell.EnterEntity.Count > 0)
                {
                    cell.HoldEntity.UnionWith(cell.EnterEntity);
                    cell.EnterEntity.Clear();
                }
                cell.CalcCellOpCombine();
            }
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
