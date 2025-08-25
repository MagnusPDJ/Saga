using Saga.Character;

namespace Saga.Items
{
    public enum ArmorType
    {
        Cloth,
        Leather,
        Mail,
        Plate
    }
    internal interface IArmor
    {
        public ArmorType ArmorType { get; set; }
        public PrimaryAttributes Attributes { get; set; }
        public SecondaryAttributes SecondaryAttributes { get; set; }
    }
}
