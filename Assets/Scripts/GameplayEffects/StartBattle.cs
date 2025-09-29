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
        [SerializeField] GameObject enemyNodePrefab;

        public override void Reset()
        {
        }

        public override Status Resolve()
        {
            if (BattleManager.Instance.IsActiveBattle) return Status.Running;
            return Status.Complete;
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

            BattleManager.Instance.StartNewBattle(left, right, BattleConclusion);
            return Status.Running;
        }

        private void BattleConclusion(BattleReport battleReport, Combatant left, Combatant right)
        {
            // TODO: Create any nodes if the battle wasnt won
            switch (battleReport.Resolution)
            {
                case Combat.Resolution.RanAway:
                case Combat.Resolution.Stole:
                    GameObject gameObject = Instantiate(enemyNodePrefab, GameManager.Instance.Hero.CurrentCell.Center, Quaternion.identity);
                    Node node = gameObject.GetComponent<Node>();
                    node.RegisterToGrid();
                    break;
            }
        }
    }
}