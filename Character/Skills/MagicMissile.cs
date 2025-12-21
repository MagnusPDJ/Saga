using Saga.Assets;
using Saga.Character.DmgLogic;
using Saga.Dungeon.Enemies;
using Saga.Items;

namespace Saga.Character.Skills
{
    [Discriminator("magicMissile")]
    public class MagicMissile : ITargetedSkill, IMagical
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public int LevelRequired => 0;
        public bool IsUnlocked { get; set; } = true;
        public TierRange Tier { get; set; } = new TierRange(1, 5);
        public int ManaCost { get; set; } = 5;
        public int Cooldown { get; set; } = 1;
        public int Timer { get; set; } = 0;
        public int ActionPointCost { get; set; } = 10;
        public MagicalType MagicalType => MagicalType.Arcane;
        public string SpeedType => "Casting Speed";
        private int Damage { get; set; } = 5;

        public MagicMissile() {
            Name = "MagicMissile";
            Description = "Conjure magic in form of small rays that can pierce most material.\n (Requires an equipped magic weapon.)";
        }
        public bool Activate(Player player, EnemyBase target) {
            if (Program.CurrentPlayer.Equipment.Right_Hand is IWeapon weapon && weapon.WeaponCategory == WeaponCategory.Magic) {
                if (player.SpendMana(ManaCost)) {
                    (IDamageType, int) damage = (this, Damage);
                    (IDamageType, int) modifiedDamage = player.CalculateDamageModifiers(damage);
                    target.TakeDamage(modifiedDamage);
                    HUDTools.Print($" You shoot a magic missile from your {weapon.ItemName}", 15);
                    HUDTools.Print($" You deal {modifiedDamage.Item2} damage to {target.Name}.", 10);
                    TextInput.PressToContinue();
                    HUDTools.ClearLastLine(4);
                    return true;
                } else {
                    HUDTools.Print(" Not enough mana!", 10);
                    TextInput.PressToContinue();
                    HUDTools.ClearLastLine(3);
                    return false;
                }
            } else {
                HUDTools.Print(" No magical weapon equipped!", 10);
                TextInput.PressToContinue();
                HUDTools.ClearLastLine(3);
                return false;
            }            
        }
        public virtual void UpgradeTier() {
            Tier.Min++;
            ManaCost += 2;
            Damage += 5;
            if (Tier.Min == Tier.Max) {
                //Arcane deal double damage for some amount of turns.
            }
        }
    }
}
