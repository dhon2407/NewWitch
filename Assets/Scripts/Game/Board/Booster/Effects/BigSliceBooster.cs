using System.Collections.Generic;
using UnityEngine;
using Utilities.Helpers;

namespace Game.Board.Booster.Effects
{
    public class BigSliceBooster : IBooster
    {
        public List<int> GetAffectedIndexes(int sourceIndex, List<KulaySlot> boardSlots, int currentBoardSideCount)
        {
            var isHorizontal = Random.Range(0f, 1f) > 0.5f;
            var affectedIndex = new List<int>(sourceIndex.GetAdjacentLineIndex(currentBoardSideCount, isHorizontal)); 
            
            var adjacentAdditionalIndex = sourceIndex.GetAdjacentOrthoIndex(currentBoardSideCount, !isHorizontal);
            
            foreach (var adjacent in adjacentAdditionalIndex)
                affectedIndex.AddRange(adjacent.GetAdjacentLineIndex(currentBoardSideCount, isHorizontal));

            return affectedIndex;
        }
    }
}