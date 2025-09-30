using System.Collections.Generic;
using System.Linq;
using Project;
using Project.Decks;
using Project.Items;
using UnityEngine;

namespace Project.UI.MainUI
{
    public class MainUI : Singleton<MainUI>
    {


        [SerializeField] private GameObject treasureChoicePrefab;
        [SerializeField] private RectTransform treasureChoiceTransform;

        GameObject currentTreasureChoice;


        public void DisplayTreasureChoice(Choice<Item> treasureChoice)
        {
            currentTreasureChoice = Instantiate(treasureChoicePrefab, treasureChoiceTransform.position, Quaternion.identity, treasureChoiceTransform);
            TreasureChoiceDisplay treasureChoiceDisplay = currentTreasureChoice.GetComponent<TreasureChoiceDisplay>();
            treasureChoiceDisplay.DisplayChoices(treasureChoice);
        }

        public void DestroyTreasureChoice()
        {
            if (currentTreasureChoice != null)
            {
                Destroy(currentTreasureChoice);
            }
        }
    }
}