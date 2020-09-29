using System.Collections.Generic;
using DG.Tweening;
using GameData;
using GameSettings;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Game.PowerUp
{
    [RequireComponent(typeof(Button))]
    public class PowerUpSlot : MonoBehaviour
    {
        [SerializeField] private PowerUpType initialType = PowerUpType.None;
        [SerializeField, Required] private Image icon = null;
        [SerializeField, Required] private TextMeshProUGUI qty = null;
        
        public class OnSelectPowerSlotEvent : UnityEvent<PowerUpSlot> {}
        public OnSelectPowerSlotEvent OnSelectPowerSlot { get; } = new OnSelectPowerSlotEvent();

        public int Qty
        {
            get => _qty;
            set
            {
                _qty = value;
                if (qty)
                    qty.text = _qty.ToString("0");
            }
        }

        public PowerUpType Type => _type;
        
        public void Setup(int amount = 1, PowerUpType? type = null)
        {
            _type = type ?? initialType;
            Qty = amount;
            
            UpdateIcon();
        }

        private Button _button;
        private int _qty;
        private PowerUpType _type;
        private List<DOTweenAnimation> _animations;

        private void Awake()
        {
            _button = GetComponent<Button>();
            _button.onClick.AddListener(() =>
            {
                OnSelectPowerSlot.Invoke(this);
            });
            
            SetupAnimations();
        }

        private void SetupAnimations()
        {
            _animations = new List<DOTweenAnimation>(GetComponentsInChildren<DOTweenAnimation>());
            foreach (var tweenAnimation in _animations)
                tweenAnimation.DORewind();
        }

        private void UpdateIcon()
        {
            if (icon)
                icon.sprite = Settings.PowerUp.GetData(_type).icon;
        }

        public void SetActive(bool active)
        {
            foreach (var tweenAnimation in _animations)
            {
                if (active)
                    tweenAnimation.DOPlay();
                else
                    tweenAnimation.DORewind();
            }
        }
    }
}