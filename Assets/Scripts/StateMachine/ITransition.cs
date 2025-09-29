namespace Project.States
{
    public interface ITransition
    {
        IState To { get; }
        IPredicate Condition { get; }
    }
}