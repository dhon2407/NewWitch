using Game.Board.Booster.Effects;
using GameData;

namespace Game.Board.Booster
{
    public class BoosterFactory
    {
        public IBooster Build(BoosterType type)
        {
            switch (type)
            {
                case BoosterType.Slice: return new SliceBooster();
                case BoosterType.Burst: return new BurstBooster();
                case BoosterType.BigBurst: return new BigBurstBooster();
                case BoosterType.SameSlot: return new SameSlotBooster();
                case BoosterType.CrossSlice: return new CrossSliceBooster();
                case BoosterType.BigSlice: return new BigSliceBooster();
                case BoosterType.BigCrossSlice: return new BigCrossSliceBooster();
                case BoosterType.BurstAll: return new BurstAllBooster();
                case BoosterType.None: return new NoEffect();
                default: return new NoEffect();    
            }
        }
    }
}