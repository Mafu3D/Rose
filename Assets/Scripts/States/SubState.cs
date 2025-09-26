namespace Project.States
{
    public abstract class SubState : State
    {
        public State SuperState;

        public SubState(State superState)
        {
            this.SuperState = superState;
        }
    }
}