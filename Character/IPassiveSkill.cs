
using Saga.Dungeon;

namespace Saga.Character
{
    public interface IPassiveSkill
    {
        void Effect(Player player, Enemy target = null);
    }
}
