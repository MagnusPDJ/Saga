namespace Saga.Character.Skills
{
    public interface ISkill
    {
        string Name { get; }
        string Description { get; }
        int LevelRequired { get; }
        bool IsUnlocked { get; set; }
        (int, int) Tier { get; set; }
        int ManaCost { get; set; }
    }
}
