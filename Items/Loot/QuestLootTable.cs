
namespace Saga.Items.Loot
{
    public class QuestLootTable : QuestItem
    {
        public readonly static QuestItem OldKey = new() {
            ItemName = "Old key",
            ItemSlot = Slot.Quest,
            ItemDescription = "An old key, with two sturdy bits, akin to those used for prison cells."
        };
        public readonly static QuestItem RatTail = new() {
            ItemName = "Rat tail",
            ItemSlot = Slot.Quest,
            ItemDescription = "This tail was cut from a large rat, gross."
        };
        public readonly static QuestItem BatWings = new() {
            ItemName = "Bat wings",
            ItemSlot = Slot.Quest,
            ItemDescription = "Wings from a huge bat, they smell quite fragrant."
        };
    }
}
