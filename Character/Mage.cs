using Saga.Assets;
using Saga.Dungeon;
using Saga.Items;
using System.Text.Json;
using System.Collections.Generic;

namespace Saga.Character
{
    public class Mage : Player
    {
        public Mage(string name) : base(name, 0, 1, 1, 1, 1, 1) {
            CurrentClass = "Mage";
            SkillTree = new MageSkillTree();
        }

        //      Stats
        public override void LevelUp() {
            int levels = 0;
            Program.SoundController.Play("levelup");
            while (CanLevelUp()) {
                Program.CurrentPlayer.Exp -= GetLevelUpValue();
                Program.CurrentPlayer.Level++;
                Program.CurrentPlayer.FreeAttributePoints++;
                Program.CurrentPlayer.SkillPoints++;
                levels++;
            }
            PrimaryAttributes levelUpValues = new() { Constitution = 1 * levels, Strength = 0 * levels, Dexterity = 0 * levels, Intellect = 1 * levels, WillPower = 1 * levels };

            BasePrimaryAttributes += levelUpValues;

            CalculateTotalStats();
            Program.CurrentPlayer.Health = Program.CurrentPlayer.TotalSecondaryAttributes.MaxHealth;

            HUDTools.Print($"\u001b[34mCongratulations! You are now level {Level}! You've gained 1 attribute point and 1 skill point.\u001b[0m", 20);
        }
        public override (int, int) CalculateDPT() {
            TotalPrimaryAttributes = CalculatePrimaryArmorBonus();
            (int, int) weaponDPT = CalculateWeaponDPT();
            if (weaponDPT == (0, 0)) {
                return (1, 1);
            }

            int dmgfromattribute = (1 + TotalPrimaryAttributes.Intellect) / 3;

            return (weaponDPT.Item1 + dmgfromattribute, weaponDPT.Item2 + dmgfromattribute);
        }

        public override void SetStartingGear() {
            List<IWeapon> weapons = JsonSerializer.Deserialize<List<IWeapon>>(HUDTools.ReadAllResourceText("Saga.Items.Loot.WeaponLootTable.json"), Program.Options);
            List<IArmor> armors = JsonSerializer.Deserialize<List<IArmor>>(HUDTools.ReadAllResourceText("Saga.Items.Loot.ArmorLootTable.json"), Program.Options);
            weapons.Find(w => w.ItemName == "Cracked Wand").Equip();
            armors.Find(w => w.ItemName == "Linen Rags").Equip();
            HealingPotion healingPotion = new();
            healingPotion.Equip();
        }
        public override bool RunAway(Enemy Monster) {
            bool escaped = false;
            if (Program.Rand.Next(0, 3) == 0 || Monster.Name == "Human captor") {
                HUDTools.Print($"You try to run from the {Monster.Name}, but it knocks you down. You are unable to escape this turn", 15);
            } else {
                HUDTools.Print($"You barely manage to shake off the {Monster.Name} and you successfully escape.", 15);
                escaped = true;
                Program.RoomController.ran = true;
            }
            return escaped;
        }
    }
}
