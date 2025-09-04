using Saga.Assets;

namespace Saga.Items
{
    public enum PotionType
    {
        Healing,
        Mana,
        Poison,
        Buff
    }
    public interface IConsumable : IAction
    {
        int PotionPotency { get; set; }
        int PotionQuantity { get; set; }
        PotionType PotionType { get; }
        bool Consume();
    }
}
