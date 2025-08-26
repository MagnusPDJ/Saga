
namespace Saga.Items
{
    public enum PotionType
    {
        Healing,
        Mana
    }
    internal interface IConsumable
    {
        int PotionPotency { get; set; }
        int PotionQuantity { get; set; }
        PotionType PotionType { get; }
        void Consume();
    }
}
