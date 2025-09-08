using Saga.Character.DmgLogic;
using System.Diagnostics.CodeAnalysis;

namespace Saga.Items
{
    public class SecondaryAffixes
    {
        public int MaxHealth { get; set; } = 0;
        public int MaxMana { get; set; } = 0;
        public int Initiative { get; set; } = 0;
        public int ArmorRating { get; set; } = 0;
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
        public Dictionary<PhysicalType, int> PhysicalResistance { get; set; } = new() {
            { PhysicalType.Normal, 0 },
            { PhysicalType.Piercing, 0 },
            { PhysicalType.Crushing, 0 }
        };
        public int ManaRegenRate { get; set; } = 0;
        public int ActionPoints { get; set; } = 0;

        /// <summary>
        /// Enumerates all slots as key/value pairs.
        /// </summary>
        public IEnumerable<KeyValuePair<string, int>> AsEnumerable() {
            yield return new(nameof(MaxHealth), MaxHealth);
            yield return new(nameof(MaxMana), MaxMana);
            yield return new(nameof(Initiative), Initiative);
            yield return new(nameof(ArmorRating), ArmorRating);
            yield return new(nameof(ManaRegenRate), ManaRegenRate);
            yield return new(nameof(ActionPoints), ActionPoints);
        }

        /// <summary>
        /// Tries to get an item in a slot by enum.
        /// </summary>
        public bool TryGetAffix(SecondaryAffixes secondaryAffixes, [NotNullWhen(true)] out int? value) {
            var prop = GetType().GetProperty(secondaryAffixes.ToString() ?? "");
            value = (int?)prop?.GetValue(this);
            return value is not null;
        }

        /// <summary>
        /// Convenience iterator to use foreach if desired.
        /// </summary>
        public IEnumerable<KeyValuePair<string, int>> GetAffixes() => AsEnumerable();
    }
}
