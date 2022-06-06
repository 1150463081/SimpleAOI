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

        public AOIEntity(int entityId,AOIMgr mgr)
        {
            EntityId = entityId;
            this.mgr = mgr;
        }
    }
}
