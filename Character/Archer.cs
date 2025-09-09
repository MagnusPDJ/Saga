using Saga.Assets;
using Saga.Character.DmgLogic;
using Saga.Character.Skills;
using Saga.Dungeon.Enemies;
using Saga.Items;
using System.Text.Json;

namespace Saga.Character
{
    public class Archer(string name) : Player(name, "Archer", new ArcherSkillTree(), 1, 2, 1)
    {
        public override void SetStartingGear() {
            List<IWeapon> weapons = JsonSerializer.Deserialize<List<IWeapon>>(HUDTools.ReadAllResourceText("Saga.Items.Loot.WeaponDataBase.json"), Program.Options) ?? [];
            List<IArmor> armors = JsonSerializer.Deserialize<List<IArmor>>(HUDTools.ReadAllResourceText("Saga.Items.Loot.ArmorDataBase.json"), Program.Options) ?? [];
            if (weapons.Find(w => w.ItemName == "Flimsy Bow") is IEquipable weapon) {
                weapon.Equip();
            }
            if (armors.Find(a => a.ItemName == "Linen Rags") is IEquipable armor) {
                armor.Equip();
            }
            SetLevelUpValue();
            HealingPotion healingPotion = new();
            healingPotion.Equip();
        }
        public override bool RunAway(EnemyBase Monster) {
            bool escaped = false;
            if (Monster.Name == "Human captor") {
                HUDTools.Print($"You try to run from the {Monster.Name}, but it knocks you down. You are unable to escape this turn", 15);
            }
            else {
                HUDTools.Print($"You use your crazy ninja moves to evade the {Monster.Name} and you successfully escape!", 20);
                escaped = true;
                Program.RoomController.ran = true;
            }
            return escaped;
        }
        public override (IDamageType, int) CalculateDamageModifiers((IDamageType, int) damage) {
            (IDamageType, int) modifiedDamage = (new OneHandedSword(), 0);
            modifiedDamage.Item1 = damage.Item1;
            modifiedDamage.Item2 = damage.Item2 + Attributes.Dexterity / 3;
            return modifiedDamage;           
        }
    }
}
