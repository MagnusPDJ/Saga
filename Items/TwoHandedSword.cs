using Saga.Assets;
using Saga.Character.DmgLogic;
using Saga.Dungeon.Monsters;
using System;

namespace Saga.Items
{
    [Discriminator("twoHandedSword")]
    public class TwoHandedSword : ITwoHanded, IPhysical
    {
        public required string ItemName { get; set; }
        public int ItemLevel { get; set; }
        public int ItemPrice { get; set; }
        public string ItemDescription { get; init; } = string.Empty;
        public WeaponCategory WeaponCategory => WeaponCategory.Melee;
        public PhysicalType PhysicalType => PhysicalType.Normal;
        public Slot ItemSlot => Slot.Right_Hand;
        public WeaponAttributes WeaponAttributes { get; set; } = new WeaponAttributes();
        public string AttackDescription { get; set; } = string.Empty;

        public void SetWeaponAttributes() => WeaponAttributes = CalculateWeaponAttributes(ItemLevel);
        public WeaponAttributes CalculateWeaponAttributes(int level) {
            return new WeaponAttributes() { MinDamage = Math.Max(1, Program.CurrentPlayer.Level / 2 + level), MaxDamage = Math.Max(5, Program.CurrentPlayer.Level + level * 2) + Program.Rand.Next(3, 8), AttackSpeed = 1 };
        }
        public int CalculateItemPrice() {
            return Convert.ToInt32(ItemLevel * 100 + (WeaponAttributes.MaxDamage * 100 + WeaponAttributes.MinDamage * 50) * (1 + 1 / (WeaponAttributes.MaxDamage - WeaponAttributes.MinDamage)));
        }

        public string Equip() {
            if (ItemLevel > Program.CurrentPlayer.Level) {
                Console.WriteLine($"Character needs to be level {ItemLevel} to equip this weapon.");
                return "Weapon not equipped.";
            } else if (Program.CurrentPlayer.CurrentClass != "Warrior") {
                Console.WriteLine($"Character can't equip a weapon of type two-handed sword, {ItemName}.");
                return "Weapon not equipped.";
            }
            bool hasRight = Program.CurrentPlayer.Equipment.TryGetSlot(Slot.Right_Hand, out IEquipable? valueRight);
            bool hasLeft = Program.CurrentPlayer.Equipment.TryGetSlot(Slot.Left_Hand, out IEquipable? valueLeft);
            if (hasRight || hasLeft) {
                Console.WriteLine($"Do you want to switch {(valueRight != null ? valueRight.ItemName : "")}{(valueRight!=null &&valueLeft!=null ? " and ": "")}{(valueLeft != null ? valueLeft.ItemName : "")} for '{ItemName}'? (Y/N)");
                while (true) {
                    string input = TextInput.PlayerPrompt();
                    if (input == "y") {
                        valueRight?.UnEquip();
                        valueLeft?.UnEquip();
                        Program.CurrentPlayer.Equipment.SetSlot(ItemSlot, this);
                        Program.CurrentPlayer.Equipment.SetSlot(Slot.Left_Hand, new TwoHandedSword() { ItemName = ItemName, WeaponAttributes = { } });
                        int a = Array.IndexOf(Program.CurrentPlayer.Inventory, this);
                        Program.CurrentPlayer.Inventory.SetValue(null, a);
                        return "New weapon equipped!";
                    } else if (input == "n") {
                        return "Weapon not equipped.";
                    } else {
                        Console.WriteLine("Invalid input.");
                    }
                }
            } else {
                Program.CurrentPlayer.Equipment.SetSlot(ItemSlot, this);
                Program.CurrentPlayer.Equipment.SetSlot(Slot.Left_Hand, new TwoHandedSword() { ItemName = ItemName, WeaponAttributes = { } });
                int a = Array.IndexOf(Program.CurrentPlayer.Inventory, this);
                if (a == -1) {
                } else {
                    Program.CurrentPlayer.Inventory.SetValue(null, a);
                }
                return "New weapon equipped!";
            }
        }
        public string UnEquip() {
            int index = Array.FindIndex(Program.CurrentPlayer.Inventory, i => i == null || Program.CurrentPlayer.Inventory.Length == 0);
            Program.CurrentPlayer.Inventory.SetValue(this, index);
            Program.CurrentPlayer.Equipment.SetSlot(ItemSlot, null);
            Program.CurrentPlayer.Equipment.SetSlot(Slot.Left_Hand, null);
            if (Program.CurrentPlayer.Health > Program.CurrentPlayer.DerivedStats.MaxHealth) {
                Program.CurrentPlayer.Health = Program.CurrentPlayer.DerivedStats.MaxHealth;
            }
            return "Weapon unequipped!";
        }
        public (IDamageType, int) Attack(Enemy monster) {
            HUDTools.Print($"{AttackDescription}", 15);
            (IDamageType, int) attack = (this, Program.Rand.Next(WeaponAttributes.MinDamage, WeaponAttributes.MaxDamage + 1));
            HUDTools.Print($"You deal {attack.Item2} damage to {monster.Name}.", 10);
            return attack;
        }
    }
}
