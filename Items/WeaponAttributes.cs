using System.Diagnostics.CodeAnalysis;

namespace Saga.Items
{
    public class WeaponAttributes
    {
        public int MinDamage { get; set; }
        public int MaxDamage { get; set; }
        public int AttackSpeed { get; set; }
        /// <summary>
        /// Enumerates all slots as key/value pairs.
        /// </summary>
        public IEnumerable<KeyValuePair<string, int>> AsEnumerable() {
            yield return new(nameof(MinDamage), MinDamage);
            yield return new(nameof(MaxDamage), MaxDamage);
            yield return new(nameof(AttackSpeed), AttackSpeed);
        }

        /// <summary>
        /// Tries to get an item in a slot by enum.
        /// </summary>
        public bool TryGetAffix(PrimaryAffixes primaryAffixes, [NotNullWhen(true)] out int? value) {
            var prop = GetType().GetProperty(primaryAffixes.ToString() ?? "");
            value = (int?)prop?.GetValue(this);
            return value is not null;
        }

        /// <summary>
        /// Convenience iterator to use foreach if desired.
        /// </summary>
        public IEnumerable<KeyValuePair<string, int>> GetAffixes() => AsEnumerable();
    }
}
