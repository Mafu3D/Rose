using System.Collections.Generic;
using System.Linq;
using Project;
using Project.Decks;
using Project.GameTiles;
using Project.Items;
using UnityEngine;

namespace Project.UI.MainUI
{
    public class MainUI : Singleton<MainUI>
    {


        [SerializeField] private GameObject treasureChoicePrefab;
        [SerializeField] private GameObject nextTileChoicePrefab;
        [SerializeField] private RectTransform choiceTransform;

        GameObject currentTreasureChoice;
        GameObject currentTileChoice;


        public void DisplayTreasureChoice(Choice<Item> treasureChoice)
        {
            currentTreasureChoice = Instantiate(treasureChoicePrefab, choiceTransform.position, Quaternion.identity, choiceTransform);
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

        public void DisplayTileChoice(Choice<TileData> tileChoice)
        {
            currentTileChoice = Instantiate(nextTileChoicePrefab, choiceTransform.position, Quaternion.identity, choiceTransform);
            NextTileChoiceDisplay nextTileChoiceDisplay = currentTileChoice.GetComponent<NextTileChoiceDisplay>();
            nextTileChoiceDisplay.DisplayChoices(tileChoice);
        }

        public void DestroyTileChoice()
        {
            if (currentTileChoice != null)
            {
                Destroy(currentTileChoice);
            }
        }
    }
}