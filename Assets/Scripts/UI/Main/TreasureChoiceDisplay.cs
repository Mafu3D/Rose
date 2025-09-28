using System;
using Project.Items;
using UnityEngine;

namespace Project.UI.MainUI
{
    public class TreasureChoiceDisplay : MonoBehaviour
    {
        [SerializeField] GameObject itemChoiceContainerPrefab;
        [SerializeField] GameObject itemChoice01Container;
        [SerializeField] GameObject itemChoice02Container;
        [SerializeField] GameObject itemChoice03Container;

        public void DisplayChoices(Choice<Item> treasureChoice)
        {
            GameObject itemChoice01 = Instantiate(itemChoiceContainerPrefab, itemChoice01Container.transform.position, Quaternion.identity, itemChoice01Container.transform);
            ItemChoiceDisplay itemChoiceDisplay01 = itemChoice01.GetComponent<ItemChoiceDisplay>();
            itemChoiceDisplay01.DisplayItem(treasureChoice.GetItem(0), 1);

            GameObject itemChoice02 = Instantiate(itemChoiceContainerPrefab, itemChoice02Container.transform.position, Quaternion.identity, itemChoice02Container.transform);
            ItemChoiceDisplay itemChoiceDisplay02 = itemChoice02.GetComponent<ItemChoiceDisplay>();
            itemChoiceDisplay02.DisplayItem(treasureChoice.GetItem(1), 2);

            GameObject itemChoice03 = Instantiate(itemChoiceContainerPrefab, itemChoice03Container.transform.position, Quaternion.identity, itemChoice03Container.transform);
            ItemChoiceDisplay itemChoiceDisplay03 = itemChoice03.GetComponent<ItemChoiceDisplay>();
            itemChoiceDisplay03.DisplayItem(treasureChoice.GetItem(2), 3);

        }
    }
}