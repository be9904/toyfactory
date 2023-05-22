using UnityEngine;

namespace AttnKare.Core
{
    public abstract class StateMachine : MonoBehaviour
    {
        protected State currentState;
        protected bool Status;

        public State GetCurrentState()
        {
            return currentState;
        }

        public bool IsGameOver()
        {
            return Status;
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
        
        public void End()
        {
            Status = true;
            // do what needs to be done when game is over
        }
    }
}
