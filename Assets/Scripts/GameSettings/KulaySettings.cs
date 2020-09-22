using System;
using System.Collections.Generic;
using GameData;
using Sirenix.OdinInspector;
using UnityEngine;
using Utilities.Helpers;

namespace GameSettings
{
    [CreateAssetMenu(fileName = "KulaySettings", menuName = "Settings/Kulay", order = 0)]
    public class KulaySettings : ScriptableObject
    {
        [SerializeField] public int maxKulayPerSet = 3;
        [SerializeField] public List<KulayData> kulays = null;

        [FoldoutGroup("Kulay Set")]
        [SerializeField] public List<Kulay> kulaySet = null;
        
        public Kulay Random => kulaySet.GetRandom();

        private Kulay GetRandomKulay()
        {
            return kulays.Count > 0 ? kulays.GetRandom().kulay : Kulay.None;
        }

        public KulayData GetData(Kulay kulay)
        {
            return kulays.Exists(d => d.kulay == kulay)
                ? kulays.Find(d => d.kulay == kulay)
                : new KulayData {kulay = Kulay.None};
        }


        [Serializable]
        public struct KulayData
        {
            public Kulay kulay;
            public Sprite icon;
        }

        [FoldoutGroup("Kulay Set")]
        [PropertyOrder(int.MinValue), Button(ButtonSizes.Large), LabelText("Randomized")]
        private void RandomizeSet()
        {
            kulaySet.Clear();
            while (kulaySet.Count < maxKulayPerSet)
            {
                var newKulay = GetRandomKulay();
                if (!kulaySet.Contains(newKulay))
                    kulaySet.Add(newKulay);
            }
        }

        private void OnValidate()
        {
            
        }
    }
}