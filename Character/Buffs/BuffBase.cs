using Saga.Character.DmgLogic;

namespace Saga.Character.Buffs
{
    public class BuffBase : IBuff
    {
        public string Name { get; protected set; } = string.Empty;
        public int Duration { get; protected set; }
        public int RemainingTurns { get; set; }

        public virtual int Strength => 0;
        public virtual int Dexterity => 0;
        public virtual int Intellect => 0;
        public virtual int Constitution => 0;
        public virtual int Willpower => 0;
        public virtual int Awareness => 0;
        public virtual int Virtue => 0;
        public virtual Dictionary<PhysicalType, int> PhysicalRes => new() {
            { PhysicalType.Normal, 0 },
            { PhysicalType.Piercing, 0 },
            { PhysicalType.Crushing, 0 }
        };
        public virtual Dictionary<ElementalType, int> ElementalRes => new() {
            { ElementalType.Frost, 0 },
            { ElementalType.Fire, 0 },
            { ElementalType.Poison, 0 },
            { ElementalType.Lightning, 0 }
        };
        public virtual Dictionary<MagicalType, int> MagicalRes => new() {
            { MagicalType.Arcane, 0 },
            { MagicalType.Chaos, 0 },
            { MagicalType.Void, 0 },
            { MagicalType.Nature, 0 },
            { MagicalType.Life, 0 },
            { MagicalType.Death, 0 }
        };
        public virtual int Initiative => 0;
        public virtual int Health => 0;
        public virtual int Mana => 0;
        public virtual int ManaRegenRate => 0;
        public virtual int ActionPoints => 0;
        public virtual int AttackSpeed => 0;
        public virtual int CastingSpeed => 0;
        public virtual int ArmorRating => 0;

        protected BuffBase(string name, int duration) {
            Name = name;
            Duration = duration;
            RemainingTurns = duration;
        }

        public virtual void OnApply(Player player) { }
        public virtual void OnRemove(Player player) { }

        public IEnumerable<KeyValuePair<string, int>> AsEnumerable() { 
            yield return new(nameof(Strength), Strength);
            yield return new(nameof(Dexterity), Dexterity);
            yield return new(nameof(Intellect), Intellect);
            yield return new(nameof(Constitution), Constitution);
            yield return new(nameof(Willpower), Willpower);
            yield return new(nameof(Awareness), Awareness);
            yield return new(nameof(Virtue), Virtue);
            yield return new(nameof(Initiative), Initiative);
            yield return new(nameof(Health), Health);
            yield return new(nameof(Mana), Mana);
            yield return new(nameof(ManaRegenRate), ManaRegenRate);
            yield return new(nameof(ActionPoints), ActionPoints);
            yield return new(nameof(AttackSpeed), AttackSpeed);
            yield return new(nameof(CastingSpeed), CastingSpeed);
            yield return new(nameof(ArmorRating), ArmorRating);
        }
    }
}
