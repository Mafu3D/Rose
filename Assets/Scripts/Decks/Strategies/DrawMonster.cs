// using Project;
// using Project.Decks;
// using Project.GameNode;
// using UnityEngine;

// namespace Projejct.Decks.CardStrategies
// {
//     [CreateAssetMenu(fileName = "NewDrawMonsterStrategy", menuName = "Cards/Strategies/Draw Monster", order = 0)]
//     public class DrawMonster : ScriptableObject, IStrategy
//     {
//         public void Execute()
//         {
//             Card card = GameManager.Instance.MonsterDeck.DrawCard();
//             if (card != null)
//             {
//                 card.Execute();
//             }
//         }
//     }
// }