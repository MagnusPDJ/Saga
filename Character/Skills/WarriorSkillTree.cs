namespace Saga.Character.Skills
{
    public class WarriorSkillTree : SkillTree
    {
        public WarriorSkillTree()
        {
            Skills.Add(new BasicAttack());
            Skills.Add(new Haste());
            Skills.Add(new RapidFire());
            Skills.Add(new ArcaneMissile());
            //    Skills.Add(new ShieldBlock());
            //    Skills.Add(new PowerStrike());
            //    Skills.Add(new Whirlwind());
            //    Skills.Add(new BattleCry());

            QuickCast = "Haste";
        }
    }
}
