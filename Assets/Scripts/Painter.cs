using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AttnKare
{
    public class Painter : MonoBehaviour
    {
        [SerializeField] private BNG.Lever painterControl;
        [SerializeField] private Transform upperLimit;
        [SerializeField] private Transform lowerLimit;
        [SerializeField] private float painterActivationThreshold;
        [SerializeField] private float painterMoveSpeed;

        private float timer;
        private bool leverUp;
        private bool leverDown;

        // Update is called once per frame
        void Update() => MovePainter();

        void MovePainter()
        {
            if (leverDown && painterControl.LeverPercentage < painterActivationThreshold)
                timer -= Time.deltaTime * painterMoveSpeed;

            if (leverUp && 100f - painterControl.LeverPercentage < painterActivationThreshold)
                timer += Time.deltaTime * painterMoveSpeed;

            if (timer < 0)
            {
                timer = 0;
            }

            if (timer > 1)
            {
                timer = 1;
            }
            
            if (timer >= 0 && timer <= 1)
            {
                transform.localPosition = Vector3.Lerp(lowerLimit.localPosition , upperLimit.localPosition,
                    timer);
            }
        }

        public void OnLeverUp()
        {
            Debug.Log("Lever Up");
            leverUp = true;
        }

        public void OnLeverDown()
        {
            Debug.Log("Lever Down");
            leverDown = true;
        }
    }
}
