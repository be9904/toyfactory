using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AttnKare
{
    public abstract class StateMachine : MonoBehaviour
    {
        protected State State;
        protected bool Status;

        public State GetState()
        {
            return State;
        }
        
        public void SetState(State state)
        {
            StartCoroutine(State.Transition());
            State = state;
            StartCoroutine(State.Keep());
        }
        
        public void SetState(State state, Action func)
        {
            StartCoroutine(State.Transition());
            func();
            State = state;
            StartCoroutine(State.Keep());
        }

        public void End()
        {
            Status = true;
        }
    }
}
