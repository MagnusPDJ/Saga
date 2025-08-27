using Saga.Dungeon;

namespace Saga.Character
{
    public abstract class ActiveSkillBase : SkillBase, IActiveSkill 
    {
        public abstract void Activate(Player player, Enemy target = null, Encounters turnTimer = null);
    }
}
