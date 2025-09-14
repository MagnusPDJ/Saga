
namespace Saga.Dungeon.Enemies
{
    public class SpawnRules
    {
        public string DungeonKey {  get; set; } = string.Empty;
        public List<LevelRange> LevelRanges { get; set; } = [];
    }
}
