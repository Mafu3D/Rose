using System.Collections.Generic;
using Project.GameNode;
using Project.Items;
using UnityEngine;

namespace Project.GameplayEffects
{
    [CreateAssetMenu(fileName = "NewTimer", menuName = "Effects/Timer", order = 1)]
    public class Timer : GameplayEffectStrategy
    {
        [SerializeField] float timer;
        float time;

        public override void ResetEffect(Node user, Node target)
        {
            time = 0f;
        }

        public override Status ResolveEffect(Node user, Node target)
        {
            if (time > timer)
            {
                return Status.Complete;
            }
            time += Time.deltaTime;
            Debug.Log(time);
            return Status.Running;
        }

        public override Status StartEffect(Node user, Node target)
        {
            Debug.Log($"{user.name} started a new timer for {timer} seconds!");
            time += Time.deltaTime;
            return Status.Running;
        }
    }
}