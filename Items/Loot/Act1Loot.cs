using Saga.assets;
using Saga.Dungeon;
using System;

namespace Saga.Items.Loot
{
    public class Act1Loot : Loot
    {
        public override int GetGold() {
            int upper = (26 * Program.CurrentPlayer.Level + 61);
            int lower = (5 * Program.CurrentPlayer.Level);
            return Program.rand.Next(lower, upper + 1);
        }
        public override void GetCombatLoot(Enemy monster, string message) {                      
            HUDTools.Print(message, 15);
            int g = (int)Math.Floor(GetGold() * monster.GoldModifier);
            if (g > 0) {
                Console.ForegroundColor = ConsoleColor.DarkYellow;
                HUDTools.Print($"You loot {g} gold coins.", 15);
                Program.CurrentPlayer.Gold += g;
            }
            int[] numbers = new[] { 0, 0, 0, 0, 0, 0, 0, 1, 1, 1, 2 };
            var pot = Program.rand.Next(0, numbers.Length);
            if (numbers[pot] != 0 && monster.EnemyTribe != Tribe.Undead && monster.EnemyTribe != Tribe.Beast) {
                Console.ForegroundColor = ConsoleColor.DarkGray;
                HUDTools.Print($"You loot {numbers[pot]} healing potions", 20);
                Program.CurrentPlayer.CurrentHealingPotion.PotionQuantity += numbers[pot];
            }
            GetQuestLoot(monster);
            monster.GetExp();
            Console.ResetColor();
            HUDTools.PlayerPrompt();
        }
        public override void GetTreasureChestLoot() {
            HUDTools.Print("You find Treasure!",10);
            int g = GetGold() * 3;
            if (g > 0) {
                Console.ForegroundColor = ConsoleColor.DarkYellow;
                HUDTools.Print($"You loot {g} gold coins.", 15);
                Program.CurrentPlayer.Gold += g;
            }
            int[] numbers = new[] { 0, 0, 0, 0, 0, 0, 0, 1, 1, 1, 2 };
            var pot = Program.rand.Next(0, numbers.Length);
            if (numbers[pot] != 0) {
                Console.ForegroundColor = ConsoleColor.DarkGray;
                HUDTools.Print($"You loot {numbers[pot]} healing potions", 20);
                Program.CurrentPlayer.CurrentHealingPotion.PotionQuantity += numbers[pot];
            }
            Console.ForegroundColor = ConsoleColor.DarkMagenta;
            int getTreasure = Program.rand.Next(1, 100 + 1);
            if (getTreasure <= 20) {
                if (Program.CurrentPlayer.CurrentClass == "Mage") {
                    switch (Program.CurrentPlayer.Level) {
                        case int n when 1 <= n && n < 4:
                            int index = Array.FindIndex(Program.CurrentPlayer.Inventory, i => i == null || Program.CurrentPlayer.Inventory.Length == 0);
                            Program.CurrentPlayer.Inventory.SetValue(ArmorLootTable.RunedSimpleRobe, index);
                            HUDTools.Print("You loot a Runed Simple Robe");
                            break;
                        case int n when 4 <= n && n < 7:
                            index = Array.FindIndex(Program.CurrentPlayer.Inventory, i => i == null || Program.CurrentPlayer.Inventory.Length == 0);
                            Program.CurrentPlayer.Inventory.SetValue(ArmorLootTable.EnchantedElegantRobe, index);
                            HUDTools.Print("You loot an Enchanted Elegant Robe");
                            break;
                        case int n when 7 <= n:
                            index = Array.FindIndex(Program.CurrentPlayer.Inventory, i => i == null || Program.CurrentPlayer.Inventory.Length == 0);
                            Program.CurrentPlayer.Inventory.SetValue(ArmorLootTable.ArcanistsRobe, index);
                            HUDTools.Print("You loot an Arcanist's Robe");
                            break;
                        default:
                            break;
                    }
                } else if (Program.CurrentPlayer.CurrentClass == "Archer") {
                    switch (Program.CurrentPlayer.Level) {
                        case int n when 1 <= n && n < 4:
                            int index = Array.FindIndex(Program.CurrentPlayer.Inventory, i => i == null || Program.CurrentPlayer.Inventory.Length == 0);
                            Program.CurrentPlayer.Inventory.SetValue(ArmorLootTable.HideArmor, index);
                            HUDTools.Print("You loot a Hide Armor");
                            break;
                        case int n when 4 <= n && n < 7:
                            index = Array.FindIndex(Program.CurrentPlayer.Inventory, i => i == null || Program.CurrentPlayer.Inventory.Length == 0);
                            Program.CurrentPlayer.Inventory.SetValue(ArmorLootTable.HuntersCuirass, index);
                            HUDTools.Print("You loot a Hunter's Cuirass");
                            break;
                        case int n when 7 <= n:
                            index = Array.FindIndex(Program.CurrentPlayer.Inventory, i => i == null || Program.CurrentPlayer.Inventory.Length == 0);
                            Program.CurrentPlayer.Inventory.SetValue(ArmorLootTable.MarksmansBrigadine, index);
                            HUDTools.Print("You loot a Marksman's Brigandine");
                            break;
                        default:
                            break;
                    }
                } else if (Program.CurrentPlayer.CurrentClass == "Warrior") {
                    switch (Program.CurrentPlayer.Level) {
                        case int n when 1 <= n && n < 4:
                            int index = Array.FindIndex(Program.CurrentPlayer.Inventory, i => i == null || Program.CurrentPlayer.Inventory.Length == 0);
                            Program.CurrentPlayer.Inventory.SetValue(ArmorLootTable.SteelMailShirt, index);
                            HUDTools.Print("You loot a Steel Mail Shirt");
                            break;
                        case int n when 4 <= n && n < 7:
                            index = Array.FindIndex(Program.CurrentPlayer.Inventory, i => i == null || Program.CurrentPlayer.Inventory.Length == 0);
                            Program.CurrentPlayer.Inventory.SetValue(ArmorLootTable.SteelBreastplate, index);
                            HUDTools.Print("You loot a Steel Breast Plate");
                            break;
                        case int n when 7 <= n:
                            index = Array.FindIndex(Program.CurrentPlayer.Inventory, i => i == null || Program.CurrentPlayer.Inventory.Length == 0);
                            Program.CurrentPlayer.Inventory.SetValue(ArmorLootTable.KnightsPlateArmor, index);
                            HUDTools.Print("You loot a Knight's Plate Armor");
                            break;
                        default:
                            break;
                    }
                }
            } else if (20 < getTreasure && getTreasure <= 40) {
                if (Program.CurrentPlayer.CurrentClass == "Mage") {
                    switch (Program.CurrentPlayer.Level) {
                        case int n when 1 <= n && n < 4:
                            int index = Array.FindIndex(Program.CurrentPlayer.Inventory, i => i == null || Program.CurrentPlayer.Inventory.Length == 0);
                            Program.CurrentPlayer.Inventory.SetValue(WeaponLootTable.EnchantedWand, index);
                            HUDTools.Print("You loot an Enchanted Wand");
                            break;
                        case int n when 4 <= n && n < 7:
                            index = Array.FindIndex(Program.CurrentPlayer.Inventory, i => i == null || Program.CurrentPlayer.Inventory.Length == 0);
                            Program.CurrentPlayer.Inventory.SetValue(WeaponLootTable.GnarledStaff, index);
                            HUDTools.Print("You loot a Gnarled Staff");
                            break;
                        case int n when 7 <= n:
                            index = Array.FindIndex(Program.CurrentPlayer.Inventory, i => i == null || Program.CurrentPlayer.Inventory.Length == 0);
                            Program.CurrentPlayer.Inventory.SetValue(WeaponLootTable.ArcanistsStaff, index);
                            HUDTools.Print("You loot an Arcanist's Staff");
                            break;
                        default:
                            break;
                    }
                } else if (Program.CurrentPlayer.CurrentClass == "Archer") {
                    switch (Program.CurrentPlayer.Level) {
                        case int n when 1 <= n && n < 4:
                            int index = Array.FindIndex(Program.CurrentPlayer.Inventory, i => i == null || Program.CurrentPlayer.Inventory.Length == 0);
                            Program.CurrentPlayer.Inventory.SetValue(WeaponLootTable.QuickShortBow, index);
                            HUDTools.Print("You loot a Quick Short Bow");
                            break;
                        case int n when 4 <= n && n < 7:
                            index = Array.FindIndex(Program.CurrentPlayer.Inventory, i => i == null || Program.CurrentPlayer.Inventory.Length == 0);
                            Program.CurrentPlayer.Inventory.SetValue(WeaponLootTable.SturdyLongBow, index);
                            HUDTools.Print("You loot a Sturdy Long Bow");
                            break;
                        case int n when 7 <= n:
                            index = Array.FindIndex(Program.CurrentPlayer.Inventory, i => i == null || Program.CurrentPlayer.Inventory.Length == 0);
                            Program.CurrentPlayer.Inventory.SetValue(WeaponLootTable.MarksmansRecurve, index);
                            HUDTools.Print("You loot a Marksman's Recurve");
                            break;
                        default:
                            break;
                    }
                } else if (Program.CurrentPlayer.CurrentClass == "Warrior") {
                    switch (Program.CurrentPlayer.Level) {
                        case int n when 1 <= n && n < 4:
                            int index = Array.FindIndex(Program.CurrentPlayer.Inventory, i => i == null || Program.CurrentPlayer.Inventory.Length == 0);
                            Program.CurrentPlayer.Inventory.SetValue(WeaponLootTable.SteelSword, index);
                            HUDTools.Print("You loot a Steel Sword");
                            break;
                        case int n when 4 <= n && n < 7:
                            index = Array.FindIndex(Program.CurrentPlayer.Inventory, i => i == null || Program.CurrentPlayer.Inventory.Length == 0);
                            Program.CurrentPlayer.Inventory.SetValue(WeaponLootTable.FineLongSword, index);
                            HUDTools.Print("You loot a Longsword");
                            break;
                        case int n when 7 <= n:
                            index = Array.FindIndex(Program.CurrentPlayer.Inventory, i => i == null || Program.CurrentPlayer.Inventory.Length == 0);
                            Program.CurrentPlayer.Inventory.SetValue(WeaponLootTable.KnightsGreatsword, index);
                            HUDTools.Print("You loot a Knight's Greatsword");
                            break;
                        default:
                            break;
                    }
                }
            }
            Console.ResetColor();
            HUDTools.PlayerPrompt();
        }
        public override void GetExp(int expModifier, int flatExp = 0) {
            int upper = (20 * Program.CurrentPlayer.Level + 21);
            int lower = (2 * Program.CurrentPlayer.Level);
            int x = Program.rand.Next(lower, upper + 1)*expModifier + flatExp;
            if (x > 0) {
                Console.ForegroundColor = ConsoleColor.DarkGreen;
                HUDTools.Print($"You've gained {x} experience points!", 10);
                Program.CurrentPlayer.Exp += x;
            }
            Program.CurrentPlayer.CheckForLevelUp();
        }
        public override void GetQuestLoot(int findgold, int findpotions, string questname, Enemy enemy=null) {
            int g = GetGold() * findgold;
            if (g > 0) {
                Console.ForegroundColor = ConsoleColor.DarkYellow;
                HUDTools.Print($"You loot {g} gold coins.", 15);
                Program.CurrentPlayer.Gold += g;
            }
            int[] numbers = new[] { 0, 0, 0, 0, 0, 0, 0, 1, 1, 1, 2 };
            var pot = Program.rand.Next(0, numbers.Length);
            if (numbers[pot] != 0 && findpotions !=0) {
                Console.ForegroundColor = ConsoleColor.DarkGray;
                HUDTools.Print($"You loot {numbers[pot]} healing potions", 20);
                Program.CurrentPlayer.CurrentHealingPotion.PotionQuantity += numbers[pot];
            }

            for (int i = 0; i < ;i++) {

            }

        }
    }
}
