

using UnityEngine;

namespace Project
{
    public class PhaseSwitch
    {
        public bool PhaseIsComplete;
        public void StartNewPhase()
        {
            Debug.Log("start phase");
            PhaseIsComplete = false;
        }
        public void CompletePhase()
        {
            Debug.Log("complete phase");
            PhaseIsComplete = true;
        }
    }
}