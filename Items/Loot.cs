using System;
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
                int getTreasure = Program.rand.Next(0, 100 + 1);
                if (getTreasure <= 10) {
                    if (Program.CurrentPlayer.currentClass == "Mage") {
                        switch (Program.CurrentPlayer.Equipment[Slot.SLOT_BODY].ItemName) {
                            case "Linen Rags":
                                Program.CurrentPlayer.Equip(ArmorLootTable.SimpleRobe);
                                HUDTools.Print("You loot a Simple Robe");
                                break;
                            case "Simple Robe":
                                Program.CurrentPlayer.Equip(ArmorLootTable.ElegantRobe);
                                HUDTools.Print("You loot a Elegant Robe");
                                break;
                            case "Elegant Robe":
                                Program.CurrentPlayer.Equip(ArmorLootTable.ArcanistsRobe);
                                HUDTools.Print("You loot an Arcanist's Robe");
                                break;
                            default:
                                break;
                        }
                    }
                    else if (Program.CurrentPlayer.currentClass == "Archer") {
                        switch (Program.CurrentPlayer.Equipment[Slot.SLOT_BODY].ItemName) {
                            case "Linen Rags":
                                Program.CurrentPlayer.Equip(ArmorLootTable.HideArmor);
                                HUDTools.Print("You loot a Hide Armor");
                                break;
                            case "Hide Armor":
                                Program.CurrentPlayer.Equip(ArmorLootTable.LeatherCuirass);
                                HUDTools.Print("You loot a Leather Cuirass");
                                break;
                            case "Leather Cuirass":
                                Program.CurrentPlayer.Equip(ArmorLootTable.MarksmansBrigadine);
                                HUDTools.Print("You loot a Marksman's Brigandine");
                                break;
                            default:
                                break;
                        }
                    }
                    else if (Program.CurrentPlayer.currentClass == "Warrior") {
                        switch (Program.CurrentPlayer.Equipment[Slot.SLOT_BODY].ItemName) {
                            case "Linen Rags":
                                Program.CurrentPlayer.Equip(ArmorLootTable.MailShirt);
                                HUDTools.Print("You loot a Mail Shirt");
                                break;
                            case "Mail Shirt":
                                Program.CurrentPlayer.Equip(ArmorLootTable.Breastplate);
                                HUDTools.Print("You loot a Breast Plate");
                                break;
                            case "Breast Plate":
                                Program.CurrentPlayer.Equip(ArmorLootTable.KnightsPlateArmor);
                                HUDTools.Print("You loot a Knight's Plate Armor");
                                break;
                            default:
                                break;
                        }
                    }
                }
                else if (10 < getTreasure && getTreasure <= 20) {
                    if (Program.CurrentPlayer.currentClass == "Mage") {
                        switch (Program.CurrentPlayer.Equipment[Slot.SLOT_WEAPON].ItemName) {
                            case "Cracked Wand":
                                Program.CurrentPlayer.Equip(WeaponLootTable.EnchantedWand);
                                HUDTools.Print("You loot an Enchanted Wand");
                                break;
                            case "Enchanted Wand":
                                Program.CurrentPlayer.Equip(WeaponLootTable.GnarledStaff);
                                HUDTools.Print("You loot a Gnarled Staff");
                                break;
                            case "Gnarled Staff":
                                Program.CurrentPlayer.Equip(WeaponLootTable.ArcanistsStaff);
                                HUDTools.Print("You loot an Arcanist's Robe");
                                break;
                            default:
                                break;
                        }
                    }
                    else if (Program.CurrentPlayer.currentClass == "Archer") {
                        switch (Program.CurrentPlayer.Equipment[Slot.SLOT_WEAPON].ItemName) {
                            case "Flimsy Bow":
                                Program.CurrentPlayer.Equip(WeaponLootTable.ShortBow);
                                HUDTools.Print("You loot a Short Bow");
                                break;
                            case "Short Bow":
                                Program.CurrentPlayer.Equip(WeaponLootTable.LongBow);
                                HUDTools.Print("You loot a Long Bow");
                                break;
                            case "Long Bow":
                                Program.CurrentPlayer.Equip(WeaponLootTable.MarksmansRecurve);
                                HUDTools.Print("You loot a Marksman's Recurve");
                                break;
                            default:
                                break;
                        }
                    }
                    else if (Program.CurrentPlayer.currentClass == "Warrior") {
                        switch (Program.CurrentPlayer.Equipment[Slot.SLOT_WEAPON].ItemName) {
                            case "Rusty Sword":
                                Program.CurrentPlayer.Equip(WeaponLootTable.SteelSword);
                                HUDTools.Print("You loot a Steel Sword");
                                break;
                            case "Steel Sword":
                                Program.CurrentPlayer.Equip(WeaponLootTable.LongSword);
                                HUDTools.Print("You loot a Longsword");
                                break;
                            case "Longsword":
                                Program.CurrentPlayer.Equip(WeaponLootTable.KnightsGreatsword);
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
