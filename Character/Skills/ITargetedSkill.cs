using Saga.Dungeon.Monsters;

namespace Saga.Character.Skills
{
    internal interface ITargetedSkill : IActiveSkill
    {
        void Activate(Player player, Enemy target);
    }
}
