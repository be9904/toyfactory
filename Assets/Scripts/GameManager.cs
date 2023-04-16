using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

namespace AttnKare
{
    public class GameManager : MonoBehaviour
    {
        // Singleton
        public static GameManager main { get; private set; }

        // Game system & settings
        private GameSystem gameSystem;
        [SerializeField] private float timer;
        [SerializeField] private List<GameSettings> settingsList;
        public GameSettings currentGameSettings;

        // Stage settings
        public int stageCounter;
        
        // Game Stats
        public GameStats gameStats;
        
        // Robot properties
        public enum RobotColor{ YELLOW, GREEN, BLUE }
        public RobotColor currentColor;
        public int existingRobots = 0;

        // Stage Events
        public static Action<Action> StartStage;
        public static Action<Action> EndStage;
        
        // Spawn Events
        public static Action RobotSpawnEvent;
        public static Action BoxSpawnEvent;
        public static Action<GameObject> RobotDestroyEvent;
        public static Action<GameObject> BoxDestroyEvent;
        
        // Game Stat Events
        

        // References to other objects
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
            if (Input.GetKeyDown(KeyCode.Space))
            {
                BoxSpawnEvent?.Invoke();
            }

            SpawnRobot();

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
            LoadGameSettings();
            // reset player stats
            // start robot, box spawn sequence
        }
        
        /// <summary>
        /// State changes from PLAYING to WAITING
        /// </summary>
        void FinishGame()
        {
            // save stage status
            SaveGameInfo();
            // stop robot, box spawn sequence
        }

        /// <summary>
        /// Saves stage info when stage ends.
        /// </summary>
        void SaveGameInfo()
        {
            stageCounter++;
            if(stageCounter > 2)
                gameSystem.End();
            // Debug.Log("Saved Stage Info");
        }
        
        /// <summary>
        /// Loads stage settings from list according to stage counter
        /// </summary>
        void LoadGameSettings()
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
            // Setup game settings on initial start
            if (gameSystem.IsWaiting() && robotPainter.PainterUp)
            {
                StartStage?.Invoke(PrepareStage);
                Conveyor.SetSpeed(currentGameSettings.conveyorSpeed);
                robotPainter.SetPaintSpeed(currentGameSettings.paintSpeed);
                robotPainter.SetPainterMoveSpeed(currentGameSettings.painterMoveSpeed);
            }
                
            // Spawn next robot when conveyor is empty
            if (existingRobots == 0 && robotPainter.PainterUp)
            {
                // Debug.Log("GameManager : Spawned Robot");
                
                // color that user needs to choose
                int rndNum = Random.Range(1, 4);
                switch (rndNum)
                {
                    case 1:
                        currentColor = RobotColor.YELLOW;
                        break;
                    case 2:
                        currentColor = RobotColor.GREEN;
                        break;
                    case 3:
                        currentColor = RobotColor.BLUE;
                        break;
                    default:
                        Debug.Log("GameManager : Color Out of Range");
                        break;
                }
                
                existingRobots++;
                RobotSpawnEvent?.Invoke();
            }
        }

        #endregion
    }
}
