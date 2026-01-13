namespace MirAI.FSM
{
    public class StateMachine
    {
        private IState _currentState;

        public IState CurrentState => _currentState;

        public void ChangeState(IState nextState)
        {
            if (nextState == null || nextState == _currentState)
            {
                return;
            }

            _currentState?.Exit();
            _currentState = nextState;
            _currentState.Enter();
        }

        public void Tick()
        {
            _currentState?.Tick();
        }
    }
}
