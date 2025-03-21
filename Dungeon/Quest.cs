﻿using System.Collections.Generic;
using System.Text.Json.Serialization;
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

    [JsonDerivedType(typeof(Act1Quest), typeDiscriminator: "act1Quest")]
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
        public string Target { get; set; }
        public int Amount { get; set; } = 1;
        public Dictionary<string, int> Requirements { get; set; }

        public bool CheckRequirements() {
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
}
