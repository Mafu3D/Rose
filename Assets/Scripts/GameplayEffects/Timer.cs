using System.Collections.Generic;
using Project.GameNode;
using Project.Items;
using UnityEngine;

namespace Project
{
    [CreateAssetMenu(fileName = "NewTimer", menuName = "Effects/Timer", order = 1)]
    public class Timer : GameplayEffectStrategy
    {
        [SerializeField] float timer;
        float time;

        public override void Reset()
        {
            time = 0f;
        }

        public override Status Resolve()
        {
            if (time > timer)
            {
                return Status.Complete;
            }
            time += Time.deltaTime;
            Debug.Log(time);
            return Status.Running;
        }

        public override Status Start()
        {
            time += Time.deltaTime;
            Debug.Log("start");
            return Status.Running;
        }
    }
}