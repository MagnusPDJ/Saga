
namespace Saga.Items
{
    public enum PotionType
    {
        Healing,
        Mana,
        Poison,
        Buff
    }
    public interface IConsumable
    {
        int PotionPotency { get; set; }
        int PotionQuantity { get; set; }
        PotionType PotionType { get; }
        int ActionPointCost { get; set; }
        void Consume();
    }
}
