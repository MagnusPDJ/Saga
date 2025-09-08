using Saga.Character.DmgLogic;
using Saga.Items.Loot;

namespace Saga.Dungeon.Monsters
{
    public class Enemy
    {
        public string Name { get; set; } = string.Empty;
        public string PlayerKillDescription { get; set; } = string.Empty;
        public int MaxHealth { get; set; }
        public int Health { get; set; }
        public int Attack { get; set; }        
        public int Initiative { get; set; }
        public int Armor { get; set; }
        public Dictionary<PhysicalType, int> PhysicalResistance { get; set; } = new() {
            { PhysicalType.Normal, 0 },
            { PhysicalType.Piercing, 0 },
            { PhysicalType.Crushing, 0 }
        };
        public Dictionary<ElementalType, int> ElementalResistance { get; set; } = new() {
            { ElementalType.Frost, 0 },
            { ElementalType.Fire, 0 },
            { ElementalType.Poison, 0 },
            { ElementalType.Lightning, 0 }
        };
        public Dictionary<MagicalType, int> MagicalResistance { get; set; } = new() {
            { MagicalType.Arcane, 0 },
            { MagicalType.Chaos, 0 },
            { MagicalType.Void, 0 },
            { MagicalType.Nature, 0 },
            { MagicalType.Life, 0 },
            { MagicalType.Death, 0 }
        };
        public List<string> Tags { get; set; } = [];
        public LootTable LootTable { get; set; } = new();
        public MonsterScaling? Scaling { get; set; }
        public int SpawnWeight { get; set; } = 1; // optional weighted spawning
        public float ExpGain { get; set; }
        public float GoldModifier { get; set; }

        public virtual void TakeDamage((IDamageType, int) amount) {
            Health -= amount.Item2;
            if (Health < 0) Health = 0;
        }
        public virtual void Heal(int amount) {
            Health += amount;
            if (Health > MaxHealth) Health = MaxHealth;
        }
    }
}

// Reference for old enemies.

////Monster type låst efter level
//public static (string, Tribe) GetName() {
//    if (Program.CurrentPlayer.Level < 3) {
//        return Program.Rand.Next(1, 100 + 1) switch {
//            int n when 91 <= n           => ("Grave Robber", Tribe.Human),
//            int n when 46 <= n && n < 91 => ("Giant Bat", Tribe.Beast),
//            _ => ("Giant Rat", Tribe.Beast),
//        };
//    } else if (Program.CurrentPlayer.Level <= 5) {
//        return Program.Rand.Next(1, 100 + 1) switch {
//            int n when 90 <= n           => ("Skeleton", Tribe.Undead),
//            int n when 80 <= n && n < 90 => ("Zombie", Tribe.Undead),
//            int n when 60 <= n && n < 80 => ("Grave Robber", Tribe.Human),
//            int n when 30 <= n && n < 60 => ("Giant Bat", Tribe.Beast),
//            _ => ("Giant Rat", Tribe.Beast),
//        };
//    } else if (5 < Program.CurrentPlayer.Level && Program.CurrentPlayer.Level <= 15) {
//        return Program.Rand.Next(1, 100 + 1) switch {
//            int n when 15 <= n && n < 30 => ("Giant Bat", Tribe.Beast),
//            int n when 30 <= n && n < 50 => ("Grave Robber", Tribe.Human),
//            int n when 50 <= n && n < 70 => ("Zombie", Tribe.Undead),
//            int n when 70 <= n && n < 85 => ("Skeleton", Tribe.Undead),
//            int n when 85 <= n && n < 90 => ("Bandit", Tribe.Human),
//            int n when 90 <= n && n < 95 => ("Human Rogue", Tribe.Human),
//            int n when 95 <= n && n < 98 => ("Human Cultist", Tribe.Human),
//            int n when 98 <= n           => ("Dire Wolf", Tribe.Beast),
//            _ => ("Giant Rat", Tribe.Beast),
//        };
//    } else {
//        return Program.Rand.Next(1, 100 + 1) switch {
//            int n when 25 <= n && n < 50 => ("Bandit", Tribe.Human),
//            int n when 50 <= n && n < 60 => ("Human Rogue", Tribe.Human),
//            int n when 60 <= n && n < 75 => ("Human Cultist", Tribe.Human),
//            int n when 75 <= n && n < 85 => ("Dire Wolf", Tribe.Beast),
//            int n when 85 <= n && n < 95 => ("Werewolf", Tribe.Beast),
//            int n when 95 <= n           => ("Vampire", Tribe.Undead),
//            _ => ("Skeleton", Tribe.Undead),
//        };
//    }
//}