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
        public static void SetRequirements(this Type type, string target, int amount) {
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
    }
    [Serializable]
    public abstract class Quest {
        public string Name { get; set; }
        public string Description { get; set; }
        public Type QuestType { get; set; }
        public string Giver { get; set; }
        public int Gold { get; set; }
        public int Exp { get; set; }
        public Item Item { get; set; }
        public bool Completed { get; set; } = false;
    }
}
