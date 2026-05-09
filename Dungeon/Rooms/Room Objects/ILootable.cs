using Saga.Items.Loot;

namespace Saga.Dungeon.Rooms.Room_Objects
{
    public interface ILootable
    {
        string LootableId { get; init; }
        LootTable LootTable { get; set; }
    }
}
