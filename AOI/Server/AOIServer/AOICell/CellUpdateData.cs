using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AOICell
{
    public class CellUpdateData
    {
        public List<EnterData> enterList;
        public List<MoveData> moveList;
        public List<ExitData> exitList;
        public CellUpdateData()
        {
            enterList = new List<EnterData>();
            moveList = new List<MoveData>();
            exitList = new List<ExitData>();
        }
        public void Clear()
        {
            enterList.Clear();
            moveList.Clear();
            exitList.Clear();
        }
    }
    public struct EnterData
    {
        public int Id;
        public int x;
        public int z;
        public EnterData(int id,int x,int z)
        {
            this.Id = id;
            this.x = x;
            this.z = z;
        }
    }
    public struct MoveData
    {
        public int Id;
        public int x;
        public int z;
        public MoveData(int id, int x, int z)
        {
            this.Id = id;
            this.x = x;
            this.z = z;
        }
    }
    public struct ExitData
    {
        public int Id;

        public ExitData(int id)
        {
            this.Id = id;

        }
    }
}
