using Project;
using Project.Decks;
using UnityEngine;

public class MainUI : Singleton<MainUI>
{
    [SerializeField] private GameObject cardDisplayPrefab;
    [SerializeField] private RectTransform cardDisplayTransform;

    GameObject currentlyDisplayedCard;

    public void DisplayCard(Card card)
    {
        currentlyDisplayedCard = Instantiate(cardDisplayPrefab, cardDisplayTransform.position, Quaternion.identity, cardDisplayTransform);
        CardDisplay cardDisplay = currentlyDisplayedCard.GetComponent<CardDisplay>();
        cardDisplay.DisplayCard(card);
    }

    public void DestroyDisplayedCard()
    {
        if (currentlyDisplayedCard != null)
        {
            Destroy(currentlyDisplayedCard);
        }
    }
}