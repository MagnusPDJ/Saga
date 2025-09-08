using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Saga.Items.Loot
{
    public static class ItemDatabase
    {
        private static Dictionary<string, IItem> _items = [];

        public static void LoadItems(string path) {
            var json = File.ReadAllText(path);
            var list = JsonSerializer.Deserialize<List<IItem>>(json, Program.Options)
                       ?? [];
            _items = list.ToDictionary(i => i.ItemId, i => i, StringComparer.OrdinalIgnoreCase);
        }

        public static IItem? GetItem(string itemId)
            => _items.TryGetValue(itemId, out var item) ? item : null;
    }
}
