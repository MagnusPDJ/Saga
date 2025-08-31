using Saga.Assets;
using Saga.Items;
using Saga.Character.Skills;
using System.Text.Json;
using System.Collections.Generic;
using Saga.Dungeon.Monsters;

namespace Saga.Character
{
    public class Archer : Player
    {
        public Archer(string name) : base(name, 0, 1, 1, 1) {
            CurrentClass = "Archer";
            SkillTree = new ArcherSkillTree();
        }

        //      Stats
        public override void LevelUp() {
            int levels = 0;
            Program.SoundController.Play("levelup");
            while (CanLevelUp()) {
                Program.CurrentPlayer.Exp -= GetLevelUpValue();
                Program.CurrentPlayer.Level++;
                Program.CurrentPlayer.FreeAttributePoints++;
                levels++;
            }
            Attributes levelUpValues = new() { Constitution = 1 * levels, Strength = 0 * levels, Dexterity = 1 * levels, Intellect = 0 * levels, WillPower = 0 * levels };

            BaseAttributes += levelUpValues;

            CalculateTotalStats();
            Program.CurrentPlayer.Health = Program.CurrentPlayer.TotalDerivedStats.MaxHealth;

            HUDTools.Print($"\u001b[34mCongratulations! You are now level {Level}! You've gained 1 attribute point.\u001b[0m", 20);
        }
        public override (int, int) CalculateDPT() {
            TotalAttributes = CalculatePrimaryArmorBonus();
            (int, int) weaponDPT = CalculateWeaponDPT();
            if (weaponDPT == (0, 0)) {
                return (1, 1);
            }

            int dmgfromattribute = (1+TotalAttributes.Dexterity) / 3;

            return (weaponDPT.Item1 + dmgfromattribute, weaponDPT.Item2 + dmgfromattribute);
        }

        public override void SetStartingGear() {
            List<IWeapon> weapons = JsonSerializer.Deserialize<List<IWeapon>>(HUDTools.ReadAllResourceText("Saga.Items.Loot.WeaponLootTable.json"), Program.Options);
            List<IArmor> armors = JsonSerializer.Deserialize<List<IArmor>>(HUDTools.ReadAllResourceText("Saga.Items.Loot.ArmorLootTable.json"), Program.Options);
            weapons.Find(w => w.ItemName == "Flimsy Bow").Equip();
            armors.Find(w => w.ItemName == "Linen Rags").Equip();
            HealingPotion healingPotion = new();
            healingPotion.Equip();
        }
        public override bool RunAway(Enemy Monster) {
            bool escaped = false;
            if (Monster.Name == "Human captor") {
                HUDTools.Print($"You try to run from the {Monster.Name}, but it knocks you down. You are unable to escape this turn", 15);
            }
            else {
                HUDTools.Print($"You use your crazy ninja moves to evade the {Monster.Name} and you successfully escape!", 20);
                escaped = true;
                Program.RoomController.ran = true;
            }
            return escaped;
        }
    }
}
