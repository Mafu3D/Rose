using System;
using System.Collections.Generic;
using Project.Custom;
using Project.GameplayEffects;
using Project.NPCs;
using UnityEngine;

namespace Project.Core.GameEvents
{
    public class ServiceEvent : ChoiceEvent<ServiceDefinition>
    {
        private string npcName;
        private string callout;

        private bool debug = true;

        public NPCInteractionDefinition InteractionDefinition;
        public ServiceEvent(int amount, NPCInteractionDefinition npcServiceDefinition) : base(amount, true)
        {
            this.npcName = npcServiceDefinition.NPCName;
            this.callout = npcServiceDefinition.Callout;
            this.InteractionDefinition = npcServiceDefinition;
        }

        public event Action OnBuyEvent;
        public event Action OnCannotBuyEvent;
        public event Action OnServicesUpdated;

        public override void ChooseItem(int index)
        {
            if (index > InteractionDefinition.Services.Count)
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

            ServiceDefinition service = Choice.GetAllItems()[index];
            if (GameManager.Instance.Player.GoldTracker.Gold >= service.Cost)
            {
                // Perform any condiitonal checks here
                if (debug) Debug.Log($"You purchased a {service.DisplayName}!");
                Choice.ChooseItem(index);
                GameManager.Instance.Player.GoldTracker.RemoveGold(service.Cost);

                if (!service.Repeatable)
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
            List<ServiceDefinition> choices = InteractionDefinition.Services;
            if (choices.Count == 0) return;
            Choice = new Choice<ServiceDefinition>(choices, ResolveCallback);

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

        protected override void ResolveCallback(List<ServiceDefinition> chosen, List<ServiceDefinition> notChosen)
        {
            foreach (ServiceDefinition service in chosen)
            {
                GameManager.Instance.EffectQueue.AddEffect(service.Effect);
                GameManager.Instance.EffectQueue.ResolveQueue();
                if (service.ExitOnUse)
                {
                    Resolve();
                    GameManager.Instance.GameEventManager.EndNPCServiceEvent();
                }
            }
            Choice.Reset();

        }


        private void DebugShop()
        {
            //Debug
            Debug.Log("---------------------------------------------");
            List<ServiceDefinition> services = Choice.GetAllItems();
            for (int i = 0; i < services.Count; i++)
            {
                if (services[i] == null) Debug.Log($"({i + 1}) SOLD OUT");
                else
                {
                    Debug.Log($"({i + 1}) {services[i].DisplayName} :: {services[i].Cost}g");
                }
            }
            Debug.Log("---------------------------------------------");
        }
    }
}