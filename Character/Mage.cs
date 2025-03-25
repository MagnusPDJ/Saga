using System;
using Saga.Assets;
using Saga.Dungeon;
using Saga.Items;
using Saga.Items.Loot;

namespace Saga.Character
{
    public class Mage : Player
    {
        public Mage(string name) : base(name, 0, 1, 1, 1, 1, 1) {
            CurrentClass = "Mage";
        }
        public override void LevelUp() {
            int levels = 0;
            Program.SoundController.Play("levelup");
            while (CanLevelUp()) {
                Program.CurrentPlayer.Exp -= GetLevelUpValue();
                Program.CurrentPlayer.Level++;
                Program.CurrentPlayer.FreeAttributePoints++;
                levels++;
            }
            PrimaryAttributes levelUpValues = new() { Constitution = 1 * levels, Strength = 0 * levels, Dexterity = 0 * levels, Intellect = 1 * levels, WillPower = 1 * levels };

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

            int dmgfromattribute = (1 + TotalPrimaryAttributes.Intellect) / 3;

            return (weaponDPT.Item1 + dmgfromattribute, weaponDPT.Item2 + dmgfromattribute);
        }
        public override string Equip(Weapon weapon) {
            if (weapon.ItemLevel > Level) {
                Console.WriteLine($"Character needs to be level {weapon.ItemLevel} to equip this item");
                return "Item not equipped";
            } else if (weapon.WeaponType != WeaponTypes.Tome && weapon.WeaponType != WeaponTypes.Staff && weapon.WeaponType != WeaponTypes.Wand) {
                Console.WriteLine($"Character can't equip a weapon {weapon.WeaponType}");
                return "Item not equipped";
            }
            if (Equipment.TryGetValue(Slot.Weapon, out Item value)) {
                Console.WriteLine($"Do you want to switch '{value.ItemName}' for '{weapon.ItemName}'? (Y/N)");
                while (true) {
                    string input = TextInput.PlayerPrompt(true);
                    if (input == "y") {
                        UnEquip(Slot.Weapon, Equipment[Slot.Weapon]);
                        Equipment[weapon.ItemSlot] = weapon;
                        int a = Array.IndexOf(Program.CurrentPlayer.Inventory, weapon);
                        Program.CurrentPlayer.Inventory.SetValue(null, a);
                        return "New weapon equipped!";
                    } else if (input == "n") {
                        return "Item not equipped";
                    } else {
                        Console.WriteLine("Invalid input");
                    }
                }
            } else {
                Equipment[weapon.ItemSlot] = weapon;
                int a = Array.IndexOf(Program.CurrentPlayer.Inventory, weapon);
                if (a == -1) {
                } else {
                    Program.CurrentPlayer.Inventory.SetValue(null, a);
                }
                Program.CurrentPlayer.CalculateTotalStats();
                return "New weapon equipped!";
            }            
        }
        public override string Equip(Armor armor) {
            if (armor.ItemLevel > Level) {
                Console.WriteLine($"Character needs to be level {armor.ItemLevel} to equip this item");
                return "Item not equipped";
            } else if (armor.ArmorType != ArmorType.Cloth && armor.ArmorType != ArmorType.Leather) {
                Console.WriteLine($"Character can't equip a {armor.ArmorType} armor");
                return "Item not equipped";
            }
            if (Equipment.TryGetValue(armor.ItemSlot, out Item value)) {
                Console.WriteLine($"Do you want to switch '{value.ItemName}' for '{armor.ItemName}'? (Y/N)");
                while (true) {
                    string input = TextInput.PlayerPrompt(true);
                    if (input == "y") {
                        UnEquip(armor.ItemSlot, Equipment[armor.ItemSlot]);
                        Equipment[armor.ItemSlot] = armor;
                        int a = Array.IndexOf(Program.CurrentPlayer.Inventory, armor);
                        Program.CurrentPlayer.Inventory.SetValue(null, a);
                        return "New armor equipped!";
                    }
                    else if (input == "n") {
                        return "Item not equipped";
                    }
                    else {
                        Console.WriteLine("Invalid input");
                    }
                }
            } else {
                Equipment[armor.ItemSlot] = armor;
                int a = Array.IndexOf(Program.CurrentPlayer.Inventory, armor);
                if (a == -1) {
                } else {
                    Program.CurrentPlayer.Inventory.SetValue(null, a);
                }                
                Program.CurrentPlayer.CalculateTotalStats();
                return "New armor piece equipped!";
            }
        }
        public override string Equip(Potion potion) {
            Equipment[potion.ItemSlot] = potion;
            Program.CurrentPlayer.CalculateTotalStats();
            return "New potion equipped!";
        }
        public override string UnEquip(Slot slot, Item item) {
            int index = Array.FindIndex(Inventory, i => i == null || Inventory.Length == 0);
            Program.CurrentPlayer.Inventory.SetValue(item, index);
            Program.CurrentPlayer.Equipment.Remove(slot);
            Program.CurrentPlayer.CalculateTotalStats();
            if (Program.CurrentPlayer.Health > Program.CurrentPlayer.TotalSecondaryAttributes.MaxHealth) {
                Program.CurrentPlayer.Health = Program.CurrentPlayer.TotalSecondaryAttributes.MaxHealth;
            }
            return "Item unequipped!";
        }
        public override void SetStartingGear() {
            Equip(WeaponLootTable.CrackedWand);
            Equip(ArmorLootTable.LinenRags);
        }
        public override void CombatActions(Enemy Monster, Encounters TurnTimer) {
            Console.WriteLine("Choose an action...");
            string input = TextInput.PlayerPrompt(true);
            if (input == "a" || input == "attack") {
                //Attack
                int damage = Attack(Monster);
                Monster.Health -= damage;
                HUDTools.WriteCombatLog("attack", TurnTimer, 0, damage, Monster);
                TurnTimer.TurnTimer++;
            }
            else if (input == "d" || input == "defend") {
                //Defend 
                Defend(Monster);
                HUDTools.WriteCombatLog("defend", TurnTimer, 0, 0, Monster);
                TurnTimer.TurnTimer++;
            }
            else if (input == "r" || input == "run") {
                //Run                   
                if (RunAway(Monster)) {
                    Program.SoundController.Stop();
                    HUDTools.ClearLog();
                    TurnTimer.Ran = true;
                }
                else {
                    HUDTools.WriteCombatLog(action: "run", TurnTimer: TurnTimer, Monster: Monster);
                    TurnTimer.TurnTimer++;
                }
            }
            else if (input == "h" || input == "heal") {
                //Heal
                Heal();
                HUDTools.WriteCombatLog(action: "heal", TurnTimer: TurnTimer, Monster: Monster);
                TurnTimer.TurnTimer++;
            }
            else if (input == "c" || input == "character" || input == "character screen") {
                HUDTools.CharacterScreen();
            }
            else if (input == "l" || input == "log" || input == "combat log") {
                Console.Clear();
                HUDTools.GetLog();
            } 
            else if (input == "q" || input == "questlog") {
                HUDTools.QuestLogHUD();
            }
            TextInput.PressToContinue();
        }
        public override void Heal() {
            if (Program.CurrentPlayer.CurrentHealingPotion.PotionQuantity == 0) {
                HUDTools.Print("No potions left!", 10);
            }
            else {
                HUDTools.Print("You use a potion amplified by your magic", 10);
                Program.CurrentPlayer.Health += Program.CurrentPlayer.CurrentHealingPotion.PotionPotency + 1 + Program.CurrentPlayer.Level * 2;
                Program.CurrentPlayer.CurrentHealingPotion.PotionQuantity--;
                if (Program.CurrentPlayer.Health > Program.CurrentPlayer.TotalSecondaryAttributes.MaxHealth) {
                    Program.CurrentPlayer.Health = Program.CurrentPlayer.TotalSecondaryAttributes.MaxHealth;
                }
                if (Program.CurrentPlayer.Health == Program.CurrentPlayer.TotalSecondaryAttributes.MaxHealth) {
                    HUDTools.Print("You heal to max health!", 10);
                }
                else {
                    HUDTools.Print($"You gain {Program.CurrentPlayer.CurrentHealingPotion.PotionPotency + 1 + Program.CurrentPlayer.Level * 2} health", 10);
                }
            }
        }
        public static int Attack(Enemy Monster) {
            HUDTools.Print($"You shoot an arcane missile from your {Program.CurrentPlayer.Equipment[Slot.Weapon].ItemName}", 15);
            int attack = Program.Rand.Next(Program.CurrentPlayer.CalculateDPT().Item1, Program.CurrentPlayer.CalculateDPT().Item2 + 1);
            HUDTools.Print($"You deal {attack} damage to {Monster.Name}", 10);
            return attack;
        }
        public static void Defend(Enemy Monster) {
            HUDTools.Print($"You defend the next three turns against {Monster.Name}", 20);
            Monster.AttackDebuff += 3+1;
        }
        public static bool RunAway(Enemy Monster) {
            bool escaped = false;
            if (Program.Rand.Next(0, 3) == 0 || Monster.Name == "Human captor") {
                HUDTools.Print($"You try to run from the {Monster.Name}, but it knocks you down. You are unable to escape this turn", 15);
            } else {
                HUDTools.Print($"You barely manage to shake off the {Monster.Name} and you successfully escape.", 15);
                escaped = true;
            }
            return escaped;
        }
    }
}
