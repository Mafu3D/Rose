using Project.GameLoop;
using Project.NPCs;
using UnityEngine;

namespace Project.GameplayEffects
{
    [CreateAssetMenu(fileName = "NewOpenNPCService", menuName = "Effects/Open NPC Service", order = 1)]
    public class OpenNPCService : GameplayEffectStrategy
    {
        [SerializeField] NPCServiceDefinition npcServiceDefinition;

        public override void ResetEffect()
        {
        }

        public override Status ResolveEffect()
        {
            return Status.Complete;
        }

        public override Status StartEffect()
        {
            GameManager.Instance.GameEventManager.StartNPCServiceEvent(npcServiceDefinition);
            GameManager.Instance.StateMachine.SwitchState(new NPCServiceState("NPC Service", GameManager.Instance.StateMachine, GameManager.Instance));
            return Status.Running;
        }
    }
}