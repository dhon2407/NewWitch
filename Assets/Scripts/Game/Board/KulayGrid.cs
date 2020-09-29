using System;
using System.Collections.Generic;
using System.Linq;
using Game.Board.Booster;
using Game.Board.PowerUp;
using GameData;
using Managers;
using MEC;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.UI;
using Utilities.Helpers;

namespace Game.Board
{
    [RequireComponent(typeof(GridLayoutGroup))]
    [HideMonoScript]
    public class KulayGrid : MonoBehaviour
    {
        [SerializeField] private bool showSlotIndex = false;
        [SerializeField, MinValue(0.1f)] private float slotMoveDuration = 0.2f;
        [SerializeField, MinValue(0.1f)] private float tapUnlockDelay = 0.5f;
        [SerializeField, MinValue(1)] private int slotSpawnOffset = 1;
        [ShowInInspector, ReadOnly] private int _gridSideCount = 0;

        public void Shuffle()
        {
            var activeSlots = (from slot in _slots where !slot.Popped select slot).ToList();
            var shuffledSlots = (from slot in _slots where !slot.Popped select slot).ToList();
            shuffledSlots.Shuffle();

            for (int i = 0; i < activeSlots.Count; i++)
                activeSlots[i].Change(shuffledSlots[i]);
        }

        private const float EmptySlotCheckDelay = 0.1f;
        
        private List<KulaySlot> _slots;
        private GridLayoutGroup _gridLayout;
        private List<Vector3> _slotsPosition;
        private List<Vector3> _spawnPosition;
        private BoosterHandler _boosterHandler;
        private PowerUpHandler _powerUpHandler;
        private bool _boardBusy;

        private void Awake()
        {
            _slots = new List<KulaySlot>(GetComponentsInChildren<KulaySlot>());
            _slotsPosition = new List<Vector3>(new Vector3[_slots.Count]);
            _gridLayout = GetComponent<GridLayoutGroup>();
            _gridSideCount = _gridLayout.constraintCount;
            _boosterHandler = GetComponent<BoosterHandler>();
            _powerUpHandler = GetComponent<PowerUpHandler>();
            
            _spawnPosition = new List<Vector3>(new Vector3[_gridSideCount]);
        }

        private void Start()
        {
            SetupSlotButtonEvents();
            InitializeGridData();
        }

        private void SetupSlotButtonEvents()
        {
            foreach (var slot in _slots)
                slot.OnTap.AddListener(OnSlotTap);
        }

        private void OnSlotTap(KulaySlot slot)
        {
            if (_boardBusy)
                return;

            if (Manager.Board.PowerUps.OnEffect)
                _powerUpHandler.Execute(Manager.Board.PowerUps.Consume(), slot.SlotIndex, _slots, _gridSideCount, _boosterHandler);
            else if (slot.IsBoostSlot)
                _boosterHandler.ExecuteBooster(slot, _slots, _gridSideCount);
            else
                if (PopSingleSlot(slot)) return;

            _boardBusy = true;
            Action moveSlot = MoveSlots;
            moveSlot.DelayInvoke(0.8f);
        }

        private bool PopSingleSlot(KulaySlot slot)
        {
            var chainCount = ChainPop(slot, new List<int>(slot.SlotIndex));

            if (chainCount <= 1)
            {
                _boardBusy = false;
                return true;
            }

            if (chainCount == 5)
                slot.SetBooster(BoosterType.Slice);
            else if (chainCount == 6)
                slot.SetBooster(BoosterType.Burst);
            else if (chainCount >= 7)
                slot.SetBooster(BoosterType.SameSlot);
            else
                slot.Pop();
            return false;
        }

        private void MoveSlots()
        {
            var rowIndexes = new int[_gridSideCount];
            for (int i = 0; i < _gridSideCount; i++)
                rowIndexes[i] = i;

            MoveRowSlots(GetRowMovements(rowIndexes));
            RefreshEmptySlotsPosition(slotMoveDuration);

            Action unlockDelay = () => _boardBusy = false;
            unlockDelay.DelayInvoke(slotMoveDuration + tapUnlockDelay);
        }

        private void MoveRowSlots(IEnumerable<(int index, int moveStep)> movements)
        {
            foreach (var (index, moveStep) in movements)
            {
                if (moveStep <= 0)
                    continue;

                var slot = GetSlotAt(index);
                if (!slot)
                    continue;

                var targetSlot = GetSlotAt(index + _gridSideCount * moveStep);

                if (targetSlot != null)
                    slot.MoveTo(targetSlot, _slotsPosition[targetSlot.SlotIndex], slotMoveDuration);
            }
        }

        private List<(int index, int moveStep)> GetRowMovements(int[] rowIndexes)
        {
            var indexDownMovement = new List<(int index, int moveStep)>();
            foreach (var rowIndex in rowIndexes)
            {
                var columnIndex = new List<int>(rowIndex.GetColumnIndexes(_gridSideCount));
                for (int i = 0; i < columnIndex.Count; i++)
                {
                    var index = columnIndex[i];
                    indexDownMovement.Add((index,
                        FreeSlotCount(columnIndex.GetRange(i + 1, _gridSideCount - (i + 1)))));
                }
            }

            return indexDownMovement;
        }

        private void RefreshEmptySlotsPosition(float delay = 0)
        {
            Action action = () =>
            {
                foreach (var slot in _slots.Where(slot => slot.Popped))
                    slot.MoveTo(_slotsPosition[slot.SlotIndex]);

                ReplenishEmptySlots();
            };
            
            action.DelayInvoke(delay + EmptySlotCheckDelay);
        }

        private void ReplenishEmptySlots()
        {
            var emptySlotsIndex = from slot in _slots where slot.Popped select slot.SlotIndex;

            foreach (var index in emptySlotsIndex)
                GetSlotAt(index)?.Renew(_spawnPosition[index % _gridSideCount], slotMoveDuration);
        }

        private int FreeSlotCount(List<int> belowIndexes)
        {
            var freeCount = 0;
            foreach (var belowIndex in belowIndexes)
            {
                var slot = GetSlotAt(belowIndex);
                if (slot && slot.Popped)
                    freeCount++;
            }

            return freeCount;
        }

        private KulaySlot GetSlotAt(int index)
        {
            foreach (var slot in _slots)
                if (slot.SlotIndex == index)
                    return slot;
            
            return null;
        }

        private int ChainPop(KulaySlot slot, List<int> excludedIndexes)
        {
            var chainCount = 0;
            var currentKulay = slot.Kulay;

            var adjacentIndexes = slot.SlotIndex.GetAdjacentIndex(_gridSideCount);
            foreach (var adjacentIndex in adjacentIndexes)
            {
                if (excludedIndexes.Contains(adjacentIndex))
                    continue;
                
                var adjacentSlot = GetSlotAt(adjacentIndex);

                if (adjacentSlot == null || adjacentSlot.Popped || adjacentSlot.IsBoostSlot)
                    continue;

                if (adjacentSlot.Kulay != currentKulay)
                    continue;

                chainCount++;
                excludedIndexes.Add(adjacentIndex);
                adjacentSlot.Pop();
                chainCount += ChainPop(adjacentSlot, excludedIndexes);
            }

            return chainCount;
        }

        private void InitializeGridData()
        {
            Timing.RunCoroutine(RefreshGridData());
        }

        private IEnumerator<float> RefreshGridData()
        {
            _gridLayout.enabled = true;
            yield return Timing.WaitForOneFrame;
            _gridLayout.enabled = false;

            for (int i = 0; i < _slots.Count; i++)
            {
                var currentIndex = i;
                _slots[i].Initialize(currentIndex, showSlotIndex);
                _slotsPosition[i] = _slots[currentIndex].CurrentPosition;
            }

            SetSpawnPosition();
        }

        private void SetSpawnPosition()
        {
            var slotVerticalDistance = _gridLayout.cellSize.y + _gridLayout.spacing.y;

            for (int i = 0; i < _spawnPosition.Count; i++)
            {
                var slot = GetSlotAt(i);
                if (!slot)
                    continue;
                
                var position = slot.CurrentPosition;
                position.y += slotVerticalDistance * slotSpawnOffset;
                _spawnPosition[i] = position;
            }
            
        }

#if UNITY_EDITOR
        [Title("Debug"), PropertyOrder(int.MaxValue)]
        [Button(ButtonSizes.Large), DisableInEditorMode]
        private void TestShuffle()
        {
            Shuffle();
        }
#endif
    }
}