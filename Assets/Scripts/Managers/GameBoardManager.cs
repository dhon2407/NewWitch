using CustomHelper;
using Game.PowerUp;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Managers
{
    public class GameBoardManager : BaseManager
    {
        [Required, SerializeField] private Button backgroundButton = null;
        [Required, SerializeField] private PowerUpController powerUpsController = null;

        public UnityEvent OnBackgroundTap { get; } = new UnityEvent();

        public PowerUpController PowerUps => powerUpsController;

        protected override void Awake()
        {
            base.Awake();
            
            if (backgroundButton)
                backgroundButton.onClick.AddListener(BackgroundTap);
        }

        private void BackgroundTap()
        {
            OnBackgroundTap.Invoke();
            this.Log("Background tapped.");
        }
    }
}