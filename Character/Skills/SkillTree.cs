using Saga.Assets;
using System.Text.Json.Serialization;

namespace Saga.Character.Skills
{
    [JsonDerivedType(typeof(WarriorSkillTree), typeDiscriminator: "warriorSkillTree")]
    [JsonDerivedType(typeof(ArcherSkillTree), typeDiscriminator: "archerSkillTree")]
    [JsonDerivedType(typeof(MageSkillTree), typeDiscriminator: "mageSkillTree")]
    public abstract class SkillTree
    {
        public List<ISkill> Skills { get; set; } = [];
        public string QuickCast { get; set; } = string.Empty;

        public List<ISkill> GetLearnedSkills() {
            return Skills.FindAll(skill => skill.IsUnlocked);
        }
        public List<ISkill> GetUnlockableSkills(int playerlvl) {
            return Skills.FindAll(skill => !skill.IsUnlocked && skill.LevelRequired <= playerlvl);
        }
    }
}
