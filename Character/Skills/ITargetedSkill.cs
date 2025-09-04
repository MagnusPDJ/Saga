using Saga.Dungeon.Monsters;

namespace Saga.Character.Skills
{
    internal interface ITargetedSkill : IActiveSkill
    {
        bool Activate(Player player, Enemy target);
    }
}
