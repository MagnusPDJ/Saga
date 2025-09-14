using System.Reflection;
using System.Text.Json;

namespace Saga.Dungeon.Enemies
{
    public static class EnemyFactory
    {
        private static readonly Dictionary<string, Type> _tagWrappers;
        private static readonly Random _rng = new();

        static EnemyFactory() {
            _tagWrappers = Assembly.GetExecutingAssembly()
                .GetTypes()
                .Select(t => new { Type = t, Attr = t.GetCustomAttribute<EnemyTagAttribute>() })
                .Where(x => x.Attr != null)
                .Select(x => new { x.Type, Attr = x.Attr! })
                .ToDictionary(x => x.Attr!.Tag, x => x.Attr.WrapperType, StringComparer.OrdinalIgnoreCase);
        }

        public static EnemyBase Create(EnemyBase template) {
            EnemyBase current = Clone(template);

            foreach (var tag in template.Tags) {
                if (_tagWrappers.TryGetValue(tag, out var wrapperType)) {
                    current = (EnemyBase)Activator.CreateInstance(wrapperType, current)!;
                }
            }

            return current;
        }

        public static EnemyBase CreateByName(string name) {
            var template = EnemyDatabase.GetByName(name)
                           ?? throw new ArgumentException($"Enemy '{name}' not found.");

            return Create(template);
        }

        public static EnemyBase CreateRandom() {
            var all = EnemyDatabase.GetAll();
            var chosen = all[_rng.Next(all.Count)];
            return Create(chosen);
        }

        public static EnemyBase CreateRandomByTag(string tag) {
            var all = EnemyDatabase.GetAll();
            var candidates = all.Where(m => m.Tags.Contains(tag, StringComparer.OrdinalIgnoreCase)).ToList();

            if (candidates.Count == 0)
                throw new InvalidOperationException($"No enemies found with tag '{tag}'.");

            var chosen = candidates[_rng.Next(candidates.Count)];
            return Create(chosen);
        }

        private static EnemyBase Clone(EnemyBase template) {
            var json = JsonSerializer.Serialize(template);
            return JsonSerializer.Deserialize<EnemyBase>(json)!;
        }
    }

}
