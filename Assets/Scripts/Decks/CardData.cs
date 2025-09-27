using Project.Decks;
using Sirenix.OdinInspector;
using UnityEngine;

[CreateAssetMenu(fileName = "NewCardData", menuName = "Cards/Card", order = 1)]
public class CardData : SerializedScriptableObject
{
    [SerializeField] public string Name;
    [SerializeField] public Sprite Sprite;
    [SerializeField] public CardType CardType;
    [SerializeField] public string DisplayText;
    [SerializeField] public int Value;
}