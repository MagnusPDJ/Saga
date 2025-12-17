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
        public void UpgradeSkill(int index) {

        }
        public void UnlockSkill(int index) {
        
        }
        public void ChangeQuickCast(int index) {
            QuickCast = Skills[index].Name;
            HUDTools.Print($" Quickcast was changes to '{QuickCast}'", 5);
            TextInput.PressToContinue();
            HUDTools.ClearLastLine(3);
        }
    }
}
