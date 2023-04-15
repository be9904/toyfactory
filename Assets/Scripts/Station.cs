using System;
using UnityEngine;

namespace AttnKare
{
    public abstract class Station : MonoBehaviour
    {
        protected ObjectPool itemPool;

        [Header("Spawner Properties")]
        [SerializeField] protected GameObject item;
        [SerializeField] protected Transform spawnPosition;
        [SerializeField] protected Transform spawnParent;

        // spawn item
        public void Spawn()
        {
            GameObject obj = itemPool?.Dequeue();
            if (obj != null)
            {
                obj.SetActive(true);
                return;
            }
        
            Debug.Log(gameObject.name + " Pool is empty");
        }
    }
}
