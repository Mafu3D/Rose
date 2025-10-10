using System.Collections;
using EasyTextEffects.Editor.MyBoxCopy.Extensions;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Project.UI
{
    public class TooltipTrigger : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
    {
        [Header("Simple")]
        [SerializeField] string header;
        [SerializeField] string content;

        [Header("Getter")]
        [SerializeField] TooltipInformationGetter tooltipInformationGetter;

        [Header("Delay")]
        [SerializeField] float delay = 0.2f;

        Coroutine delayRoutine;

        public void OnPointerEnter(PointerEventData eventData)
        {
            delayRoutine = StartCoroutine(ShowTooltipDelayRoutine());
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            if (delayRoutine != null)
            {
                StopCoroutine(delayRoutine);
                delayRoutine = null;
            }
            TooltipSystem.Hide();
        }

        private IEnumerator ShowTooltipDelayRoutine()
        {
            yield return new WaitForSeconds(delay);
            Show();
        }

        private void Show()
        {
            string contentString = content;
            string headerString = header;
            if (tooltipInformationGetter != null)
            {
                string outContent;
                string outHeader;
                if (tooltipInformationGetter.TryGetTooltipInformation(out outContent, out outHeader))
                {
                    contentString = outContent;
                    headerString = outHeader;
                }
            }
            if (!contentString.IsNullOrEmpty() || !headerString.IsNullOrEmpty())
            {
                TooltipSystem.Show(contentString, headerString);
            }
        }
    }
}
