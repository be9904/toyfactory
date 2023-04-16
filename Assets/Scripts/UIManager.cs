using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace AttnKare
{
    public class UIManager : MonoBehaviour
    {
        [Header("Main UI Canvas")]
        [SerializeField] private GameObject mainCanvas;
        [SerializeField] private List<Sprite> spriteImages;
        [SerializeField] private Image mainImage;
        [SerializeField] private Text gameOverText;
        [SerializeField] private Text scoreText;
        [SerializeField] private Text clock;

        [Header("Warning UI Canvas")] 
        [SerializeField] private GameObject warningCanvas;

        private void OnEnable()
        {
            GameManager.RobotSpawnEvent += ChangeIcon;
            GameManager.EndGame += ShowGameOverText;
        }
        
        private void OnDisable()
        {
            GameManager.RobotSpawnEvent -= ChangeIcon;
            GameManager.EndGame -= ShowGameOverText;
        }

        private void Start()
        {
            gameOverText.gameObject.SetActive(false);
            scoreText.gameObject.SetActive(false);
        }

        private void Update()
        {
            UpdateClock();
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

        public void ShowGameOverText()
        {
            mainImage.gameObject.SetActive(false);
            gameOverText.gameObject.SetActive(true);
            scoreText.text = "Score : " + GameManager.main.gameStats.robotCount;
            scoreText.gameObject.SetActive(true);
        }

        void UpdateClock()
        {
            if (GameManager.main.currentGameSettings != null)
            {
                int time = (int)GameManager.main.currentGameSettings.timeLimit - (int)GameManager.main.timer;
                if (time < 0)
                    return;
                int minutes = time / 60;
                int seconds = time % 60;

                clock.text = minutes.ToString("00") + ":" + seconds.ToString("00");
            }
        }

        public void DisableWarningUI()
        {
            StartCoroutine(AutoCloseWarningUI());
        }

        IEnumerator AutoCloseWarningUI()
        {
            yield return new WaitForSeconds(5f);
            warningCanvas.SetActive(false);
            mainCanvas.SetActive(true);
        }
    }
}
