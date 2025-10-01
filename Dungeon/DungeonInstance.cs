using Saga.Dungeon.Rooms;

namespace Saga.Dungeon
{
    public class DungeonInstance
    {
        public string DungeonName { get; set; } = "";
        public int Level { get; set; } = 1;
        public double DifficultyMultiplier { get; set; } = 1.0;
        public List<RoomBase> Rooms { get; set; } = [];
    }
}
