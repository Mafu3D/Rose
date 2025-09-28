namespace Project.GameNode.Strategies
{
    public interface INodeStrategy
    {
        public Status Resolve(Node other);
        public void Reset();
    }
}