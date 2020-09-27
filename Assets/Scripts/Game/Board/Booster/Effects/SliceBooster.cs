using System.Collections.Generic;
using UnityEngine;
using Utilities.Helpers;

namespace Game.Board.Booster.Effects
{
    public class SliceBooster : IBooster
    {
        public List<int> GetAffectedIndexes(int sourceIndex, List<KulaySlot> boardSlots, int currentBoardSideCount)
        {
            var isHorizontal = Random.Range(0f, 1f) > 0.5f;
            return new List<int>(sourceIndex.GetAdjacentLineIndex(currentBoardSideCount, isHorizontal));
        }
    }
}