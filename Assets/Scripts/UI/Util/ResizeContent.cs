using TMPro;
using UnityEngine;
using UnityEngine.UI;

[ExecuteInEditMode()]
public class ResizeContent : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI header;
    [SerializeField] TextMeshProUGUI content;
    [SerializeField] LayoutElement layoutElement;
    [SerializeField] int characterWrapLimit;

    void Update()
    {
        int headerLength = header.text.Length;
        int contentLength = content.text.Length;

        layoutElement.enabled = (headerLength > characterWrapLimit) || (contentLength > characterWrapLimit) ? true : false;

    }
}
