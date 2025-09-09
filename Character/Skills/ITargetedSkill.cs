using Saga.Dungeon.Enemies;

namespace Saga.Character.Skills
{
    internal interface ITargetedSkill : IActiveSkill
    {
        bool Activate(Player player, EnemyBase target);
    }
}
