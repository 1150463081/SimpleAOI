using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace AOIClient
{
    public class RoleEntity
    {
        public GameObject gameObject { get; private set; }

        public RoleEntity(GameObject prefab)
        {
            gameObject = GameObject.Instantiate(prefab);
            gameObject.transform.position = Vector3.zero;
        }

        public void UpdatePos(float x,float z)
        {
            gameObject.transform.position = new Vector3(x, gameObject.transform.position.y, z);
        }
        public void OnSpawn()
        {
            gameObject.SetActive(true);
        }
        public void OnDeSpawn()
        {
            gameObject.SetActive(false);
        }
    }
}
