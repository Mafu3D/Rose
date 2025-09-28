using System.Collections.Generic;
using Project.Items;
using UnityEngine;

namespace Project.GameNode.Strategies
{
    [CreateAssetMenu(fileName = "NewChooseTreasure", menuName = "Nodes/ChooseTreasure", order = 1)]
    public class ChooseTreasure : ScriptableObject, INodeStrategy
    {
        private bool choiceHasBeenShown;
        public Status Resolve(Node other)
        {
            if (!choiceHasBeenShown)
            {
                List<Item> choiceItems = GameManager.Instance.ItemDeck.DrawMultiple(3);
                TreasureChoice treasureChoice = new TreasureChoice(choiceItems);
                GameManager.Instance.StartNewTreasureChoice(treasureChoice);

                choiceHasBeenShown = true;
                Debug.Log($"{other.gameObject.name}: showing treasure");
            }

            if (GameManager.Instance.IsChoosingTreasure) return Status.Running;
            return Status.Complete;
        }

        public void Reset()
        {
            choiceHasBeenShown = false;
        }
    }
}