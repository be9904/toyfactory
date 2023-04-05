using System;
using UnityEngine;

namespace AttnKare
{
    public abstract class StateMachine : MonoBehaviour
    {
        protected State currentState;
        protected bool Status;

        public State GetCurrentState()
        {
            return currentState;
        }

        public bool IsCurrentState(State state)
        {
            return currentState.GetStateID() == state.GetStateID();
        }
        
        public void InvokeTransition(State from, State to)
        {
            if (IsCurrentState(from))
            {
                StartCoroutine(from.EndState());
                currentState = to;
                StartCoroutine(to.LoopState());
            }
            else
            {
                Debug.Log(GetType() + " : Invalid transition invoked.");
            }
        }

        /*public void SetState(State state)
        {
            StartCoroutine(currentState.EndState());
            currentState = state;
            StartCoroutine(currentState.LoopState());
        }
        
        public void SetState(State state, Action func)
        {
            StartCoroutine(currentState.EndState());
            func();
            currentState = state;
            StartCoroutine(currentState.LoopState());
        }*/

        // end state machine
        public void End()
        {
            Status = true;
        }
    }
}
