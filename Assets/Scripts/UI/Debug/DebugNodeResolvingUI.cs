using Project.GameNode;
using Project.GameNode.Strategies;
using Project.GameStates;
using Project.States;
using TMPro;
using UnityEngine;

namespace Project.UI.DebugUI
{
    public class DebugNodeResolvingUI : MonoBehaviour
    {
        [SerializeField] TMP_Text nodeProcessingText;

        void Update()
        {
            string textString = "Resolving: ";

            State superState = GameManager.Instance.StateMachine.CurrentSuperState;
            if (superState is TurnResolving)
            {
                Node resolvingNode = (superState as TurnResolving).CurrentResolvingNode;
                if (resolvingNode != null)
                {
                    textString += resolvingNode.name;
                }
                else
                {
                    textString += "None";
                }

                INodeStrategy strategy = resolvingNode.ResolvingStrategy;
                if (strategy != null)
                {
                    textString += $"\n {strategy}";
                }
            }
            else
            {
                textString += "None";
            }
            nodeProcessingText.text = textString;
        }
    }
}

