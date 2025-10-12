namespace Project.NPCs
{
    using System;
    using System.Collections.Generic;
    using Project.Custom;
    using Project.GameplayEffects;
    using Sirenix.OdinInspector;
    using UnityEngine;

    [CreateAssetMenu(fileName = "NewNPCInteractionDefinition", menuName = "NPCs/Interaction Definition", order = 0)]
    public class NPCInteractionDefinition : SerializedScriptableObject
    {
        [Header("Shop Header")]
        [SerializeField] public string NPCName = "NPC";
        [SerializeField] public string Callout = "How can I help you?";

        [Header("Services")]
        [DictionaryDrawerSettings(KeyLabel = "Effect", ValueLabel = "Price")]
        public List<ServiceDefinition> Services = new();
        public bool ExitAfterPurchase = true;
        public bool ServicesAreRepeatable = false;
    }

}