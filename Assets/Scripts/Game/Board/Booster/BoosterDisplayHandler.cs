using DG.Tweening;
using GameData;
using GameSettings;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.UI;

namespace Game.Board.Booster
{
    public class BoosterDisplayHandler : MonoBehaviour
    {
        [Required, SerializeField] private Image primaryImage = null;
        [SerializeField] private Image secondaryImage = null;

        public void SetIcon(BoosterType booster)
        {
            primaryImage.sprite = Settings.Booster.GetIcon(booster);
            primaryImage.DOFade(1, 0);
        }

        public void HideIcon()
        {
            primaryImage.DOFade(0, 0);
        }
    }
}