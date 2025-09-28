using System.Collections.Generic;
using Project.GameNode;
using Project.Items;
using UnityEngine;

namespace Project
{
    [CreateAssetMenu(fileName = "NewChooseTreasure", menuName = "Effects/Choose Treasure", order = 1)]
    public class ChooseTreaure : GameplayEffectStrategy
    {
        public override void Reset()
        {
        }

        public override Status Resolve()
        {
            if (GameManager.Instance.IsChoosingTreasure) return Status.Running;
            return Status.Complete;
        }

        public override Status Start()
        {
            List<Item> choiceItems = GameManager.Instance.ItemDeck.DrawMultiple(3);
            TreasureChoice treasureChoice = new TreasureChoice(choiceItems);
            GameManager.Instance.StartNewTreasureChoice(treasureChoice);
            return Status.Running;
        }
    }
}