using Saga.Assets;
using Saga.Character.DmgLogic;

namespace Saga.Dungeon.Monsters
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
        public string Name { get; set; } = string.Empty;
        private int _maxHealth;
        public int MaxHealth
        {
            get => _maxHealth;
            set
            {
                _maxHealth = value;
                Health = value;
            }
        }
        public int Health { get; private set; }
        public int Power { get; set; }
        public float ExpModifier { get; set; }
        public float GoldModifier { get; set; }


        public int EnemyTurn { get; set; } = 1;
        public int AttackDebuff { get; set; } = 0;
        public int Armor { get; set; } = 0;
        public int Initiative { get; set; } = 0;
        public abstract void GetExp();
        public abstract int GetHealth(string name);
        public abstract int GetPower(string name);
        public abstract void EnemyActions(CombatController combatController);

        public void TakeDamage((IDamageType, int) amount) {
            Health -= amount.Item2;
            if (Health < 0) Health = 0;
        }
        public void Heal(int amount) {
            Health += amount;
            if (Health > MaxHealth) Health = MaxHealth;
        }
    }
}
