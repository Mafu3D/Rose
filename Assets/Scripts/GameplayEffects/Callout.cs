using UnityEngine;

namespace Project.GameplayEffects
{
    [CreateAssetMenu(fileName = "NewCallout", menuName = "Effects/Callout", order = 1)]
    public class Callout : GameplayEffectStrategy
    {
        [SerializeField] string text = "";
        [SerializeField] Color color = Color.white;
        [SerializeField] float duration = 0.2f;
        public override void ResetEffect()
        {
        }

        public override Status ResolveEffect()
        {
            return Status.Complete;
        }

        public override Status StartEffect()
        {
            CalloutUI.Instance.QueueCallout(text, color, duration);
            return Status.Running;
        }
    }
}