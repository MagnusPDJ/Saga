using Saga.Assets;
using Saga.Character;
using Saga.Dungeon;
using System;

namespace Saga.Items.Loot
{
    namespace WeaponLootTable
    {
        public class Sword(int level) : WeaponBase
        {
            public override WeaponCategory WeaponCategory => WeaponCategory.Melee;
            public override WeaponAttributes WeaponAttributes => GetWeaponAttributes(level);
            public override string ItemName => throw new NotImplementedException();

            public override int Attack(Enemy Monster) {
                HUDTools.Print($"You swing your {ItemName}", 15);
                int attack = Program.Rand.Next(Program.CurrentPlayer.CalculateDPT().Item1, Program.CurrentPlayer.CalculateDPT().Item2 + 1);
                HUDTools.Print($"You deal {attack} damage to {Monster.Name}", 10);
                return attack;
            }
            public override WeaponAttributes GetWeaponAttributes(int level) {
                return new WeaponAttributes() { MinDamage = Math.Max(1, Program.CurrentPlayer.Level + level), MaxDamage = Math.Max(1, Program.CurrentPlayer.Level + level) + Program.Rand.Next(2, 6), AttackSpeed = 1 };
            }
        }
        
    }



    //    public readonly static WeaponBase RustySword = new() {
    //        ItemName = "Rusty Sword",
    //        ItemLevel = 1,
    //        ItemSlot = Slot.Right_Hand,
    //        WeaponType = WeaponCategory.Sword,
    //        WeaponAttributes = new WeaponAttributes() { MinDamage = 1, MaxDamage = 2, AttackSpeed = 1 },
    //        ItemDescription = "Old and fragile but it is yours... You looted it yourself!",
    //    };
    //    public readonly static WeaponBase CrackedWand = new() {
    //        ItemName = "Cracked Wand",
    //        ItemLevel = 1,
    //        ItemSlot = Slot.Right_Hand,
    //        WeaponType = WeaponCategory.Wand,
    //        WeaponAttributes = new WeaponAttributes() { MinDamage = 1, MaxDamage = 2, AttackSpeed = 1 },
    //        ItemDescription = "It may be cracked, but nothing some duct tape couldn't fix!",
    //    };
    //    public readonly static WeaponBase FlimsyBow = new() {
    //        ItemName = "Flimsy Bow",
    //        ItemLevel = 1,
    //        ItemSlot = Slot.Right_Hand,
    //        WeaponType = WeaponCategory.Bow,
    //        WeaponAttributes = new WeaponAttributes() { MinDamage = 1, MaxDamage = 3, AttackSpeed = 1 },
    //        ItemDescription = "If you pull it too hard, it would probably snap. On the positive side, you'd have a nunchuck.",
    //    };
    //    public readonly static WeaponBase EnchantedWand = new() {
    //        ItemName = "Enchanted Wand",
    //        ItemLevel = 2,
    //        ItemSlot = Slot.Right_Hand,
    //        WeaponType = WeaponCategory.Wand,
    //        WeaponAttributes = new WeaponAttributes() { MinDamage = 2, MaxDamage = 4, AttackSpeed = 1 },
    //    };
    //    public readonly static WeaponBase GnarledStaff = new() {
    //        ItemName = "Gnarled Staff",
    //        ItemLevel = 4,
    //        ItemSlot = Slot.Right_Hand,
    //        WeaponType = WeaponCategory.Staff,
    //        WeaponAttributes = new WeaponAttributes() { MinDamage = 4, MaxDamage = 8, AttackSpeed = 1 },
    //    };
    //    public readonly static WeaponBase ArcanistsStaff = new() {
    //        ItemName = "Arcanist's Staff",
    //        ItemLevel = 7,
    //        ItemSlot = Slot.Right_Hand,
    //        WeaponType = WeaponCategory.Staff,
    //        WeaponAttributes = new WeaponAttributes() { MinDamage = 6, MaxDamage = 16, AttackSpeed = 1 },
    //    };
    //    public readonly static WeaponBase QuickShortBow = new() {
    //        ItemName = "Quick Shortbow",
    //        ItemLevel = 2,
    //        ItemSlot = Slot.Right_Hand,
    //        WeaponType = WeaponCategory.Bow,
    //        WeaponAttributes = new WeaponAttributes() { MinDamage = 2, MaxDamage = 4, AttackSpeed = 1 },
    //    };
    //    public readonly static WeaponBase SturdyLongBow = new() {
    //        ItemName = "Sturdy Longbow",
    //        ItemLevel = 4,
    //        ItemSlot = Slot.Right_Hand,
    //        WeaponType = WeaponCategory.Bow,
    //        WeaponAttributes = new WeaponAttributes() { MinDamage = 4, MaxDamage = 8, AttackSpeed = 1 },
    //    };
    //    public readonly static WeaponBase MarksmansRecurve = new() {
    //        ItemName = "Marksman's Recurve",
    //        ItemLevel = 7,
    //        ItemSlot = Slot.Right_Hand,
    //        WeaponType = WeaponCategory.Bow,
    //        WeaponAttributes = new WeaponAttributes() { MinDamage = 6, MaxDamage = 16, AttackSpeed = 1 },
    //    };
    //    public readonly static WeaponBase SteelSword = new() {
    //        ItemName = "Steel Sword",
    //        ItemLevel = 2,
    //        ItemSlot = Slot.Right_Hand,
    //        WeaponType = WeaponCategory.Sword,
    //        WeaponAttributes = new WeaponAttributes() { MinDamage = 2, MaxDamage = 4, AttackSpeed = 1 },
    //    };
    //    public readonly static WeaponBase FineLongSword = new() {
    //        ItemName = "Fine Longsword",
    //        ItemLevel = 4,
    //        ItemSlot = Slot.Right_Hand,
    //        WeaponType = WeaponCategory.Sword,
    //        WeaponAttributes = new WeaponAttributes() { MinDamage = 4, MaxDamage = 8, AttackSpeed = 1 },
    //    };
    //    public readonly static WeaponBase KnightsGreatsword = new() {
    //        ItemName = "Knight's Greatsword",
    //        ItemLevel = 7,
    //        ItemSlot = Slot.Right_Hand,
    //        WeaponType = WeaponCategory.Sword,
    //        WeaponAttributes = new WeaponAttributes() { MinDamage = 6, MaxDamage = 16, AttackSpeed = 1 },
    //    };
    //}
}
