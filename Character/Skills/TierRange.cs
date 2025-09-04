
namespace Saga.Character.Skills
{
    public class TierRange
    {
        public int Min { get; set; }
        public int Max { get; set; }

        public TierRange() { }

        public TierRange(int min, int max) {
            Min = min;
            Max = max;
        }
    }
}
