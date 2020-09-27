using System.Collections.Generic;
using Utilities.Helpers;

namespace Game.Board.Booster.Effects
{
    public class CrossSliceBooster : IBooster
    {
        public List<int> GetAffectedIndexes(int sourceIndex, List<KulaySlot> boardSlots, int currentBoardSideCount)
        {
            var affectedIndex = new List<int>(sourceIndex.GetAdjacentLineIndex(currentBoardSideCount, true));
            affectedIndex.AddRange(sourceIndex.GetAdjacentLineIndex(currentBoardSideCount, false));

            return affectedIndex;
        }
    }
}