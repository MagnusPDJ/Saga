using Saga.Assets;
using Saga.Character.DmgLogic;
using Saga.Dungeon;
using Saga.Dungeon.Monsters;
using Saga.Items;

namespace Saga.Character.Skills
{
    [Discriminator("basicAttack")]
    public class BasicAttack : IActiveSkill, IPhysical
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public int LevelRequired => 0;
        public bool IsUnlocked { get; set; }
        public (int, int) Tier {  get; set; }
        public PhysicalType PhysicalType => PhysicalType.Normal;
        public BasicAttack() {
            Name = "Basic Attack";
            Description = "Attack using your equipped weapon.";
            IsUnlocked = true;
            Tier = (1, 1);
        }
        public void Activate(Player player, Enemy target = null, Encounters turnTimer = null) {
            if (player.Equipment.TryGetValue(Slot.Right_Hand, out IEquipable value) && value is IWeapon weapon) {
                (IDamageType, int) damage = weapon.Attack(target);
                damage = player.CalculateDamageModifiers(damage);
                target.TakeDamage(damage);
                HUDTools.WriteCombatLog("attack", turnTimer, 0, damage.Item2, target);
            } else {
                HUDTools.Print($"You punch the {target.Name}!", 15);
                (IDamageType, int) damage = (this, 1);
                damage = player.CalculateDamageModifiers(damage);
                target.TakeDamage(damage);
                HUDTools.Print($"You deal {1} damage to {target.Name}.", 10);
                HUDTools.WriteCombatLog("attack", turnTimer, 0, damage.Item2, target);
            }
        }
    }
}
