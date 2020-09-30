using System;
using System.Collections.Generic;
using GameData;
using UnityEngine;

namespace GameSettings
{
    [CreateAssetMenu(fileName = "PowerUpSettings", menuName = "Settings/Power Ups", order = 0)]
    public class PowerUpSettings : ScriptableObject
    {
        [SerializeField] public float slotEmptyAlpha = 0.5f;
        [SerializeField] public float slotAlphaChangeDuration = 0.2f;
        [SerializeField] public float slotQtyPunchDuration = 0.3f;
        [SerializeField] public float slotQtyPunchScale = 0.1f;
        
        [SerializeField] private List<PowerUpData> powerUps = null;

        public PowerUpData GetData(PowerUpType powerType)
        {
            return powerUps.Exists(d => d.type == powerType)
                ? powerUps.Find(d => d.type == powerType)
                : new PowerUpData {type = PowerUpType.None};
        }
        
        
        [Serializable]
        public struct PowerUpData
        {
            public PowerUpType type;
            public Sprite icon;
        } 
    }
}