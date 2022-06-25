namespace FiniteStateMachine
{
    /*
     FiniteStateMachine script used with permission from Immergo Media on YouTube
     via https://www.youtube.com/watch?v=il1_1KskiDk 
     */

    public interface IState
    {
        void OnEnter();
        void OnUpdate();
        void OnExit();
    }
    public class StateMachine
    {
        public IState CurrentState { get; private set; }

        public StateMachine() { }

        public StateMachine(IState initState)
        {
            //set state to initial state
            SetState(initState);
        }

        public void OnUpdate()
        {
            if (CurrentState != null)
            {
                CurrentState.OnUpdate();
            }
        }

        public void SetState(IState newState)
        {
            if (CurrentState != null)
            {
                CurrentState.OnExit();
            }
            CurrentState = newState;
            CurrentState.OnEnter();
        }

        public StateType GetCurrentStateAsType<StateType>() where StateType : class, IState
        {
            return CurrentState as StateType;
        }
    }
}