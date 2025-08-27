
namespace Saga.Character
{
    public class SecondaryAttributes
    {
        public int MaxHealth { get; set; }
        public int MaxMana {  get; set; }
        public int Awareness { get; set; }
        public int ArmorRating { get; set; }
        public int ElementalResistance { get; set; }

        // Checks if two SecondaryAttributes objects are equal.
        // <param name="obj">Object to compare to</param>
        // <returns>True if objects are equal, otherwise false</returns>
        public override bool Equals(object obj) {
            return obj is SecondaryAttributes attributes &&
                   MaxHealth == attributes.MaxHealth &&
                   ArmorRating == attributes.ArmorRating &&
                   ElementalResistance == attributes.ElementalResistance &&
                   MaxMana == attributes.MaxMana &&
                   Awareness == attributes.Awareness;
        }
        public override int GetHashCode() {
            return System.HashCode.Combine(MaxHealth, MaxMana, Awareness, ArmorRating, ElementalResistance);
        }
        // <summary>
        // Adds two PrimaryAttributes together.
        // </summary>
        // <param name="a">Object one</param>
        // <param name="b">Object two</param>
        // <returns>New PrimaryAttributes object of sum of the inputs</returns>
        public static SecondaryAttributes operator +(SecondaryAttributes a, SecondaryAttributes b) => new() {
            MaxHealth = a.MaxHealth + b.MaxHealth,
            ArmorRating = a.ArmorRating + b.ArmorRating,
            ElementalResistance = a.ElementalResistance + b.ElementalResistance,
            MaxMana = a.MaxMana + b.MaxMana,
            Awareness = a.Awareness + b.Awareness
        };
    }
}
