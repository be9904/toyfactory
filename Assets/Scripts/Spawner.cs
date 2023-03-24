using UnityEngine;

public class Spawner : MonoBehaviour
{
    private ObjectPool itemPool;
    
    [Header("Spawner Properties")] 
    [SerializeField] private GameObject item;
    [SerializeField] private Transform spawnPosition;
    [SerializeField] private Transform spawnParent;

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
