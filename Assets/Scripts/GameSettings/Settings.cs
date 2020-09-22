using Sirenix.OdinInspector;
using UnityEngine;

namespace GameSettings
{
    [CreateAssetMenu(fileName = "SettingsController", menuName = "Settings/Controller", order = 0)]
    public class Settings : ScriptableObject
    {
        [Required, SerializeField] private KulaySettings kulaySettings = null;
        [Required, SerializeField] private BoosterSettings boosterSetting = null;
        
        public static KulaySettings Kulay => Instance.kulaySettings ? Instance.kulaySettings : throw new UnityException("No Kulay settings found.");
        public static BoosterSettings Booster => Instance.boosterSetting ? Instance.boosterSetting : throw new UnityException("No Kulay settings found.");

        private static Settings _instance;
        private static Settings Instance => _instance ? _instance : Initialize();
        private static Settings Initialize()
        {
            _instance = Resources.Load<Settings>("SettingsController");
            return _instance;
        }
    }
}