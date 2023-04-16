using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AttnKare
{
    public class RobotStation : Station
    {
        private void OnEnable()
        {
            GameManager.RobotSpawnEvent += Spawn;
            GameManager.RobotDestroyEvent += Destroy;
        }

        private void OnDisable()
        {
            GameManager.RobotSpawnEvent -= Spawn;
            GameManager.RobotDestroyEvent -= Destroy;
        }
        
        // Start is called before the first frame update
        void Start()
        {
            // return if prefab to spawn is null
            if (item == null)
            {
                Debug.Log("Spawn Item is null");
                return;
            }

            // initialize object pool
            itemPool = new ObjectPool();
            for (int i = 0; i < 10; i++)
            {
                GameObject obj = Instantiate(
                    item,
                    spawnPosition.position,
                    Quaternion.Euler(item.transform.eulerAngles),
                    spawnParent
                ); 
                obj.SetActive(false);
                itemPool.Enqueue(obj);
            }
        }
    }
}
