using Saga.Character;
using Saga.Dungeon;

namespace Saga.Items
{
    public enum WeaponCategory
    {
        Melee,
        Ranged,
        Magic
    }

    public interface IWeapon
    {
        WeaponCategory WeaponCategory { get; }
        WeaponAttributes WeaponAttributes { get; set; }
        string AttackDescription { get; set; }
        WeaponAttributes CalculateWeaponAttributes(int level);
        int Attack(Enemy Monster);
    }
}
