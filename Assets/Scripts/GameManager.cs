using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AttnKare
{
    public class GameManager : MonoBehaviour
    {
        public static GameManager main { get; private set; }

        private GameSystem gameSystem;
        [SerializeField] private List<StageSettings> settingsList;

        // public event Action<GameState> UpdateGameState;

        private void Awake()
        {
            // singleton initialization
            if (main != null && main != this)
            {
                Debug.Log("Singleton instance of " + GetType().Name + " already exists.");
                Destroy(this);
            }
            else
            {
                main = this;
                gameSystem = gameObject.AddComponent<GameSystem>();

                if (settingsList.Count == 0)
                    Debug.Log("No Stage Settings Profile");
                else
                    gameSystem.Init(settingsList[0]);
            }
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                gameSystem.OnStateChange?.Invoke();
            }
        }
    }
}
