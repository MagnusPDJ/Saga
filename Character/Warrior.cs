using Saga.Assets;
using Saga.Items;
using Saga.Dungeon;
using System.Collections.Generic;
using System.Text.Json;

namespace Saga.Character
{
    public class Warrior : Player
    {
        public Warrior(string name) : base(name, 0, 1, 1, 1, 1, 0) {
            CurrentClass = "Warrior";
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
            PrimaryAttributes levelUpValues = new() { Constitution = 1*levels, Strength = 1*levels, Dexterity = 0*levels, Intellect = 0*levels, WillPower = 0*levels};
            
            BasePrimaryAttributes += levelUpValues;

            CalculateTotalStats();
            Program.CurrentPlayer.Health = Program.CurrentPlayer.TotalSecondaryAttributes.MaxHealth;

            HUDTools.Print($"\u001b[34mCongratulations! You are now level {Level}! You've gained 1 attribute point.\u001b[0m", 20);      
        }
        public override (int, int) CalculateDPT() {
            TotalPrimaryAttributes = CalculatePrimaryArmorBonus();
            (int, int) weaponDPT = CalculateWeaponDPT();
            if (weaponDPT == (0, 0)) {
                return (1, 1);
            }

            int dmgfromattribute = TotalPrimaryAttributes.Strength / 3;
            int dmgfromwarrior = Level;
            return (weaponDPT.Item1 + dmgfromattribute+dmgfromwarrior, weaponDPT.Item2 + dmgfromattribute+dmgfromwarrior);
        }

        public override void SetStartingGear() {
            List<ItemBase> weapons = JsonSerializer.Deserialize<List<ItemBase>>(HUDTools.ReadAllResourceText("Saga.Items.Loot.WeaponLootTable.json"));
            List<ItemBase> armors = JsonSerializer.Deserialize<List<ItemBase>>(HUDTools.ReadAllResourceText("Saga.Items.Loot.ArmorLootTable.json"));
            ((IEquipable)weapons.Find(w => w.ItemName == "Rusty Sword")).Equip();
            ((IEquipable)armors.Find(w => w.ItemName == "Linen Rags")).Equip();
            HealingPotion healingPotion = new();
            healingPotion.Equip();
        }

        //      Skills
        public override void Defend(Enemy Monster) {
            HUDTools.Print($"You defend the next three turns against {Monster.Name}", 20);
            Monster.AttackDebuff += 3+1;
        }
        public override bool RunAway(Enemy Monster) {
            bool escaped = false;
            if (Program.Rand.Next(0, 3) == 0 || Monster.Name == "Human captor") {
                HUDTools.Print($"You try to run from the {Monster.Name}, but it knocks you down. You are unable to escape this turn", 15);
            }
            else {
                    HUDTools.Print($"You barely manage to shake off the {Monster.Name} and you successfully escape.", 20);
                escaped = true;
                Program.RoomController.ran = true;
            }
            return escaped;
        }
    }
}
