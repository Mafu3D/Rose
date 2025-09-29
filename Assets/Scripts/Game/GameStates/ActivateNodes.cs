// using UnityEngine;
// using Project.Decks;
// using Project.States;
// using Project.UI.MainUI;
// using System.Collections.Generic;
// using Project.GameNode;
// namespace Project.GameStates
// {
//     public class ActivateNodes : SubState
//     {
//         Card card;
//         public ActivateNodes(State superState, StateMachine stateMachine) : base(superState, stateMachine) { }

//         public override void Enter()
//         {
//             List<Node> registeredNodes;
//             if (GameManager.Instance.Grid.TryGetNodesRegisteredToCell(GameManager.Instance.Hero.CurrentCell, out registeredNodes))
//             {
//                 foreach (Node node in registeredNodes)
//                 {
//                     node.ExecuteOnActivate();
//                     StateMachine.SwitchState(new EndOfRound(new ResolvingEffects(StateMachine), StateMachine));
//                     // TODO: Need to only move to end of round after effect queue is finished
//                 }
//             }
//             else
//             {
//                 StateMachine.SwitchState(new ResolveCardDraw(new ResolvingEffects(StateMachine), StateMachine));
//             }
//         }

//         public override void Exit() { }

//         public override void Subscribe()
//         {
//             // GameManager.Instance.Player.InputReader.OnProceedInput += OnProceed;
//         }

//         public override void Unsubscribe()
//         {
//             // GameManager.Instance.Player.InputReader.OnProceedInput -= OnProceed;
//         }

//         public override void Update(float deltaTime) { }

//         private void OnProceed()
//         {
//         }
//     }
// }