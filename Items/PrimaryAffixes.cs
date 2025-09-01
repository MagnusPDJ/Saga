using System.Diagnostics.CodeAnalysis;

namespace Saga.Items
{
    public class PrimaryAffixes
    {
        public int Strength {  get; set; } = 0;
        public int Dexterity { get; set; } = 0;
        public int Intellect { get; set; } = 0;
        public int Constitution { get; set; } = 0;
        public int WillPower { get; set; } = 0;
        public int Awareness { get; set; } = 0;
        public int Virtue { get; set; } = 0;

        /// <summary>
        /// Enumerates all slots as key/value pairs.
        /// </summary>
        public IEnumerable<KeyValuePair<string, int>> AsEnumerable() {
            yield return new(nameof(Strength), Strength);
            yield return new(nameof(Dexterity), Dexterity);
            yield return new(nameof(Intellect), Intellect);
            yield return new(nameof(Constitution), Constitution);
            yield return new(nameof(WillPower), WillPower);
            yield return new(nameof(Awareness), Awareness);
            yield return new(nameof(Virtue), Virtue);
        }

        /// <summary>
        /// Tries to get an item in a slot by enum.
        /// </summary>
        public bool TryGetAffix(PrimaryAffixes primaryAffixes, [NotNullWhen(true)] out int? value) {
            var prop = GetType().GetProperty(primaryAffixes.ToString()?? "");
            value = (int?)prop?.GetValue(this);
            return value is not null;
        }

        /// <summary>
        /// Convenience iterator to use foreach if desired.
        /// </summary>
        public IEnumerable<KeyValuePair<string, int>> GetAffixes() => AsEnumerable();
    }
}
