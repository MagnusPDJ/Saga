using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Saga.Dungeon.Monsters
{
    public static class MonsterSpawner
    {
        public static Enemy PickRandom(IReadOnlyList<Enemy> monsters) {
            int total = monsters.Sum(m => Math.Max(1, m.SpawnWeight));
            int roll = Program.Rand.Next(1, total + 1);
            int acc = 0;

            foreach (var m in monsters) {
                acc += Math.Max(1, m.SpawnWeight);
                if (roll <= acc) return m;
            }
            return monsters[^1];
        }
    }
}
