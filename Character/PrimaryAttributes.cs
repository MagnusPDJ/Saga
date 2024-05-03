using System;

namespace Saga.Character
{
    [Serializable]
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
            int hashCode = -1364746490;
            hashCode = hashCode * -1521134295 + Strength.GetHashCode();
            hashCode = hashCode * -1521134295 + Dexterity.GetHashCode();
            hashCode = hashCode * -1521134295 + Intellect.GetHashCode();
            hashCode = hashCode * -1521134295 + Constitution.GetHashCode();
            hashCode = hashCode * -1521134295 + WillPower.GetHashCode();
            return hashCode;
        }

        // <summary>
        // Adds two PrimaryAttributes together.
        // </summary>
        // <param name="a">Object one</param>
        // <param name="b">Object two</param>
        // <returns>New PrimaryAttributes object of sum of the inputs</returns>
        public static PrimaryAttributes operator +(PrimaryAttributes a, PrimaryAttributes b) => new PrimaryAttributes()
        {
            Strength = a.Strength + b.Strength,
            Dexterity = a.Dexterity + b.Dexterity,
            Intellect = a.Intellect + b.Intellect,
            Constitution = a.Constitution + b.Constitution,
            WillPower = a.WillPower + b.WillPower
        };
    }
}
