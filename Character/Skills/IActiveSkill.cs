using Saga.Dungeon;
using Saga.Dungeon.Monsters;

namespace Saga.Character.Skills
{
    public interface IActiveSkill : ISkill
    {
        void Activate(Player player, Enemy target = null, Encounters turnTimer = null);
    }
}
