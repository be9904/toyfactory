using System;
using System.Collections.Generic;
using UnityEngine;

namespace AttnKare
{
    public class GameManager : MonoBehaviour
    {
        public static GameManager main { get; private set; }

        private GameSystem gameSystem;
        [SerializeField] private float timer;
        [SerializeField] private List<GameSettings> settingsList;

        public int stageCounter;
        public GameSettings currentGameSettings;
        private int existingRobots = 0;

        public static Action<Action> StartStage;
        public static Action<Action> EndStage;
        
        public static Action RobotSpawnSequence;
        public static Action BoxSpawnSequence;

        [Header("Reference to GameObjects")] 
        [SerializeField] private Painter robotPainter;
        [SerializeField] private Station robotStation;
        [SerializeField] private Station boxStation;

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
            /*// stage ready (waiting -> playing)
            if (Input.GetKeyDown(KeyCode.Space))
            {
                gameSystem.OnStageReady?.Invoke(PrepareStage);
            }*/

            /*// stage end (playing -> waiting)
            if (Input.GetKeyDown(KeyCode.LeftControl))
            {
                gameSystem.OnStageEnd?.Invoke(FinishStage);
            }*/
            
            SpawnRobot();
            // SpawnBox();

            if (!gameSystem.IsGameOver() && !gameSystem.IsWaiting())
                if(timer <= currentGameSettings.timeLimit)
                    timer += Time.deltaTime;
        }

        #region STAGE_MANAGEMENT

        /// <summary>
        /// State changes from WAITING to PLAYING
        /// </summary>
        void PrepareStage()
        {
            // load stage settings
            LoadStageSettings();
            // reset player stats
            // start robot, box spawn sequence
        }
        
        /// <summary>
        /// State changes from PLAYING to WAITING
        /// </summary>
        void FinishStage()
        {
            // save stage status
            SaveStageInfo();
            // stop robot, box spawn sequence
        }

        /// <summary>
        /// Saves stage info when stage ends.
        /// </summary>
        void SaveStageInfo()
        {
            stageCounter++;
            if(stageCounter > 2)
                gameSystem.End();
            // Debug.Log("Saved Stage Info");
        }
        
        /// <summary>
        /// Loads stage settings from list according to stage counter
        /// </summary>
        void LoadStageSettings()
        {
            if (stageCounter < settingsList.Count)
                currentGameSettings = settingsList[stageCounter];
            else
                Debug.Log(GetType() + " : Invalid stage counter: " + stageCounter);

        }
        #endregion

        #region STAGE_FUNCTIONS

        /// <summary>
        /// All the work that needs to be done to start the process of making a robot.
        /// </summary>
        public void SpawnRobot()
        {
            if (gameSystem.IsWaiting() && robotPainter.PainterUp)
            {
                StartStage?.Invoke(PrepareStage);
            }
                
            if (existingRobots == 0 && robotPainter.PainterUp)
            {
                Debug.Log("Invoke SpawnRobot()");
                
                existingRobots++;
                RobotSpawnSequence?.Invoke();
            }
        }

        /// <summary>
        /// All the work that needs to be done to start the process of making a box.
        /// <br/>
        /// Enqueue robot to robot pool and dequeue box from box pool. 
        /// </summary>
        public void SpawnBox()
        {
            if (existingRobots == 1 && robotPainter.PainterUp)
            {
                Debug.Log("Invoke SpawnBox()");
                
                existingRobots--;
                BoxSpawnSequence?.Invoke();
            }
        }

        #endregion
    }
}
