
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
        public int Awareness { get; set; } = 0;
        public abstract void GetExp();
        public abstract int GetHealth(string name);
        public abstract int GetPower(string name);
        public abstract void MonsterActions(Encounters TurnTimer);

        public void TakeDamage(int amount) {
            Health -= amount;
            if (Health < 0) Health = 0;
        }
        public void Heal(int amount) {
            Health += amount;
            if (Health > MaxHealth) Health = MaxHealth;
        }
    }
}
