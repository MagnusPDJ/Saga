using Saga.Assets;
using System.Text.Json;
using Saga.Dungeon.Quests;
using Saga.Dungeon.Monsters;
using Saga.Items;

namespace Saga
{
    public static class Act1Loot
    {
        //Metode til at få loot efter successfuld kamp:
        public override void GetCombatLoot(Enemy monster, string message) {                      
            HUDTools.Print(message, 15);
            GetGold(monster.GoldModifier);
            if (monster.Name == "Human Captor") {
                GetPotions(Program.Rand.Next(5, 8));
            } else if (monster.EnemyTribe != Tribe.Beast) {
                GetPotions();
            }
            GetQuestLoot(0,0,"",monster);
            monster.GetExp();
            TextInput.PressToContinue();
        }
        //Metode til at få loot fra en skattekiste:
        public override void GetTreasureChestLoot() {
            List<IWeapon> weapons = JsonSerializer.Deserialize<List<IWeapon>>(HUDTools.ReadAllResourceText("Saga.Items.Loot.WeaponLootTable.json"), Program.Options) ?? [];
            List<IArmor> armors = JsonSerializer.Deserialize<List<IArmor>>(HUDTools.ReadAllResourceText("Saga.Items.Loot.ArmorLootTable.json"), Program.Options) ?? [];
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
        public override void GetQuestLoot(int findgold, int findpotions, string questname, Enemy? enemy=null) {
            List<IQuestItem> questItems = JsonSerializer.Deserialize<List<IQuestItem>>(HUDTools.ReadAllResourceText("Saga.Items.Loot.QuestLootTable.json"), Program.Options) ?? [];
            GetGold(findgold);
            if (findpotions != 0) {
                GetPotions(findpotions);
            }
            Console.ForegroundColor = ConsoleColor.Cyan;
            if (questname == "MeetFlemsha") {
                int index = Array.FindIndex(Program.CurrentPlayer.Inventory, i => i == null || Program.CurrentPlayer.Inventory.Length == 0);
                var questItem = questItems.Find(x => x != null && x.ItemName == "Old Key");
                Program.CurrentPlayer.Inventory.SetValue(questItem, index);
                HUDTools.Print($"You gain {questItem?.ItemName}", 15);
            }
            if (enemy != null) {
                Quest? found;
                if (enemy.Name == "Giant Rat" && (found = Program.CurrentPlayer.QuestLog.Find(x => x.Name == "Collect rat tails" && x.Completed != true)) != null) {
                    int index = Array.FindIndex(Program.CurrentPlayer.Inventory, i => i != null && i.ItemName == "Rat Tail");
                    var questItem = questItems.Find(x => x != null && x.ItemName == "Rat Tail");
                    if (index != -1) {
                        ((IQuestItem)Program.CurrentPlayer.Inventory[index]).Amount++;
                        HUDTools.Print($"You gain {questItem?.ItemName}", 15);
                    }else {
                        index = Array.FindIndex(Program.CurrentPlayer.Inventory, i => i == null || Program.CurrentPlayer.Inventory.Length == 0);
                        Program.CurrentPlayer.Inventory.SetValue(questItem, index);
                        HUDTools.Print($"You gain {questItem?.ItemName}", 15);
                    }                   
                } else if (enemy.Name == "Giant Bat" && 
                    (found = Program.CurrentPlayer.QuestLog.Find(x => x is not null && x.Name == "Collect bat wings" && x.Completed != true)) is not null) {
                    int index = Array.FindIndex(Program.CurrentPlayer.Inventory, x => x != null && x.ItemName == "Bat Wings");
                    var questItem = questItems.Find(x => x != null && x.ItemName == "Bat Wings");
                    if (index != -1) {
                        ((IQuestItem)Program.CurrentPlayer.Inventory[index]).Amount++;
                        HUDTools.Print($"You gain {questItem?.ItemName}", 15);
                    } else {
                        index = Array.FindIndex(Program.CurrentPlayer.Inventory, i => i == null || Program.CurrentPlayer.Inventory.Length == 0);
                        Program.CurrentPlayer.Inventory.SetValue(questItem, index);
                        HUDTools.Print($"You gain {questItem?.ItemName}", 15);
                    }
                }
                if ((found = Program.CurrentPlayer.QuestLog.Find(x => x.QuestType == Dungeon.Quests.Type.Elimination && x.Target == "Enemy" && x.Completed != true)) != null) {
                    found.Amount++;
                }
            }
            Console.ResetColor();
            Program.CurrentPlayer.UpdateQuestLog();
        }
    }
}
