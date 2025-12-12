namespace Saga.Character.Skills
{
    public class ArcherSkillTree : SkillTree
    {
        public ArcherSkillTree()
        {
            Skills.Add(new BasicAttack());
            Skills.Add(new RapidFire());
            //Skills.Add(new PiercingArrow());
            //Skills.Add(new MultiShot());
            //Skills.Add(new Evasion());
            //Skills.Add(new PoisonTippedArrows());
            //Skills.Add(new HawkEye());
            //Skills.Add(new RainOfArrows());
            //Skills.Add(new Camouflage());
            //Skills.Add(new ExplosiveArrow());
            //Skills.Add(new TrueShot());

            QuickCast = "Rapid Fire";
        }
    }
}
