using Saga.Dungeon.Monsters;

namespace Saga.Character.Skills
{
    public interface IPassiveSkill : ISkill
    {
        void Effect(Player player, Enemy target = null);
    }
}
