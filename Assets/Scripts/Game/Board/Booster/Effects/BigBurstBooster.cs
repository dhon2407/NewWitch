using System.Collections.Generic;
using Utilities.Helpers;

namespace Game.Board.Booster.Effects
{
    public class BigBurstBooster : IBooster
    {
        public List<int> GetAffectedIndexes(int sourceIndex, List<KulaySlot> boardSlots, int currentBoardSideCount)
        {
            var affectedIndexes = new List<int>(sourceIndex.GetAdjacentIndex(currentBoardSideCount, true));
            var extendedIndexes = new List<int>();

            foreach (var index in affectedIndexes)
            {
                var moreIndex = index.GetAdjacentIndex(currentBoardSideCount, true);

                foreach (var additionalIndex in moreIndex)
                {
                    if (affectedIndexes.Contains(additionalIndex) || extendedIndexes.Contains(additionalIndex))
                        continue;
                    
                    extendedIndexes.Add(additionalIndex);
                }
            }

            affectedIndexes.AddRange(extendedIndexes);

            return affectedIndexes;
        }
    }
}