using Project.Combat;
using UnityEngine;

namespace Project.NewStateMachine
{
    public class StartBattleState : BaseState
    {
        public StartBattleState(Battle battle) : base(battle) { }

        public override void OnEnter()
        {
            Debug.Log("On Enter StartBattle");
        }

        public override void Update()
        {
            Debug.Log("Update StartBattle");
        }

        public override void OnExit()
        {
            Debug.Log("On Exit StartBattle");
        }
    }
}