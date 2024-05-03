﻿using System;
using Saga.assets;
using System.Reflection.Emit;
using Saga.Items;
using System.Security.Cryptography;
using Saga.Dungeon;

namespace Saga.Character
{
    [Serializable]
    public class Warrior : Player
    {
        public Warrior(string name) : base(name, 0, 1, 1, 1, 1, 0) {
            currentClass = "Warrior";
        }

        public override void LevelUp() {
            int levels = 0;
            AudioManager.soundLvlUp.Play();
            while (CanLevelUp()) {
                Program.CurrentPlayer.Exp -= GetLevelUpValue();
                Program.CurrentPlayer.Level++;
                levels++;
            }
            PrimaryAttributes levelUpValues = new PrimaryAttributes() { Constitution = 1*levels, Strength = 1*levels, Dexterity = 1*levels, Intellect = 1*levels, WillPower = 1*levels};
            
            BasePrimaryAttributes += levelUpValues;

            CalculateTotalStats();
            Program.CurrentPlayer.Health = Program.CurrentPlayer.BaseSecondaryAttributes.MaxHealth;

            Console.ForegroundColor = ConsoleColor.DarkBlue;
            HUDTools.Print($"Congratulations! You are now level {Level}!", 20);
            Console.ResetColor();            
            HUDTools.PlayerPrompt();
        }

        public override (int, int) CalculateDPT() {
            TotalPrimaryAttributes = CalculateArmorBonus();
            (int, int) weaponDPT = CalculateWeaponDPT();
            if (weaponDPT == (0, 0)) {
                return (1, 1);
            }

            double multiplier = 1 + TotalPrimaryAttributes.Strength / 100.0 + Level;

            return (Convert.ToInt32(Math.Floor(weaponDPT.Item1 * multiplier)),Convert.ToInt32(Math.Floor(weaponDPT.Item2*multiplier)));
        }

        public override string Equip(Weapon weapon) {
            if (weapon.ItemLevel > Level) {
                Console.WriteLine($"Character needs to be level {weapon.ItemLevel} to equip this item");
            }
            if (weapon.WeaponType != WeaponType.WEAPON_HAMMER && weapon.WeaponType != WeaponType.WEAPON_AXE && weapon.WeaponType != WeaponType.WEAPON_SWORD) {
                Console.WriteLine($"Character can't equip a {weapon.WeaponType}");
            }

            Equipment[weapon.ItemSlot] = weapon;
            return "New weapon equipped!";
        }

        public override string Equip(Armor armor) {
            if (armor.ItemLevel > Level) {
                Console.WriteLine($"Character needs to be level {armor.ItemLevel} to equip this item");
            }
            if (armor.ArmorType != ArmorType.ARMOR_MAIL && armor.ArmorType != ArmorType.ARMOR_PLATE && armor.ArmorType != ArmorType.ARMOR_LEATHER) {
                Console.WriteLine($"Character can't equip a {armor.ArmorType}");
            }

            Equipment[armor.ItemSlot] = armor;
            return "New armor piece equipped!";
        }

        public override string Equip(Potion potion) {
            Equipment[potion.ItemSlot] = potion;
            return "New potion equipped!";
        }

        public override void SetStartingGear() {
            Equip(WeaponLootTable.RustySword);
            Equip(ArmorLootTable.LinenRags);
            Equip(Potion.HealingPotion);
        }
        public override void PlayerActions(Enemy Monster, Encounters TurnTimer) {
            Console.WriteLine("Choose an action...");
            string input = HUDTools.PlayerPrompt().ToLower();
            if (input == "a" || input == "attack") {
                //Attack
                int damage = Attack(Monster);
                Monster.health -= damage;
                HUDTools.WriteCombatLog("attack", TurnTimer, 0, damage, Monster);
                TurnTimer.turnTimer++;
            }
            else if (input == "d" || input == "defend") {
                //Defend 
                Defend(Monster);
                HUDTools.WriteCombatLog("defend", TurnTimer, 0, 0, Monster);
                TurnTimer.turnTimer++;
            }
            else if (input == "r" || input == "run") {
                //Run                   
                if (RunAway(Monster)) {
                    AudioManager.soundKamp.Stop();
                    AudioManager.soundBossKamp.Stop();
                    HUDTools.ClearCombatLog();
                    TurnTimer.ran = true;
                }
                else {
                    HUDTools.WriteCombatLog(action: "run", TurnTimer: TurnTimer, Monster: Monster);
                    TurnTimer.turnTimer++;
                }
            }
            else if (input == "h" || input == "heal") {
                //Heal
                Heal();
                HUDTools.WriteCombatLog(action: "heal", TurnTimer: TurnTimer, Monster: Monster);
                TurnTimer.turnTimer++;
            }
            else if (input == "c" || input == "character" || input == "character screen") {
                HUDTools.CharacterScreen();
            }
            else if (input == "l" || input == "log" || input == "combat log") {
                Console.Clear();
                HUDTools.GetCombatLog();
            }
            Console.ReadKey(true);
        }
        public override void Heal() {
            if (((Potion)Program.CurrentPlayer.Equipment[Slot.SLOT_POTION]).PotionQuantity == 0) {
                HUDTools.Print("No potions left!", 20);
            }
            else {
                HUDTools.Print("You use a potion", 20);
                Program.CurrentPlayer.Health += ((Potion)Program.CurrentPlayer.Equipment[Slot.SLOT_POTION]).PotionPotency;
                ((Potion)Program.CurrentPlayer.Equipment[Slot.SLOT_POTION]).PotionQuantity--;
                if (Program.CurrentPlayer.Health > Program.CurrentPlayer.BaseSecondaryAttributes.MaxHealth) {
                    Program.CurrentPlayer.Health = Program.CurrentPlayer.BaseSecondaryAttributes.MaxHealth;
                }
                if (Program.CurrentPlayer.Health == Program.CurrentPlayer.BaseSecondaryAttributes.MaxHealth) {
                    HUDTools.Print("You heal to max health!", 20);
                }
                else {
                    HUDTools.Print($"You gain {((Potion)Program.CurrentPlayer.Equipment[Slot.SLOT_POTION]).PotionPotency} health", 20);
                }
            }
        }

        public static int Attack(Enemy Monster) {
            HUDTools.Print($"You swing your {Program.CurrentPlayer.Equipment[Slot.SLOT_WEAPON].ItemName}", 15);
            int attack = Program.rand.Next(Program.CurrentPlayer.CalculateDPT().Item1, Program.CurrentPlayer.CalculateDPT().Item2 + 1);
            HUDTools.Print($"You deal {attack} damage to {Monster.name}", 10);
            return attack;
        }
        public static void Defend(Enemy Monster) {
            HUDTools.Print($"You defend the next two attacks from {Monster.name}", 20);
            Monster.attackDebuff += 2;
        }
        public static bool RunAway(Enemy Monster) {
            bool escaped = false;
            if (Program.rand.Next(0, 2) == 0 || Monster.name == "Human captor") {
                HUDTools.Print($"You try to run from the {Monster.name}, but it knocks you down. You are unable to escape this turn", 15);
            }
            else {
                    HUDTools.Print($"You barely manage to shake off the {Monster.name} and you successfully escape.", 20);
                escaped = true;
            }
            return escaped;
        }
    }
}