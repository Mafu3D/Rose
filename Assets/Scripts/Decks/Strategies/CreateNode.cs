using Project;
using Project.GameNode;
using UnityEngine;

namespace Projejct.Decks.CardStrategies
{
    [CreateAssetMenu(fileName = "NewCreateNodeStrategy", menuName = "Cards/Strategies/Create Node", order = 0)]
    public class CreateNode : ScriptableObject, ICardStrategy
    {
        [SerializeField] GameObject nodePrefab;

        public void Execute()
        {
            GameObject gameObject = Instantiate(nodePrefab, GameManager.Instance.Hero.CurrentCell.Center, Quaternion.identity);
            Node node = gameObject.GetComponent<Node>();
            node.RegisterToGrid();
        }
    }
}