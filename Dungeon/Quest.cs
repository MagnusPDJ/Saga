using System;
using System.Collections.Generic;
using Saga.Items;

namespace Saga.Dungeon
{
    public enum Type {
        Elimination,
        Collect,
        Find,
        KillTarget,
        Story,
    }
    public static class TypeExtensions {
        public static Dictionary<string, int> Requirements { get; set; }
        public static void SetRequirements(this Type type, string target, int amount=1) {
            switch (type) { 
                case Type.Elimination:
                    Requirements.Add(target, amount);
                    break;
                case Type.Collect:
                    Requirements.Add(target, amount);
                    break;
                case Type.Find:
                    Requirements.Add(target, 1);
                    break;
                case Type.KillTarget:
                    Requirements.Add(target, 1);
                    break;
                case Type.Story:
                    Requirements.Add(target, 1);
                    break;
            }
        }
        public static bool CheckRequirements(this Type type) {
            foreach (Item item in Program.CurrentPlayer.Inventory) {
                if (item == null) {
                    continue;
                }
                foreach (string target in Requirements.Keys) {
                    if (item.ItemSlot == Slot.Quest && item.ItemName == target) {
                        if (((QuestItem)item).Amount == Requirements[target]) {
                            return true;
                        }
                    }
                }              
            }
            return false;
        }
    }
    [Serializable]
    public abstract class Quest {
        public string Name { get; set; }
        public string Objective { get; set; }
        public string TurnIn { get; set; }
        public Type QuestType { get; set; }
        public string Giver { get; set; }
        public int Gold { get; set; }
        public int Exp { get; set; }
        public int Potions { get; set; } = -1;
        public Item Item { get; set; }
        public bool Accepted { get; set; } = false;
        public bool Completed { get; set; } = false;
    }
}
