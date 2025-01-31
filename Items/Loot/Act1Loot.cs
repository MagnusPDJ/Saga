using System;
using Saga.Assets;
using Saga.Dungeon;

namespace Saga.Items.Loot
{
    public class Act1Loot : Loot
    {
        //Metode til at få en tilfældig mængde guld:
        public override void GetGold(float modifier = 1) {
            int upper = (26 * Program.CurrentPlayer.Level + 61);
            int lower = (5 * Program.CurrentPlayer.Level);
            int g = (int)Math.Floor(Program.rand.Next(lower, upper + 1) * modifier);
            if (g > 0) {
                Console.ForegroundColor = ConsoleColor.DarkYellow;
                HUDTools.Print($"You loot {g} gold coins.", 15);
                Console.ResetColor();
                Program.CurrentPlayer.Gold += g;
            }
        }
        //Metode til at få en bestemt mængde guld:
        public override void GetFixedGold(int g) {
            if (g > 0) {
                Console.ForegroundColor = ConsoleColor.DarkYellow;
                HUDTools.Print($"You loot {g} gold coins.", 15);
                Console.ResetColor();
                Program.CurrentPlayer.Gold += g;
            }
        }
        //Metode til at få healing potions, default random 0-2:
        public override void GetPotions(int amount = 0) {
            int p;
            if (amount == 0) {
                int[] numbers = new[] { 0, 0, 0, 0, 0, 0, 0, 1, 1, 1, 2 };
                var picked = Program.rand.Next(0, numbers.Length);
                p = numbers[picked];
            } else { 
                p = amount;
            }
            if (p > 0) {
                Console.ForegroundColor = ConsoleColor.DarkGray;
                HUDTools.Print($"You loot {p} healing potions", 20);
                Console.ResetColor();
                Program.CurrentPlayer.CurrentHealingPotion.PotionQuantity += p;
            }
        }
        //Metode til at få loot efter successfuld kamp:
        public override void GetCombatLoot(Enemy monster, string message) {                      
            HUDTools.Print(message, 15);
            GetGold(monster.GoldModifier);
            if (monster.EnemyTribe != Tribe.Beast) {
                GetPotions();
            }
            GetQuestLoot(0,0,"",monster);
            monster.GetExp();
            HUDTools.PlayerPrompt();
        }
        //Metode til at få loot fra en skattekiste:
        public override void GetTreasureChestLoot() {
            HUDTools.Print("You find Treasure!",10);
            GetGold(3);
            GetPotions();
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
        //Metode til at få en tilfældig mængde exp eller en bestemt mængde:
        public override void GetExp(int expModifier, int flatExp = 0) {
            int upper = (20 * Program.CurrentPlayer.Level + 21);
            int lower = (2 * Program.CurrentPlayer.Level);
            int x = Program.rand.Next(lower, upper + 1)*expModifier + flatExp;
            if (x > 0) {
                Console.ForegroundColor = ConsoleColor.DarkGreen;
                HUDTools.Print($"You've gained {x} experience points!", 10);
                Console.ResetColor();
                Program.CurrentPlayer.Exp += x;
            }
            Program.CurrentPlayer.CheckForLevelUp();
        }
        //Metode til at få specifikke quest items i encounters:
        public override void GetQuestLoot(int findgold, int findpotions, string questname, Enemy enemy=null) {
            GetGold(findgold);
            if (findpotions != 0) {
                GetPotions(findpotions);
            }
            if (questname == "MeetFlemsha") {
                int index = Array.FindIndex(Program.CurrentPlayer.Inventory, i => i == null || Program.CurrentPlayer.Inventory.Length == 0);
                Program.CurrentPlayer.Inventory.SetValue(QuestLootTable.OldKey, index);
                Console.ForegroundColor = ConsoleColor.Cyan;
                HUDTools.Print($"You gain {QuestLootTable.OldKey.ItemName}", 15);
            }
            if (enemy != null) {
                if (enemy.Name == "Giant Rat" && Program.CurrentPlayer.QuestLog.Find(x => x.Name == "Collect rat tails" && x.Completed != true) != null) {
                    int index = Array.FindIndex(Program.CurrentPlayer.Inventory, i => i != null && i.ItemName == "Rat tail");
                    if (index != -1) {
                        ((QuestItem)Program.CurrentPlayer.Inventory[index]).Amount++;
                        Console.ForegroundColor = ConsoleColor.Cyan;
                        HUDTools.Print($"You gain {QuestLootTable.RatTail.ItemName}", 15);
                    }else {
                        index = Array.FindIndex(Program.CurrentPlayer.Inventory, i => i == null || Program.CurrentPlayer.Inventory.Length == 0);
                        Program.CurrentPlayer.Inventory.SetValue(QuestLootTable.RatTail, index);
                        Console.ForegroundColor = ConsoleColor.Cyan;
                        HUDTools.Print($"You gain {QuestLootTable.RatTail.ItemName}", 15);
                    }                   
                } else if (enemy.Name == "Giant Bat" && Program.CurrentPlayer.QuestLog.Find(x => x.Name == "Collect bat wings" && x.Completed != true) != null) {
                    int index = Array.FindIndex(Program.CurrentPlayer.Inventory, x => x != null && x.ItemName == "Bat wings");
                    if (index != -1) {
                        ((QuestItem)Program.CurrentPlayer.Inventory[index]).Amount++;
                        Console.ForegroundColor = ConsoleColor.Cyan;
                        HUDTools.Print($"You gain {QuestLootTable.BatWings.ItemName}", 15);
                    } else {
                        index = Array.FindIndex(Program.CurrentPlayer.Inventory, i => i == null || Program.CurrentPlayer.Inventory.Length == 0);
                        Program.CurrentPlayer.Inventory.SetValue(QuestLootTable.BatWings, index);
                        Console.ForegroundColor = ConsoleColor.Cyan;
                        HUDTools.Print($"You gain {QuestLootTable.BatWings.ItemName}", 15);
                    }
                }
            }
            Console.ResetColor();
            Program.CurrentPlayer.UpdateQuestLog();
        }
    }
}
