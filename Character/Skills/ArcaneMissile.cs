using Saga.Assets;
using Saga.Character.DmgLogic;
using Saga.Dungeon.Monsters;
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
        public (int, int) Tier { get; set; }
        public int ManaCost { get; set; }
        public int ActionPointCost { get; set; } = 1;
        public MagicalType MagicalType => MagicalType.Arcane;

        public ArcaneMissile() {
            Name = "Arcane Missiles";
            Description = "Conjure magic in form of small rays that can pierce most material.\n(Requires an equipped magic weapon)";
            IsUnlocked = true;
            Tier = (1, 5);
            ManaCost = 5;
        }
        public void Activate(Player player, Enemy target) {
            if (Program.CurrentPlayer.Equipment.Right_Hand is IWeapon weapon && weapon.WeaponCategory == WeaponCategory.Magic) {
                if (player.SpendMana(ManaCost)) {
                    (IDamageType, int) damage = (this, 5);
                    (IDamageType, int) modifiedDamage = player.CalculateDamageModifiers(damage);
                    target.TakeDamage(modifiedDamage);
                    HUDTools.Print($"You shoot an arcane missile from your {weapon.ItemName}", 15);
                    HUDTools.Print($"You deal {modifiedDamage.Item2} damage to {target.Name}.", 10);
                } else {
                    HUDTools.Print("Not enough mana!", 10);
                    TextInput.PressToContinue();
                    HUDTools.ClearLastLine(3);
                }
            } else {
                HUDTools.Print("No magical weapon equipped!", 10);
                TextInput.PressToContinue();
                HUDTools.ClearLastLine(3);
            }            
        }
    }
}
