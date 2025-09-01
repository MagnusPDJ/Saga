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
        public PrimaryAffixes PrimaryAffixes { get; set; }
        public SecondaryAffixes SecondaryAffixes { get; set; }
        PrimaryAffixes CalculatePrimaryAffixes(int level);
        void SetPrimaryAffixes();
        SecondaryAffixes CalculateSecondaryAffixes(int level);
        void SetSecondaryAffixes();
    }
}
