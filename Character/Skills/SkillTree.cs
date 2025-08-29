using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Saga.Character.Skills
{
    [JsonDerivedType(typeof(WarriorSkillTree), typeDiscriminator: "warriorSkillTree")]
    [JsonDerivedType(typeof(ArcherSkillTree), typeDiscriminator: "archerSkillTree")]
    [JsonDerivedType(typeof(MageSkillTree), typeDiscriminator: "mageSkillTree")]
    public abstract class SkillTree
    {
        public List<ISkill> Skills { get; set; } = [];

        public List<ISkill> GetAvailableSkills(int playerlvl) {
            return Skills.FindAll(skill => !skill.IsUnlocked && skill.LevelRequired <= playerlvl);
        }
    }
}
