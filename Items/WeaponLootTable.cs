using Saga.Character;
using System;

namespace Saga.Items
{
    [Serializable]
    public class WeaponLootTable : Weapon
    {
        public static Weapon RustySword = new Weapon() {
            ItemName = "Rusty Sword",
            ItemLevel = 1,
            ItemSlot = Slot.SLOT_WEAPON,
            WeaponType = WeaponType.WEAPON_SWORD,
            WeaponAttributes = new WeaponAttributes() { MinDamage = 1, MaxDamage = 2, AttackSpeed = 1 },
        };

        public static Weapon CrackedWand = new Weapon() {
            ItemName = "Cracked Wand",
            ItemLevel = 1,
            ItemSlot = Slot.SLOT_WEAPON,
            WeaponType = WeaponType.WEAPON_WAND,
            WeaponAttributes = new WeaponAttributes() { MinDamage = 1, MaxDamage = 2, AttackSpeed = 1 },
        };

        public static Weapon FlimsyBow = new Weapon() {
            ItemName = "Flimsy Bow",
            ItemLevel = 1,
            ItemSlot = Slot.SLOT_WEAPON,
            WeaponType = WeaponType.WEAPON_BOW,
            WeaponAttributes = new WeaponAttributes() { MinDamage = 1, MaxDamage = 2, AttackSpeed = 1 },
        };
        public static Weapon EnchantedWand = new Weapon() {
            ItemName = "Enchanted Wand",
            ItemLevel = 1,
            ItemSlot = Slot.SLOT_WEAPON,
            WeaponType = WeaponType.WEAPON_WAND,
            WeaponAttributes = new WeaponAttributes() { MinDamage = 2, MaxDamage = 4, AttackSpeed = 1 },
        };
        public static Weapon GnarledStaff = new Weapon() {
            ItemName = "Gnarled Staff",
            ItemLevel = 1,
            ItemSlot = Slot.SLOT_WEAPON,
            WeaponType = WeaponType.WEAPON_STAFF,
            WeaponAttributes = new WeaponAttributes() { MinDamage = 4, MaxDamage = 8, AttackSpeed = 1 },
        };
        public static Weapon ArcanistsStaff = new Weapon() {
            ItemName = "Arcanist's Staff",
            ItemLevel = 1,
            ItemSlot = Slot.SLOT_WEAPON,
            WeaponType = WeaponType.WEAPON_STAFF,
            WeaponAttributes = new WeaponAttributes() { MinDamage = 6, MaxDamage = 16, AttackSpeed = 1 },
        };
        public static Weapon ShortBow = new Weapon() {
            ItemName = "Short Bow",
            ItemLevel = 1,
            ItemSlot = Slot.SLOT_WEAPON,
            WeaponType = WeaponType.WEAPON_BOW,
            WeaponAttributes = new WeaponAttributes() { MinDamage = 2, MaxDamage = 4, AttackSpeed = 1 },
        };
        public static Weapon LongBow = new Weapon() {
            ItemName = "Long Bow",
            ItemLevel = 1,
            ItemSlot = Slot.SLOT_WEAPON,
            WeaponType = WeaponType.WEAPON_BOW,
            WeaponAttributes = new WeaponAttributes() { MinDamage = 4, MaxDamage = 8, AttackSpeed = 1 },
        };
        public static Weapon MarksmansRecurve = new Weapon() {
            ItemName = "Marksman's Recurve",
            ItemLevel = 1,
            ItemSlot = Slot.SLOT_WEAPON,
            WeaponType = WeaponType.WEAPON_BOW,
            WeaponAttributes = new WeaponAttributes() { MinDamage = 6, MaxDamage = 16, AttackSpeed = 1 },
        };
        public static Weapon SteelSword = new Weapon() {
            ItemName = "Steel Sword",
            ItemLevel = 1,
            ItemSlot = Slot.SLOT_WEAPON,
            WeaponType = WeaponType.WEAPON_SWORD,
            WeaponAttributes = new WeaponAttributes() { MinDamage = 2, MaxDamage = 4, AttackSpeed = 1 },
        };
        public static Weapon LongSword = new Weapon() {
            ItemName = "Long Sword",
            ItemLevel = 1,
            ItemSlot = Slot.SLOT_WEAPON,
            WeaponType = WeaponType.WEAPON_SWORD,
            WeaponAttributes = new WeaponAttributes() { MinDamage = 4, MaxDamage = 8, AttackSpeed = 1 },
        };
        public static Weapon KnightsGreatsword = new Weapon() {
            ItemName = "Knight's Greatsword",
            ItemLevel = 1,
            ItemSlot = Slot.SLOT_WEAPON,
            WeaponType = WeaponType.WEAPON_SWORD,
            WeaponAttributes = new WeaponAttributes() { MinDamage = 6, MaxDamage = 16, AttackSpeed = 1 },
        };

    }
}
