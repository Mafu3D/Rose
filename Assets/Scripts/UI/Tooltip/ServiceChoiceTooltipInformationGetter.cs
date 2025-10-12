using Project.NPCs;
using Project.UI.Shop;
using UnityEngine;

namespace Project.UI
{
    public class ServiceChoiceTooltipInformationGetter : TooltipInformationGetter
    {
        [SerializeField] ServiceChoiceUI serviceChoiceUI;

        public override bool TryGetTooltipInformation(out string content, out string header)
        {
            ServiceDefinition service = serviceChoiceUI.GetServiceDefinition();
            if (service != null)
            {
                header = service.DisplayName;
                content = service.Description;
                return true;
            }
            content = default;
            header = default;
            return false;
        }
    }
}
