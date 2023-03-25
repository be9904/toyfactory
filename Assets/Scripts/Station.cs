using UnityEngine;

namespace AttnKare
{
    public class Station : MonoBehaviour
    {
        private ObjectPool itemPool;

        [Header("Spawner Properties")]
        [Header("Spawn End")]
        [SerializeField] private bool useSpawnEnd = true;
        [SerializeField] private GameObject item;
        [SerializeField] private Transform spawnPosition;
        [SerializeField] private Transform spawnParent;
    
        [Header("Destroy End")]
        [SerializeField] private bool useDestroyEnd;
        [SerializeField] private BoxCollider destroyDetector;
        
        // Start is called before the first frame update
        void Start()
        {
            // init spawn end
            if (useSpawnEnd)
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

            // init destroy end
            if (useDestroyEnd)
            {
                // setup destroy detector
                if (destroyDetector == null)
                {
                    destroyDetector = GetComponent<BoxCollider>();
                }
                // Debug.Log(destroyDetector.name);
            }
        }

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
