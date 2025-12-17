namespace Saga.Character.Skills
{
    public class MageSkillTree : SkillTree
    {
        public MageSkillTree()
        {
            Skills.Add(new BasicAttack());
            Skills.Add(new MagicMissile());
            //Skills.Add(new Fireball());
            //Skills.Add(new FrostNova());
            //Skills.Add(new ManaShield());
            //Skills.Add(new Teleport());

            QuickCast = "MagicMissile";
        }
    }
}
