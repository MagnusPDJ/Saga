
namespace Saga.Items.Loot
{
    public class QuestLootTable : QuestItem
    {
        public static QuestItem OldKey = new QuestItem() {
            ItemName = "Old key",
            ItemSlot = Slot.Quest,
            ItemDescription = "An old key, with two sturdy bits, akin to those used for prison cells."
        };
        public static QuestItem RatTail = new QuestItem() {
            ItemName = "Rat tail",
            ItemSlot = Slot.Quest,
            ItemDescription = "This tail was cut from a large rat, gross."
        };
        public static QuestItem BatWings = new QuestItem() {
            ItemName = "Bat wings",
            ItemSlot = Slot.Quest,
            ItemDescription = "Wings from a huge bat, they smell quite fragrant."
        };
    }
}
