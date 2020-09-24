using DG.Tweening;
using Game.Board.Booster;
using GameData;
using GameSettings;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Game.Board
{
    [RequireComponent(typeof(Button))]
    [HideMonoScript]
    public class KulaySlot : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI indexDisplay = null;
        
        [SerializeField, LabelText("Randomized Start")] private bool randomizeKulayAtStart = false;
        [SerializeField, HideIf("@randomizeKulayAtStart")] private Kulay defaultKulay = Kulay.None;
        [Required, SerializeField] private Image slotIcon = null;

        [Title("Booster Data")] [SerializeField]
        private BoosterType booster = BoosterType.None;
        public Kulay Kulay => _currentKulay;
        public BoosterType Booster => booster;
        public bool IsBoostSlot => booster != BoosterType.None; 
        public Vector3 CurrentPosition { get; private set; }
        public bool Popped => _popped;
        
        public class OnTapEvent : UnityEvent<KulaySlot> {}
        public OnTapEvent OnTap { get; } = new OnTapEvent();

        public int SlotIndex
        {
            get => _slotIndex;
            private set
            {
                _slotIndex = value;
                if (indexDisplay)
                    indexDisplay.text = _slotIndex.ToString("0");
            }
        }

        public void Change(Kulay newKulay)
        {
            _currentKulay = newKulay;
            LoadSlotIcon();
        }
        
        public void Initialize(int currentIndex, bool showIndex)
        {
            SlotIndex = currentIndex;
            _initialPosition = transform.position;
            CurrentPosition = _initialPosition;
            
            indexDisplay.enabled = showIndex;
        }
        
        public void Pop()
        {
            if (_popped)
                return;

            _popped = true;
            booster = BoosterType.None;
            if (_boosterDisplayHandler)
                _boosterDisplayHandler.HideIcon();
            slotIcon.DOFade(0, 0);
        }
        
        public void MoveTo(KulaySlot targetSlot, Vector3 targetPosition, float duration)
        {
            if (targetSlot == null)
                return;
            
            targetSlot.CurrentPosition = CurrentPosition;
            CurrentPosition = targetPosition;
            
            _transform.DOMove(CurrentPosition, duration)
                .SetEase(Ease.OutQuad)
                .OnComplete(()=> SwapIndex(targetSlot));

            targetSlot._transform.DOMove(targetSlot.CurrentPosition, duration)
                .SetEase(Ease.Linear);
        }
        
        public void MoveTo(Vector3 targetPosition)
        {
            CurrentPosition = targetPosition;
            _transform.position = CurrentPosition;
        }

        private void SwapIndex(KulaySlot targetSlot)
        {
            var targetSlotIndex = targetSlot.SlotIndex;
            targetSlot.SlotIndex = SlotIndex;
            SlotIndex = targetSlotIndex;
        }

        public void SetBooster(BoosterType type)
        {
            booster = type;
            _popped = false;
            
            LoadSlotIcon();
        }

        private int _slotIndex;
        private Transform _transform;
        private Kulay _currentKulay;
        private Button _button;
        private Vector3 _initialPosition;
        private bool _popped;
        private BoosterDisplayHandler _boosterDisplayHandler;

        private void Awake()
        {
            _transform = transform;
            _button = GetComponent<Button>();
            _boosterDisplayHandler = GetComponentInChildren<BoosterDisplayHandler>();
            _currentKulay = randomizeKulayAtStart ? Settings.Kulay.Random : defaultKulay;
        }

        private void Start()
        {
            LoadSlotIcon();
            _button.onClick.AddListener(SlotTap);
        }

        private void SlotTap()
        {
            OnTap.Invoke(this);
        }

        private void LoadSlotIcon()
        {
            if (!IsBoostSlot)
            {
                if (_boosterDisplayHandler)
                    _boosterDisplayHandler.HideIcon();
                slotIcon.sprite = Settings.Kulay.GetData(_currentKulay).icon;
            }
            else
            {
                if (_boosterDisplayHandler)
                {
                    slotIcon.sprite = null;
                    slotIcon.DOFade(0, 0);
                    _boosterDisplayHandler.SetIcon(booster, _currentKulay);
                }
                else
                {
                    slotIcon.sprite = Settings.Booster.GetIcon(booster);
                }
            }
        }

        public void Renew(Vector3 spawnPosition, float duration)
        {
            _transform.position = spawnPosition;
            _transform.DOMove(CurrentPosition, duration)
                .SetEase(Ease.OutQuad);

            _popped = false;
            _currentKulay = Settings.Kulay.Random;
            LoadSlotIcon();
            
            slotIcon.DOFade(1, 0.1f);
        }
    }
}