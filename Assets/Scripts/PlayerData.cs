using UnityEngine;

namespace Project.Hero
{
    [CreateAssetMenu(fileName = "HeroData", menuName = "HeroData", order = 0)]
    public class HeroData : ScriptableObject
    {
        [SerializeField] public int MoveDistance = 1;
        [SerializeField] public float MoveSpeed = 4f;
    }
}