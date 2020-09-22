﻿using System.Collections.Generic;
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
            if (boosterSlotBooster == BoosterType.Slice)
            {
                var isHorizontal = Random.Range(0f, 1f) > 0.5f;

                PopSlots(new List<int>(sourceIndex.GetAdjacentLineIndex(_currentBoardSideCount, isHorizontal)));
            }
        }

        private void PopSlots(List<int> popSlotIndexes)
        {
            foreach (var slot in _currentSlots.Where(slot => popSlotIndexes.Contains(slot.SlotIndex)))
                slot.Pop();
        }
    }
}