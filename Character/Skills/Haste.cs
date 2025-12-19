using Saga.Assets;
using Saga.Character.Buffs;

namespace Saga.Character.Skills
{
    [Discriminator("haste")]
    public class Haste : ISelfSkill
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public int LevelRequired => 0;
        public bool IsUnlocked { get; set; } = true;
        public TierRange Tier { get; set; } = new TierRange(1, 5);
        public int ManaCost { get; set; } = 5;
        public int Cooldown { get; set; } = 1;
        public int Timer { get; set; } = 0;
        public int ActionPointCost { get; set; } = 0;
        public string SpeedType => "Casting Speed";
        private int Duration { get; set; } = 1;
        private int SpeedBonus { get; set; } = 1;
        public Haste() {
            Name = "Haste";
            Description = "Being trained for combat you can take multiple actions during a turn. (Lowers the\n AP cost of attacks, skills and Potions)";
        }
        public bool Activate(Player player) {
            if (player.SpendMana(ManaCost)) {

                HUDTools.Print($" You've cast haste on yourself.", 10);
                player.BuffedStats.AddBuff(new HasteBuff(Duration, SpeedBonus));               
                Timer = Cooldown;
                TextInput.PressToContinue();
                HUDTools.ClearLastLine(3);
                return true;
            } else {
                HUDTools.Print(" Not enough mana!", 10);
                TextInput.PressToContinue();
                HUDTools.ClearLastLine(3);
                return false;
            }
        }
        public virtual void UpgradeTier() {
            Tier.Min++;
            Duration++;
            Cooldown++;
            ManaCost += 2;
            if (Tier.Min == Tier.Max) {
                SpeedBonus++;
            }
        }
    }
}
