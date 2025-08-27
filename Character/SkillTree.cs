using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Saga.Character
{
    [JsonDerivedType(typeof(WarriorSkillTree), typeDiscriminator: "warriorSkillTree")]
    [JsonDerivedType(typeof(ArcherSkillTree), typeDiscriminator: "archerSkillTree")]
    [JsonDerivedType(typeof(MageSkillTree), typeDiscriminator: "mageSkillTree")]
    public abstract class SkillTree
    {
        public List<SkillBase> Skills { get; set; } = [];

        public List<SkillBase> GetAvailableSkills(int playerlvl) {
            return Skills.FindAll(skill => !skill.IsUnlocked && skill.LevelRequired <= playerlvl);
        }
    }
}
