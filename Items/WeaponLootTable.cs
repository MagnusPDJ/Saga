using NAudio.Codecs;
using Saga.Character;
using System;

namespace Saga.Items
{
    [Serializable]
    public class WeaponLootTable : Weapon
    {
        public static Item CreateRandomWeapon(int level) {
            Item item;
            int weaponType = Program.rand.Next(9);
            item = new Weapon() {
                ItemLevel = Program.CurrentPlayer.Level + level,
                ItemSlot = Slot.Weapon,
                WeaponType = (WeaponType)weaponType,
                WeaponAttributes = new WeaponAttributes() { MinDamage = Program.CurrentPlayer.Level + level, MaxDamage = Program.CurrentPlayer.Level + level + Program.rand.Next(2, 6), AttackSpeed = 1 },
                ItemName = RandomWeaponName((WeaponType)weaponType)
            };

            return item;
        }
        public static Weapon RustySword = new Weapon() {
            ItemName = "Rusty Sword",
            ItemLevel = 1,
            ItemSlot = Slot.Weapon,
            WeaponType = WeaponType.Sword,
            WeaponAttributes = new WeaponAttributes() { MinDamage = 1, MaxDamage = 2, AttackSpeed = 1 },
        };

        public static Weapon CrackedWand = new Weapon() {
            ItemName = "Cracked Wand",
            ItemLevel = 1,
            ItemSlot = Slot.Weapon,
            WeaponType = WeaponType.Wand,
            WeaponAttributes = new WeaponAttributes() { MinDamage = 1, MaxDamage = 2, AttackSpeed = 1 },
        };

        public static Weapon FlimsyBow = new Weapon() {
            ItemName = "Flimsy Bow",
            ItemLevel = 1,
            ItemSlot = Slot.Weapon,
            WeaponType = WeaponType.Bow,
            WeaponAttributes = new WeaponAttributes() { MinDamage = 1, MaxDamage = 2, AttackSpeed = 1 },
        };
        public static Weapon EnchantedWand = new Weapon() {
            ItemName = "Enchanted Wand",
            ItemLevel = 2,
            ItemSlot = Slot.Weapon,
            WeaponType = WeaponType.Wand,
            WeaponAttributes = new WeaponAttributes() { MinDamage = 2, MaxDamage = 4, AttackSpeed = 1 },
        };
        public static Weapon GnarledStaff = new Weapon() {
            ItemName = "Gnarled Staff",
            ItemLevel = 4,
            ItemSlot = Slot.Weapon,
            WeaponType = WeaponType.Staff,
            WeaponAttributes = new WeaponAttributes() { MinDamage = 4, MaxDamage = 8, AttackSpeed = 1 },
        };
        public static Weapon ArcanistsStaff = new Weapon() {
            ItemName = "Arcanist's Staff",
            ItemLevel = 7,
            ItemSlot = Slot.Weapon,
            WeaponType = WeaponType.Staff,
            WeaponAttributes = new WeaponAttributes() { MinDamage = 6, MaxDamage = 16, AttackSpeed = 1 },
        };
        public static Weapon QuickShortBow = new Weapon() {
            ItemName = "Quick Shortbow",
            ItemLevel = 2,
            ItemSlot = Slot.Weapon,
            WeaponType = WeaponType.Bow,
            WeaponAttributes = new WeaponAttributes() { MinDamage = 2, MaxDamage = 4, AttackSpeed = 1 },
        };
        public static Weapon SturdyLongBow = new Weapon() {
            ItemName = "Sturdy Longbow",
            ItemLevel = 4,
            ItemSlot = Slot.Weapon,
            WeaponType = WeaponType.Bow,
            WeaponAttributes = new WeaponAttributes() { MinDamage = 4, MaxDamage = 8, AttackSpeed = 1 },
        };
        public static Weapon MarksmansRecurve = new Weapon() {
            ItemName = "Marksman's Recurve",
            ItemLevel = 7,
            ItemSlot = Slot.Weapon,
            WeaponType = WeaponType.Bow,
            WeaponAttributes = new WeaponAttributes() { MinDamage = 6, MaxDamage = 16, AttackSpeed = 1 },
        };
        public static Weapon SteelSword = new Weapon() {
            ItemName = "Steel Sword",
            ItemLevel = 2,
            ItemSlot = Slot.Weapon,
            WeaponType = WeaponType.Sword,
            WeaponAttributes = new WeaponAttributes() { MinDamage = 2, MaxDamage = 4, AttackSpeed = 1 },
        };
        public static Weapon FineLongSword = new Weapon() {
            ItemName = "Fine Longsword",
            ItemLevel = 4,
            ItemSlot = Slot.Weapon,
            WeaponType = WeaponType.Sword,
            WeaponAttributes = new WeaponAttributes() { MinDamage = 4, MaxDamage = 8, AttackSpeed = 1 },
        };
        public static Weapon KnightsGreatsword = new Weapon() {
            ItemName = "Knight's Greatsword",
            ItemLevel = 7,
            ItemSlot = Slot.Weapon,
            WeaponType = WeaponType.Sword,
            WeaponAttributes = new WeaponAttributes() { MinDamage = 6, MaxDamage = 16, AttackSpeed = 1 },
        };
    }
}
