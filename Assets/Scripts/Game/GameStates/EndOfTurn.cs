// using System;
// using System.Collections.Generic;
// using Project.GameNode;
// using Project.States;

// namespace Project.GameStates
// {
//     public class EndOfTurn : SubState
//     {
//         public EndOfTurn(State superState, StateMachine stateMachine) : base(superState, stateMachine) { }

//         public override void Enter()
//         {
//             List<Node> registeredNodes = GameManager.Instance.Grid.GetAllRegisteredNodes();
//             foreach (Node node in registeredNodes)
//             {
//                 node.ExecuteOnEndOfTurn();
//             }
//         }

//         public override void Exit()
//         {

//         }

//         public override void Subscribe()
//         {
//             GameManager.Instance.EffectQueue.OnResolveQueueEnd += MoveToNextState;
//         }

//         public override void Unsubscribe()
//         {
//             GameManager.Instance.EffectQueue.OnResolveQueueEnd -= MoveToNextState;
//         }

//         private void MoveToNextState()
//         {
//             StateMachine.SwitchState(new ActivateNodes(new ResolvingEffects(StateMachine), StateMachine));
//         }

//         public override void Update(float deltaTime)
//         {

//         }
//     }
// }