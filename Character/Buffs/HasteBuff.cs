
using Saga.Assets;

namespace Saga.Character.Buffs
{
    public class HasteBuff(int duration, int speedBonus) : BuffBase("Haste", duration) {
        public override int AttackSpeed { get; } = speedBonus;
        public override int CastingSpeed { get; } = speedBonus;

        public override void OnApply(Player player) {
            HUDTools.Print($" You've gained faster attack speed and casting speed for {Duration} turns.", 3);
        }

        public override void OnRemove(Player player) {
            HUDTools.Print($" Your {Name} buff has worn off", 3);
        }

    }
}
