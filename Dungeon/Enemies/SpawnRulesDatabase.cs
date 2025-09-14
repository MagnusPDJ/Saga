using Saga.Assets;
using System.Text.Json;

namespace Saga.Dungeon.Enemies
{
    public static class SpawnRulesDatabase
    {
        private static readonly Dictionary<string, SpawnRules> _spawnRules = new(StringComparer.OrdinalIgnoreCase);

        public static void LoadFromFile(string path) {
            string json = HUDTools.ReadAllResourceText(path);
            var dungeons = JsonSerializer.Deserialize<List<SpawnRules>>(json, Program.Options)
                           ?? throw new InvalidOperationException("Failed to deserialize spawn rules.");

            foreach (var dungeon in dungeons) {
                _spawnRules[dungeon.DungeonKey] = dungeon;
            }
        }

        public static SpawnRules? GetByName(string name) =>
            _spawnRules.TryGetValue(name, out var spawnRules) ? spawnRules : null;

        public static List<SpawnRules> GetAll() => [.. _spawnRules.Values];
    }
}
