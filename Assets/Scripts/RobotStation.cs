using UnityEngine;
using AttnKare.Utilities;

namespace AttnKare
{
    public class RobotStation : Station
    {
        private void OnEnable()
        {
            GameManager.RobotSpawnEvent += Spawn;
            GameManager.RobotDestroyEvent += Destroy;
            GameManager.EndGame += DisableStation;
        }

        private void OnDisable()
        {
            GameManager.RobotSpawnEvent -= Spawn;
            GameManager.RobotDestroyEvent -= Destroy;
            GameManager.EndGame -= DisableStation;
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


        public override void Spawn()
        {
            GameObject obj = itemPool.Dequeue();
            if (obj != null)
            {
                obj.SetActive(true);
                obj.transform.position = spawnPosition.position;
                return;
            }
        
            Debug.Log(gameObject.name + " Pool is empty");
        }
        
        public override void Destroy(GameObject obj)
        {
            itemPool.Enqueue(obj);
            if (obj != null)
            {
                obj.SetActive(false);
                return;
            }
            
            Debug.Log(gameObject.name + " Pool is FULL");
        }

        public override void DisableStation()
        {
            enabled = false;
        }
    }
}
