using Saga.Assets;
using System.Text.Json;

namespace Saga.Dungeon.Rooms.Room_Objects
{
    public static class ContainerDatabase
    {
        private static readonly Dictionary<string, ILootable> _containers = new(StringComparer.OrdinalIgnoreCase);

        public static void LoadFromFile(string path) {
            var json = HUDTools.ReadAllResourceText(path);
            var containers = JsonSerializer.Deserialize<List<ILootable>>(json, Program.Options)
                       ?? throw new InvalidOperationException("Failed to deserialize containers.");
            foreach (var container in containers) {
                _containers[container.LootableId] = container;
            }
        }

        public static ILootable? GetByLootableId(string lootableId) =>
            _containers.TryGetValue(lootableId, out var container) ? container : null;
    }
}
