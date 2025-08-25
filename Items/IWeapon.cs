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

    internal interface IWeapon
    {
        public WeaponCategory WeaponCategory { get; }
        public WeaponAttributes WeaponAttributes { get; }
        WeaponAttributes GetWeaponAttributes(int level);
        int Attack(Enemy Monster);
    }
}
