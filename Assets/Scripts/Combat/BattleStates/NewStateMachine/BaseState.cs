using Project.Combat;

namespace Project.NewStateMachine
{
    public abstract class BaseState : IState
    {
        Battle battle;

        public BaseState(Battle battle)
        {
            this.battle = battle;
        }

        public virtual void OnEnter() { }

        public virtual void OnExit() { }

        public virtual void Update() { }
    }
}