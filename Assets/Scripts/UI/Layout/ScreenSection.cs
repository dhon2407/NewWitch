using Sirenix.OdinInspector;
using UnityEngine;

namespace UI.Layout
{
    [RequireComponent(typeof(RectTransform))]
    [HideMonoScript]
    public class ScreenSection : MonoBehaviour
    {
        public void DockedBottom()
        {
            _rectTransform.anchorMax = new Vector2(0.5f, 0f);
            _rectTransform.anchorMin = new Vector2(0.5f, 0f);
        }
        
        public void SetDimensions(float rectHeight, float rectWidth)
        {
            _rectTransform.sizeDelta = new Vector2(rectWidth, rectHeight);
        }
        
        public void SetVerticalPosition(float verticalPosition)
        {
            var currentPosition = _rectTransform.position;
            _rectTransform.position = new Vector3(currentPosition.x, verticalPosition, currentPosition.z);
        }
        
        private RectTransform _rectTransform;

        private void Awake()
        {
            InitializeComponents();
        }

        private void InitializeComponents()
        {
            _rectTransform = GetComponent<RectTransform>();
        }
    }
}