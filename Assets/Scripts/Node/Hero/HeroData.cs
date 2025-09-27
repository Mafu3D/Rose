using Project.Stats;
using UnityEngine;

namespace Project.GameNode.Hero
{
    [CreateAssetMenu(fileName = "HeroData", menuName = "HeroData", order = 0)]
    public class HeroData : ScriptableObject
    {
        [SerializeField] public int MoveDistance = 1;
        [SerializeField] public StatsData StatsData;
    }
}