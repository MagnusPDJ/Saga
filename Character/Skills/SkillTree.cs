using Saga.Assets;
using System.Text.Json.Serialization;

namespace Saga.Character.Skills
{
    [JsonDerivedType(typeof(WarriorSkillTree), typeDiscriminator: "warriorSkillTree")]
    [JsonDerivedType(typeof(ArcherSkillTree), typeDiscriminator: "archerSkillTree")]
    [JsonDerivedType(typeof(MageSkillTree), typeDiscriminator: "mageSkillTree")]
    public abstract class SkillTree
    {
        public List<List<ISkill>> Skills { get; set; } = [];
        public string QuickCast { get; set; } = string.Empty;

        public List<ISkill> GetLearnedSkills() {
            List<ISkill> learnedSkills = [];
            foreach(List<ISkill> skills in Skills) {
                learnedSkills.AddRange(skills.FindAll(skill => skill.IsUnlocked));
            }
            return learnedSkills;
        }
        public void UpgradeSkill(int branch, int index) {

        }
        public void UnlockSkill(int branch, int index) {
        
        }
        public void ChangeQuickCast(int index) {
            QuickCast = GetLearnedSkills()[index].Name;
            HUDTools.Print($" Quickcast was changed to '{QuickCast}'", 5);
            TextInput.PressToContinue();
            HUDTools.ClearLastLine(3);
        }
    }
}
