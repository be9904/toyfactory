using System;
using System.Collections;
using System.Collections.Generic;
using AttnKare.Interactables;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.VFX;

namespace AttnKare
{
    public class Painter : MonoBehaviour
    {
        [SerializeField] private Lever painterControl;
        [SerializeField] private Transform upperLimit;
        [SerializeField] private Transform lowerLimit;
        [SerializeField] private float painterActivationThreshold;
        [SerializeField] private float painterMoveSpeed;

        private float timer;
        [SerializeField] private bool painterUp;
        [SerializeField] private bool painterDown;

        private GameManager.RobotColor previousColor;
        private GameManager.RobotColor currentColor;
        [SerializeField] private List<Material> robotMaterials;

        public bool PainterUp => painterUp;
        public bool PainterDown => painterDown;
        public bool IsPaintable => robotIn && PainterDown;

        private Robot robotToPaint;
        private float paintSpeed;
        private bool robotIn;
        public Action<float> UpdateRobotPaintProgress;
        public Slider progressBar;

        [SerializeField] List<VisualEffect> vfxAssets;
        

        private void OnEnable()
        {
            Conveyor.RobotInPosition += IsRobotInPosition;
            HingeHelper.OnKnobTurn += SetPaintColor;
            GameManager.EndGame += DisablePainter;
        }

        private void OnDisable()
        {
            Conveyor.RobotInPosition -= IsRobotInPosition;
            HingeHelper.OnKnobTurn -= SetPaintColor;
            GameManager.EndGame -= DisablePainter;
        }

        private void Start()
        {
            timer = 0.5f;
            Vector3.Lerp(lowerLimit.localPosition, upperLimit.localPosition, timer);
            SprayPaint(GameManager.RobotColor.YELLOW);
        }

        // Update is called once per frame
        void Update()
        {
            MovePainter();
        }
            

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

        public void SetPaintColor(GameManager.RobotColor color)
        {
            if (painterDown)
            {
                currentColor = color;

                // reset progress on color change
                if (currentColor != previousColor && robotToPaint)
                {
                    robotToPaint.PaintProgress = 0f;
                    
                    // reset to default color
                    if (robotToPaint.Color != GameManager.RobotColor.NONE)
                    {
                        robotToPaint.ResetRobot(robotToPaint.gameObject);
                        for(int i = 0 ; i < 10000 ; i++)
                            SprayPaint(GameManager.RobotColor.NONE);
                    }
                }

                previousColor = currentColor;
            }
        }

        void SprayPaint(GameManager.RobotColor color)
        {
            vfxAssets[(int)color].Play();
        }

        public void SetPainterMoveSpeed(float speed)
        {
            painterMoveSpeed = speed;
        }

        void IsRobotInPosition(bool isIn, GameObject robot)
        {
            robotIn = isIn;
            if (robot)
            {
                robotToPaint = robot.GetComponent<Robot>();
                UpdateRobotPaintProgress?.Invoke(robotToPaint.PaintProgress);
            }
        }

        public void OnPaintButton()
        {
            if (robotToPaint && IsPaintable)
            {
                // play vfx
                SprayPaint(currentColor);

                // update progress
                UpdateRobotPaintProgress?.Invoke(robotToPaint.PaintProgress);
            
                // increase paint percentage of robot   
                robotToPaint.SetColor(currentColor);
                robotToPaint.PaintRobot(paintSpeed);
            }

            if (robotToPaint && robotToPaint.PaintProgress > 100)
            {
                // robotToPaint.PaintProgress = 100;
                
                if (robotToPaint.Color == GameManager.RobotColor.YELLOW)
                    robotToPaint.gameObject.GetComponent<Renderer>().material = robotMaterials[0];
                else if (robotToPaint.Color == GameManager.RobotColor.GREEN)
                    robotToPaint.gameObject.GetComponent<Renderer>().material = robotMaterials[1];
                else if (robotToPaint.Color == GameManager.RobotColor.BLUE)
                    robotToPaint.gameObject.GetComponent<Renderer>().material = robotMaterials[2];
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.gameObject.CompareTag("Robot"))
            {
                Robot r = other.gameObject.GetComponent<Robot>();
                if (r.PaintProgress >= 100)
                {
                    robotToPaint.IsPainted = true;
                    GameManager.RobotSpawnEvent?.Invoke();
                }
            }
        }

        void DisablePainter()
        {
            enabled = false;
        }
    }
}
