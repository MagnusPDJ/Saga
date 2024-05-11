using System;
using System.Reflection;
using Saga.assets;
using Saga.Character;
using Saga.Dungeon;

namespace Saga.Items
{
    public class Loot
    {
        public static int GetGold() {
            int upper = (30 * Program.CurrentPlayer.Level + 101);
            int lower = (10 * Program.CurrentPlayer.Level);
            return Program.rand.Next(lower, upper + 1);
        }

        //Metode til at genere loot
        public static void GetLoot(int xpModifier, int goldModifier, string name, string message) {
            int x = Enemy.GetXP() * xpModifier;
            int[] numbers = new[] { 0, 0, 0, 0, 0, 0, 0, 1, 1, 1, 2 };
            var pot = Program.rand.Next(0, numbers.Length);
            HUDTools.Print(message, 15);
            if (x > 0) {
                Console.ForegroundColor = ConsoleColor.DarkGreen;
                HUDTools.Print($"You've gained {x} experience points!", 10);
                Program.CurrentPlayer.Exp += x;
            }
            int g = GetGold() * goldModifier;
            if (g > 0) {
                Console.ForegroundColor = ConsoleColor.DarkYellow;
                HUDTools.Print($"You loot {g} gold coins.", 15);
                Program.CurrentPlayer.Gold += g;
            }
            if (numbers[pot] != 0 && name != "Trap") {
                Console.ForegroundColor = ConsoleColor.DarkGray;
                HUDTools.Print($"You loot {numbers[pot]} healing potions", 20);
                ((Potion)Program.CurrentPlayer.Equipment[Slot.SLOT_POTION]).PotionQuantity += numbers[pot];
            }
            if (name == "Treasure") {
                Console.ForegroundColor = ConsoleColor.DarkMagenta;
                int getTreasure = Program.rand.Next(1, 100 + 1);
                if (getTreasure <= 20) {
                    if (Program.CurrentPlayer.currentClass == "Mage") {
                        switch (Program.CurrentPlayer.Level) {
                            case int n when 1<=n && n<4:
                                int index = Array.FindIndex(Program.CurrentPlayer.Inventory, i => i == null || Program.CurrentPlayer.Inventory.Length == 0);
                                Program.CurrentPlayer.Inventory.SetValue(ArmorLootTable.RunedSimpleRobe, index);
                                HUDTools.Print("You loot a Runed Simple Robe");
                                break;
                            case int n when 4<=n && n<7:
                                index = Array.FindIndex(Program.CurrentPlayer.Inventory, i => i == null || Program.CurrentPlayer.Inventory.Length == 0);
                                Program.CurrentPlayer.Inventory.SetValue(ArmorLootTable.EnchantedElegantRobe, index);
                                HUDTools.Print("You loot an Enchanted Elegant Robe");
                                break;
                            case int n when 7<=n:
                                index = Array.FindIndex(Program.CurrentPlayer.Inventory, i => i == null || Program.CurrentPlayer.Inventory.Length == 0);
                                Program.CurrentPlayer.Inventory.SetValue(ArmorLootTable.ArcanistsRobe, index);
                                HUDTools.Print("You loot an Arcanist's Robe");
                                break;
                            default:
                                break;
                        }
                    }
                    else if (Program.CurrentPlayer.currentClass == "Archer") {
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
                    }
                    else if (Program.CurrentPlayer.currentClass == "Warrior") {
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
                }
                else if (20 < getTreasure && getTreasure <= 40) {
                    if (Program.CurrentPlayer.currentClass == "Mage") {
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
                    }
                    else if (Program.CurrentPlayer.currentClass == "Archer") {
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
                    }
                    else if (Program.CurrentPlayer.currentClass == "Warrior") {
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
            }
            Console.ResetColor();
            HUDTools.PlayerPrompt();
        }

    }
}
