using System;
using System.Collections.Generic;
using GameData;
using UnityEngine;

namespace GameSettings
{
    [CreateAssetMenu(fileName = "PowerUpSettings", menuName = "Settings/Power Ups", order = 0)]
    public class PowerUpSettings : ScriptableObject
    {
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