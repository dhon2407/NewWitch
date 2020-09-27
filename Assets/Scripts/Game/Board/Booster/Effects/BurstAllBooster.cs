using System.Collections.Generic;
using System.Linq;

namespace Game.Board.Booster.Effects
{
    public class BurstAllBooster : IBooster
    {
        public List<int> GetAffectedIndexes(int sourceIndex, List<KulaySlot> boardSlots, int currentBoardSideCount)
        {
            return boardSlots.Select(slot => slot.SlotIndex).ToList();
        }
    }
}