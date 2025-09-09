using Saga.Character.DmgLogic;

namespace Saga.Dungeon.Enemies.Behaviour
{
    public abstract class EnemyWrapperBase(EnemyBase inner) : EnemyBase
    {
        protected readonly EnemyBase inner = inner;

        public override string Name { get => inner.Name; set => inner.Name = value; }
        public override string PlayerKillDescription { get => inner.PlayerKillDescription; set => inner.PlayerKillDescription = value; }
        public override int MaxHealth { get => inner.MaxHealth; set => inner.MaxHealth = value; }
        public override int Health { get => inner.Health; set => inner.Health = value; }
        public override int Attack { get => inner.Attack; set => inner.Attack = value; }
        public override int Initiative { get => inner.Initiative; set => inner.Initiative = value; }
        public override int Armor { get => inner.Armor; set => inner.Armor = value; }

        public override Dictionary<PhysicalType, int> PhysicalResistance { get => inner.PhysicalResistance; set => inner.PhysicalResistance = value; }
        public override Dictionary<ElementalType, int> ElementalResistance { get => inner.ElementalResistance; set => inner.ElementalResistance = value; }
        public override Dictionary<MagicalType, int> MagicalResistance { get => inner.MagicalResistance; set => inner.MagicalResistance = value; }

        public override List<string> Tags { get => inner.Tags; set => inner.Tags = value; }
        public override int ExpGain { get => inner.ExpGain; set => inner.ExpGain = value; }
        public override float GoldModifier { get => inner.GoldModifier; set => inner.GoldModifier = value; }
    }
}