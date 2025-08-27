using System.Text.Json.Serialization;

namespace Saga.Character
{
    [JsonDerivedType(typeof(BasicAttack), typeDiscriminator: "basicAttack")]
    public abstract class SkillBase : ISkill
    {
        public string Name { get; protected set; }
        public string Description { get; protected set; }
        public int LevelRequired { get; protected set; }
        public bool IsUnlocked { get; set; } = false;
        public (int, int) Tier { get; set; }
    }
}
