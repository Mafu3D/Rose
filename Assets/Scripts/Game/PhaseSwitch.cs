namespace Project
{
    public class PhaseSwitch
    {
        public bool PhaseIsComplete;
        public void StartNewPhase() => PhaseIsComplete = false;
        public void CompletePhase() => PhaseIsComplete = true;
    }
}