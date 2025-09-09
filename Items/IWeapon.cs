using Saga.Character.DmgLogic;
using Saga.Dungeon.Enemies;

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
        (IDamageType, int) Attack(EnemyBase Monster);
    }
}
