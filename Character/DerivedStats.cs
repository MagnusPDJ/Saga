
using Saga.Character;

namespace Saga.Character
{
    public class DerivedStats
    {
        private int maxHealth;
        private int maxMana;
        private int armorRating;
        private int elementalResistance;
        
        public int MaxHealth { get; set; }
        public int MaxMana {  get; set; }
        public int ArmorRating { get; set; }
        public int ElementalResistance { get; set; }

        // <summary>
        // Adds two PrimaryAttributes together.
        // </summary>
        // <param name="a">Object one</param>
        // <param name="b">Object two</param>
        // <returns>New PrimaryAttributes object of sum of the inputs</returns>
        public static DerivedStats operator +(DerivedStats a, DerivedStats b) => new() {
            MaxHealth = a.MaxHealth + b.MaxHealth,
            ArmorRating = a.ArmorRating + b.ArmorRating,
            ElementalResistance = a.ElementalResistance + b.ElementalResistance,
            MaxMana = a.MaxMana + b.MaxMana,
        };
    }
}




//public DerivedStats CalculateBaseSecondaryStats() {
//    return new DerivedStats()
//    {
//        MaxHealth = 5 + TotalPrimaryAttributes.Constitution * 5,
//        MaxMana = 5 + TotalPrimaryAttributes.WillPower * 5,
//        ArmorRating = (TotalPrimaryAttributes.Strength + TotalPrimaryAttributes.Dexterity) / 2,
//        ElementalResistance = TotalPrimaryAttributes.Intellect
//    };
//}