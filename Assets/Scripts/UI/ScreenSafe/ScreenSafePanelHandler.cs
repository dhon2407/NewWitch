using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

namespace UI.ScreenSafe
{
    public class ScreenSafePanelHandler : MonoBehaviour
    {
        public static void AddPanel(ScreenSafePanel panel)
        {
            if (ActivePanels.Contains(panel)) return;
            
            ActivePanels.Add(panel);
            
            panel.ApplySafeArea(_currentSafeArea);
        }

        private static readonly List<ScreenSafePanel> ActivePanels = new List<ScreenSafePanel>();
        private static ScreenSafePanelHandler _instance;
        
        private static Rect _currentSafeArea;
        private static Vector2Int _currentScreenSize = Vector2Int.zero;
        private static ScreenOrientation _currentScreenOrientation;
        private static ISafeAreaHandler _safeAreaHandler;

        private void Awake()
        {
            if (_instance != null)
            {
                Destroy(gameObject);
                return;
            }

            _instance = this;
            
            
#if UNITY_EDITOR
            //Get custom safe area handler such as for testing
            _safeAreaHandler = GetComponent<ISafeAreaHandler>() ?? new SafeAreaHandler();
#else
            _safeAreaHandler = new SafeAreaHandler();
#endif
        }

        private void Start()
        {
            Refresh();
        }

        [Button(ButtonSizes.Medium), HideInEditorMode]
        private void Refresh()
        {
            var safeArea = GetSafeArea();

            if (SameConfiguration(safeArea)) return;
            
            _currentSafeArea = safeArea;
            _currentScreenSize.x = Screen.width;
            _currentScreenSize.y = Screen.height;
            _currentScreenOrientation = Screen.orientation;

            foreach (var panel in ActivePanels)
                panel.ApplySafeArea(_currentSafeArea);
        }

        private bool SameConfiguration(Rect safeArea)
        {
            return safeArea == _currentSafeArea
                   && Screen.width == _currentScreenSize.x
                   && Screen.height == _currentScreenSize.y
                   && Screen.orientation == _currentScreenOrientation;
        }

        private Rect GetSafeArea()
        {
            return _safeAreaHandler.GetSafeArea();
        }
        
        private class SafeAreaHandler : ISafeAreaHandler
        {
            public Rect GetSafeArea() => Screen.safeArea;
        }
    }
}