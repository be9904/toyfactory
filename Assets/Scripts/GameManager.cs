using System;
using System.Collections.Generic;
using UnityEngine;

namespace AttnKare
{
    public class GameManager : MonoBehaviour
    {
        public static GameManager main { get; private set; }

        private GameSystem gameSystem;
        [SerializeField] private List<StageSettings> settingsList;

        public int stageCounter;
        public StageSettings currentStageSettings;

        // public event Action<GameState> UpdateGameState;

        private void Awake()
        {
            // singleton initialization
            if (main != null && main != this)
            {
                Debug.Log(
                    "Singleton instance of " + GetType().Name + " already exists. " + 
                    "Destroying instance " + gameObject.GetInstanceID() + "."
                    );
                Destroy(this);
            }
            else
            {
                main = this;
                gameSystem = gameObject.AddComponent<GameSystem>();

                if (settingsList.Count == 0)
                    Debug.Log(GetType() + " : No Stage Settings Profile");
                else
                    gameSystem.Init(settingsList[0]);
            }
        }

        private void Update()
        {
            // stage ready (waiting -> playing)
            if (Input.GetKeyDown(KeyCode.Space))
            {
                gameSystem.OnStageReady?.Invoke(PrepareStage);
            }

            // stage end (playing -> waiting)
            if (Input.GetKeyDown(KeyCode.LeftControl))
            {
                gameSystem.OnStageEnd?.Invoke(FinishStage);
            }
        }

        // Waiting -> Playing
        void PrepareStage()
        {
            // load stage settings
            LoadStageSettings();
            // reset player stats
            // start robot, box spawn sequence
        }
        
        // Playing -> Waiting
        void FinishStage()
        {
            // save stage status
            SaveStageInfo();
            // stop robot, box spawn sequence
        }

        void SaveStageInfo()
        {
            stageCounter++;
            if(stageCounter > 2)
                gameSystem.End();
            // Debug.Log("Saved Stage Info");
        }
        
        void LoadStageSettings()
        {
            if (stageCounter < settingsList.Count)
                currentStageSettings = settingsList[stageCounter];
            else
                Debug.Log("Invalid stage counter: " + stageCounter);
        }
    }
}
