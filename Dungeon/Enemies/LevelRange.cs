
namespace Saga.Dungeon.Enemies
{
    public class LevelRange
    {
        public int MinLevel { get; set; }
        public int MaxLevel { get; set; }
        public Dictionary<string, int> Enemies { get; set; } = [];
    }
}
