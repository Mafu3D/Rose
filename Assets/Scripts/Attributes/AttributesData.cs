using UnityEngine;

namespace Project.Attributes
{
    [CreateAssetMenu(fileName = "NewAttributesData", menuName = "Attributes Data", order = 0)]
    public class AttributesData : ScriptableObject
    {
        [SerializeField] public int Health = 3;
        [SerializeField] public int Speed = 3;
        [SerializeField] public int Strength = 0;
        [SerializeField] public int Magic = 0;
        [SerializeField] public int Dexterity = 0;
        [SerializeField] public int Armor = 0;
    }
}