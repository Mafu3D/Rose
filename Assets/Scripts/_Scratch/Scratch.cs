using System.Collections.Generic;
using Project;
using Project.Decks;
using Project.Grid;
using Project.Items;
using Project.UI.MainUI;
using UnityEngine;

public class Scratch : MonoBehaviour
{
    [SerializeField] GameObject hitPointObject;
    [SerializeField] Inventory inventory;
    [SerializeField] ItemData item;
    [SerializeField] List<ItemData> items;
    bool lmbClicked = false;
    bool rmbClicked = false;

    int itemIndex;

    bool firstUpdate = true;


    void Start()
    {

    }

    void Update()
    {
        if (firstUpdate)
        {
            FirstUpdate();
            firstUpdate = false;
        }
        if (Input.GetMouseButton(0))
        {
            if (!lmbClicked)
            {
                lmbClicked = true;
                return;
                Item newItem = new Item(item);
                itemIndex = inventory.AddItem(newItem);
                // CheckWorldPosForCell();
            }
        }
        else
        {
            lmbClicked = false;
        }

        if (Input.GetMouseButton(1))
        {
            if (!rmbClicked)
            {
                return;
                inventory.RemoveItem(itemIndex);
                rmbClicked = true;
            }
        }
        else
        {
            rmbClicked = false;
        }
    }

    private void FirstUpdate() {
        // List<Item> choiceItems = new();
        // foreach (ItemData itemData in items)
        // {
        //     Item item = new Item(itemData);
        //     choiceItems.Add(item);
        // }

        // List<Item> choiceItems = GameManager.Instance.ItemDeck.DrawMultiple(3);

        // TreasureChoice treasureChoice = new TreasureChoice(choiceItems);
        // MainUI.Instance.DisplayTreasureChoice(treasureChoice);
    }

    private void CheckWorldPosForCell()
    {
        Vector2 worldPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Cell hitCell = GameManager.Instance.Grid.WorldPositionToCell(worldPosition);
        Instantiate(hitPointObject, worldPosition, Quaternion.identity);
        Debug.Log($"X: {hitCell.x} Y: {hitCell.y}");
        // Debug.Log(hitCell.Center);
        // Debug.Log(hitCell.GetHashCode());
    }
}