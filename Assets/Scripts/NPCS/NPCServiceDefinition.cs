namespace Project.NPCs
{
    using System;
    using System.Collections.Generic;
    using Project.Custom;
    using Project.GameplayEffects;
    using Sirenix.OdinInspector;
    using UnityEngine;

    [CreateAssetMenu(fileName = "NewNPCServiceDefinition", menuName = "NPCs/Service Definition", order = 0)]
    public class NPCServiceDefinition : SerializedScriptableObject
    {
        [Header("Shop Header")]
        [SerializeField] public string NPCName = "NPC";
        [SerializeField] public string Callout = "How can I help you?";

        [Header("Services")]
        [DictionaryDrawerSettings(KeyLabel = "Effect", ValueLabel = "Price")]
        public List<SerializableKeyValuePair<GameplayEffectStrategy, int>> Services = new List<SerializableKeyValuePair<GameplayEffectStrategy, int>>();
        public bool ExitAfterPurchase = true;
        public bool ServicesAreRepeatable = false;
    }

}