using System.Collections.Generic;
using Project;
using Project.Decks;
using Project.Grid;
using UnityEngine;

public class Scratch : MonoBehaviour
{
    [SerializeField] GameObject hitPointObject;
    [SerializeField] DeckData deckData;
    bool lmbClicked = false;
    bool rmbClicked = false;

    Deck deck;

    void Start()
    {
        deck = new Deck();

        deck.AddCards(deckData.UnpackCards());
        deck.Shuffle();
    }

    void Update()
    {
        if (Input.GetMouseButton(0))
        {
            if (!lmbClicked)
            {
                // CheckWorldPosForCell();
                Card card = deck.DrawCard();
                if (card != null)
                {
                    Debug.Log($"{card.name}: {card.text}");
                }

                lmbClicked = true;
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
                deck.Reset();
                Debug.Log($"Reset");
                // Debug.Log(deck.NumberRemaining);

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
        Debug.Log(hitCell.Center);
        // Debug.Log(hitCell.GetHashCode());
    }
}