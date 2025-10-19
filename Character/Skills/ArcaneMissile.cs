using Saga.Assets;
using Saga.Character.DmgLogic;
using Saga.Dungeon.Enemies;
using Saga.Items;

namespace Saga.Character.Skills
{
    [Discriminator("arcaneMissile")]
    public class ArcaneMissile : ITargetedSkill, IMagical
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public int LevelRequired => 0;
        public bool IsUnlocked { get; set; }
        public TierRange Tier { get; set; } = new TierRange(1, 5);
        public int ManaCost { get; set; }
        public int Cooldown => 1;
        public int Timer { get; set; } = 0;
        public int ActionPointCost { get; set; } = 10;
        public MagicalType MagicalType => MagicalType.Arcane;
        public string SpeedType => "Casting Speed";

        public ArcaneMissile() {
            Name = "Arcane Missiles";
            Description = "Conjure magic in form of small rays that can pierce most material.\n (Requires an equipped magic weapon.)";
            IsUnlocked = true;
            ManaCost = 5;
        }
        public bool Activate(Player player, EnemyBase target) {
            if (Program.CurrentPlayer.Equipment.Right_Hand is IWeapon weapon && weapon.WeaponCategory == WeaponCategory.Magic) {
                if (player.SpendMana(ManaCost)) {
                    (IDamageType, int) damage = (this, 5);
                    (IDamageType, int) modifiedDamage = player.CalculateDamageModifiers(damage);
                    target.TakeDamage(modifiedDamage);
                    HUDTools.Print($" You shoot an arcane missile from your {weapon.ItemName}", 15);
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
    }
}
