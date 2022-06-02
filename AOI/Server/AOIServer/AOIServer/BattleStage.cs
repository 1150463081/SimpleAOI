using System;
using System.Collections.Generic;
using System.Text;
using System.Collections.Concurrent;

namespace AOIServer
{
    public class BattleStage
    {
        ConcurrentDictionary<int, RoleEntity> roleDict = new ConcurrentDictionary<int, RoleEntity>();

        public void Init()
        {

        }
        public void Tick()
        {

        }
        public void Destroy()
        {

        }
        public void AddRoleEntity(RoleEntity roleEntity)
        {
            roleDict[roleEntity.RoleId] = roleEntity;
        }
    }
}
