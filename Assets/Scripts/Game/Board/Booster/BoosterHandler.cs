using System.Collections.Generic;
using System.Linq;
using GameData;
using UnityEngine;
using Utilities.Helpers;

namespace Game.Board.Booster
{
    public class BoosterHandler : MonoBehaviour
    {
        private List<KulaySlot> _currentSlots;
        private int _currentBoardSideCount;

        public void ExecuteBooster(KulaySlot boosterSlot, List<KulaySlot> boardSlots, int boardSideCount)
        {
            _currentSlots = boardSlots;
            _currentBoardSideCount = boardSideCount;
            
            ExecuteBoosterEffect(boosterSlot.SlotIndex, boosterSlot.Booster);
        }

        private void ExecuteBoosterEffect(int sourceIndex, BoosterType boosterSlotBooster)
        {
            switch (boosterSlotBooster)
            {
                case BoosterType.Slice:
                    var isHorizontal = Random.Range(0f, 1f) > 0.5f;
                    PopSlots(new List<int>(sourceIndex.GetAdjacentLineIndex(_currentBoardSideCount, isHorizontal)));
                    break;
                case BoosterType.Burst:
                    PopSlots(new List<int>(sourceIndex.GetAdjacentIndex(_currentBoardSideCount, true)));
                    break;
                case BoosterType.SameSlot:
                    PopSlots(GetSameSlotsTypeAs(sourceIndex));
                    break;
            }
        }

        private List<int> GetSameSlotsTypeAs(int sourceIndex)
        {
            var sameSlots = new List<int>();
            var referenceSlot = _currentSlots.Find(slot => slot.SlotIndex == sourceIndex);

            if (referenceSlot == null)
                return sameSlots;

            sameSlots.AddRange(from slot in _currentSlots where !slot.Popped && !slot.IsBoostSlot && slot.Kulay == referenceSlot.Kulay select slot.SlotIndex);

            return sameSlots;
        }

        private void PopSlots(List<int> popSlotIndexes)
        {
            foreach (var slot in _currentSlots.Where(slot => popSlotIndexes.Contains(slot.SlotIndex)))
            {
                var boosterType = slot.IsBoostSlot ? slot.Booster : (BoosterType?) null;
                slot.Pop();
                
                if (boosterType.HasValue)
                    ExecuteBoosterEffect(slot.SlotIndex, boosterType.Value);
            }
        }
    }
}