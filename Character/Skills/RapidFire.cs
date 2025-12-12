using Saga.Assets;
using Saga.Character.DmgLogic;
using Saga.Dungeon.Enemies;
using Saga.Items;

namespace Saga.Character.Skills
{
    [Discriminator("rapidFire")]
    public class RapidFire : ITargetedSkill
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public int LevelRequired => 0;
        public bool IsUnlocked { get; set; } = true;
        public TierRange Tier { get; set; } = new TierRange(1, 5);
        public int ManaCost { get; set; }
        public int Cooldown => 1;
        public int Timer { get; set; } = 0;
        public int ActionPointCost { get; set; } = 10;
        public string SpeedType => "Attack Speed";
        public RapidFire() {
            Name = "Rapid Fire";
            Description = "Fire a weapon more quickly, letting you shoot multiple times in one turn\n (Requires equipped ranged weapon.)";
            IsUnlocked = true;
            ManaCost = 5;
        }
        public bool Activate(Player player, EnemyBase target) {
            if (Program.CurrentPlayer.Equipment.Right_Hand is IWeapon weapon && weapon.WeaponCategory == WeaponCategory.Ranged) {
                if (player.SpendMana(ManaCost)) {
                    (IDamageType, int) firstAttack = weapon.Attack(target);
                    (IDamageType, int) modifiedFirstAttack = player.CalculateDamageModifiers(firstAttack);
                    target.TakeDamage(modifiedFirstAttack);
                    HUDTools.Print($" You deal {modifiedFirstAttack.Item2} damage to {target.Name}.", 10);
                    (IDamageType, int) secondAttack = weapon.Attack(target);
                    (IDamageType, int) modifiedSecondAttack = player.CalculateDamageModifiers(secondAttack);
                    target.TakeDamage(modifiedSecondAttack);
                    HUDTools.Print($" You deal {modifiedSecondAttack.Item2} damage to {target.Name}.", 10);
                    TextInput.PressToContinue();
                    HUDTools.ClearLastLine(6);
                    return true;
                } else {
                    HUDTools.Print(" Not enough mana!", 10);
                    TextInput.PressToContinue();
                    HUDTools.ClearLastLine(3);
                    return false;
                }
            } else {
                HUDTools.Print(" No ranged weapon equipped!", 10);
                TextInput.PressToContinue();
                HUDTools.ClearLastLine(3);
                return false;
            }
        }
    }
}
