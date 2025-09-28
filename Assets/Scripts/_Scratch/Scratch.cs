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


    void Start()
    {
        TreasureChoice treasureChoice = new TreasureChoice(items);
        MainUI.Instance.DisplayTreasureChoice(treasureChoice);
    }

    void Update()
    {
        if (Input.GetMouseButton(0))
        {
            if (!lmbClicked)
            {
                lmbClicked = true;
                return;
                itemIndex = inventory.AddItem(item);
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