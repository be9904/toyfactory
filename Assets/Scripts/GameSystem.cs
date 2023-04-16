using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AttnKare
{
    public class GameSystem : StateMachine
    {
        private GameSettings _gameSettings;
        
        private State waitingState;
        private State playingState;

        public Action<Action> OnStageReady;
        public Action<Action> OnStageEnd;

        // custom initialization function for monobehaviour inheriting class
        public GameSystem Init(GameSettings gameSettings)
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
            if (gameSettings == null)
            {
                Debug.Log(GetType() + " : Invalid stage settings asset.");
                return null;
            }

            // setup state machine event
            GameManager.StartGame += StartGame;
            GameManager.EndGame   += EndGame;
            
            // start state machine
            StartCoroutine(waitingState.LoopState());
            
            return this;
        }

        public bool IsWaiting()
        {
            return currentState.GetStateID() == waitingState.GetStateID();
        }

        public bool IsPlaying()
        {
            return currentState.GetStateID() == playingState.GetStateID();
        }

        // waiting -> playing
        void StartGame()
        {
            InvokeTransition(waitingState, playingState);
        }
        
        // playing -> waiting
        void EndGame()
        {
            InvokeTransition(playingState, waitingState);
        }
    }

    #region STATE_WAITING
    public class Waiting : State { }
    #endregion

    #region STATE_PLAYING
    public class Playing : State { }
    #endregion
}
