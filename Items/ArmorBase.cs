
using NAudio.CoreAudioApi;
using Saga.Assets;
using Saga.Character;
using System;
using System.Xml.Linq;

namespace Saga.Items
{
    public class ArmorBase : ItemBase, IArmor, IEquipable
    {
        public ArmorType ArmorType { get; set; }
        public Slot ItemSlot { get; set; }
        public PrimaryAttributes PrimaryAttributes { get; set; }
        public SecondaryAttributes SecondaryAttributes { get; set; }

        public ArmorBase() {

        }

        public void SetPrimaryAttributes() => PrimaryAttributes = CalculatePrimaryAttributes(ItemLevel);
        public void SetSecondaryAttributes() => SecondaryAttributes = CalculateSecondaryAttributes(ItemLevel);
        public PrimaryAttributes CalculatePrimaryAttributes(int level) {
            int constitution = 0;
            int strength = 0;
            int dexterity = 0;
            int intellect = 0;
            int willpower = 0;
            
            //Roll for primary stats:
            for (int i = 0; i < 5; i++) {

                int roll = Program.Rand.Next(1, 100 + 1);

                if (i == 0 && roll <= 10) {
                    constitution = Program.Rand.Next(Math.Max(1, Program.CurrentPlayer.Level + level));
                }
                if (i == 1 && roll <= 10) {
                    strength = Program.Rand.Next(Math.Max(1, Program.CurrentPlayer.Level + level));
                }
                if (i == 2 && roll <= 10) {
                    dexterity = Program.Rand.Next(Math.Max(1, Program.CurrentPlayer.Level + level));
                }
                if (i == 3 && roll <= 10) {
                    intellect = Program.Rand.Next(Math.Max(1, Program.CurrentPlayer.Level + level));
                }
                if (i == 4 && roll <= 10) {
                    willpower = Program.Rand.Next(Math.Max(1, Program.CurrentPlayer.Level + level));
                }

            }
            return new PrimaryAttributes() { Constitution = constitution, Strength = strength, Dexterity = dexterity, Intellect = intellect, WillPower = willpower };
        }
        public SecondaryAttributes CalculateSecondaryAttributes(int level) {
            int maxHealth = 0;
            int maxMana = 0;
            int awareness = 0;
            int armorRating = 1;
            int elementalResistance = 0;

            //Roll for secondary stats:
            for (int i = 0; i < 5; i++) {

                int roll = Program.Rand.Next(1, 100 + 1);

                if (i == 0 && roll <= -1) {
                    maxHealth = Program.Rand.Next(Math.Max(1, Program.CurrentPlayer.Level + level));
                }
                if (i == 1 && roll <= -1) {
                    maxMana = Program.Rand.Next(Math.Max(1, Program.CurrentPlayer.Level + level));
                }
                if (i == 2 && roll <= -1) {
                    awareness = Program.Rand.Next(Math.Max(1, Program.CurrentPlayer.Level + level));
                }
                if (i == 3 && roll <= 10) {
                    armorRating = Program.Rand.Next(Program.CurrentPlayer.Level / 2, 2 + Math.Max(3, Program.CurrentPlayer.Level + level));
                }
                if (i == 4 && roll <= -1) {
                    elementalResistance = Program.Rand.Next(Math.Max(1, Program.CurrentPlayer.Level + level));
                }
            }
            return new SecondaryAttributes() { MaxHealth = maxHealth, MaxMana = maxMana, Awareness = awareness, ArmorRating = armorRating, ElementalResistance = elementalResistance };
        }
        public override int CalculateItemPrice() {
            return Convert.ToInt32(
                ItemLevel * 30 + SecondaryAttributes.ArmorRating * 95 + (
                    Math.Pow(PrimaryAttributes.Strength, 1 / 1000) * 55 + 
                    Math.Pow(PrimaryAttributes.Dexterity, 1 / 1000) * 55 + 
                    Math.Pow(PrimaryAttributes.Intellect, 1 / 1000) * 55 + 
                    Math.Pow(PrimaryAttributes.Constitution, 1 / 1000) * 40 + 
                    Math.Pow(PrimaryAttributes.WillPower, 1 / 1000) * 40) 
                    * (PrimaryAttributes.Strength + PrimaryAttributes.Dexterity + PrimaryAttributes.Intellect + PrimaryAttributes.Constitution + PrimaryAttributes.WillPower)
             );
        }
        public string Equip() {
            if (ItemLevel > Program.CurrentPlayer.Level) {
                Console.WriteLine($"Character needs to be level {ItemLevel} to equip this item.");
                return "Armor not equipped.";
            } else if (Program.CurrentPlayer.CurrentClass == "Warrior" && ArmorType != ArmorType.Mail && ArmorType != ArmorType.Plate && ItemName != "Linen Rags" ||
                Program.CurrentPlayer.CurrentClass == "Archer" && ArmorType != ArmorType.Leather && ArmorType != ArmorType.Mail && ItemName != "Linen Rags" ||
                Program.CurrentPlayer.CurrentClass == "Mage" && ArmorType != ArmorType.Cloth && ArmorType != ArmorType.Leather) {
                Console.WriteLine($"Character can't equip a {ArmorType} armor.");
                return "Armor not equipped.";
            }
            if (Program.CurrentPlayer.Equipment.TryGetValue(ItemSlot, out ItemBase value)) {
                Console.WriteLine($"Do you want to switch '{value.ItemName}' for '{ItemName}'? (Y/N)");
                while (true) {
                    string input = TextInput.PlayerPrompt();
                    if (input == "y") {
                        ((IEquipable)value).UnEquip();
                        Program.CurrentPlayer.Equipment[ItemSlot] = this;                        
                        int a = Array.IndexOf(Program.CurrentPlayer.Inventory, this);
                        Program.CurrentPlayer.Inventory.SetValue(null, a);
                        Program.CurrentPlayer.CalculateTotalStats();
                        return "New armor piece equipped!";
                    } else if (input == "n") {
                        return "Armor not equipped.";
                    } else {
                        Console.WriteLine("Invalid input.");
                    }
                }
            } else {
                Program.CurrentPlayer.Equipment[ItemSlot] = this;
                int a = Array.IndexOf(Program.CurrentPlayer.Inventory, this);
                if (a == -1) {
                } else {
                    Program.CurrentPlayer.Inventory.SetValue(null, a);
                }
                Program.CurrentPlayer.CalculateTotalStats();
                return "New armor piece equipped!";
            }
        }
        public string UnEquip() {
            int index = Array.FindIndex(Program.CurrentPlayer.Inventory, i => i == null || Program.CurrentPlayer.Inventory.Length == 0);
            Program.CurrentPlayer.Inventory.SetValue(this, index);
            Program.CurrentPlayer.Equipment.Remove(ItemSlot);
            Program.CurrentPlayer.CalculateTotalStats();
            if (Program.CurrentPlayer.Health > Program.CurrentPlayer.TotalSecondaryAttributes.MaxHealth) {
                Program.CurrentPlayer.Health = Program.CurrentPlayer.TotalSecondaryAttributes.MaxHealth;
            }
            return "Armor piece unequipped!";
        }
    }
}
