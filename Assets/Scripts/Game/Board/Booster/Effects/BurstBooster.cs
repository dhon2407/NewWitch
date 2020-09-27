using System.Collections.Generic;
using Utilities.Helpers;

namespace Game.Board.Booster.Effects
{
    public class BurstBooster : IBooster
    {
        public List<int> GetAffectedIndexes(int sourceIndex, List<KulaySlot> boardSlots, int currentBoardSideCount)
        {
            return new List<int>(sourceIndex.GetAdjacentIndex(currentBoardSideCount, true));
        }
    }
}