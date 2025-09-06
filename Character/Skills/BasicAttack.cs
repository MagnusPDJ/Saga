using Saga.Assets;
using Saga.Character.DmgLogic;
using Saga.Dungeon.Monsters;
using Saga.Items;

namespace Saga.Character.Skills
{
    [Discriminator("basicAttack")]
    public class BasicAttack : ITargetedSkill, IPhysical
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public int LevelRequired => 0;
        public bool IsUnlocked { get; set; }
        public TierRange Tier {  get; set; } = new TierRange(1,1);
        public int ManaCost {  get; set; }
        public int ActionPointCost { get; set; } = 10;
        public PhysicalType PhysicalType => PhysicalType.Normal;
        public BasicAttack() {
            Name = "Basic Attack";
            Description = "Attack using your equipped weapon.";
            IsUnlocked = true;
        }
        public bool Activate(Player player, Enemy target) {
            if (player.Equipment.Right_Hand is IWeapon weapon) {
                (IDamageType, int) damage = weapon.Attack(target);
                (IDamageType, int) modifiedDamage = player.CalculateDamageModifiers(damage);
                target.TakeDamage(modifiedDamage);
                HUDTools.Print($"You deal {modifiedDamage.Item2} damage to {target.Name}.", 10);
                TextInput.PressToContinue();
                HUDTools.ClearLastLine(3);
                return true;
            } else {
                HUDTools.Print($"You punch the {target.Name}!", 15);
                (IDamageType, int) damage = (this, 1);
                damage = player.CalculateDamageModifiers(damage);
                target.TakeDamage(damage);
                HUDTools.Print($"You deal {1} damage to {target.Name}.", 10);
                TextInput.PressToContinue();
                HUDTools.ClearLastLine(4);
                return true;
            }
        }
    }
}
