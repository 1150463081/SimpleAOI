using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PEUtils;

namespace AOIClient
{
    public  class Pool<T>
        where T: class
    {
        Queue<T> queue=new Queue<T>();

        public Action<T> OnSpawnEvent;
        public Action<T> OnDeSpawnEvent;
        public Func<T> OnGenerateEvent;

        public T Spawn()
        {
            if (queue.Count > 0)
            {
                var go= queue.Dequeue();
                OnSpawnEvent?.Invoke(go);
                return go;
            }
            else
            {
                T go = OnGenerateEvent?.Invoke();
                return go;
            }
        }
        public void DeSpawn(T item)
        {
            if (item == null)
            {
                PELog.Error($"物品为空，无法回收");
            }
            OnDeSpawnEvent?.Invoke(item);
            queue.Enqueue(item);
        }

    }
}
