﻿using System;
using Saga.assets;
using Saga.Dungeon;
using Saga.Items;

namespace Saga.Character
{
    [Serializable]
    public class Mage : Player
    {
        public Mage(string name) : base(name, 0, 1, 1, 1, 1, 1) {
            currentClass = "Mage";
        }

        public override void LevelUp() {
            int levels = 0;
            AudioManager.soundLvlUp.Play();
            while (CanLevelUp()) {
                Program.CurrentPlayer.Exp -= GetLevelUpValue();
                Program.CurrentPlayer.Level++;
                Program.CurrentPlayer.FreeAttributePoints++;
                levels++;
            }
            PrimaryAttributes levelUpValues = new PrimaryAttributes() { Constitution = 1 * levels, Strength = 0 * levels, Dexterity = 0 * levels, Intellect = 1 * levels, WillPower = 1 * levels };

            BasePrimaryAttributes += levelUpValues;

            CalculateTotalStats();
            Program.CurrentPlayer.Health = Program.CurrentPlayer.BaseSecondaryAttributes.MaxHealth;

            Console.ForegroundColor = ConsoleColor.DarkBlue;
            HUDTools.Print($"Congratulations! You are now level {Level}! You've gained 1 attribute point.", 20);
            Console.ResetColor();
            HUDTools.PlayerPrompt();
        }

        public override (int, int) CalculateDPT() {
            TotalPrimaryAttributes = CalculateArmorBonus();
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
            }
            if (weapon.WeaponType != WeaponType.Tome && weapon.WeaponType != WeaponType.Staff && weapon.WeaponType != WeaponType.Wand) {
                Console.WriteLine($"Character can't equip a {weapon.WeaponType}");
            }

            Equipment[weapon.ItemSlot] = weapon;
            Program.CurrentPlayer.CalculateTotalStats();
            return "New weapon equipped!";
        }

        public override string Equip(Armor armor) {
            if (armor.ItemLevel > Level) {
                Console.WriteLine($"Character needs to be level {armor.ItemLevel} to equip this item");
            }
            if (armor.ArmorType != ArmorType.Cloth && armor.ArmorType != ArmorType.Leather) {
                Console.WriteLine($"Character can't equip a {armor.ArmorType}");
            }

            Equipment[armor.ItemSlot] = armor;
            Program.CurrentPlayer.CalculateTotalStats();
            return "New armor piece equipped!";
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
            return "Item unequipped!";
        }

        public override void SetStartingGear() {
            Equip(WeaponLootTable.CrackedWand);
            Equip(ArmorLootTable.LinenRags);
            Equip(Potion.HealingPotion);
        }
        public override void PlayerActions(Enemy Monster, Encounters TurnTimer) {
            Console.WriteLine("Choose an action...");
            string input = HUDTools.PlayerPrompt().ToLower();
            if (input.ToLower() == "a" || input == "attack") {
                //Attack
                int damage = Attack(Monster);
                Monster.health -= damage;
                HUDTools.WriteCombatLog("attack", TurnTimer, 0, damage, Monster);
                TurnTimer.turnTimer++;
            }
            else if (input.ToLower() == "d" || input == "defend") {
                //Defend 
                Defend(Monster);
                HUDTools.WriteCombatLog("defend", TurnTimer, 0, 0, Monster);
                TurnTimer.turnTimer++;
            }
            else if (input.ToLower() == "r" || input == "run") {
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
            else if (input.ToLower() == "h" || input == "heal") {
                //Heal
                Heal();
                HUDTools.WriteCombatLog(action: "heal", TurnTimer: TurnTimer, Monster: Monster);
                TurnTimer.turnTimer++;
            }
            else if (input.ToLower() == "c" || input == "character" || input == "character screen") {
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
                HUDTools.Print("You use a potion amplified by your magic", 20);
                Program.CurrentPlayer.Health += ((Potion)Program.CurrentPlayer.Equipment[Slot.SLOT_POTION]).PotionPotency + 1 + Program.CurrentPlayer.Level * 2;
                ((Potion)Program.CurrentPlayer.Equipment[Slot.SLOT_POTION]).PotionQuantity--;
                if (Program.CurrentPlayer.Health > Program.CurrentPlayer.BaseSecondaryAttributes.MaxHealth) {
                    Program.CurrentPlayer.Health = Program.CurrentPlayer.BaseSecondaryAttributes.MaxHealth;
                }
                if (Program.CurrentPlayer.Health == Program.CurrentPlayer.BaseSecondaryAttributes.MaxHealth) {
                    HUDTools.Print("You heal to max health!", 20);
                }
                else {
                    HUDTools.Print($"You gain {((Potion)Program.CurrentPlayer.Equipment[Slot.SLOT_POTION]).PotionPotency + 1 + Program.CurrentPlayer.Level * 2} health", 20);
                }
            }
        }

        public static int Attack(Enemy Monster) {
            HUDTools.Print($"You shoot an arcane missile from your {Program.CurrentPlayer.Equipment[Slot.Weapon].ItemName}", 15);
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
            if (Monster.name == "Human captor") {
                HUDTools.Print($"You try to run from the {Monster.name}, but it knocks you down. You are unable to escape this turn", 15);
            }
            else {
                HUDTools.Print($"You use your crazy ninja moves to evade the {Monster.name} and you successfully escape!", 20);
                escaped = true;
            }
            return escaped;
        }
    }
}
