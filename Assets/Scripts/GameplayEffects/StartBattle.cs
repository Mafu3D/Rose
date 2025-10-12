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
        [SerializeField] CharacterData enemyCharacterData;
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
            Character left = GameManager.Instance.Hero.Character;

            Character right = new Character(enemyCharacterData);
            Inventory inventory;
            if (enemyCharacterData.InventoryDefinition != null)
            {
                inventory = new Inventory(right, enemyCharacterData.InventoryDefinition);
            }
            else
            {
                inventory = new Inventory(right);
            }
            right.SetInventory(inventory);

            GameManager.Instance.BattleManager.StartNewBattle(left, right, BattleConclusion);
            return Status.Running;
        }

        private void BattleConclusion(BattleReport battleReport, Character left, Character right)
        {
            // TODO: Create any nodes if the battle wasnt won
            switch (battleReport.Resolution)
            {
                case Combat.Resolution.RanAway:
                case Combat.Resolution.Stole:
                    Debug.Log("instantiating new tile");
                    GameObject gameObject = Instantiate(enemyNodePrefab, GameManager.Instance.Hero.CurrentCell.Center, Quaternion.identity);
                    Tile tile = gameObject.GetComponent<Tile>();
                    tile.RegisterToGrid();
                    tile.RegisterCharacter(right);
                    break;
                case Combat.Resolution.Victory:
                    GameManager.Instance.Player.GemTracker.AddGem(enemyCharacterData.gemReward);
                    GameManager.Instance.Player.GoldTracker.AddGold(enemyCharacterData.goldReward);
                    break;
            }
        }
    }
}