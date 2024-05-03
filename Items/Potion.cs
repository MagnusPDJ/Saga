using Saga.Character;
using System;

namespace Saga.Items
{
    public enum PotionType
    {
        POTION_HEALING,
        POTION_MANA
    }
    [Serializable]
    public class Potion : Item
    {
        public int PotionPotency { get; set; }
        public int PotionQuantity { get; set; }
        public PotionType PotionType { get; set; }
        public override string ItemDescription() {
            return $"Potion of type {PotionType}";
        }

        public static Potion HealingPotion = new Potion() {
            ItemName = "Healing Potion",
            ItemLevel = 1,
            ItemSlot = Slot.SLOT_POTION,
            PotionType = PotionType.POTION_HEALING,
            PotionPotency = 5,
            PotionQuantity = 5
        };
    }
}
