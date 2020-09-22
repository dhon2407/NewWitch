using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.UI;

namespace Utilities.Helpers
{
    [RequireComponent(typeof(GridLayoutGroup))]
    public class GridScreenFitter : MonoBehaviour
    {
        [SerializeField] private RectTransform maxReference = null;

        [Title("Data")]
        [ShowInInspector, ReadOnly] private Vector2 _originalSize;
        [ShowInInspector, ReadOnly] private Vector2 _maxReferenceSize;
        
        [ShowInInspector, ReadOnly] private Vector2 _originalSizeDelta;
        [ShowInInspector, ReadOnly] private Vector2 _maxReferenceSizeDelta;

        public void UpdateFit()
        {
            GetCurrentReference();
            UpdateGridScale(_maxReferenceSize.x / _originalSize.x);
        }

        private void UpdateGridScale(float scaleDelta)
        {
            var newSizeDelta = _rectTransform.sizeDelta;
            newSizeDelta *= scaleDelta;
            _rectTransform.sizeDelta = newSizeDelta;
            
            var newCellSize = _gridOriginalCellSize * scaleDelta;
            _gridLayout.cellSize = newCellSize;
        }

        private GridLayoutGroup _gridLayout;
        private RectTransform _rectTransform;
        private Vector2 _gridOriginalCellSize;

        private void Awake()
        {
            Init();
        }

        private void Init()
        {
            _gridLayout = GetComponent<GridLayoutGroup>();
            _rectTransform = GetComponent<RectTransform>();
            _originalSize = _rectTransform.rect.size;
            _originalSizeDelta = _rectTransform.sizeDelta;
        }

        private void GetCurrentReference()
        {
            if (!maxReference)
                return;
            
            _maxReferenceSize = maxReference.rect.size;
            _maxReferenceSizeDelta = maxReference.sizeDelta;

            _gridOriginalCellSize = _gridLayout.cellSize;
        }


#if UNITY_EDITOR
        [Button(ButtonSizes.Large)]
        private void TestFit()
        {
            Init();
            UpdateFit();
        }
        
        #endif
    }
}