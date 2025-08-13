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
                ItemName = RandomWeaponName((WeaponTypes)weaponType),
                ItemDescription = "There is a sticker on the handle. It reads 'Gheed's standard second combat guarantee'",
            };
            return item;
        }

        public readonly static Weapon RustySword = new() {
            ItemName = "Rusty Sword",
            ItemLevel = 1,
            ItemSlot = Slot.Weapon,
            WeaponType = WeaponTypes.Sword,
            WeaponAttributes = new WeaponAttributes() { MinDamage = 1, MaxDamage = 2, AttackSpeed = 1 },
            ItemDescription = "Old and fragile but it is yours... You looted it yourself!",
        };
        public readonly static Weapon CrackedWand = new() {
            ItemName = "Cracked Wand",
            ItemLevel = 1,
            ItemSlot = Slot.Weapon,
            WeaponType = WeaponTypes.Wand,
            WeaponAttributes = new WeaponAttributes() { MinDamage = 1, MaxDamage = 2, AttackSpeed = 1 },
            ItemDescription = "It may be cracked, but nothing some duct tape couldn't fix!",
        };
        public readonly static Weapon FlimsyBow = new() {
            ItemName = "Flimsy Bow",
            ItemLevel = 1,
            ItemSlot = Slot.Weapon,
            WeaponType = WeaponTypes.Bow,
            WeaponAttributes = new WeaponAttributes() { MinDamage = 1, MaxDamage = 3, AttackSpeed = 1 },
            ItemDescription = "If you pull it too hard, it would probably snap. On the positive side, you'd have a nunchuck.",
        };
        public readonly static Weapon EnchantedWand = new() {
            ItemName = "Enchanted Wand",
            ItemLevel = 2,
            ItemSlot = Slot.Weapon,
            WeaponType = WeaponTypes.Wand,
            WeaponAttributes = new WeaponAttributes() { MinDamage = 2, MaxDamage = 4, AttackSpeed = 1 },
        };
        public readonly static Weapon GnarledStaff = new() {
            ItemName = "Gnarled Staff",
            ItemLevel = 4,
            ItemSlot = Slot.Weapon,
            WeaponType = WeaponTypes.Staff,
            WeaponAttributes = new WeaponAttributes() { MinDamage = 4, MaxDamage = 8, AttackSpeed = 1 },
        };
        public readonly static Weapon ArcanistsStaff = new() {
            ItemName = "Arcanist's Staff",
            ItemLevel = 7,
            ItemSlot = Slot.Weapon,
            WeaponType = WeaponTypes.Staff,
            WeaponAttributes = new WeaponAttributes() { MinDamage = 6, MaxDamage = 16, AttackSpeed = 1 },
        };
        public readonly static Weapon QuickShortBow = new() {
            ItemName = "Quick Shortbow",
            ItemLevel = 2,
            ItemSlot = Slot.Weapon,
            WeaponType = WeaponTypes.Bow,
            WeaponAttributes = new WeaponAttributes() { MinDamage = 2, MaxDamage = 4, AttackSpeed = 1 },
        };
        public readonly static Weapon SturdyLongBow = new() {
            ItemName = "Sturdy Longbow",
            ItemLevel = 4,
            ItemSlot = Slot.Weapon,
            WeaponType = WeaponTypes.Bow,
            WeaponAttributes = new WeaponAttributes() { MinDamage = 4, MaxDamage = 8, AttackSpeed = 1 },
        };
        public readonly static Weapon MarksmansRecurve = new() {
            ItemName = "Marksman's Recurve",
            ItemLevel = 7,
            ItemSlot = Slot.Weapon,
            WeaponType = WeaponTypes.Bow,
            WeaponAttributes = new WeaponAttributes() { MinDamage = 6, MaxDamage = 16, AttackSpeed = 1 },
        };
        public readonly static Weapon SteelSword = new() {
            ItemName = "Steel Sword",
            ItemLevel = 2,
            ItemSlot = Slot.Weapon,
            WeaponType = WeaponTypes.Sword,
            WeaponAttributes = new WeaponAttributes() { MinDamage = 2, MaxDamage = 4, AttackSpeed = 1 },
        };
        public readonly static Weapon FineLongSword = new() {
            ItemName = "Fine Longsword",
            ItemLevel = 4,
            ItemSlot = Slot.Weapon,
            WeaponType = WeaponTypes.Sword,
            WeaponAttributes = new WeaponAttributes() { MinDamage = 4, MaxDamage = 8, AttackSpeed = 1 },
        };
        public readonly static Weapon KnightsGreatsword = new() {
            ItemName = "Knight's Greatsword",
            ItemLevel = 7,
            ItemSlot = Slot.Weapon,
            WeaponType = WeaponTypes.Sword,
            WeaponAttributes = new WeaponAttributes() { MinDamage = 6, MaxDamage = 16, AttackSpeed = 1 },
        };
    }
}
