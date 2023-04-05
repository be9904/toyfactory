using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AttnKare
{
    public class GameSystem : StateMachine
    {
        private StageSettings stageSettings;
        
        private State waitingState;
        private State playingState;

        public Action<Action> OnStageReady;
        public Action<Action> OnStageEnd;

        // custom initialization function for monobehaviour inheriting class
        public GameSystem Init(StageSettings stageSettings)
        {
            // create states
            waitingState = gameObject.AddComponent<Waiting>();
            playingState = gameObject.AddComponent<Playing>();
            waitingState.hideFlags = HideFlags.HideInInspector;
            playingState.hideFlags = HideFlags.HideInInspector;
            
            int stateCount = 0;
            waitingState.SetStateID(stateCount++);
            playingState.SetStateID(stateCount++);
            
            currentState = waitingState;

            // load stage settings
            if (stageSettings == null)
            {
                Debug.Log(GetType() + " : Invalid stage settings asset.");
                return null;
            }

            // setup state machine event
            OnStageReady += StartState;
            OnStageEnd   += EndState;
            
            // start state machine
            StartCoroutine(waitingState.LoopState());
            
            return this;
        }

        // waiting -> playing
        void StartState(Action func)
        {
            InvokeTransition(waitingState, playingState);
            func();
        }
        
        // playing -> waiting
        void EndState(Action func)
        {
            InvokeTransition(playingState, waitingState);
            func();
        }
    }

    #region STATE_WAITING
    public class Waiting : State { }
    #endregion

    #region STATE_PLAYING
    public class Playing : State { }
    #endregion
}
