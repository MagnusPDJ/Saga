
using Saga.Assets;
using Saga.Character;
using Saga.Character.DmgLogic;
using Saga.Dungeon.Monsters;
using System;

namespace Saga.Items
{
    [Discriminator("bow")]
    public class Bow : ITwoHanded, IPhysical
    {
        public string ItemName { get; set; }
        public int ItemLevel { get; set; }
        public int ItemPrice { get; set; }
        public string ItemDescription { get; init; }
        public WeaponCategory WeaponCategory => WeaponCategory.Ranged;
        public PhysicalType PhysicalType => PhysicalType.Piercing;
        public Slot ItemSlot => Slot.Right_Hand;
        public WeaponAttributes WeaponAttributes { get; set; }
        public string AttackDescription { get; set; }

        public Bow() {

        }

        public void SetWeaponAttributes() => WeaponAttributes = CalculateWeaponAttributes(ItemLevel);
        public WeaponAttributes CalculateWeaponAttributes(int level) {
            return new WeaponAttributes() { MinDamage = Math.Max(1, Program.CurrentPlayer.Level + level), MaxDamage = Math.Max(1, Program.CurrentPlayer.Level + level) + Program.Rand.Next(2, 6), AttackSpeed = 1 };
        }
        public int CalculateItemPrice() {
            return Convert.ToInt32(ItemLevel * 100 + (WeaponAttributes.MaxDamage * 100 + WeaponAttributes.MinDamage * 50) * (1 + 1 / (WeaponAttributes.MaxDamage - WeaponAttributes.MinDamage)));
        }
        public string Equip() {
            if (ItemLevel > Program.CurrentPlayer.Level) {
                Console.WriteLine($"Character needs to be level {ItemLevel} to equip this weapon.");
                return "Weapon not equipped.";
            } else if (Program.CurrentPlayer.CurrentClass != "Archer") {
                Console.WriteLine($"Character can't equip a weapon of type bow, {ItemName}.");
                return "Weapon not equipped.";
            }
            bool hasRight = Program.CurrentPlayer.Equipment.TryGetValue(Slot.Right_Hand, out IEquipable valueRight);
            bool hasLeft = Program.CurrentPlayer.Equipment.TryGetValue(Slot.Left_Hand, out IEquipable valueLeft);
            if (hasRight || hasLeft) {
                Console.WriteLine($"Do you want to switch {(valueRight != null ? valueRight.ItemName : "")}{(valueRight != null && valueLeft != null ? " and " : "")}{(valueLeft != null ? valueLeft.ItemName : "")} for '{ItemName}'? (Y/N)");
                while (true) {
                    string input = TextInput.PlayerPrompt();
                    if (input == "y") {
                        valueRight?.UnEquip();
                        valueLeft?.UnEquip();
                        Program.CurrentPlayer.Equipment[ItemSlot] = this;
                        Program.CurrentPlayer.Equipment[Slot.Left_Hand] = new Bow() { ItemName = ItemName, WeaponAttributes = { } };
                        int a = Array.IndexOf(Program.CurrentPlayer.Inventory, this);
                        Program.CurrentPlayer.Inventory.SetValue(null, a);
                        Program.CurrentPlayer.CalculateTotalStats();
                        return "New weapon equipped!";
                    } else if (input == "n") {
                        return "Weapon not equipped.";
                    } else {
                        Console.WriteLine("Invalid input.");
                    }
                }
            } else {
                Program.CurrentPlayer.Equipment[ItemSlot] = this;
                Program.CurrentPlayer.Equipment[Slot.Left_Hand] = new Bow() { ItemName = ItemName, WeaponAttributes = { } };
                int a = Array.IndexOf(Program.CurrentPlayer.Inventory, this);
                if (a == -1) {
                } else {
                    Program.CurrentPlayer.Inventory.SetValue(null, a);
                }
                Program.CurrentPlayer.CalculateTotalStats();
                return "New weapon equipped!";
            }
        }
        public string UnEquip() {
            int index = Array.FindIndex(Program.CurrentPlayer.Inventory, i => i == null || Program.CurrentPlayer.Inventory.Length == 0);
            Program.CurrentPlayer.Inventory.SetValue(this, index);
            Program.CurrentPlayer.Equipment.Remove(ItemSlot);
            Program.CurrentPlayer.Equipment.Remove(Slot.Left_Hand);
            Program.CurrentPlayer.CalculateTotalStats();
            if (Program.CurrentPlayer.Health > Program.CurrentPlayer.TotalSecondaryAttributes.MaxHealth) {
                Program.CurrentPlayer.Health = Program.CurrentPlayer.TotalSecondaryAttributes.MaxHealth;
            }
            return "Weapon unequipped!";
        }
        public (IDamageType, int) Attack(Enemy monster) {
            HUDTools.Print($"{AttackDescription}", 15);
            (IDamageType, int) attack = (this, Program.Rand.Next(Program.CurrentPlayer.CalculateDPT().Item1, Program.CurrentPlayer.CalculateDPT().Item2 + 1));
            HUDTools.Print($"You deal {attack.Item2} damage to {monster.Name}.", 10);
            return attack;
        }
    }
}
