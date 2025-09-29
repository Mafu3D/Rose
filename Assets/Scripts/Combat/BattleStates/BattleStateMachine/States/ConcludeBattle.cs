
namespace Project.Combat.BattleStateMachine
{
    public class ConcludeBattle : State
    {
        public ConcludeBattle(StateMachine stateMachine, Battle battle) : base(stateMachine, battle) { }

        public override void Enter() { }

        public override void Exit() { }

        public override void Subscribe() { }

        public override void Unsubscribe() { }

        public override void Update(float deltaTime) { }
    }

}