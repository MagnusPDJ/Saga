using Saga.Dungeon;

namespace Saga.Items.Loot
{
    public abstract class Loot
    {
        public abstract void GetGold(float modifier);
        public abstract void GetFixedGold(int g);
        public abstract void GetPotions(int amount);
        public abstract void GetCombatLoot(Enemy monster, string message);
        public abstract void GetTreasureChestLoot();
        public abstract void GetExp(int expModifier, int flatExp = 0);
        public abstract void GetQuestLoot(int findgold, int findpotions, string questname, Enemy enemy=null);
    }
}
