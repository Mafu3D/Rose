using Project.Sequences;
using UnityEngine;

public class RoundStartNode : Node
{
    public RoundStartNode(string name, Project.GameManager gameManager) : base(name, gameManager) { }

    public override void Enter()
    {
        // GameManager.BattleManager.ActiveBattle.StartNewRound();
    }

    public override Status Execute()
    {
        Debug.Log(this.Name);
        if (!GameManager.BattleManager.ActiveBattle.CombatQueue.QueueNeedsToBeResolved) return Status.Complete;
        return Status.Running;
     }

    public override void Exit() { }
}

public class TurnStartNode : Node
{
    public TurnStartNode(string name, Project.GameManager gameManager) : base(name, gameManager) { }

    public override void Enter()
    {
        // GameManager.BattleManager.ActiveBattle.StartNewTurn();
    }

    public override Status Execute() {
        Debug.Log(this.Name);
        if (!GameManager.BattleManager.ActiveBattle.CombatQueue.QueueNeedsToBeResolved) return Status.Complete;
        return Status.Running;
     }

    public override void Exit() { }
}

public class TurnEndNode : Node
{
    public TurnEndNode(string name, Project.GameManager gameManager) : base(name, gameManager) { }

    public override void Enter()
    {
        // GameManager.BattleManager.ActiveBattle.EndTurn();
    }

    public override Status Execute() {
        Debug.Log(this.Name);
        if (!GameManager.BattleManager.ActiveBattle.CombatQueue.QueueNeedsToBeResolved) return Status.Complete;
        return Status.Running;
     }

    public override void Exit() { }
}

public class RoundEndNode : Node
{
    public RoundEndNode(string name, Project.GameManager gameManager) : base(name, gameManager) { }

    public override void Enter()
    {
        // GameManager.BattleManager.ActiveBattle.EndRound();
    }

    public override Status Execute() {
        Debug.Log(this.Name);
        if (!GameManager.BattleManager.ActiveBattle.CombatQueue.QueueNeedsToBeResolved) return Status.Complete;
        return Status.Running;
     }

    public override void Exit() { }
}

public class AttackNode : Node
{
    public AttackNode(string name, Project.GameManager gameManager) : base(name, gameManager) { }

    public override void Enter()
    {
        // GameManager.BattleManager.ActiveBattle.DoAttack();
    }

    public override Status Execute() {
        Debug.Log(this.Name);
        if (!GameManager.BattleManager.ActiveBattle.CombatQueue.QueueNeedsToBeResolved) return Status.Complete;
        return Status.Running;
     }

    public override void Exit() { }
}