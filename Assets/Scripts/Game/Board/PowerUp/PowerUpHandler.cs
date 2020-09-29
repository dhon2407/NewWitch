using System;
using System.Collections.Generic;
using System.Linq;
using Game.Board.Booster;
using GameData;
using UnityEngine;
using Utilities.Helpers;

namespace Game.Board.PowerUp
{
    public class PowerUpHandler : MonoBehaviour
    {
        public void Execute(PowerUpType type, int targetSlotIndex, List<KulaySlot> boardSlots, int boardSideCount, BoosterHandler boosterHandler)
        {
            switch (type)
            {
                case PowerUpType.None:
                    return;
                case PowerUpType.VerticalSlice:
                    PopSlots(new List<int>(targetSlotIndex.GetAdjacentLineIndex(boardSideCount, false)), boardSlots,
                        boosterHandler);
                    break;
                case PowerUpType.HorizontalSlice:
                    PopSlots(new List<int>(targetSlotIndex.GetAdjacentLineIndex(boardSideCount, true)), boardSlots,
                        boosterHandler);
                    break;
                case PowerUpType.ClearSlot:
                    PopSlots(new List<int>{targetSlotIndex}, boardSlots, boosterHandler);
                    break;
                case PowerUpType.Shuffle:
                    Shuffle(boardSlots);
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(type), type, null);
            }
        }

        private void Shuffle(List<KulaySlot> boardSlots)
        {
            var activeSlots = (from slot in boardSlots where !slot.Popped select slot).ToList();
            var shuffledSlots = (from slot in boardSlots where !slot.Popped select slot).ToList();
            shuffledSlots.Shuffle();

            for (int i = 0; i < activeSlots.Count; i++)
                activeSlots[i].Change(shuffledSlots[i]);
        }
        
        private void PopSlots(List<int> popSlotIndexes, List<KulaySlot> boardSlots,  BoosterHandler boosterHandler)
        {
            foreach (var slot in boardSlots.Where(slot => popSlotIndexes.Contains(slot.SlotIndex)))
            {
                var boosterType = slot.IsBoostSlot ? slot.Booster : (BoosterType?) null;
                slot.Pop();
                
                if (boosterType.HasValue)
                    boosterHandler.ExecuteBoosterEffect(slot.SlotIndex, boosterType.Value);
            }
        }
    }
}