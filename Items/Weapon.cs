using Saga.Character;
using System;

namespace Saga.Items
{
    public enum WeaponType
    {
        WEAPON_AXE,
        WEAPON_SWORD,
        WEAPON_HAMMER,
        WEAPON_BOW,
        WEAPON_DAGGER,
        WEAPON_CROSSBOW,
        WEAPON_STAFF,                     
        WEAPON_WAND,        
        WEAPON_TOME
    }
    [Serializable]
    public class Weapon : Item
    {
        public WeaponType WeaponType { get; set; }
        public WeaponAttributes WeaponAttributes { get; set; }

        /// <inheritdoc/>
        public override string ItemDescription() {
            return $"Weapon of type {WeaponType}";
        }
    }
}
