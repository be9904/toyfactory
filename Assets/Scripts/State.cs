using System;
using System.Collections;

namespace AttnKare
{
    public abstract class State
    {
        private int _stateID = -1;
        protected bool TransitionCondition;
        protected float TimeInterval;

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

        public virtual IEnumerator Keep()
        {
            yield break;
        }
        
        public virtual IEnumerator Transition()
        {
            SetCondition(true);
            yield break;
        }
        
        public virtual IEnumerator Transition(Action func)
        {
            SetCondition(true);
            func();
            yield break;
        }
    }
}
