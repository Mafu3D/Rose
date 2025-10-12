namespace Project.NPCs
{
    using Project.GameplayEffects;
    using Sirenix.OdinInspector;
    using UnityEngine;

    [CreateAssetMenu(fileName = "NewServiceDefinition", menuName = "NPCs/Service Definition", order = 0)]
    public class ServiceDefinition : SerializedScriptableObject
    {
        [SerializeField] public string DisplayName;
        [SerializeField] public string Description;
        [SerializeField] public Sprite DisplaySprite;
        [SerializeField] public int Cost;
        [SerializeField] public bool Repeatable;
        [SerializeField] public bool ExitOnUse;
        [SerializeField] public GameplayEffectStrategy Effect;
    }

}