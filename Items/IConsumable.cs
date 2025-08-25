
namespace Saga.Items
{
    public enum PotionType
    {
        POTION_HEALING,
        POTION_MANA
    }
    internal interface IConsumable
    {
        int PotionPotency { get; set; }
        int PotionQuantity { get; set; }
        PotionType PotionType { get; set; }
        void Consume();
    }
}
