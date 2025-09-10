using Saga.Assets;
using Saga.Character.DmgLogic;
using Saga.Character.Skills;
using Saga.Items;
using System.Text.Json;

namespace Saga.Character
{
    public class Mage(string name) : Player(name, "Mage", new MageSkillTree(), 1, 1, 2)
    {
        public override void SetStartingGear() {
            List<IWeapon> weapons = JsonSerializer.Deserialize<List<IWeapon>>(HUDTools.ReadAllResourceText("Saga.Items.Loot.WeaponDatabase.json"), Program.Options) ?? [];
            List<IArmor> armors = JsonSerializer.Deserialize<List<IArmor>>(HUDTools.ReadAllResourceText("Saga.Items.Loot.ArmorDatabase.json"), Program.Options) ?? [];
            if (weapons.Find(w => w.ItemName == "Cracked Wand") is IEquipable weapon) {
                weapon.Equip();
            }            
            if (armors.Find(a => a.ItemName == "Linen Rags") is IEquipable armor) {
                armor.Equip();
            }
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