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
    }
}
