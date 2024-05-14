using System;
using Saga.assets;

namespace Saga.Dungeon
{
    public enum Tribe 
    {
        Undead,
        Beast,
        Human,
        Demon,
        Orc,
        Goblin,
        Mythical,
        Troll,
        Dragon
    }
    public abstract class Enemy
    {
        public Tribe EnemyTribe { get; set; } = Tribe.Undead;
        public string Name { get; set; }
        public int Health { get; set; }             
        public int Power { get; set; }
        public int Awareness { get; set; } = 0;
        public int ExpModifier { get; set; } = 1;
        public int GoldModifier { get; set; } = 1;
        public int EnemyTurn { get; set; } = 1;
        public int AttackDebuff { get; set; } = 0;
        public int Armor { get; set; } = 0;
        public abstract void GetExp();
        public abstract int GetHealth(string name);
        public abstract int GetPower(string name);
        public abstract void MonsterActions(Enemy Monster, Encounters TurnTimer);
    }
}
