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
        public RoleEntity MainPlayer { get; private set; }
        private Pkg_S2CLogin mLoginPkg;

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
            if (roleId == MyRoleId)
            {
                MainPlayer = role;
            }
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
        public void SetMyRoleData(Pkg_S2CLogin pkg)
        {
            mLoginPkg = pkg;
            MyRoleId = pkg.roleId;
        }

        private void FixedUpdate()
        {
            if (MyRoleId != 0 && MainPlayer != null)
            {
                float x = Input.GetAxis("Horizontal");
                float y = Input.GetAxis("Vertical");
                Vector3 dir = new Vector3(x, 0, y);
                if (dir != Vector3.zero)
                {
                    MainPlayer.gameObject.transform.position += dir * mLoginPkg.moveSpeed * Time.fixedDeltaTime;
                    Pkg_C2SEntityMove pkg = new Pkg_C2SEntityMove()
                    {
                        operateCode = OperateCode.C2SEntityMove,
                        entityId = MyRoleId,
                        posX = MainPlayer.gameObject.transform.position.x,
                        posY = MainPlayer.gameObject.transform.position.z
                    };
                    NetManager.Instance.SendMsg(pkg);
                }
            }
        }
    }
}
