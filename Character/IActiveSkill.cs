
using Saga.Dungeon;

namespace Saga.Character
{
    public interface IActiveSkill
    {
        void Activate(Player player, Enemy target = null, Encounters turnTimer = null);
    }
}
