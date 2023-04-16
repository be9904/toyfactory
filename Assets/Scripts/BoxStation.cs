using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AttnKare
{
    public class BoxStation : Station
    {
        private void OnEnable()
        {
            GameManager.BoxSpawnEvent += Spawn;
            GameManager.BoxDestroyEvent += Destroy;
        }

        private void OnDisable()
        {
            GameManager.BoxSpawnEvent -= Spawn;
            GameManager.BoxDestroyEvent -= Destroy;
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
                return true;
            }

            // Debug.Log("Robot Color: " + robot.Color + ", Current Color: " + GameManager.main.currentColor);
            return false;
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.CompareTag("Robot"))
            {
                // enqueue robot to pool
                GameObject o;
                Robot r = (o = other.gameObject).GetComponent<Robot>();
                GameManager.main.existingRobots--;
                GameManager.RobotDestroyEvent?.Invoke(o);
                
                // if robot is painted with right color, spawn box
                if (CheckRobot(r))
                {
                    GameManager.BoxSpawnEvent?.Invoke();
                }
            }
        }
    }
}
