namespace Project.GameplayEffects
{
    using Project.Attributes;
    using Project.Combat;
    using Project.GameTiles;
    using Project.Items;
    using Unity.VisualScripting;
    using Unity.VisualScripting.Antlr3.Runtime.Misc;
    using UnityEngine;

    [CreateAssetMenu(fileName = "NewStartBattle", menuName = "Effects/Start Battle", order = 1)]
    public class StartBattle : GameplayEffectStrategy
    {
        [SerializeField] TileData enemyNodeData;
        [SerializeField] GameObject enemyNodePrefab;

        public override void ResetEffect()
        {
        }

        public override Status ResolveEffect()
        {
            if (GameManager.Instance.BattleManager.IsActiveBattle) return Status.Running;
            return Status.Complete;
        }

        public override Status StartEffect()
        {
            // Character left = new Character(GameManager.Instance.Hero.Attributes,
            //                                GameManager.Instance.Hero.NodeData.DisplayName,
            //                                GameManager.Instance.Hero.NodeData.Description,
            //                                GameManager.Instance.Hero.NodeData.Sprite,
            //                                GameManager.Instance.Hero.gameObject.GetComponent<Inventory>());

            // CharacterAttributes enemyAttributes = new CharacterAttributes(enemyNodeData.AttributesData);
            // Character right = new Character(enemyAttributes,
            //                                 enemyNodeData.DisplayName,
            //                                 enemyNodeData.Description,
            //                                 enemyNodeData.Sprite,
            //                                 null);

            // GameManager.Instance.BattleManager.StartNewBattle(left, right, BattleConclusion);
            return Status.Running;
        }

        private void BattleConclusion(BattleReport battleReport, Character left, Character right)
        {
            // TODO: Create any nodes if the battle wasnt won
            switch (battleReport.Resolution)
            {
                case Combat.Resolution.RanAway:
                case Combat.Resolution.Stole:
                    GameObject gameObject = Instantiate(enemyNodePrefab, GameManager.Instance.Hero.CurrentCell.Center, Quaternion.identity);
                    Tile node = gameObject.GetComponent<Tile>();
                    node.RegisterToGrid();
                    break;
            }
        }
    }
}