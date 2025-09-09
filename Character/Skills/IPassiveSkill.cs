using Saga.Dungeon.Enemies;

namespace Saga.Character.Skills
{
    public interface IPassiveSkill : ISkill
    {
        void Effect(Player player, EnemyBase target = null);
    }
}
