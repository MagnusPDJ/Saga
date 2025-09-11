using Saga.Assets;
using System.Text.Json;

namespace Saga.Items.Loot
{
    public static class ItemDatabase
    {
        private static readonly Dictionary<string, IItem> _items = new(StringComparer.OrdinalIgnoreCase);

        public static void LoadFromFile(string path) {
            var json = HUDTools.ReadAllResourceText(path);
            var items = JsonSerializer.Deserialize<List<IItem>>(json, Program.Options)
                       ?? throw new InvalidOperationException("Failed to deserialize items.");
            foreach (var item in items) {
                _items[item.ItemId] = item;
            }
        }

        public static IItem? GetByItemId(string itemId) => 
            _items.TryGetValue(itemId, out var item) ? item : null;
    }
}
