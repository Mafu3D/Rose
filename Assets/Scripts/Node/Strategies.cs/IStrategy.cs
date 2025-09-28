namespace Project.GameNode.Strategies
{
    public interface IStrategy
    {
        public Status Resolve(Node other);

        public void Reset();

        public void ResetNode();
    }
}