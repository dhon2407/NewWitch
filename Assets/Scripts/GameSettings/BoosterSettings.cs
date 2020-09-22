using System;
using System.Collections.Generic;
using GameData;
using UnityEngine;

namespace GameSettings
{
    [CreateAssetMenu(fileName = "BoosterSettings", menuName = "Settings/Booster", order = 0)]
    public class BoosterSettings : ScriptableObject
    {
        [SerializeField] private List<BoosterData> boosters = null;

        public Sprite GetIcon(BoosterType boosterType)
        {
            return boosters.Find(b => b.type == boosterType).icon;
        }


        [Serializable]
        private struct BoosterData
        {
            public BoosterType type;
            public Sprite icon;

            public BoosterData(BoosterType boosterType)
            {
                type = boosterType;
                icon = null;
            }
        }
    }
}