using System.Collections;
using System.Collections.Generic;
using UnityEditor.SceneManagement;
using UnityEngine;

namespace AttnKare
{
    public class GameSystem : StateMachine
    {
        private StageSettings stageSettings;

        public GameSystem Init(StageSettings stageSettings)
        {
            if (stageSettings == null)
            {
                Debug.Log("Invalid stage settings asset.");
                return null;
            }
            this.stageSettings = stageSettings;
            return this;
        }

        public void LoadStageSettings(StageSettings settings)
        {
            stageSettings = settings;
        }
    }
}
