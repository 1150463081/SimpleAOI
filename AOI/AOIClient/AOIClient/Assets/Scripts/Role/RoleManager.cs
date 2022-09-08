using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AOICellProtocol;
using UnityEngine;

namespace AOIClient
{
    public class RoleManager:MonoSingleton<RoleManager>
    {
        private Dictionary<int, RoleEntity> roleDict = new Dictionary<int, RoleEntity>();
        private Pool<RoleEntity> rolePool = new Pool<RoleEntity>();
        private GameObject prefab;

        public int MyRoleId { get; private set; }

        protected override void OnInit()
        {
            prefab = Resources.Load<GameObject>("Cube");
            rolePool.OnGenerateEvent += () =>
            {
                return new RoleEntity(prefab);
            };
            rolePool.OnSpawnEvent += (RoleEntity r) =>
            {
                r.OnSpawn();
            };
            rolePool.OnDeSpawnEvent += (RoleEntity r) =>
            {
                r.OnDeSpawn();
            };
        }
        protected override void OnTermination()
        {
            
        }

        public void AddRole(int roleId,float posX,float posZ )
        {
            var role= rolePool.Spawn();
            role.gameObject.name = roleId.ToString();
            roleDict[roleId] = role;
            role.UpdatePos(posX, posZ);
        }
        public void RemoveRole(int roleId)
        {
            if (roleDict.ContainsKey(roleId))
            {
                rolePool.DeSpawn(roleDict[roleId]);
                roleDict.Remove(roleId);
            }
        }
        public RoleEntity GetRole(int roleId)
        {
            if (roleDict.ContainsKey(roleId))
            {
                return roleDict[roleId];
            }
            return null;
        }
        public void SetMyRoleId(int roleId)
        {
            MyRoleId = roleId;
        }
    }
}
