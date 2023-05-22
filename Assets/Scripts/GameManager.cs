using System;
using System.Collections.Generic;
using AttnKare.Core;
using AttnKare.Data;
using UnityEngine;
using Random = UnityEngine.Random;

namespace AttnKare
{
    public class GameManager : MonoBehaviour
    {
        // Singleton
        public static GameManager main { get; private set; }

        [SerializeField] private UIManager uiManager;
        
        // Game system & settings
        private GameSystem gameSystem;
        public float timer;
        [SerializeField] private List<GameSettings> settingsList;
        public GameSettings currentGameSettings;

        // Stage settings
        public int stageCounter;
        
        // Game Stats
        public GameStats gameStats;
        
        // Robot properties
        public enum RobotColor{ NONE, YELLOW, GREEN, BLUE }
        public RobotColor currentColor;

        // Stage Events
        public static Action StartGame;
        public static Action EndGame;
        
        // Spawn Events
        public static Action RobotSpawnEvent;
        public static Action BoxSpawnEvent;
        public static Action<GameObject> RobotDestroyEvent;
        public static Action<GameObject> BoxDestroyEvent;

        // References to other objects
        [Header("Reference to GameObjects")] 
        [SerializeField] private Painter robotPainter;

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
            
            gameStats.ResetStats();
        }

        private void Start()
        {
            // Load and setup game settings
            LoadGameSettings();
            Conveyor.SetSpeed(currentGameSettings.conveyorSpeed);
            robotPainter.SetPaintSpeed(currentGameSettings.paintSpeed);
            robotPainter.SetPainterMoveSpeed(currentGameSettings.painterMoveSpeed);
        }

        private void Update()
        {
            SpawnRobot();
            
            if(gameSystem.IsPlaying() && timer > currentGameSettings.timeLimit)
                gameSystem.End();

            if (!gameSystem.IsGameOver() && !gameSystem.IsWaiting())
                if(timer <= currentGameSettings.timeLimit)
                    timer += Time.deltaTime;

            if (gameSystem.IsGameOver() && gameSystem.IsPlaying())
            {
                DataManager.main.dataList["gameQuit"] = false;
                EndGame?.Invoke();
                Debug.Log("GAME OVER");
            }
        }

        #region STAGE_MANAGEMENT

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
                uiManager.ShowMainImage();
                StartGame?.Invoke();
                SpawnRandomRobot();
                RobotSpawnEvent?.Invoke();
            }
        }

        public void SpawnRandomRobot()
        {
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
        }

        #endregion

        public static void QuitGame()
        {
            Debug.Log("QUIT GAME!");
            Application.Quit();
        }
    }
}
