using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace Utilities.Helpers
{
    public static class GridIndex
    {
        public static int[] GetAdjacentLineIndex(this int index, int gridSideCount, bool isHorizontal)
        {
            var lineIndexes = new int[gridSideCount];
            if (isHorizontal)
            {
                var minIndex = index / gridSideCount * gridSideCount;
                for (int i = 0; i < gridSideCount; i++)
                    lineIndexes[i] = minIndex + i;
            }
            else
            {
                var minIndex = index % gridSideCount;
                for (int i = 0; i < gridSideCount; i++)
                    lineIndexes[i] = minIndex + i * gridSideCount;
            }

            return lineIndexes;
        }
        
        public static int[] GetAdjacentIndex(this int index, int gridSideCount, bool includeDiagonals = false)
        {
            if (IsCornerIndex(index, gridSideCount))
                return GetCornerAdjacentIndexes(index, gridSideCount, includeDiagonals);
            if (IsSideIndex(index,gridSideCount))
                return GetSideAdjacentIndexes(index, gridSideCount, includeDiagonals);
            
            return GetMiddleAdjacentIndexes(index, gridSideCount, includeDiagonals); 
        }

        public static int[] GetRotatingIndex(this int index, int gridSideCount)
        {
            if (IsCornerIndex(index, gridSideCount))
                return GetCornerAdjacentIndexes(index, gridSideCount, true);
            if (IsSideIndex(index, gridSideCount))
                return GetSideAdjacentIndexes(index, gridSideCount, true);

            return GetMiddleAdjacentIndexes(index, gridSideCount, true);
        }

        public static int[] GetColumnIndexes(this int index, int gridSideCount)
        {
            var columnIndexes = new int[gridSideCount];
            var referenceIndex = index % gridSideCount;

            for (int i = 0; i < columnIndexes.Length; i++)
                columnIndexes[i] = referenceIndex + i * gridSideCount;

            return columnIndexes;
        }

        public static int[] GetBottomIndexes(this int gridSideCount)
        {
            var bottomIndexes = new int[gridSideCount];

            for (int i = 0; i < gridSideCount; i++)
                bottomIndexes[i] = gridSideCount * gridSideCount - (gridSideCount - i);

            return bottomIndexes;
        }

        public static int? GetBelowIndex(this int index, int gridSideCount)
        {
            var adjacent = index.GetAdjacentIndex(gridSideCount);

            if (adjacent.Contains(index + gridSideCount))
                return index + gridSideCount;
            
            return null;
        }

        private static int[] GetMiddleAdjacentIndexes(int index, int sideCount, bool includeDiagonals = false)
        {
            var indexes = new List<int> {index + 1, index - 1, index + sideCount, index - sideCount};
            
            if (!includeDiagonals)
                return indexes.ToArray();
            
            indexes.Add(index + sideCount + 1);
            indexes.Add(index + sideCount - 1);
            indexes.Add(index - sideCount + 1);
            indexes.Add(index - sideCount - 1);

            return indexes.ToArray();
        }

        private static int[] GetSideAdjacentIndexes(int index, int sideCount, bool includeDiagonals = false)
        {
            var indexes = new List<int>();
            //Top Side
            if (index < sideCount - 1)
            {
                indexes.Add(index + 1);
                indexes.Add(index - 1);
                indexes.Add(index + sideCount);

                if (!includeDiagonals)
                    return indexes.ToArray();
                
                indexes.Add(index + sideCount - 1);
                indexes.Add(index + sideCount + 1);
            }
            //Bottom Side
            else if (index > sideCount * (sideCount - 1) && index < sideCount * sideCount - 1)
            {
                indexes.Add(index + 1);
                indexes.Add(index - 1);
                indexes.Add(index - sideCount);

                if (!includeDiagonals)
                    return indexes.ToArray();
                
                indexes.Add(index - sideCount + 1);
                indexes.Add(index - sideCount - 1);
            }
            //Left Side
            else if (index % sideCount == 0)
            {
                indexes.Add(index + 1);
                indexes.Add(index + sideCount);
                indexes.Add(index - sideCount);

                if (!includeDiagonals)
                    return indexes.ToArray();
                
                indexes.Add(index + sideCount + 1);
                indexes.Add(index - sideCount + 1);
            }
            //Right Side
            else if ((index + 1) % sideCount == 0)
            {
                indexes.Add(index - 1);
                indexes.Add(index + sideCount);
                indexes.Add(index - sideCount);

                if (!includeDiagonals)
                    return indexes.ToArray();
                
                indexes.Add(index + sideCount - 1);
                indexes.Add(index - sideCount - 1);
            }
            
            return indexes.ToArray();
        }

        private static int[] GetCornerAdjacentIndexes(int index, int sideCount, bool includeDiagonals = false)
        {
            var indexes = new List<int>();
            if (index == 0)
            {
                indexes.Add(1);
                indexes.Add(sideCount);
                if (includeDiagonals)
                    indexes.Add(sideCount + 1);
            }
            else if (index == sideCount - 1)
            {
                indexes.Add(index - 1);
                indexes.Add(index + sideCount);
                if (includeDiagonals)
                    indexes.Add(index + sideCount - 1);
            }
            else if (index == sideCount * sideCount - sideCount)
            {
                indexes.Add(index - sideCount);
                indexes.Add(index + 1);
                if (includeDiagonals)
                    indexes.Add(index - sideCount + 1);
            }
            else if (index == sideCount * sideCount - 1)
            {
                indexes.Add(index - 1);
                indexes.Add(index - sideCount);
                if (includeDiagonals)
                    indexes.Add(index - sideCount - 1);
            }

            return indexes.ToArray();
        }

        private static bool IsCornerIndex(int index, int sideCount)
        {
            if (index == 0)
                return true;
            if (index == sideCount - 1)
                return true;
            if (index == sideCount * sideCount - sideCount)
                return true;
            if (index == sideCount * sideCount - 1)
                return true;
                
            return false;
        }
        
        private static bool IsSideIndex(int index, int sideCount)
        {
            if (IsCornerIndex(index, sideCount))
                return false;
            if (index != 0 && index < sideCount - 1)
                return true;
            if (index > sideCount * (sideCount - 1) && index < sideCount * sideCount - 1)
                return true;
            if (index % sideCount == 0)
                return true;
            if ((index + 1) % sideCount == 0)
                return true;
            if (index == sideCount)
                return true;

            return false;
        }
    }
}