using UnityEngine;

namespace Project.UI
{
    public abstract class TooltipInformationGetter : MonoBehaviour
    {
        public abstract bool TryGetTooltipInformation(out string content, out string header);
    }
}
