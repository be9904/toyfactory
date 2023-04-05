using System;
using System.Collections;
using UnityEngine;

namespace AttnKare
{
    public abstract class State : StateMachine
    {
        private int _stateID = -1;
        protected bool TransitionCondition;
        protected float TimeInterval = 1f;

        public int GetStateID()
        {
            return _stateID;
        }

        public void SetStateID(int id)
        {
            _stateID = id;
        }
        
        protected void SetCondition(bool condition)
        {
            TransitionCondition = condition;
        }

        public virtual IEnumerator LoopState()
        {
            SetCondition(false);

            while (!TransitionCondition)
            {
                // do what this state is supposed to do
                yield return null;
                Debug.Log("Current State: " + GetType());
            }
        }
        
        public virtual IEnumerator EndState()
        {
            SetCondition(true);
            yield return new WaitForSeconds(TimeInterval);
        }
    }
}
