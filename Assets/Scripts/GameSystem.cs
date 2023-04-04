using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;

namespace AttnKare
{
    public class GameSystem : StateMachine
    {
        private StageSettings stageSettings;
        
        private State waitingState = new Waiting();
        private State playingState = new Playing();

        public Action OnStateChange;

        // custom initialization function for monobehaviour inheriting class
        public GameSystem Init(StageSettings stageSettings)
        {
            // create states
            waitingState = new Waiting();
            playingState = new Playing();
            
            int stateCount = 0;
            waitingState.SetStateID(stateCount++);
            playingState.SetStateID(stateCount++);
            
            State = waitingState;

            // load stage settings
            if (stageSettings == null)
            {
                Debug.Log(GetType() + " : Invalid stage settings asset.");
                return null;
            }
            LoadStageSettings(stageSettings);
            Debug.Log("Total number of states in state machine: " + stateCount);
            
            // setup state machine event
            OnStateChange += UpdateState;
            
            // start state machine
            StartCoroutine(waitingState.Keep());
            
            return this;
        }

        void UpdateState()
        {
            if (State.GetStateID() == waitingState.GetStateID())
            {
                SetState(playingState);
            }
            else if (State.GetStateID() == playingState.GetStateID())
            {
                SetState(waitingState);
            }
        }

        public void LoadStageSettings(StageSettings settings)
        {
            Debug.Log("Loaded: " + settings.name);
            stageSettings = settings;
        }
    }

    #region STATE_WAITING
    public class Waiting : State
    {
        public override IEnumerator Keep()
        {
            SetCondition(false);

            while (!TransitionCondition)
            {
                // do what this state is supposed to do
                yield return null;
                Debug.Log("Current State: " + GetType());
            }
        }
    }
    #endregion

    #region STATE_PLAYING
    public class Playing : State
    {
        public override IEnumerator Keep()
        {
            SetCondition(false);

            while (!TransitionCondition)
            {
                // do what this state is supposed to do
                yield return null;
                Debug.Log("Current State: " + GetType());
            }
        }
    }
    #endregion
}
