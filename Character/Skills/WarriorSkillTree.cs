namespace Saga.Character.Skills
{
    public class WarriorSkillTree : SkillTree
    {
        public WarriorSkillTree()
        {
            Skills.Add([new BasicAttack()]);
            Skills.Add([new Haste(), new RapidFire(), new MagicMissile()]);
            //    Skills.Add(new ShieldBlock());
            //    Skills.Add(new PowerStrike());
            //    Skills.Add(new Whirlwind());
            //    Skills.Add(new BattleCry());

            QuickCast = "Haste";
        }
    }
}
