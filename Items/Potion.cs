
namespace Saga.Items
{
    public enum PotionType
    {
        POTION_HEALING,
        POTION_MANA
    }

    public class Potion : Item
    {
        public int PotionPotency { get; set; }
        public int PotionQuantity { get; set; }
        public PotionType PotionType { get; set; }

        public Potion() {
            ItemName = "Healing Potion";
            ItemLevel = 1;
            PotionType = PotionType.POTION_HEALING;
            PotionPotency = 5;
            PotionQuantity = 5;
            ItemDescription = "They have a metallic taste and are somewhat sweet, but they reek of sulphur";
        }
        public override int CalculateItemPrice() {
            ItemPrice = 20+10*Program.CurrentPlayer.Level;
            return ItemPrice;
        }
    }
}
