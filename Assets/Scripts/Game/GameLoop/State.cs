namespace Project.GameLoop
{
    public abstract class State : IState
    {
        protected GameManager GameManager;
        protected StateMachine StateMachine;
        public string Name => name;
        private string name;
        public float TimeInState { get; private set; } = 0f;

        public State(string name, StateMachine stateMachine, GameManager gameManager)
        {
            this.name = name;
            this.GameManager = gameManager;
            this.StateMachine = stateMachine;
        }

        public virtual void OnEnter() { }
        public virtual void Update(float time) { }
        public virtual void OnExit() { }

        public void UpdateTimeInState(float deltaTime) { TimeInState += deltaTime; }
    }
}