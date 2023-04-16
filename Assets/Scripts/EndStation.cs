using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AttnKare
{
    public class EndStation : Station
    {
        [SerializeField] private BoxStation boxStation;

        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.CompareTag("Box"))
            {
                GameManager.BoxDestroyEvent?.Invoke(other.gameObject);
                // play success sound
            }
        }
        
        public override void Spawn()
        {
            throw new NotImplementedException();
        }

        public override void Destroy(GameObject obj)
        {
            throw new NotImplementedException();
        }
    }
}
