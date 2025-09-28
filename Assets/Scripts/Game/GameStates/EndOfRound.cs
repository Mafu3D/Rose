using Project.States;

namespace Project.GameStates
{
    public class EndOfRound : SubState
    {
        public EndOfRound(State superState, StateMachine stateMachine) : base(superState, stateMachine) { }

        public override void Enter()
        {
            EndTurn();
        }

        public override void Exit()
        {

        }

        public override void Subscribe()
        {

        }

        public override void Unsubscribe()
        {

        }

        public override void Update(float deltaTime)
        {

        }

        private void EndTurn()
        {
            GameManager.Instance.DestroyMarkedNodes();
            GameManager.Instance.IncrementTurn();
            StateMachine.SwitchState(new PlayerMove(new PlayerTurn(StateMachine), StateMachine));
        }
    }
}