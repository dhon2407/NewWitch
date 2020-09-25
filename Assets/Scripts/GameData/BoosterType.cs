using System;
using System.Collections.Generic;
using System.Linq;

namespace GameData
{
    public enum BoosterType
    {
        None = -1,
        Slice,
        Burst,
        SameSlot,
        CrossSlice,
        BigBurst,
        BigSlice,
        BigCrossSlice,
        BurstAll,
    }

    public enum BoosterTier
    {
        None = 0,
        One = 1,
        Two = 2,
        Three = 3,
        Four = 4,
    }

    public static class BoosterTypeHelper
    {
        public static BoosterTier GetTier(this BoosterType type)
        {
            switch (type)
            {
                case BoosterType.Slice:
                case BoosterType.Burst:
                case BoosterType.SameSlot:
                    return BoosterTier.One;
                case BoosterType.CrossSlice:
                case BoosterType.BigBurst:
                case BoosterType.BigSlice:
                    return BoosterTier.Two;
                case BoosterType.BigCrossSlice:
                    return BoosterTier.Three;
                case BoosterType.BurstAll:
                    return BoosterTier.Four;
                default: return BoosterTier.None;
            }
        }

        public static bool AllSame(this BoosterType type, ICollection<BoosterType> otherTypes)
        {
            return otherTypes.Count > 0 && otherTypes.All(otherType => otherType == type);
        }
    }
}