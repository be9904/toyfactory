using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AttnKare
{
    public class Painter : MonoBehaviour
    {
        private static readonly int triggerID = Animator.StringToHash("Trigger_Painter");

        private Animator painterAnimator;
        private AnimatorClipInfo[] animationClips;

        // Start is called before the first frame update
        void Start()
        {
            painterAnimator = GetComponent<Animator>();
            animationClips = painterAnimator.GetCurrentAnimatorClipInfo(0);
        }

        // Update is called once per frame
        void Update()
        {
            if(Input.GetKeyDown(KeyCode.LeftControl))
                painterAnimator.SetTrigger(triggerID);
        }
    }
}
