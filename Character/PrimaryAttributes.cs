﻿
namespace Saga.Character
{
    public class PrimaryAttributes
    {
        public int Strength { get; set; }
        public int Dexterity { get; set; }
        public int Intellect { get; set; }
        public int Constitution { get; set; }
        public int WillPower { get; set; }

        // <summary>
        // Checks if two PrimaryAttributes objects are equal.
        // </summary>
        // <param name="obj">Object to compare to</param>
        // <returns>True if objects are equal, otherwise false</returns>
        public override bool Equals(object obj) {
            return obj is PrimaryAttributes attributes &&
                Strength == attributes.Strength &&
                Dexterity == attributes.Dexterity &&
                Intellect == attributes.Intellect &&
                Constitution == attributes.Constitution &&
                WillPower == attributes.WillPower;
        }
        public override int GetHashCode() {
            return System.HashCode.Combine(Strength, Dexterity, Intellect, Constitution, WillPower);
        }
        // <summary>
        // Adds two PrimaryAttributes together.
        // </summary>
        // <param name="a">Object one</param>
        // <param name="b">Object two</param>
        // <returns>New PrimaryAttributes object of sum of the inputs</returns>
        public static PrimaryAttributes operator +(PrimaryAttributes a, PrimaryAttributes b) => new()
        {
            Strength = a.Strength + b.Strength,
            Dexterity = a.Dexterity + b.Dexterity,
            Intellect = a.Intellect + b.Intellect,
            Constitution = a.Constitution + b.Constitution,
            WillPower = a.WillPower + b.WillPower
        };
    }
}
