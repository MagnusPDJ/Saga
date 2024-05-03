using System;

namespace Saga.Character
{
    [Serializable]
    public class SecondaryAttributes
    {
        public int MaxHealth { get; set; }
        public int MaxMana {  get; set; }
        public int Awareness { get; set; }
        public int ArmorRating { get; set; }
        public int ElementalResistence { get; set; }

        // Checks if two SecondaryAttributes objects are equal.
        // <param name="obj">Object to compare to</param>
        // <returns>True if objects are equal, otherwise false</returns>
        public override bool Equals(object obj) {
            return obj is SecondaryAttributes attributes &&
                   MaxHealth == attributes.MaxHealth &&
                   ArmorRating == attributes.ArmorRating &&
                   ElementalResistence == attributes.ElementalResistence &&
                   MaxMana == attributes.MaxMana &&
                   Awareness == attributes.Awareness;
        }

        public override int GetHashCode() {
            int hashCode = 1903012575;
            hashCode = hashCode * -1521134295 + MaxHealth.GetHashCode();
            hashCode = hashCode * -1521134295 + MaxMana.GetHashCode();
            hashCode = hashCode * -1521134295 + Awareness.GetHashCode();
            hashCode = hashCode * -1521134295 + ArmorRating.GetHashCode();
            hashCode = hashCode * -1521134295 + ElementalResistence.GetHashCode();
            return hashCode;
        }
    }
}
