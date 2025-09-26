namespace Project.States
{
    public abstract class SubState : State
    {
        public State SuperState;

        public SubState(State superState, StateMachine stateMachine) : base(stateMachine)
        {
            this.SuperState = superState;
        }
    }
}