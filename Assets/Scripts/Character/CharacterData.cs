using Project.Attributes;
using UnityEngine;


namespace Project
{

    [CreateAssetMenu(fileName = "NewCharacterData", menuName = "Character Data", order = 0)]
    public class CharacterData : ScriptableObject
    {
        [SerializeField] public string DisplayName;
        [SerializeField] public string Description;
        [SerializeField] public string CombatDescription;
        [SerializeField] public Sprite CombatSprite;
        [SerializeField] public AttributesData AttributesData;
    }
}