using System.Collections.Generic;
using UnityEngine;

namespace AttnKare.Utilities
{
    public class ObjectPool
    {
        private Queue<GameObject> pool;

        // constructor
        public ObjectPool()
        {
            pool = new Queue<GameObject>();
        }

        public int GetCount()
        {
            return pool.Count;
        }

        public void Enqueue(GameObject item)
        {
            pool.Enqueue(item);
        }

        public GameObject Dequeue()
        {
            GameObject objectToSpawn = pool.Count > 8 ? pool.Dequeue() : null;
        
            if (objectToSpawn) 
                return objectToSpawn;
            
            return null;
        }
    }
}
