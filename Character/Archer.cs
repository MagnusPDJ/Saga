using Saga.Assets;
using Saga.Dungeon.Enemies;
using Saga.Character.DmgLogic;
using Saga.Character.Skills;
using Saga.Items;
using Saga.Items.Loot;

namespace Saga.Character
{
    public class Archer(string name) : Player(name, "Archer", new ArcherSkillTree(), 1, 2, 1)
    {
        public override void SetStartingGear() {
            (ItemDatabase.GetByItemId("archerstarterweapon") as IEquipable)?.Equip();
            (ItemDatabase.GetByItemId("starterarmor") as IEquipable)?.Equip();
            SetLevelUpValue();
            LearnedSkills.Add(new RapidFire());
            SkillTree.QuickCast = "Rapid Fire";
        }
        public override bool RunAway(EnemyBase Monster) {
            bool escaped = false;
            if (Monster.Name == "Human captor") {
                HUDTools.Print($" You try to run from the {Monster.Name}, but it knocks you down. You are unable to escape this turn", 15);
            }
            else {
                HUDTools.Print($" You outmanoeuvre the {Monster.Name} and you successfully escape!", 20);
                escaped = true;
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
