using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace AttnKare
{
    public class UIManager : MonoBehaviour
    {
        [SerializeField] private List<Sprite> spriteImages;
        [SerializeField] private Image mainImage;

        private void OnEnable()
        {
            GameManager.RobotSpawnEvent += ChangeIcon;
        }
        
        private void OnDisable()
        {
            GameManager.RobotSpawnEvent -= ChangeIcon;
        }

        public void ChangeIcon()
        {
            if(GameManager.main.currentColor == GameManager.RobotColor.YELLOW)
                mainImage.sprite = spriteImages[0];
            else if(GameManager.main.currentColor == GameManager.RobotColor.GREEN)
                mainImage.sprite = spriteImages[1];
            else if(GameManager.main.currentColor == GameManager.RobotColor.BLUE)
                mainImage.sprite = spriteImages[2];
        }
    }
}
