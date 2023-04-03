using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AttnKare
{
    public abstract class State : MonoBehaviour
    {
        public virtual IEnumerator Keep()
        {
            yield break;
        }
        
        public virtual IEnumerator Transition()
        {
            yield break;
        }
    }
}
