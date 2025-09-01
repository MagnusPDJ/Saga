using Saga.Assets;
using Saga.Character;

namespace Saga.Items
{
    [Discriminator("armorBase")]
    public class ArmorBase : IArmor
    {
        public string ItemName { get; set; } = string.Empty;
        public int ItemLevel { get; set; }
        public int ItemPrice { get; set; }
        public string ItemDescription { get; init; } = string.Empty;
        public ArmorType ArmorType { get; set; }
        public Slot ItemSlot { get; set; }
        public PrimaryAffixes PrimaryAffixes { get; set; } = new PrimaryAffixes();
        public SecondaryAffixes SecondaryAffixes { get; set; } = new SecondaryAffixes();

        public void SetPrimaryAffixes() => PrimaryAffixes = CalculatePrimaryAffixes(ItemLevel);
        public void SetSecondaryAffixes() => SecondaryAffixes = CalculateSecondaryAffixes(ItemLevel);
        public PrimaryAffixes CalculatePrimaryAffixes(int level) {
            int strength = 0;
            int dexterity = 0;
            int intellect = 0;
            int constitution = 0;
            int awareness = 0;
            int willpower = 0;
            
            //Roll for primary stats:
            for (int i = 0; i < 6; i++) {

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
                if (i == 5 && roll <= 10) {
                    awareness = Program.Rand.Next(Math.Max(1, Program.CurrentPlayer.Level + level));
                }

            }
            return new PrimaryAffixes() { Strength = strength, Dexterity = dexterity, Intellect = intellect, Constitution = constitution, Awareness = awareness , WillPower = willpower };
        }
        public SecondaryAffixes CalculateSecondaryAffixes(int level) {
            int maxHealth = 0;
            int maxMana = 0;
            int armorRating = 1;
            int elementalResistance = 0;
            int magicalResistance = 0;

            //Roll for secondary stats:
            for (int i = 0; i < 4; i++) {

                int roll = Program.Rand.Next(1, 100 + 1);

                if (i == 0 && roll <= -1) {
                    maxHealth = Program.Rand.Next(Math.Max(1, Program.CurrentPlayer.Level + level));
                }
                if (i == 1 && roll <= -1) {
                    maxMana = Program.Rand.Next(Math.Max(1, Program.CurrentPlayer.Level + level));
                }
                if (i == 2 && roll <= 10) {
                    armorRating = Program.Rand.Next(Program.CurrentPlayer.Level / 2, 2 + Math.Max(3, Program.CurrentPlayer.Level + level));
                }
                if (i == 3 && roll <= -1) {
                    elementalResistance = Program.Rand.Next(Math.Max(1, Program.CurrentPlayer.Level + level));
                }
            }
            return new SecondaryAffixes() { MaxHealth = maxHealth, MaxMana = maxMana, ArmorRating = armorRating, ElementalResistance = elementalResistance, MagicalResistance = magicalResistance };
        }
        public int CalculateItemPrice() {
            return Convert.ToInt32(
                ItemLevel * 30 + SecondaryAffixes.ArmorRating * 95 + (
                    Math.Pow(PrimaryAffixes.Strength, 1 / 1000) * 55 + 
                    Math.Pow(PrimaryAffixes.Dexterity, 1 / 1000) * 55 + 
                    Math.Pow(PrimaryAffixes.Intellect, 1 / 1000) * 55 + 
                    Math.Pow(PrimaryAffixes.Constitution, 1 / 1000) * 40 + 
                    Math.Pow(PrimaryAffixes.WillPower, 1 / 1000) * 40) 
                    * (PrimaryAffixes.Strength + PrimaryAffixes.Dexterity + PrimaryAffixes.Intellect + PrimaryAffixes.Constitution + PrimaryAffixes.WillPower)
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
            if (Program.CurrentPlayer.Equipment.TryGetSlot(ItemSlot, out IEquipable? value)) {
                Console.WriteLine($"Do you want to switch '{value.ItemName}' for '{ItemName}'? (Y/N)");
                while (true) {
                    string input = TextInput.PlayerPrompt();
                    if (input == "y") {
                        value.UnEquip();
                        Program.CurrentPlayer.Equipment.SetSlot(ItemSlot, this);                        
                        int a = Array.IndexOf(Program.CurrentPlayer.Inventory, this);
                        Program.CurrentPlayer.Inventory.SetValue(null, a);
                        return "New armor piece equipped!";
                    } else if (input == "n") {
                        return "Armor not equipped.";
                    } else {
                        Console.WriteLine("Invalid input.");
                    }
                }
            } else {
                Program.CurrentPlayer.Equipment.SetSlot(ItemSlot, this);
                int a = Array.IndexOf(Program.CurrentPlayer.Inventory, this);
                if (a == -1) {
                } else {
                    Program.CurrentPlayer.Inventory.SetValue(null, a);
                }
                return "New armor piece equipped!";
            }
        }
        public string UnEquip() {
            int index = Array.FindIndex(Program.CurrentPlayer.Inventory, i => i == null || Program.CurrentPlayer.Inventory.Length == 0);
            Program.CurrentPlayer.Inventory.SetValue(this, index);
            Program.CurrentPlayer.Equipment.SetSlot(ItemSlot, null);
            if (Program.CurrentPlayer.Health > Program.CurrentPlayer.DerivedStats.MaxHealth) {
                Program.CurrentPlayer.SetHealth(Program.CurrentPlayer.DerivedStats.MaxHealth);
            }
            if (Program.CurrentPlayer.Mana > Program.CurrentPlayer.DerivedStats.MaxMana) {
                Program.CurrentPlayer.SetMana(Program.CurrentPlayer.DerivedStats.MaxMana);
            }
            return "Armor piece unequipped!";
        }
    }
}
