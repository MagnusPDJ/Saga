using Saga.Character.DmgLogic;
using Saga.Items.Loot;

namespace Saga.Dungeon.Enemies
{
    public class EnemyBase
    {
        public virtual string Name { get; set; } = string.Empty;
        public virtual int Power { get; set; }
        public virtual string PlayerDeathDescription { get; set; } = string.Empty;
        public virtual string EnemyCorpseDescription {  get; set; } = string.Empty;
        public virtual int MaxHealth { get; set; }
        public virtual int Health { get; set; }
        public virtual int Attack { get; set; }        
        public virtual int Initiative { get; set; }
        public virtual int Armor { get; set; }
        public virtual Dictionary<PhysicalType, int> PhysicalResistance { get; set; } = new() {
            { PhysicalType.Normal, 0 },
            { PhysicalType.Piercing, 0 },
            { PhysicalType.Crushing, 0 }
        };
        public virtual Dictionary<ElementalType, int> ElementalResistance { get; set; } = new() {
            { ElementalType.Frost, 0 },
            { ElementalType.Fire, 0 },
            { ElementalType.Poison, 0 },
            { ElementalType.Lightning, 0 }
        };
        public virtual Dictionary<MagicalType, int> MagicalResistance { get; set; } = new() {
            { MagicalType.Arcane, 0 },
            { MagicalType.Chaos, 0 },
            { MagicalType.Void, 0 },
            { MagicalType.Nature, 0 },
            { MagicalType.Life, 0 },
            { MagicalType.Death, 0 }
        };
        public virtual List<string> Tags { get; set; } = [];
        public virtual int ExpGain { get; set; }
        public virtual float GoldModifier { get; set; }
        public virtual LootTable LootTable { get; set; } = new();
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