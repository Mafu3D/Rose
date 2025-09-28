using System;
using Project.Combat;
using Project.States;
using UnityEngine.InputSystem;

namespace Project.GameStates
{
    public class Choosing : SubState
    {
        public Choosing(State superState, StateMachine stateMachine) : base(superState, stateMachine) { }

        public override void Enter()
        {

        }

        public override void Exit()
        {

        }

        public override void Subscribe()
        {
            GameManager.Instance.Player.InputReader.OnChoice1Input += Choice1;
            GameManager.Instance.Player.InputReader.OnChoice2Input += Choice2;
            GameManager.Instance.Player.InputReader.OnChoice3Input += Choice3;
            GameManager.Instance.OnTresureChoiceEnded += ExitChoosing;
        }


        public override void Unsubscribe()
        {
            GameManager.Instance.Player.InputReader.OnChoice1Input -= Choice1;
            GameManager.Instance.Player.InputReader.OnChoice2Input -= Choice2;
            GameManager.Instance.Player.InputReader.OnChoice3Input -= Choice3;
            GameManager.Instance.OnTresureChoiceEnded -= ExitChoosing;
        }

        private void ExitChoosing()
        {
            StateMachine.SwitchState(new WaitForTurnProcess(SuperState, StateMachine));
        }

        private void Choice3()
        {
            GameManager.Instance.ActiveTreasureChoice.ChooseItem(2);
            GameManager.Instance.EndTreasureChoice();
        }

        private void Choice2()
        {
            GameManager.Instance.ActiveTreasureChoice.ChooseItem(1);
            GameManager.Instance.EndTreasureChoice();
        }

        private void Choice1()
        {
            GameManager.Instance.ActiveTreasureChoice.ChooseItem(0);
            GameManager.Instance.EndTreasureChoice();
        }

        private void ExitChoice()
        {

        }

        public override void Update(float deltaTime)
        {

        }
    }
}