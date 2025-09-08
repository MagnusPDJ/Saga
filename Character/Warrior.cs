using Saga.Assets;
using Saga.Character.DmgLogic;
using Saga.Character.Skills;
using Saga.Dungeon.Monsters;
using Saga.Items;
using System.Text.Json;

namespace Saga.Character
{
    public class Warrior(string name) : Player(name, "Warrior", new WarriorSkillTree(), 2, 1, 1)
    {
        public override void SetStartingGear() {
            List<IWeapon> weapons = JsonSerializer.Deserialize<List<IWeapon>>(HUDTools.ReadAllResourceText("Saga.Items.Loot.WeaponDataBase.json"), Program.Options) ?? [];
            List<IArmor> armors = JsonSerializer.Deserialize<List<IArmor>>(HUDTools.ReadAllResourceText("Saga.Items.Loot.ArmorDataBase.json"), Program.Options) ?? [];
            if (weapons.Find(w => w.ItemName == "Rusty Sword") is IEquipable weapon) {
                weapon.Equip();
            }
            if (armors.Find(a => a.ItemName == "Linen Rags") is IEquipable armor) {
                armor.Equip();
            }
            SetLevelUpValue();
            HealingPotion healingPotion = new();
            healingPotion.Equip();
        }
        public override (IDamageType, int) CalculateDamageModifiers((IDamageType, int) damage) {          
            (IDamageType, int) modifiedDamage = (new OneHandedSword(), 0);
            modifiedDamage.Item1 = damage.Item1;
            modifiedDamage.Item2 = damage.Item2 + Level + Attributes.Strength / 3;
            return modifiedDamage;        
        }
    }
}
