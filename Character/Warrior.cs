using Saga.Character.DmgLogic;
using Saga.Character.Skills;
using Saga.Items;
using Saga.Items.Loot;

namespace Saga.Character
{
    public class Warrior(string name) : Player(name, "Warrior", new WarriorSkillTree(), 2, 1, 1)
    {
        public override void SetStartingGear() {
            (ItemDatabase.GetByItemId("warriorstarterweapon") as IEquipable)?.Equip();
            (ItemDatabase.GetByItemId("starterarmor") as IEquipable)?.Equip();
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
