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
        public Tribe EnemyTribe { get; set; }
        public string Name { get; set; }
        public int Health { get; set; }             
        public int Power { get; set; }
        public float ExpModifier { get; set; }
        public float GoldModifier { get; set; }


        public int EnemyTurn { get; set; } = 1;
        public int AttackDebuff { get; set; } = 0;
        public int Armor { get; set; } = 0;
        public int Awareness { get; set; } = 0;
        public abstract void GetExp();
        public abstract int GetHealth(string name);
        public abstract int GetPower(string name);
        public abstract void MonsterActions(Enemy Monster, Encounters TurnTimer);
    }
}
