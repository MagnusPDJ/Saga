using Saga.Assets;
using Saga.Dungeon.Enemies;
using Saga.Dungeon.Enemies.Interfaces;
using Saga.Dungeon.Quests;
using System.Text.Json;

namespace Saga.Items.Loot
{
    public static class LootSystem
    {
        public static List<IItem> RollLoot(LootTable table) {
            var drops = new List<IItem>();
            foreach (var loot in table.Items) {
                if (Program.Rand.NextDouble() <= loot.DropChance) {
                    var item = ItemDatabase.GetByItemId(loot.ItemId);
                    if (item != null) drops.Add(item);
                }
            }
            return drops;
        }

        // OLD methods

        //Metode til at få loot efter successfuld kamp:
        public static void GetCombatLoot(EnemyBase monster, string message) {
            HUDTools.Print(message, 15);
            GetGold(monster.GoldModifier);
            if (monster.Name == "Human captor") {
                GetPotions(Program.Rand.Next(5, 8));
            } else if (monster is IHuman || monster is IGreenskin || monster.Name == "Mimic") {
                GetPotions();
            }
            List<IItem> drops = RollLoot(monster.LootTable);
            foreach (var item in drops) {
                Quest? found;
                if (item.ItemName == "Rat tail"){
                    if ((found = Program.CurrentPlayer.QuestLog.Find(x => x.Name == "Collect rat tails" && x.Completed is not true)) is not null) {
                        int index = Array.FindIndex(Program.CurrentPlayer.Inventory, i => i is not null && i.ItemName == "Rat tail");
                        if (index != -1) {
                            ((IQuestItem)Program.CurrentPlayer.Inventory[index]).Amount++;
                        } else {
                            index = Array.FindIndex(Program.CurrentPlayer.Inventory, i => i == null || Program.CurrentPlayer.Inventory.Length == 0);
                            Program.CurrentPlayer.Inventory.SetValue(item, index);
                        }
                    } else {
                        continue;
                    }
                } else if (item.ItemName == "Bat wings"){
                    if ((found = Program.CurrentPlayer.QuestLog.Find(x => x.Name == "Collect bat wings" && x.Completed is not true)) is not null) {
                        int index = Array.FindIndex(Program.CurrentPlayer.Inventory, x => x is not null && x.ItemName == "Bat wings");
                        if (index != -1) {
                            ((IQuestItem)Program.CurrentPlayer.Inventory[index]).Amount++;
                        } else {
                            index = Array.FindIndex(Program.CurrentPlayer.Inventory, i => i == null || Program.CurrentPlayer.Inventory.Length == 0);
                            Program.CurrentPlayer.Inventory.SetValue(item, index);
                        }
                    } else {
                        continue;
                    }
                } else {
                    int index = Array.FindIndex(Program.CurrentPlayer.Inventory, i => i == null || Program.CurrentPlayer.Inventory.Length == 0);
                    Program.CurrentPlayer.Inventory.SetValue(item, index);                  
                }
                HUDTools.Print($"You gain {item?.ItemName}", 15);
            }
            GetExp(monster.Power, monster.ExpGain);
            TextInput.PressToContinue();
        }
        //Metode til at få loot fra en skattekiste:
        public static void GetTreasureChestLoot() {
            List<IWeapon> weapons = JsonSerializer.Deserialize<List<IWeapon>>(HUDTools.ReadAllResourceText("Saga.Items.Loot.WeaponDatabase.json"), Program.Options) ?? [];
            List<IArmor> armors = JsonSerializer.Deserialize<List<IArmor>>(HUDTools.ReadAllResourceText("Saga.Items.Loot.ArmorDatabase.json"), Program.Options) ?? [];
            HUDTools.Print("You find Treasure!", 10);
            GetGold(3);
            GetPotions();
            Console.ForegroundColor = ConsoleColor.DarkMagenta;
            int getTreasure = Program.Rand.Next(1, 100 + 1);
            if (getTreasure <= 20) {
                if (Program.CurrentPlayer.CurrentClass == "Mage") {
                    switch (Program.CurrentPlayer.Level) {
                        case int n when 1 <= n && n < 4:
                            int index = Array.FindIndex(Program.CurrentPlayer.Inventory, i => i == null || Program.CurrentPlayer.Inventory.Length == 0);
                            var armor = armors.Find(x => x != null && x.ItemName == "Runed Simple Robe");
                            Program.CurrentPlayer.Inventory.SetValue(armor, index);
                            HUDTools.Print("You loot a Runed Simple Robe");
                            break;
                        case int n when 4 <= n && n < 7:
                            index = Array.FindIndex(Program.CurrentPlayer.Inventory, i => i == null || Program.CurrentPlayer.Inventory.Length == 0);
                            armor = armors.Find(x => x != null && x.ItemName == "Enchanted Elegant Robe");
                            Program.CurrentPlayer.Inventory.SetValue(armor, index);
                            HUDTools.Print("You loot an Enchanted Elegant Robe");
                            break;
                        case int n when 7 <= n:
                            index = Array.FindIndex(Program.CurrentPlayer.Inventory, i => i == null || Program.CurrentPlayer.Inventory.Length == 0);
                            armor = armors.Find(x => x != null && x.ItemName == "Arcanist's Robe");
                            Program.CurrentPlayer.Inventory.SetValue(armor, index);
                            HUDTools.Print("You loot an Arcanist's Robe");
                            break;
                        default:
                            break;
                    }
                } else if (Program.CurrentPlayer.CurrentClass == "Archer") {
                    switch (Program.CurrentPlayer.Level) {
                        case int n when 1 <= n && n < 4:
                            int index = Array.FindIndex(Program.CurrentPlayer.Inventory, i => i == null || Program.CurrentPlayer.Inventory.Length == 0);
                            var armor = armors.Find(x => x != null && x.ItemName == "Hide Armor");
                            Program.CurrentPlayer.Inventory.SetValue(armor, index);
                            HUDTools.Print("You loot a Hide Armor");
                            break;
                        case int n when 4 <= n && n < 7:
                            index = Array.FindIndex(Program.CurrentPlayer.Inventory, i => i == null || Program.CurrentPlayer.Inventory.Length == 0);
                            armor = armors.Find(x => x != null && x.ItemName == "Hunter's Cuirass");
                            Program.CurrentPlayer.Inventory.SetValue(armor, index);
                            HUDTools.Print("You loot a Hunter's Cuirass");
                            break;
                        case int n when 7 <= n:
                            index = Array.FindIndex(Program.CurrentPlayer.Inventory, i => i == null || Program.CurrentPlayer.Inventory.Length == 0);
                            armor = armors.Find(x => x != null && x.ItemName == "Marksman's Brigandine");
                            Program.CurrentPlayer.Inventory.SetValue(armor, index);
                            HUDTools.Print("You loot a Marksman's Brigandine");
                            break;
                        default:
                            break;
                    }
                } else if (Program.CurrentPlayer.CurrentClass == "Warrior") {
                    switch (Program.CurrentPlayer.Level) {
                        case int n when 1 <= n && n < 4:
                            int index = Array.FindIndex(Program.CurrentPlayer.Inventory, i => i == null || Program.CurrentPlayer.Inventory.Length == 0);
                            var armor = armors.Find(x => x != null && x.ItemName == "Steel Mail Shirt");
                            Program.CurrentPlayer.Inventory.SetValue(armor, index);
                            HUDTools.Print("You loot a Steel Mail Shirt");
                            break;
                        case int n when 4 <= n && n < 7:
                            index = Array.FindIndex(Program.CurrentPlayer.Inventory, i => i == null || Program.CurrentPlayer.Inventory.Length == 0);
                            armor = armors.Find(x => x != null && x.ItemName == "Steel Breastplate");
                            Program.CurrentPlayer.Inventory.SetValue(armor, index);
                            HUDTools.Print("You loot a Steel Breastplate");
                            break;
                        case int n when 7 <= n:
                            index = Array.FindIndex(Program.CurrentPlayer.Inventory, i => i == null || Program.CurrentPlayer.Inventory.Length == 0);
                            armor = armors.Find(x => x != null && x.ItemName == "Knight's Plate Armor");
                            Program.CurrentPlayer.Inventory.SetValue(armor, index);
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
                            var weapon = weapons.Find(x => x != null && x.ItemName == "Enchanted Wand");
                            Program.CurrentPlayer.Inventory.SetValue(weapon, index);
                            HUDTools.Print("You loot an Enchanted Wand");
                            break;
                        case int n when 4 <= n && n < 7:
                            index = Array.FindIndex(Program.CurrentPlayer.Inventory, i => i == null || Program.CurrentPlayer.Inventory.Length == 0);
                            weapon = weapons.Find(x => x != null && x.ItemName == "Gnarled Staff");
                            Program.CurrentPlayer.Inventory.SetValue(weapon, index);
                            HUDTools.Print("You loot a Gnarled Staff");
                            break;
                        case int n when 7 <= n:
                            index = Array.FindIndex(Program.CurrentPlayer.Inventory, i => i == null || Program.CurrentPlayer.Inventory.Length == 0);
                            weapon = weapons.Find(x => x != null && x.ItemName == "Arcanist's Staff");
                            Program.CurrentPlayer.Inventory.SetValue(weapon, index);
                            HUDTools.Print("You loot an Arcanist's Staff");
                            break;
                        default:
                            break;
                    }
                } else if (Program.CurrentPlayer.CurrentClass == "Archer") {
                    switch (Program.CurrentPlayer.Level) {
                        case int n when 1 <= n && n < 4:
                            int index = Array.FindIndex(Program.CurrentPlayer.Inventory, i => i == null || Program.CurrentPlayer.Inventory.Length == 0);
                            var weapon = weapons.Find(x => x != null && x.ItemName == "Quick Shortbow");
                            Program.CurrentPlayer.Inventory.SetValue(weapon, index);
                            HUDTools.Print("You loot a Quick Shortbow");
                            break;
                        case int n when 4 <= n && n < 7:
                            index = Array.FindIndex(Program.CurrentPlayer.Inventory, i => i == null || Program.CurrentPlayer.Inventory.Length == 0);
                            weapon = weapons.Find(x => x != null && x.ItemName == "Sturdy Longbow");
                            Program.CurrentPlayer.Inventory.SetValue(weapon, index);
                            HUDTools.Print("You loot a Sturdy Longbow");
                            break;
                        case int n when 7 <= n:
                            index = Array.FindIndex(Program.CurrentPlayer.Inventory, i => i == null || Program.CurrentPlayer.Inventory.Length == 0);
                            weapon = weapons.Find(x => x != null && x.ItemName == "Marksman's Recurve");
                            Program.CurrentPlayer.Inventory.SetValue(weapon, index);
                            HUDTools.Print("You loot a Marksman's Recurve");
                            break;
                        default:
                            break;
                    }
                } else if (Program.CurrentPlayer.CurrentClass == "Warrior") {
                    switch (Program.CurrentPlayer.Level) {
                        case int n when 1 <= n && n < 4:
                            int index = Array.FindIndex(Program.CurrentPlayer.Inventory, i => i == null || Program.CurrentPlayer.Inventory.Length == 0);
                            var weapon = weapons.Find(x => x != null && x.ItemName == "Steel Sword");
                            Program.CurrentPlayer.Inventory.SetValue(weapon, index);
                            HUDTools.Print("You loot a Steel Sword");
                            break;
                        case int n when 4 <= n && n < 7:
                            index = Array.FindIndex(Program.CurrentPlayer.Inventory, i => i == null || Program.CurrentPlayer.Inventory.Length == 0);
                            weapon = weapons.Find(x => x != null && x.ItemName == "Fine Longsword");
                            Program.CurrentPlayer.Inventory.SetValue(weapon, index);
                            HUDTools.Print("You loot a Fine Longsword");
                            break;
                        case int n when 7 <= n:
                            index = Array.FindIndex(Program.CurrentPlayer.Inventory, i => i == null || Program.CurrentPlayer.Inventory.Length == 0);
                            weapon = weapons.Find(x => x != null && x.ItemName == "Knight's Greatsword");
                            Program.CurrentPlayer.Inventory.SetValue(weapon, index);
                            HUDTools.Print("You loot a Knight's Greatsword");
                            break;
                        default:
                            break;
                    }
                }
            }
            Console.ResetColor();
            TextInput.PressToContinue();
        }
        //Metode til at få specifikke quest items i encounters:
        public static void GetQuestLoot(int findgold, int findpotions, string questname) {
            GetGold(findgold);
            if (findpotions != 0) {
                GetPotions(findpotions);
            }
            Console.ForegroundColor = ConsoleColor.Cyan;
            if (questname == "MeetFlemsha") {
                int index = Array.FindIndex(Program.CurrentPlayer.Inventory, i => i == null || Program.CurrentPlayer.Inventory.Length == 0);
                var questItem = ItemDatabase.GetByItemId("oldkey");
                Program.CurrentPlayer.Inventory.SetValue(questItem, index);
                HUDTools.Print($"You gain {questItem?.ItemName}", 15);
            }
            Console.ResetColor();
            Program.CurrentPlayer.UpdateQuestLog();
        }
        //Metode til at få en tilfældig mængde guld:
        public static void GetGold(float modifier = 1) {
            int upper = 26 * Program.CurrentPlayer.Level + 61;
            int lower = 5 * Program.CurrentPlayer.Level;
            int g = (int)Math.Floor(Program.Rand.Next(lower, upper + 1) * modifier);
            if (g > 0) {
                HUDTools.Print($"\u001b[33mYou loot {g} gold coins.\u001b[0m", 15);
                Program.CurrentPlayer.Gold += g;
            }
        }
        //Metode til at få en bestemt mængde guld:
        public static void GetFixedGold(int g) {
            if (g > 0) {
                HUDTools.Print($"\u001b[33mYou loot {g} gold coins.\u001b[0m", 15);
                Program.CurrentPlayer.Gold += g;
            }
        }
        //Metode til at få healing potions, default random 0-2:
        public static void GetPotions(int amount = 0) {
            int p;
            if (amount == 0) {
                int[] numbers = [0, 0, 0, 0, 0, 0, 0, 1, 1, 1, 2];
                var picked = Program.Rand.Next(0, numbers.Length);
                p = numbers[picked];
            } else {
                p = amount;
            }
            if (p > 0) {
                var potion = Array.Find(Program.CurrentPlayer.Equipment.Potion, p => p is IItem { ItemName: "Healing Potion" });
                if (potion is not null) {
                    HUDTools.Print($"\u001b[90mYou loot {p} healing potions\u001b[0m", 20);
                    potion.PotionQuantity += p;
                }
            }
        }
        //Metode til at få en tilfældig mængde exp eller en bestemt mængde:
        public static void GetExp(int monsterPower, int monsterExp = 0) {
            int powerDifference = monsterPower - Program.CurrentPlayer.Level;
            int xpGain;
            if (powerDifference >= 0) {
                xpGain = powerDifference switch {
                    0 => monsterExp,
                    1 => (int)(monsterExp * 1.1),
                    2 => (int)(monsterExp * 1.25),
                    3 => (int)(monsterExp * 1.5),
                    4 => (int)(monsterExp * 1.9),
                    5 => (int)(monsterExp * 2.5),
                    _ => (int)(monsterExp * 0.5 * powerDifference),
                };
            } else {
                xpGain = powerDifference switch {
                    -1 => (int)(monsterExp * 0.9),
                    -2 => (int)(monsterExp * 0.75),
                    -3 => (int)(monsterExp * 0.5),
                    -4 => (int)(monsterExp * 0.1),
                    _ => monsterExp * 0,
                };
            }
            if (xpGain > 0) {
                HUDTools.Print($"\u001b[32mYou've gained {xpGain} experience points!\u001b[0m", 10);
                Program.CurrentPlayer.Exp += xpGain;
            }
            Program.CurrentPlayer.CheckForLevelUp();
        }
        public static void GetFixedExp(int expGain) {
            if (expGain > 0) {
                HUDTools.Print($"\u001b[32mYou've gained {expGain} experience points!\u001b[0m", 10);
                Program.CurrentPlayer.Exp += expGain;
            }
            Program.CurrentPlayer.CheckForLevelUp();
        }
    }
}
