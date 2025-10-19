﻿using Saga.Assets;

namespace Saga.Character.Skills
{
    [Discriminator("haste")]
    public class Haste : ISelfSkill
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public int LevelRequired => 0;
        public bool IsUnlocked { get; set; }
        public TierRange Tier { get; set; } = new TierRange(1, 5);
        public int ManaCost { get; set; }
        public int Cooldown => 1;
        public int Timer { get; set; } = 0;
        public int ActionPointCost { get; set; } = 0;
        public string SpeedType => "Casting Speed";
        public Haste() {
            Name = "Haste";
            Description = "Being trained for combat you can take multiple actions during a turn. (Lowers the AP cost of attacks, skills and Potions)";
            IsUnlocked = true;
            ManaCost = 5;
        }
        public bool Activate(Player player) {
            if (player.SpendMana(ManaCost)) {

                // Buff logic here

                Timer = Cooldown;
                return true;
            } else {
                HUDTools.Print(" Not enough mana!", 10);
                TextInput.PressToContinue();
                HUDTools.ClearLastLine(3);
                return false;
            }
        }
    }
}
