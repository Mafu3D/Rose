// namespace Project.GameplayEffects
// {
//     using Project.Combat;
//     using Unity.VisualScripting.Antlr3.Runtime.Misc;
//     using UnityEngine;

//     [CreateAssetMenu(fileName = "NewStartBattle", menuName = "Effects/Start Battle", order = 1)]
//     public class StartBattle : GameplayEffectStrategy
//     {
//         public override void Reset()
//         {
//         }

//         public override Status Resolve()
//         {
//             if (BattleManager.Instance.IsActiveBattle)
//             {
//                 return Status.Running;
//             }
//             else
//             {
//                 return Status.Complete;
//             }
//         }

//         public override Status Start()
//         {
//             BattleManager.Instance.StartNewBattle(GameManager.Instance.Hero, this);
//             return Status.Running;
//         }
//     }
// }