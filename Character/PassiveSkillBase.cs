
using Saga.Dungeon;

namespace Saga.Character
{
    public abstract class PassiveSkillBase : SkillBase, IPassiveSkill
    {
        public abstract void Effect(Player player, Enemy target = null);
    }
}
