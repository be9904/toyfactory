using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace AttnKare
{
    public class ColorWheel : MonoBehaviour
    {
        public GameManager.RobotColor currentColor;
        public Painter robotPainter;
        public Text colorWheelText;

        public Slider progressBar;
        public Image progressBarFill;

        private void OnEnable()
        {
            robotPainter.UpdateRobotPaintProgress += UpdateProgress;
        }

        private void OnDisable()
        {
            robotPainter.UpdateRobotPaintProgress -= UpdateProgress;
        }

        // Update is called once per frame
        void Update()
        {
            GetColor();
            robotPainter.ChooseColor(currentColor);
        }
        
        void GetColor()
        {
            if (colorWheelText.text.Equals("Yellow"))
            {
                currentColor = GameManager.RobotColor.YELLOW;
            }
            else if (colorWheelText.text.Equals("Green"))
            {
                currentColor = GameManager.RobotColor.GREEN;
            }
            else if (colorWheelText.text.Equals("Blue"))
            {
                currentColor = GameManager.RobotColor.BLUE;
            }
        }

        void UpdateProgress(float progress)
        {
            progressBar.value = progress / 100f;
            switch (currentColor)
            {
                case GameManager.RobotColor.YELLOW:
                    progressBarFill.color = new Color(0.984f, 0.773f, 0.192f);
                    break;
                case GameManager.RobotColor.GREEN:
                    progressBarFill.color = new Color(0.267f, 0.741f, 0.196f);
                    break;
                case GameManager.RobotColor.BLUE:
                    progressBarFill.color = new Color(0.29f, 0.62f, 0.906f);
                    break;
            }
        }
    }
}
