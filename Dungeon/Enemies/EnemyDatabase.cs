using Saga.Assets;
using System.Text.Json;

namespace Saga.Dungeon.Enemies
{
    public static class EnemyDatabase
    {
        private static readonly Dictionary<string, EnemyBase> _monsters = new(StringComparer.OrdinalIgnoreCase);

        public static void LoadFromFile(string path) {
            string json = HUDTools.ReadAllResourceText(path);
            var monsters = JsonSerializer.Deserialize<List<EnemyBase>>(json, Program.Options)
                           ?? throw new InvalidOperationException("Failed to deserialize monsters.");

            foreach (var m in monsters) {
                _monsters[m.Name] = m;
            }
        }

        public static EnemyBase? GetByName(string name) =>
            _monsters.TryGetValue(name, out var monster) ? monster : null;

        public static List<EnemyBase> GetAll() => [.. _monsters.Values];
    }

}
