using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Saga.Items
{
    public class SecondaryAffixes
    {
        public int MaxHealth { get; set; } = 0;
        public int MaxMana { get; set; } = 0;
        public int Initiative { get; set; } = 0;
        public int ArmorRating { get; set; } = 0;
        public int ElementalResistance { get; set; } = 0;
        public int MagicalResistance { get; set; } = 0;

        /// <summary>
        /// Enumerates all slots as key/value pairs.
        /// </summary>
        public IEnumerable<KeyValuePair<string, int>> AsEnumerable() {
            yield return new(nameof(MaxHealth), MaxHealth);
            yield return new(nameof(MaxMana), MaxMana);
            yield return new(nameof(Initiative), Initiative);
            yield return new(nameof(ArmorRating), ArmorRating);
            yield return new(nameof(ElementalResistance), ElementalResistance);
            yield return new(nameof(MagicalResistance), MagicalResistance);
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
