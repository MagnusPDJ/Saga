using Saga.Items;
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
        public int Potions { get; set; } = -1;
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
                        if (item is ICraftingItem item1 && item.ItemId == target) {
                            if (item1.Amount == Requirements[target]) {
                                return true;
                            }
                        } else if (item is IQuestItem qItem && qItem.ItemId == target) {
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

    }
}
