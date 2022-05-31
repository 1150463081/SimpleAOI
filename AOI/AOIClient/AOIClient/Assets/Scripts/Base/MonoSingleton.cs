using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace AOIClient
{
    public class MonoSingleton<T>:MonoBehaviour
        where T: MonoBehaviour
    {
        private static T instance;
        public static T Instance
        {
            get {
                if (instance == null)
                {
                    var go = new GameObject(typeof(T).Name);
                    instance = go.AddComponent<T>();
                }
                return instance;
            }
        }
        private void Awake()
        {
            DontDestroyOnLoad(gameObject);
            OnInit();
        }
        private void Start()
        {
            OnStart();
        }
        private void OnDestroy()
        {
            OnTermination();
        }
        public void Init()
        {

        }
        protected virtual void OnInit()
        {

        }
        protected virtual void OnStart()
        {

        }
        protected virtual void OnTermination()
        {

        }

    }
}
