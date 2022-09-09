using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PEUtils;

namespace AOICell
{ 
    //核心思路，存量增删(针对AOIEntity)，增量叠加(针对AOICell)
    public static class AOICfg
    {
        public static int cellSize = 20;
        public static int cellCount = 100;
        public static int moveSpeed = 40;
    }
    public class AOIMgr
    {
        public int CellSize => AOICfg.cellSize;
        public int CellCount => AOICfg.cellCount;
        public Action<AOIEntity, CellUpdateData> EntityViewChangeEvent;
        public Action<AOICell, CellUpdateData> CellEntityOpCombineEvent;

        public Dictionary<string, AOICell> CellDict;
        private List<AOIEntity> entityList;

        public AOIMgr()
        {
            CellDict = new Dictionary<string, AOICell>();
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
            if(CellDict.TryGetValue(entity.CellKey,out var cell))
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
            foreach (var cell in CellDict.Values)
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
            if (!CellDict.ContainsKey(key))
            {
                CreateCell(xIndex, zIndex);
            }
            return CellDict[key];
        }
        public bool HasCell(int xIndex,int zIndex)
        {
            var key = GetCellKey(xIndex, zIndex);
            return CellDict.ContainsKey(key);
        }
        public void CreateCell(int xIndex, int zIndex)
        {
            var key = GetCellKey(xIndex, zIndex);
            if (!CellDict.TryGetValue(key, out var cell))
            {
                cell = new AOICell(xIndex, zIndex, this);
                CellDict[key] = cell;
            }
        }
        public string GetCellKey(int xIndex,int zIndex)
        {
            return $"{xIndex},{zIndex}";
        }
    }
}
