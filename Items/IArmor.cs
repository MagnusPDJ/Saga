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
        public PrimaryAttributes PrimaryAttributes { get; set; }
        public SecondaryAttributes SecondaryAttributes { get; set; }
        PrimaryAttributes CalculatePrimaryAttributes(int level);
        void SetPrimaryAttributes();
        SecondaryAttributes CalculateSecondaryAttributes(int level);
        void SetSecondaryAttributes();
    }
}
