using System;
using System.Collections.Generic;
using System.Linq;
using GameData;
using UnityEngine;
using Utilities.Helpers;
using Random = UnityEngine.Random;

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
            var boostType = boosterSlot.Booster;

            if (boostType == BoosterType.BigBurst)
            {
                ExecuteBoosterEffect(boosterSlot.SlotIndex, boostType);
                boosterSlot.Pop();
                return;
            }
            
            var adjacentBoosterSlots = GetAdjacentBoosterSlots(boosterSlot);
            if (adjacentBoosterSlots.Count > 0)
            {
                var adjacentBoosterTypes = from slot in adjacentBoosterSlots where slot.IsBoostSlot select slot.Booster;
                boosterSlot.SetBooster(CombineBooster(boostType, adjacentBoosterTypes.ToList()));

                foreach (var adjacentSlot in adjacentBoosterSlots)
                    adjacentSlot.Pop();
            }
            else
            {
                ExecuteBoosterEffect(boosterSlot.SlotIndex, boostType);
                boosterSlot.Pop();    
            }
        }

        private BoosterType CombineBooster(BoosterType referenceBooster, ICollection<BoosterType> otherBoosters)
        {
            switch (referenceBooster.GetTier())
            {
                case BoosterTier.One:
                    if (referenceBooster.AllSame(otherBoosters))
                        return UpdateSameTier(referenceBooster);

                    var otherMaxTier = GetMaxTier(otherBoosters);
                    
                    if (otherMaxTier == BoosterTier.One || otherMaxTier == BoosterTier.Two)
                        return BoosterType.BigCrossSlice;

                    return BoosterType.BurstAll;
                case BoosterTier.Two:
                    if (referenceBooster.AllSame(otherBoosters))
                        return UpdateSameTier(referenceBooster);

                    return GetMaxTier(otherBoosters) < referenceBooster.GetTier() ? BoosterType.BigCrossSlice : BoosterType.BurstAll;
                case BoosterTier.Three:
                case BoosterTier.Four:
                    return BoosterType.BurstAll;
                default:
                    return BoosterType.None;
            }
        }

        private BoosterTier GetMaxTier(ICollection<BoosterType> otherBoosters)
        {
            var currentTier = BoosterTier.One;
            foreach (var booster in otherBoosters)
            {
                var boosterTier = booster.GetTier();
                if (boosterTier > currentTier)
                    currentTier = booster.GetTier();
            }

            return currentTier;
        }

        private BoosterType UpdateSameTier(BoosterType referenceBooster)
        {
            switch (referenceBooster)
            {
                case BoosterType.Slice:
                    return BoosterType.CrossSlice;
                case BoosterType.Burst:
                    return BoosterType.BigBurst;
                case BoosterType.SameSlot:
                    return BoosterType.BigSlice;
                case BoosterType.CrossSlice:
                case BoosterType.BigBurst:
                case BoosterType.BigSlice:
                case BoosterType.BigCrossSlice:
                case BoosterType.BurstAll:
                    return BoosterType.BurstAll;
                case BoosterType.None:
                    return BoosterType.None;
                default:
                    throw new ArgumentOutOfRangeException(nameof(referenceBooster), referenceBooster, null);
            }
        }

        private List<KulaySlot> GetAdjacentBoosterSlots(KulaySlot boosterSlot)
        {
            var adjacentIndexes = boosterSlot.SlotIndex.GetAdjacentIndex(_currentBoardSideCount);
            var adjacentSlots = new List<KulaySlot>(_currentSlots.FindAll(slot => adjacentIndexes.Contains(slot.SlotIndex)));
            
            return new List<KulaySlot>(adjacentSlots.FindAll(slot => slot.IsBoostSlot));
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
                case BoosterType.BigBurst:
                    PopSlots(GetSameSlotsTypeAs(sourceIndex), true);
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

        private void PopSlots(List<int> popSlotIndexes, bool clearAll = false)
        {
            foreach (var slot in _currentSlots.Where(slot => popSlotIndexes.Contains(slot.SlotIndex)))
            {
                var boosterType = slot.IsBoostSlot ? slot.Booster : (BoosterType?) null;
                slot.Pop();
                
                if (boosterType.HasValue && !clearAll)
                    ExecuteBoosterEffect(slot.SlotIndex, boosterType.Value);
            }
        }
    }
}