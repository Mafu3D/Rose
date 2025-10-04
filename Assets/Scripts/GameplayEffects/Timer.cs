using System.Collections.Generic;
using Project.GameTiles;
using Project.Items;
using UnityEngine;

namespace Project.GameplayEffects
{
    [CreateAssetMenu(fileName = "NewTimer", menuName = "Effects/Timer", order = 1)]
    public class Timer : GameplayEffectStrategy
    {
        [SerializeField] float timer;
        float time;

        public override void ResetEffect()
        {
            time = 0f;
        }

        public override Status ResolveEffect()
        {
            if (time > timer)
            {
                return Status.Complete;
            }
            time += Time.deltaTime;
            Debug.Log(time);
            return Status.Running;
        }

        public override Status StartEffect()
        {
            time += Time.deltaTime;
            return Status.Running;
        }
    }
}