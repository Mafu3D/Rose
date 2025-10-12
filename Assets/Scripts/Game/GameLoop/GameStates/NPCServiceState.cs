using Project.States;
using UnityEngine;
using Project.Core.GameEvents;
using Project.Items;
using Project.Custom;
using Project.GameplayEffects;
using Project.NPCs;

namespace Project.GameLoop
{
    public class NPCServiceState : State
    {
        public NPCServiceState(string name,
                                StateMachine stateMachine,
                                GameManager gameManager) : base(name, stateMachine, gameManager) { }

        State interruptedState;
        ChoiceEvent<ServiceDefinition> choiceEvent;

        public override void OnEnter()
        {
            Debug.Log($"Enter: {Name}");
            interruptedState = StateMachine.PreviousState;

            GameManager.Player.InputReader.OnNumInput += Choose;
            GameManager.Player.InputReader.OnExitInput += Exit;
            GameManager.GameEventManager.OnNPCServiceEnded += GoBackToInterruptedState;

            choiceEvent = GameManager.GameEventManager.CurrentGameEvent as ChoiceEvent<ServiceDefinition>;
        }

        private void Choose(int num)
        {
            if (num == 0)
            {
                return;
            }

            if (num > choiceEvent.Choice.NumberOfChoices) return;
            choiceEvent.ChooseItem(num - 1);
            choiceEvent.Resolve();
        }

        private void Exit()
        {
            if (choiceEvent.IsExitable)
            {
                choiceEvent.Resolve();
                GameManager.GameEventManager.EndNPCServiceEvent();
            }
        }

        public override void Update(float time)
        {
        }

        private void GoBackToInterruptedState(IGameEvent _)
        {
            StateMachine.SwitchState(interruptedState);
        }

        public override void OnExit()
        {
            GameManager.Player.InputReader.OnNumInput -= Choose;
            GameManager.Player.InputReader.OnExitInput -= Exit;
            GameManager.GameEventManager.OnNPCServiceEnded -= GoBackToInterruptedState;
        }
    }
}