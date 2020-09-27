using System.Collections.Generic;

namespace Game.Board.Booster.Effects
{
    public class NoEffect : IBooster
    {
        public List<int> GetAffectedIndexes(int sourceIndex, List<KulaySlot> boardSlots, int currentBoardSideCount)
        {
            return new List<int>();
        }
    }
}