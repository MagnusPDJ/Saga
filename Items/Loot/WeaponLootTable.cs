using System;
using Saga.Character;

namespace Saga.Items.Loot
{
    public class WeaponLootTable : Weapon
    {
        public static Item CreateRandomWeapon(int level) {
            int weaponType = Program.Rand.Next(9);
            Item item = new Weapon() {
                ItemLevel = Math.Max(1, Program.CurrentPlayer.Level + level),
                ItemSlot = Slot.Weapon,
                WeaponType = (WeaponTypes)weaponType,
                WeaponAttributes = new WeaponAttributes() { MinDamage = Math.Max(1, Program.CurrentPlayer.Level + level), MaxDamage = Math.Max(1, Program.CurrentPlayer.Level + level) + Program.Rand.Next(2, 6), AttackSpeed = 1 },
                ItemName = RandomWeaponName((WeaponTypes)weaponType)
            };

            return item;
        }

        public static Weapon RustySword = new Weapon() {
            ItemName = "Rusty Sword",
            ItemLevel = 1,
            ItemSlot = Slot.Weapon,
            WeaponType = WeaponTypes.Sword,
            WeaponAttributes = new WeaponAttributes() { MinDamage = 1, MaxDamage = 2, AttackSpeed = 1 },
        };
        public static Weapon CrackedWand = new Weapon() {
            ItemName = "Cracked Wand",
            ItemLevel = 1,
            ItemSlot = Slot.Weapon,
            WeaponType = WeaponTypes.Wand,
            WeaponAttributes = new WeaponAttributes() { MinDamage = 1, MaxDamage = 2, AttackSpeed = 1 },
        };
        public static Weapon FlimsyBow = new Weapon() {
            ItemName = "Flimsy Bow",
            ItemLevel = 1,
            ItemSlot = Slot.Weapon,
            WeaponType = WeaponTypes.Bow,
            WeaponAttributes = new WeaponAttributes() { MinDamage = 1, MaxDamage = 2, AttackSpeed = 1 },
        };
        public static Weapon EnchantedWand = new Weapon() {
            ItemName = "Enchanted Wand",
            ItemLevel = 2,
            ItemSlot = Slot.Weapon,
            WeaponType = WeaponTypes.Wand,
            WeaponAttributes = new WeaponAttributes() { MinDamage = 2, MaxDamage = 4, AttackSpeed = 1 },
        };
        public static Weapon GnarledStaff = new Weapon() {
            ItemName = "Gnarled Staff",
            ItemLevel = 4,
            ItemSlot = Slot.Weapon,
            WeaponType = WeaponTypes.Staff,
            WeaponAttributes = new WeaponAttributes() { MinDamage = 4, MaxDamage = 8, AttackSpeed = 1 },
        };
        public static Weapon ArcanistsStaff = new Weapon() {
            ItemName = "Arcanist's Staff",
            ItemLevel = 7,
            ItemSlot = Slot.Weapon,
            WeaponType = WeaponTypes.Staff,
            WeaponAttributes = new WeaponAttributes() { MinDamage = 6, MaxDamage = 16, AttackSpeed = 1 },
        };
        public static Weapon QuickShortBow = new Weapon() {
            ItemName = "Quick Shortbow",
            ItemLevel = 2,
            ItemSlot = Slot.Weapon,
            WeaponType = WeaponTypes.Bow,
            WeaponAttributes = new WeaponAttributes() { MinDamage = 2, MaxDamage = 4, AttackSpeed = 1 },
        };
        public static Weapon SturdyLongBow = new Weapon() {
            ItemName = "Sturdy Longbow",
            ItemLevel = 4,
            ItemSlot = Slot.Weapon,
            WeaponType = WeaponTypes.Bow,
            WeaponAttributes = new WeaponAttributes() { MinDamage = 4, MaxDamage = 8, AttackSpeed = 1 },
        };
        public static Weapon MarksmansRecurve = new Weapon() {
            ItemName = "Marksman's Recurve",
            ItemLevel = 7,
            ItemSlot = Slot.Weapon,
            WeaponType = WeaponTypes.Bow,
            WeaponAttributes = new WeaponAttributes() { MinDamage = 6, MaxDamage = 16, AttackSpeed = 1 },
        };
        public static Weapon SteelSword = new Weapon() {
            ItemName = "Steel Sword",
            ItemLevel = 2,
            ItemSlot = Slot.Weapon,
            WeaponType = WeaponTypes.Sword,
            WeaponAttributes = new WeaponAttributes() { MinDamage = 2, MaxDamage = 4, AttackSpeed = 1 },
        };
        public static Weapon FineLongSword = new Weapon() {
            ItemName = "Fine Longsword",
            ItemLevel = 4,
            ItemSlot = Slot.Weapon,
            WeaponType = WeaponTypes.Sword,
            WeaponAttributes = new WeaponAttributes() { MinDamage = 4, MaxDamage = 8, AttackSpeed = 1 },
        };
        public static Weapon KnightsGreatsword = new Weapon() {
            ItemName = "Knight's Greatsword",
            ItemLevel = 7,
            ItemSlot = Slot.Weapon,
            WeaponType = WeaponTypes.Sword,
            WeaponAttributes = new WeaponAttributes() { MinDamage = 6, MaxDamage = 16, AttackSpeed = 1 },
        };
    }
}
