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
        public bool IsPaintable => robotIn && painterDown;

        [SerializeField] private Robot robotToPaint;
        private float paintSpeed;
        private bool robotIn;

        private void OnEnable()
        {
            Conveyor.RobotInPosition += IsRobotInPosition;
        }

        private void OnDisable()
        {
            Conveyor.RobotInPosition -= IsRobotInPosition;
        }

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

        public void SetPaintSpeed(float speed)
        {
            paintSpeed = speed;
        }

        void IsRobotInPosition(bool isIn, GameObject robot)
        {
            robotIn = isIn;
            robotToPaint = robot.GetComponent<Robot>();
        }

        public void OnPaintButton()
        {
            Debug.Log("Paint Button is Down!");
            
            // play vfx
            
            // increase paint percentage of robot
            robotToPaint.PaintRobot(paintSpeed);
        }

        /*private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.CompareTag("Robot"))
            {
                Debug.Log(other.gameObject.name + " is in");
                robotIn = true;
                robotToPaint = other.gameObject.GetComponent<Robot>();
            }
        }
        
        private void OnTriggerExit(Collider other)
        {
            if (other.gameObject.CompareTag("Robot"))
            {
                robotIn = false;
                robotToPaint = null;
            }
        }*/
    }
}
