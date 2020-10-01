using UnityEngine;
using UnityEngine.Events;

namespace UI.ScreenSafe
{
    public class ScreenSafePanel : MonoBehaviour
    {
        [SerializeField] private bool conformX = true;
        [SerializeField] private bool conformY = true;
        public UnityEvent OnChangePanel { get; } = new UnityEvent();

        public void ApplySafeArea(Rect safeRect)
        {
            if (!conformX)
            {
                safeRect.x = 0;
                safeRect.width = Screen.width;
            }

            if (!conformY)
            {
                safeRect.y = 0;
                safeRect.height = Screen.height;
            }

            // Convert safe area rectangle from absolute pixels to normalised anchor coordinates
            var anchorMin = safeRect.position;
            var anchorMax = safeRect.position + safeRect.size;
            anchorMin.x /= Screen.width;
            anchorMin.y /= Screen.height;
            anchorMax.x /= Screen.width;
            anchorMax.y /= Screen.height;
            _panel.anchorMin = anchorMin;
            _panel.anchorMax = anchorMax;
            
            OnChangePanel.Invoke();

#if UNITY_EDITOR && SHOWALLDEBUG
            Debug.LogFormat ("New safe area applied to {0}: x={1}, y={2}, w={3}, h={4} on full extents w={5}, h={6}",
                name, safeRect.x, safeRect.y, safeRect.width, safeRect.height, Screen.width, Screen.height);
#endif
        }
        
        private RectTransform _panel;

        private void Awake()
        {
            _panel = GetComponent<RectTransform>();

            if (_panel == null)
            {
                Debug.LogError($"Cannot find rect component, screen safe panel not enabled for {gameObject.name}");
                return;
            }

            ScreenSafePanelHandler.AddPanel(this);
        }
    }
}