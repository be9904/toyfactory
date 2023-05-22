using UnityEngine;
using AttnKare.Utilities;

namespace AttnKare
{
    public class BoxStation : Station
    {
        private void OnEnable()
        {
            GameManager.BoxSpawnEvent += Spawn;
            GameManager.BoxDestroyEvent += Destroy;
            GameManager.EndGame += DisableStation;
        }

        private void OnDisable()
        {
            GameManager.BoxSpawnEvent -= Spawn;
            GameManager.BoxDestroyEvent -= Destroy;
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

        public bool CheckRobot(Robot robot)
        {
            if (robot == null)
                return false;

            if (robot.Color == GameManager.main.currentColor && robot.IsPainted)
            {
                GameManager.main.gameStats.robotCount++;
                DataManager.main.dataList["correctColorRobotCount"] =
                    (int)DataManager.main.dataList["correctColorRobotCount"] + 1;
                return true;
            }

            DataManager.main.dataList["wrongColorRobotCount"] =
                (int)DataManager.main.dataList["wrongColorRobotCount"] + 1;
            return false;
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.gameObject.CompareTag("Robot"))
            {
                // enqueue robot to pool
                GameObject o = other.gameObject;

                // if robot is painted with right color, spawn box
                if (CheckRobot(o.GetComponent<Robot>()))
                {
                    GameManager.BoxSpawnEvent?.Invoke();
                }
                
                GameManager.RobotDestroyEvent?.Invoke(o);
                
                GameManager.RobotDataEvent?.Invoke();
                // GameManager.RobotSpawnEvent?.Invoke();
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
