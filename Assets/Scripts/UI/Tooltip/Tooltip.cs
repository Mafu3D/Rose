using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Project.UI
{

    [ExecuteInEditMode()]
    public class Tooltip : MonoBehaviour
    {
        [SerializeField] bool positionWithMouse = false;
        [SerializeField] TextMeshProUGUI headerText;
        [SerializeField] TextMeshProUGUI contentText;
        [SerializeField] LayoutElement layoutElement;
        [SerializeField] int characterWrapLimit;

        private RectTransform rectTransform;

        void Awake()
        {
            rectTransform = GetComponent<RectTransform>();
        }

        public void SetText(string content, string header = "")
        {
            if (string.IsNullOrEmpty(header))
            {
                headerText.gameObject.SetActive(false);
            }
            else
            {
                headerText.gameObject.SetActive(true);
                headerText.text = header;
            }
            contentText.text = content;

            int headerLength = headerText.text.Length;
            int contentLength = contentText.text.Length;
            layoutElement.enabled = (headerLength > characterWrapLimit) || (contentLength > characterWrapLimit) ? true : false;
        }

        void Update()
        {
            if (Application.isEditor)
            {
                int headerLength = headerText.text.Length;
                int contentLength = contentText.text.Length;
                layoutElement.enabled = (headerLength > characterWrapLimit) || (contentLength > characterWrapLimit) ? true : false;
            }
            else
            {
                if (positionWithMouse)
                {
                    // hmmm... not working? whatever
                    // May want to switch how im getting the mouse input later
                    Vector2 position = Input.mousePosition;
                    float pivotX = position.x / Screen.width;
                    float pivotY = position.y / Screen.height;
                    rectTransform.pivot = new Vector2(pivotX, pivotY);
                    transform.position = position;
                }
            }
        }
    }
}
