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
    public interface IArmor : IEquipable
    {
        public ArmorType ArmorType { get; set; }
        public Attributes PrimaryAttributes { get; set; }
        public DerivedStats SecondaryAttributes { get; set; }
        Attributes CalculatePrimaryAttributes(int level);
        void SetPrimaryAttributes();
        DerivedStats CalculateSecondaryAttributes(int level);
        void SetSecondaryAttributes();
    }
}
