using Saga.Character;

namespace Saga.Dungeon.Enemies
{
    public static class SpawnManager
    {
        public static EnemyBase SpawnEnemyInRoom(Player player, string room) {
            SpawnRules? rules = SpawnRulesDatabase.GetByName(room);

            if (rules is not null) {
                foreach (var levelRange in rules.LevelRanges) {
                    if (levelRange.MaxLevel >= player.Level && levelRange.MinLevel <= player.Level) {
                        int total = levelRange.Enemies.Sum(x => Math.Max(1, x.Value));
                        int roll = Program.Rand.Next(1, total + 1);
                        int acc = 0;
                        foreach (var enemy in levelRange.Enemies) {                            
                            acc += enemy.Value;
                            if (roll <= acc) return EnemyFactory.CreateByName(enemy.Key);                            
                        }                   
                    }
                }
            }

            return new EnemyBase();
        }
    }
}
