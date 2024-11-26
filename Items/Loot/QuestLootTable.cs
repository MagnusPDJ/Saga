using Saga.Character;
using System;


namespace Saga.Items.Loot
{
    [Serializable]
    public class QuestLootTable : QuestItem
    {
        public static QuestItem OldKey = new QuestItem() {
            ItemName = "Old Key",
            ItemSlot = Slot.Quest,
            ItemDescription = "An old key with two sturdy bits akin to those used for prison cells."
        };
    }
}
