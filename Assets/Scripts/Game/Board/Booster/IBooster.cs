using System.Collections.Generic;

namespace Game.Board.Booster
{
    public interface IBooster
    {
        List<int> GetAffectedIndexes(int sourceIndex, List<KulaySlot> boardSlots, int currentBoardSideCount);
    }
}