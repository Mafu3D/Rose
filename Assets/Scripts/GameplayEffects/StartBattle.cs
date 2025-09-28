namespace Project.GameplayEffects
{
    using Project.Attributes;
    using Project.Combat;
    using Project.GameNode;
    using Unity.VisualScripting.Antlr3.Runtime.Misc;
    using UnityEngine;

    [CreateAssetMenu(fileName = "NewStartBattle", menuName = "Effects/Start Battle", order = 1)]
    public class StartBattle : GameplayEffectStrategy
    {
        [SerializeField] NodeData enemyNodeData;

        public override void Reset()
        {
        }

        public override Status Resolve()
        {
            if (BattleManager.Instance.IsActiveBattle)
            {
                switch (BattleManager.Instance.GetActiveBattleResolution())
                {
                    case BattleResolution.None:
                        return Status.Running;
                    default:
                        return Status.Complete;
                }
            }
            else
            {
                return Status.Complete;
            }
        }

        public override Status Start()
        {
            Combatant left = new Combatant(GameManager.Instance.Hero.Attributes,
                                           GameManager.Instance.Hero.NodeData.DisplayName,
                                           GameManager.Instance.Hero.NodeData.Description,
                                           GameManager.Instance.Hero.NodeData.Sprite);

            CharacterAttributes enemyAttributes = new CharacterAttributes(enemyNodeData.AttributesData);
            Combatant right = new Combatant(enemyAttributes,
                                            enemyNodeData.DisplayName,
                                            enemyNodeData.Description,
                                            enemyNodeData.Sprite);

            BattleManager.Instance.StartNewBattle(left, right, Dummy);
            return Status.Running;
        }

        public void Dummy(BattleResolution resolution, Combatant left, Combatant right)
        {
            Debug.Log(resolution);
            Debug.Log(left);
            Debug.Log(right);
        }
    }
}