
using Saga.Items.Loot;

namespace Saga.Dungeon.Rooms.Room_Objects
{
    public interface ILootable
    {
        LootTable LootTable { get; set; }
    }
}
