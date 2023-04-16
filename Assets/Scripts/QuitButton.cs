using System.Collections;
using System.Collections.Generic;
using UnityEditor.Rendering;
using UnityEngine;
using UnityEngine.UI;

namespace AttnKare
{
    public class QuitButton : MonoBehaviour
    {
        [SerializeField] private Slider progressSlider;
        public static float Progress = 0f;

        private bool buttonPressed;
        
        // Update is called once per frame
        void Update()
        {
            progressSlider.value = Progress / 3f;
            
            if (Progress > 0f && !buttonPressed)
                Progress -= Time.deltaTime;
            else if(Progress > 3f)
                GameManager.QuitGame();
        }

        public void OnButtonPress()
        {
            buttonPressed = true;
            if (Progress < 3f)
            {
                Progress += Time.deltaTime;
            }
        }

        public void OnButtonUp()
        {
            buttonPressed = false;
        }

        public static void ResetProgress()
        {
            Progress = 0f;
        }
    }
}
