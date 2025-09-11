using Saga.Assets;
using Saga.Character.DmgLogic;
using Saga.Character.Skills;
using Saga.Items;
using Saga.Items.Loot;
using System.Text.Json;

namespace Saga.Character
{
    public class Mage(string name) : Player(name, "Mage", new MageSkillTree(), 1, 1, 2)
    {
        public override void SetStartingGear() {
            (ItemDatabase.GetByItemId("magestarterweapon") as IEquipable)?.Equip();
            (ItemDatabase.GetByItemId("starterarmor") as IEquipable)?.Equip();
            SetLevelUpValue();
            HealingPotion healingPotion = new();
            healingPotion.Equip();
            LearnedSkills.Add(new ArcaneMissile());
            SkillTree.QuickCast = "Arcane Missiles";
        }
        public override (IDamageType, int) CalculateDamageModifiers((IDamageType, int) damage) {          
            (IDamageType, int) modifiedDamage = (new OneHandedSword(), 0);
            modifiedDamage.Item1 = damage.Item1;
            modifiedDamage.Item2 = damage.Item2 + Attributes.Intellect / 3;
            return modifiedDamage;          
        }
    }
}