using Sirenix.OdinInspector;
using UnityEngine;

[CreateAssetMenu(fileName = "NewCardData", menuName = "Cards/Card", order = 1)]
public class CardData : SerializedScriptableObject
{
    [SerializeField] public string Name;
    [SerializeField] public string Text;
}