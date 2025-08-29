using Saga.Character;
using Saga.Character.DmgLogic;
using Saga.Dungeon.Monsters;

namespace Saga.Items
{
    public enum WeaponCategory
    {
        Melee,
        Ranged,
        Magic
    }
    public interface IWeapon : IEquipable
    {
        WeaponCategory WeaponCategory { get; }
        WeaponAttributes WeaponAttributes { get; set; }
        string AttackDescription { get; set; }
        WeaponAttributes CalculateWeaponAttributes(int level);
        void SetWeaponAttributes();
        (IDamageType, int) Attack(Enemy Monster);
    }
}
