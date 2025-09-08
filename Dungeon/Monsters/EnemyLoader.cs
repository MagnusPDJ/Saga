using System.Text.Json;

namespace Saga.Dungeon.Monsters
{
    public static class EnemyLoader
    {
        public static List<Enemy> LoadMonsters(string path) {
            var json = File.ReadAllText(path);
            return JsonSerializer.Deserialize<List<Enemy>>(json, Program.Options)
                   ?? [];
        }
    }
}
