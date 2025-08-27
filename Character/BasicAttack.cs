using Saga.Assets;
using Saga.Dungeon;
using Saga.Items;
using System.Threading;

namespace Saga.Character
{
    public class BasicAttack : ActiveSkillBase
    {
        public BasicAttack() {
            Name = "Basic Attack";
            Description = "Attack using your equipped weapon.";
            LevelRequired = 0;
            IsUnlocked = true;
            Tier = (1, 1);
        }
        public override void Activate(Player player, Enemy target = null, Encounters turnTimer = null) {
            if (player.Equipment.TryGetValue(Slot.Right_Hand, out ItemBase value) && value is IWeapon weapon) {
                int damage = weapon.Attack(target);
                target.TakeDamage(damage);
                HUDTools.WriteCombatLog("attack", turnTimer, 0, damage, target);
            } else {
                HUDTools.Print($"You punch the {target.Name}!", 15);
                target.TakeDamage(1);
                HUDTools.Print($"You deal {1} damage to {target.Name}.", 10);
            }
        }
    }
}
