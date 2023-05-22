using UnityEngine;
using AttnKare.Utilities;

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
        public abstract void Spawn();

        // destroy item
        public abstract void Destroy(GameObject obj);

        public abstract void DisableStation();
    }
}
