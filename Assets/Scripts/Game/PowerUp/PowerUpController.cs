using System.Collections.Generic;
using CustomHelper;
using UnityEngine;

namespace Game.PowerUp
{
    public class PowerUpController : MonoBehaviour
    {
        private List<PowerUpSlot> _powerUps;

        private void Awake()
        {
            _powerUps = new List<PowerUpSlot>(GetComponentsInChildren<PowerUpSlot>());
        }

        private void Start()
        {
            InitializePowerUps();
        }

        private void InitializePowerUps()
        {
            foreach (var powerUp in _powerUps)
            {
                powerUp.Setup();
                powerUp.OnSelectPowerSlot.AddListener(HandlePowerUpTrigger);
            }
        }

        private void HandlePowerUpTrigger(PowerUpSlot slot)
        {
            this.Log($"Handling power up {slot.Type}");
        }
    }
}