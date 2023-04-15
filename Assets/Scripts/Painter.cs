using System;
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
        [SerializeField] private bool painterUp;
        [SerializeField] private bool painterDown;

        private GameManager.RobotColor currentColor;

        public bool PainterUp => painterUp;
        public bool PainterDown => painterDown;

        private void Start()
        {
            timer = 0.5f;
            Vector3.Lerp(lowerLimit.localPosition, upperLimit.localPosition, timer);
        }

        // Update is called once per frame
        void Update() => MovePainter();

        void MovePainter()
        {
            // decrease lerp factor when lever is down
            if (painterControl.isActivated && painterControl.LeverPercentage < painterActivationThreshold)
                timer -= Time.deltaTime * painterMoveSpeed;
            
            // increase lerp factor when lever is up
            if (painterControl.isActivated && 100f - painterControl.LeverPercentage < painterActivationThreshold)
                timer += Time.deltaTime * painterMoveSpeed;

            // clamp lerp factor
            if (timer < 0)
            {
                timer = 0;
                painterDown = true;
            }
            else if (timer > 1)
            {
                timer = 1;
                painterUp = true;
            }
            else if (timer > 0 && timer < 1)
            {
                painterDown = false;
                painterUp = false;
            }
            
            // move painter when lerp factor is in range
            if (timer >= 0 && timer <= 1)
            {
                transform.localPosition = Vector3.Lerp(lowerLimit.localPosition , upperLimit.localPosition,
                    timer);
            }
        }

        public void ChooseColor(GameManager.RobotColor color)
        {
            currentColor = color;
        }
    }
}
