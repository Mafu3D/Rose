namespace Project.NewStateMachine
{
    public interface IState
    {
        void OnEnter();
        void Update();
        void OnExit();
    }
}