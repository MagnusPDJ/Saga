using Saga.Assets;

namespace Saga.Dungeon.Monsters
{
    public static class MonsterFactory
    {
        private static readonly Dictionary<string, Type> _tagMap = new(StringComparer.OrdinalIgnoreCase);

        static MonsterFactory() {
            var types = typeof(MonsterFactory).Assembly.GetTypes();
            foreach (var type in types) {
                if (type.GetCustomAttributes(typeof(MonsterTagAttribute), inherit: false)
                               .FirstOrDefault() is MonsterTagAttribute attr && typeof(Enemy).IsAssignableFrom(type))
                    _tagMap[attr.Tag] = type;
            }
        }

        public static Enemy Create(Enemy baseMonster, MonsterScaling? scaling = null, int playerLevel = 1, DungeonTemplate? dungeon = null) {
            // Start with the plain data monster (no mutation of the original list entry)
            Enemy current = baseMonster;

            bool scalingApplied = false;

            foreach (var tag in baseMonster.Tags) {
                if (_tagMap.TryGetValue(tag, out var wrapperType)) {
                    var ctor = wrapperType.GetConstructor([typeof(Enemy), typeof(MonsterScaling), typeof(int), typeof(DungeonTemplate)])
                               ?? wrapperType.GetConstructor([typeof(Enemy), typeof(MonsterScaling), typeof(int)])
                               ?? wrapperType.GetConstructor([typeof(Enemy), typeof(MonsterScaling)])
                               ?? wrapperType.GetConstructor([typeof(Enemy)]);

                    if (ctor == null) continue;

                    object?[] args = ctor.GetParameters().Length switch {
                        4 => [current, scalingApplied ? null : scaling, playerLevel, dungeon],
                        3 => [current, scalingApplied ? null : scaling, playerLevel],
                        2 => [current, scalingApplied ? null : scaling],
                        1 => [current],
                        _ => []
                    };

                    current = (Enemy)ctor.Invoke(args)!;

                    // Ensure scaling only applied once across all wrappers
                    if (!scalingApplied && scaling != null) scalingApplied = true;
                }
            }

            //// If no tag wrappers were applied but scaling was requested, apply a neutral scaling wrapper.
            //if (!scalingApplied && scaling != null)
            //    current = new ScaledMonster(current, scaling, playerLevel, dungeon);

            return current;
        }
    }
}
