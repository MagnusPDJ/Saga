using System.Collections.Generic;
using System.Linq;

namespace Saga.Character
{
    public class Attributes
    {
        private int constitution;
        private int willPower;
        private int awareness;
        private int virtue;
        //Primary attributes
        public int Strength { get; set; }
        public int Dexterity { get; set; }
        public int Intellect { get; set; }

        //Secondary attributes
        public int Constitution {
            get { return constitution; }
            set { constitution = CalculateConstiution(); }
        }
        public int WillPower {
            get { return willPower; }
            set { willPower = CalculateWillPower(); }
        }
        public int Awareness {
            get { return awareness; }
            set { awareness = CalculateAwareness(); }
        }
        public int Virtue { 
            get { return  virtue; }
            set { virtue = CalculateVirtue(); } 
        }

        // <summary>
        // Adds two PrimaryAttributes together.
        // </summary>
        // <param name="a">Object one</param>
        // <param name="b">Object two</param>
        // <returns>New PrimaryAttributes object of sum of the inputs</returns>
        public static Attributes operator +(Attributes a, Attributes b) => new()
        {
            Strength = a.Strength + b.Strength,
            Dexterity = a.Dexterity + b.Dexterity,
            Intellect = a.Intellect + b.Intellect,
            Constitution = a.Constitution + b.Constitution,
            WillPower = a.WillPower + b.WillPower,
            Awareness = a.Awareness + b.Awareness,
        };

        int CalculateConstiution() {
            return (Strength + Dexterity) / 2;
        }
        int CalculateWillPower() {
            return (Strength + Intellect) / 2;
        }
        int CalculateAwareness() {
            return (Dexterity + Intellect) / 2;
        }
        int CalculateVirtue() {
            List<int> list = [Strength, Constitution, WillPower];
            return list.Min();
        }

    }
}
