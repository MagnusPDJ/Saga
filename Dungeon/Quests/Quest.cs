using Saga.Assets;
using Saga.Character;
using Saga.Items;
using Saga.Items.Loot;
using System.Text.Json.Serialization;

namespace Saga.Dungeon.Quests
{
    public enum Act
    {
        Start,
        Act1,
        Act2,
        Act3,
        Act4,
        Act5
    }

    public enum Type {
        Elimination,
        Collect,
        Find,
        KillTarget,
        Story,
    }

    [JsonDerivedType(typeof(Act1Quest), typeDiscriminator: "act1Quest")]
    public abstract class Quest {
        public string Name { get; set; } = string.Empty;
        public string Objective { get; set; } = string.Empty;
        public string TurnIn { get; set; } = string.Empty;
        public Type QuestType { get; set; }
        public string Giver { get; set; } = string.Empty;
        public int Gold { get; set; }
        public int Exp { get; set; }
        public List<(PotionType, int)>? Potions { get; set; }
        public IItem? Item { get; set; }
        public bool Accepted { get; set; } = false;
        public bool Completed { get; set; } = false;
        public string Target { get; set; } = string.Empty;
        public int Amount { get; set; } = 0;
        public Dictionary<string, int> Requirements { get; set; } = [];
        public bool CheckRequirements() {
            if (QuestType == Type.Collect || QuestType == Type.Find) {
                foreach (IItem item in Program.CurrentPlayer.Inventory) {
                    if (item == null) {
                        continue;
                    }                   
                    foreach (string target in Requirements.Keys) {
                        if (item.ItemId != target) {
                            continue;
                        } 
                        if (item is ICraftingItem item1) {
                            if (item1.Amount >= Requirements[target]) {
                                return true;
                            }
                        } else if (item is IQuestItem qItem) {
                            if (qItem.Amount >= Requirements[target]) {
                                return true;
                            }
                            if (Requirements[target] <= 1) {
                                return true;
                            }
                        } else {
                            return true;
                        }
                    }
                }
            } else if (QuestType == Type.Elimination) {
                foreach (string target in Requirements.Keys) {
                    if (Amount == Requirements[target]) {
                        return true;
                    }
                }
            }
                return false;
        }
        //Metode til at opdatere questloggen hver gang ny quest eller item bliver added til spilleren.
        public static void UpdateQuestLog(Player player) {
            foreach (Quest quest in player.QuestLog) {
                quest.Completed = quest.CheckRequirements();
            }
        }
        //Metode til at få alle quest rewards og opdatere questlogs.
        public static void TurnInQuest(Player player, Quest quest) {
            if (quest.QuestType == Type.Collect || quest.QuestType == Type.Find) {
                int qItem = Array.FindIndex(player.Inventory, item => item != null && item.ItemId == quest.Target);
                if (qItem != -1) {
                    player.Inventory.SetValue(null, qItem);
                }
            }
            player.QuestLog.Remove(quest);
            player.CompletedQuests.Add(quest);
            Program.SoundController.Play("win");
            HUDTools.Print($"\u001b[96m You've completed the quest: {quest.Name}!\u001b[0m", 15);
            LootSystem.GetFixedGold(quest.Gold);
            if (quest.Potions is not null) LootSystem.GetPotionsByType(quest.Potions);
            LootSystem.GetFixedExp(quest.Exp);
            if (quest.Item != null) {
                int index = Array.FindIndex(player.Inventory, i => i == null || player.Inventory.Length == 0);
                player.Inventory.SetValue(quest.Item, index);
                HUDTools.Print($"\u001b[35m You've gained {quest.Item.ItemName}\u001b[0m");
            }
        }

    }
}
