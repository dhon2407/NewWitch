using System.Collections.Generic;
using Utilities.Helpers;

namespace Game.Board.Booster.Effects
{
    public class BigCrossSliceBooster : IBooster
    {
        public List<int> GetAffectedIndexes(int sourceIndex, List<KulaySlot> boardSlots, int currentBoardSideCount)
        {
            var affectedIndex = new List<int>();
            var adjacentIndexes = sourceIndex.GetAdjacentIndex(currentBoardSideCount);

            foreach (var adjacentIndex in adjacentIndexes)
            {
                var crossIndex = new List<int>(adjacentIndex.GetAdjacentLineIndex(currentBoardSideCount, true));
                crossIndex.AddRange(adjacentIndex.GetAdjacentLineIndex(currentBoardSideCount, false));

                foreach (var cIndex in crossIndex)
                {
                    if (affectedIndex.Contains(cIndex))
                        continue;
                    
                    affectedIndex.Add(cIndex);
                }
            }

            return affectedIndex;
        }
    }
}