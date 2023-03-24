using System.Collections.Generic;
using UnityEngine;

public class ObjectPool
{
    private Queue<GameObject> pool;

    // constructor
    public ObjectPool()
    {
        pool = new Queue<GameObject>();
    }

    public void Enqueue(GameObject item)
    {
        pool.Enqueue(item);
    }

    public GameObject Dequeue()
    {
        GameObject objectToSpawn = pool.Count > 0 ? pool.Dequeue() : null;
        
        if (objectToSpawn == null) return null;
        
        return objectToSpawn;
    }
}
