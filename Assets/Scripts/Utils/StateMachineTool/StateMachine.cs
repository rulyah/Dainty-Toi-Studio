namespace Utils.StateMachineTool
{
    public class StateMachine<T>
    {
        private State<T> _current;

        public StateMachine(State<T> state) => ChangeState(state);

        public void ChangeState(State<T> newState)
        {
            _current?.OnExit();
            _current = newState;
            _current.Init(this);
            _current.OnEnter();
        }

        public void Dispose()
        {
            _current?.OnExit();
        }
    }
}