using System.Collections.Generic;
using CustomHelper;
using GameData;
using UnityEngine;

namespace Game.PowerUp
{
    public class PowerUpController : MonoBehaviour
    {
        public bool OnEffect => _activePowerUp != null;
        public void Cancel()
        {
            ClearPowerUp();
            this.Log($"Cancelling power up");
        }

        private void ClearPowerUp()
        {
            foreach (var powerUp in _powerUps)
                powerUp.SetActive(false);

            _activePowerUp = null;
        }

        private List<PowerUpSlot> _powerUps;
        private PowerUpSlot _activePowerUp;

        private void Awake()
        {
            _powerUps = new List<PowerUpSlot>(GetComponentsInChildren<PowerUpSlot>());
        }

        private void Start()
        {
            InitializePowerUps();
            Managers.Manager.Board.OnBackgroundTap.AddListener(Cancel);
        }

        private void InitializePowerUps()
        {
            foreach (var powerUp in _powerUps)
            {
                powerUp.Setup(2);// TODO 2 each temporary
                powerUp.OnSelectPowerSlot.AddListener(HandlePowerUpTrigger);
            }
        }

        private void HandlePowerUpTrigger(PowerUpSlot slot)
        {
            if (slot == _activePowerUp)
            {
                Cancel();
                return;
            }

            _activePowerUp = slot;
            
            foreach (var powerUp in _powerUps)
                powerUp.SetActive(powerUp == _activePowerUp);

            this.Log($"Handling power up {slot.Type}");
        }

        public PowerUpType Consume()
        {
            var currentPowerUpType = _activePowerUp.Type;
            _activePowerUp.Consume();
            ClearPowerUp();

            return currentPowerUpType;
        }
    }
}