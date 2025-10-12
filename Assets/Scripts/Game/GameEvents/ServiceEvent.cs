using System;
using System.Collections.Generic;
using Project.Custom;
using Project.GameplayEffects;
using Project.NPCs;
using UnityEngine;

namespace Project.Core.GameEvents
{
    public class ServiceEvent : ChoiceEvent<SerializableKeyValuePair<GameplayEffectStrategy, int>>
    {
        private string npcName;
        private string callout;

        private bool debug = true;

        private NPCServiceDefinition npcServiceDefinition;
        public ServiceEvent(int amount, NPCServiceDefinition npcServiceDefinition) : base(amount, true)
        {
            this.npcName = npcServiceDefinition.NPCName;
            this.callout = npcServiceDefinition.Callout;
            this.npcServiceDefinition = npcServiceDefinition;
        }

        public event Action OnBuyEvent;
        public event Action OnCannotBuyEvent;
        public event Action OnServicesUpdated;

        public override void ChooseItem(int index)
        {
            if (index > npcServiceDefinition.Services.Count)
            {
                return;
            }

            if (Choice.GetAllItems()[index] == null)
            {
                if (debug)
                {
                    Debug.Log("!!! CANNOT BUY THAT SERVICE AGAIN !!!");
                    DebugShop();
                }
                OnCannotBuyEvent?.Invoke();
                return;
            }

            SerializableKeyValuePair<GameplayEffectStrategy, int> service = Choice.GetAllItems()[index];
            if (GameManager.Instance.Player.GoldTracker.Gold >= service.Value)
            {
                // Perform any condiitonal checks here
                if (debug) Debug.Log($"You purchased a {service.Key.name}!");
                Choice.ChooseItem(index);
                GameManager.Instance.Player.GoldTracker.RemoveGold(service.Value);

                if (!npcServiceDefinition.ServicesAreRepeatable)
                {
                    Choice.TEMPReplaceLikeArrayForShop(null, index);
                }

                Choice.Resolve();
                OnBuyEvent?.Invoke();
                OnServicesUpdated?.Invoke();

                Debug.Log("!!! BOUGHT SERVICE !!!");
            }
            else
            {
                OnCannotBuyEvent?.Invoke();
                if (debug) Debug.Log("!!! NOT ENOUGH GOLD !!!");
            }
            if (debug) DebugShop();
        }

        public override void GenerateChoices()
        {
            List<SerializableKeyValuePair<GameplayEffectStrategy, int>> choices = npcServiceDefinition.Services;
            if (choices.Count == 0) return;
            Choice = new Choice<SerializableKeyValuePair<GameplayEffectStrategy, int>>(choices, ResolveCallback);

            if (debug)
            {
                Debug.Log($"!!! {npcName} IS READY FOR SERVICE !!!");
                Debug.Log(callout);
                DebugShop();
            }
        }

        public override void Resolve()
        {
        }

        protected override void ResolveCallback(List<SerializableKeyValuePair<GameplayEffectStrategy, int>> chosen, List<SerializableKeyValuePair<GameplayEffectStrategy, int>> notChosen)
        {
            foreach (SerializableKeyValuePair<GameplayEffectStrategy, int> service in chosen)
            {
                GameManager.Instance.EffectQueue.AddEffect(service.Key);
                GameManager.Instance.EffectQueue.ResolveQueue();
            }
            Choice.Reset();

            if (npcServiceDefinition.ExitAfterPurchase)
            {
                Resolve();
                GameManager.Instance.GameEventManager.EndNPCServiceEvent();
            }
        }


        private void DebugShop()
        {
            //Debug
            Debug.Log("---------------------------------------------");
            List<SerializableKeyValuePair<GameplayEffectStrategy, int>> services = Choice.GetAllItems();
            for (int i = 0; i < services.Count; i++)
            {
                if (services[i] == null) Debug.Log($"({i + 1}) SOLD OUT");
                else
                {
                    Debug.Log($"({i + 1}) {services[i].Key.name} :: {services[i].Value}g");
                }
            }
            Debug.Log("---------------------------------------------");
        }
    }
}