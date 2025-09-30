using System.Text.Json;
using Saga.Assets;

namespace Saga.Dungeon
{
    public static class DungeonDatabase
    {
        private static readonly Dictionary<string, List<string>[]> _dungeons = new(StringComparer.OrdinalIgnoreCase);
        private static readonly Dictionary<string, List<string>[]> _exits = new(StringComparer.OrdinalIgnoreCase);

        public static void LoadFromFile(string path) {
            string json = HUDTools.ReadAllResourceText(path);

            using var doc = JsonDocument.Parse(json);

            string dungeonsJson = "";
            string exitsJson = "";

            foreach (var element in doc.RootElement.EnumerateArray()) { 
                if (element.TryGetProperty("Dungeons", out var dungeons))
                    dungeonsJson = dungeons.GetRawText();
                if (element.TryGetProperty("Exits", out var exits))
                    exitsJson = exits.GetRawText();
            }

            var dungeonsDict = JsonSerializer.Deserialize<Dictionary<string,List<string>[]>>(dungeonsJson, Program.Options)
                           ?? throw new InvalidOperationException("Failed to deserialize dungeons.");
            foreach (var dungeon in dungeonsDict) {
                _dungeons[dungeon.Key] = dungeon.Value;
            }

            var exitsDict = JsonSerializer.Deserialize<Dictionary<string, List<string>[]>>(exitsJson, Program.Options)
                           ?? throw new InvalidOperationException("Failed to deserialize exits.");
            foreach (var exit in exitsDict) {
                _exits[exit.Key] = exit.Value;
            }
        }

        public static IReadOnlyDictionary<string, List<string>[]> GetDungeons() => _dungeons;
        public static IReadOnlyDictionary<string, List<string>[]> GetExits() => _exits;
    }
}
