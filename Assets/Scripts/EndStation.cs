using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AttnKare
{
    public class EndStation : Station
    {
        private void OnEnable()
        {
            GameManager.BoxDestroyEvent += Destroy;
        }
        
        private void OnDisable()
        {
            GameManager.BoxDestroyEvent -= Destroy;
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.CompareTag("Box"))
            {
                if (other.gameObject.GetComponent<Box>().IsProperBox)
                {
                    // increment success count
                    GameManager.main.gameStats.boxCount++;
                    // play success sound
                }
            }
        }
    }
}
