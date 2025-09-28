using System.Collections.Generic;
using Project;
using Project.Decks;
using Project.GameNode;
using Project.Items;
using UnityEngine;

public class Scratch : MonoBehaviour
{
    [SerializeField] GameObject hitPointObject;
    [SerializeField] Inventory inventory;
    [SerializeField] ItemData item;
    [SerializeField] List<ItemData> items;
    [SerializeField] CardData cardData;
    Card card;
    bool lmbClicked = false;
    bool rmbClicked = false;

    int itemIndex;

    bool firstUpdate = true;
    float timer = 2f;
    float time;
    bool hasExecuted = false;


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
        return;
        time += Time.deltaTime;
        if (time > timer && !hasExecuted)
        {
            card = new Card(cardData);
            card.Execute();
            hasExecuted = true;
        }

        // if (cardStatus != Status.Complete)
        // {
        //     cardStatus = card.Resolve();

        // }

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

    private void FirstUpdate()
    {
        // GameManager.Instance.Player.HeroNode.Attributes.ModifyAttributeValue(Project.Attributes.AttributeType.Health, -9);

        // List<Item> choiceItems = new();
        // foreach (ItemData itemData in items)
        // {
        //     Item item = new Item(itemData);
        //     choiceItems.Add(item);
        // }

        // List<Item> choiceItems = GameManager.Instance.ItemDeck.DrawMultiple(3);

        // Choice<Item> treasureChoice = new Choice<Item(choiceItems);
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