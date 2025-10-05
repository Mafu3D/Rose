using System.Collections.Generic;
using Project.Decks;
using Project.GameTiles;
using Project.GameplayEffects;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Project.Decks
{
    [CreateAssetMenu(fileName = "NewCardData", menuName = "Cards/Card", order = 1)]
    public class CardData : SerializedScriptableObject
    {
        [SerializeField] public string Name;
        [SerializeField] public Sprite Sprite;
        [SerializeField] public CardType CardType;
        [SerializeField] public string DisplayText;
        [SerializeField] public List<GameplayEffectStrategy> Strategies;
    }
}