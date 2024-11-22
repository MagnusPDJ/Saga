using System;
using System.Reflection;
using Saga.assets;
using Saga.Character;
using Saga.Dungeon;

namespace Saga.Items.Loot
{
    public abstract class Loot
    {
        public abstract int GetGold();
        public abstract void GetCombatLoot(Enemy monster, string message);
        public abstract void GetTreasureChestLoot();
        public abstract void GetExp(int expModifier, int flatExp = 0);
    }
}
