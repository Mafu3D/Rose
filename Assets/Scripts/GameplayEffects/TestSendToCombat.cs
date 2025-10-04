using System.Collections.Generic;
using Project.GameLoop;
using Project.GameTiles;
using Project.Items;
using UnityEngine;

namespace Project.GameplayEffects
{
    [CreateAssetMenu(fileName = "NewTestSendToCombat", menuName = "Effects/Debug/TestSendToCombat", order = 1)]
    public class TestSendToCombat : GameplayEffectStrategy
    {
        [SerializeField] string ToSay;

        public override void ResetEffect() { }

        public override Status ResolveEffect() => Status.Complete;

        public override Status StartEffect()
        {
            GameManager.Instance.StateMachine.SwitchState(new CombatState("interjectedCombatState", GameManager.Instance.StateMachine, GameManager.Instance));
            return Status.Complete;
        }
    }
}