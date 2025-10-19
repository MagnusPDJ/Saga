namespace Saga.Character.Skills
{
    public interface ISkill
    {
        string Name { get; }
        string Description { get; }
        int LevelRequired { get; }
        bool IsUnlocked { get; set; }
        TierRange Tier { get; set; }
        int ManaCost { get; set; }
        int Cooldown { get; }
        int Timer { get; set; }
    }
}
