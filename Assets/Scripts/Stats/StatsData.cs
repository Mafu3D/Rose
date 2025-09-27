using UnityEngine;

namespace Project.Stats
{
    [CreateAssetMenu(fileName = "StatsData", menuName = "StatsData", order = 0)]
    public class StatsData : ScriptableObject
    {
        [SerializeField] public int Health = 3;
        [SerializeField] public int Speed = 3;
        [SerializeField] public int Strength = 0;
        [SerializeField] public int Magic = 0;
        [SerializeField] public int Dexterity = 0;
        [SerializeField] public int Armor = 0;
    }
}