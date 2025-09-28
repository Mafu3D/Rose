using System.Collections.Generic;
using Project.Items;
using UnityEngine;

namespace Project.GameNode.Strategies
{
    [CreateAssetMenu(fileName = "NewChooseTreasure", menuName = "Nodes/ChooseTreasure", order = 1)]
    public class ChooseTreasure : ScriptableObject, INodeStrategy
    {
        bool hasBeenUsed = false;

        public Status Resolve(Node other)
        {
            if (!hasBeenUsed)
            {
                List<Item> choiceItems = GameManager.Instance.ItemDeck.DrawMultiple(3);
                TreasureChoice treasureChoice = new TreasureChoice(choiceItems);
                GameManager.Instance.StartNewTreasureChoice(treasureChoice);

                hasBeenUsed = true;
            }

            if (GameManager.Instance.IsChoosingTreasure) return Status.Running;
            return Status.Complete;
        }

        public void Reset()
        {
        }

        public void ResetNode()
        {
            hasBeenUsed = false;
        }
    }
}