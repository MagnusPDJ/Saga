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
        public int ManaCost { get; set; } = 5;
        public int Cooldown => 1;
        public int Timer { get; set; } = 0;
        public int ActionPointCost { get; set; } = 10;
        public string SpeedType => "Attack Speed";
        private float BonusDamage { get; set; } = 1f;
        public RapidFire() {
            Name = "Rapid Fire";
            Description = "Fire a weapon more quickly, letting you shoot multiple times in one turn\n (Requires equipped ranged weapon.)";
        }
        public bool Activate(Player player, EnemyBase target) {
            if (Program.CurrentPlayer.Equipment.Right_Hand is IWeapon weapon && weapon.WeaponCategory == WeaponCategory.Ranged) {
                if (player.SpendMana(ManaCost)) {
                    HUDTools.Print($"You use rapid fire and attack {Tier.Min + 1} times:", 10);
                    (IDamageType, int) firstAttack = weapon.Attack(target);
                    firstAttack = (firstAttack.Item1, (int)(firstAttack.Item2 * BonusDamage));
                    (IDamageType, int) modifiedFirstAttack = player.CalculateDamageModifiers(firstAttack);
                    target.TakeDamage(modifiedFirstAttack);
                    HUDTools.Print($" You deal {modifiedFirstAttack.Item2} damage to {target.Name}.", 10);
                    TextInput.PressToContinue();
                    HUDTools.ClearLastLine(3);
                    for (int i = 0; i < Tier.Min; i++) {                        
                        (IDamageType, int) secondAttack = weapon.Attack(target);
                        (IDamageType, int) modifiedSecondAttack = player.CalculateDamageModifiers(secondAttack);
                        target.TakeDamage(modifiedSecondAttack);
                        HUDTools.Print($" You deal {modifiedSecondAttack.Item2} damage to {target.Name}.", 10);
                        TextInput.PressToContinue();
                        HUDTools.ClearLastLine(3);
                    }                   
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
        public virtual void UpgradeTier() {
            Tier.Min++;
            ManaCost += 2;
            if (Tier.Min == Tier.Max) {
                BonusDamage = 2f;
            }
        }
    }
}
