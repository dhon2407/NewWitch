using System.Collections.Generic;
using System.Linq;
using CustomHelper;
using Sirenix.OdinInspector;
using UI.ScreenSafe;
using UnityEngine;

namespace UI.Layout
{
    [RequireComponent(typeof(RectTransform))]
    [HideMonoScript]
    public class ScreenVerticalSectionsFitter : MonoBehaviour
    {
        [Title("Screen Sections")]
        [InfoBox("Exceed content percentage limit", InfoMessageType.Error,"Exceed100Percent")]
        [SerializeField, LabelText("Sections")] private List<SectionData> sections;

        private bool Exceed100Percent => sections.Sum(data => data.Coverage) > 100;
        
        private ScreenSafePanel _screenSafePanel;

        private void Awake()
        {
            InitializeComponents();
        }

        private void SetupSections()
        {
            if (Exceed100Percent)
                return;
            
            var rect = GetComponent<RectTransform>().rect;
            var remainingHeight = rect.height;

            foreach (var sectionData in sections)
            {
                var section = sectionData.section;
                if (sectionData.section)
                {
                    var sectionHeight = rect.height * (sectionData.Coverage / 100f);
                    section.DockedBottom();
                    section.SetDimensions(sectionHeight, rect.width);
                    section.SetVerticalPosition(sectionHeight / 2 + remainingHeight - sectionHeight);
                    remainingHeight -= sectionHeight;
                }
            }
        }

        private void InitializeComponents()
        {
            _screenSafePanel = GetComponent<ScreenSafePanel>();
            
            if (_screenSafePanel)
                _screenSafePanel.OnChangePanel.AddListener(PanelUpdated);
            else
                SetupSections();
        }

        private void PanelUpdated()
        {
            SetupSections();
        }
        

#if UNITY_EDITOR
        [Button(ButtonSizes.Large), PropertyOrder(int.MaxValue), PropertySpace(SpaceBefore = 5)]
        private void DebugFit()
        {
            InitializeComponents();
        }
        #endif

        [System.Serializable]
        private struct SectionData
        {
            public ScreenSection section;
            [MinValue(0)]
            public int Coverage;
        }
    }
}