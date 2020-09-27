using System.Collections.Generic;
using System.Linq;

namespace Game.Board.Booster.Effects
{
    public class SameSlotBooster : IBooster
    {
        public List<int> GetAffectedIndexes(int sourceIndex, List<KulaySlot> boardSlots, int currentBoardSideCount)
        {
            var sameSlots = new List<int>();
            var referenceSlot = boardSlots.Find(slot => slot.SlotIndex == sourceIndex);

            if (referenceSlot == null)
                return sameSlots;

            sameSlots.AddRange(from slot in boardSlots
                where !slot.Popped && !slot.IsBoostSlot && slot.Kulay == referenceSlot.Kulay
                select slot.SlotIndex);

            return sameSlots;
        }
    }
}