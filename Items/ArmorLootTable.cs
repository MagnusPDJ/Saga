using Saga.Character;
using System;

namespace Saga.Items
{
    [Serializable]
    public class ArmorLootTable : Armor
    {
        public static Item CreateRandomArmor(int level) {
            Item item;
            int slot = Program.rand.Next(7);
            int armorType = Program.rand.Next(4);
            int constitution = Program.rand.Next(Math.Max(1, Program.CurrentPlayer.Level + level));
            int strength = Program.rand.Next(Math.Max(1, Program.CurrentPlayer.Level + level));
            int dexterity = Program.rand.Next(Math.Max(1, Program.CurrentPlayer.Level + level));
            int intellect = Program.rand.Next(Math.Max(1, Program.CurrentPlayer.Level + level));
            int willpower = Program.rand.Next(Math.Max(1, Program.CurrentPlayer.Level + level));
            int armorRating = Program.rand.Next(1+Program.CurrentPlayer.Level,2+ Math.Max(1, Program.CurrentPlayer.Level + level));

            item = new Armor() {
                ItemLevel = Math.Max(1, Program.CurrentPlayer.Level + level),
                ItemSlot = (Slot)slot,
                ArmorType = (ArmorType)armorType,
                Attributes = new PrimaryAttributes() { Constitution = constitution, Strength = strength, Dexterity = dexterity, Intellect = intellect, WillPower = willpower },
                SecondaryAttributes = new SecondaryAttributes() { ArmorRating = armorRating },
                ItemName = RandomArmorName((ArmorType)armorType, (Slot)slot)
            };
            return item;
        }
        public static Item Empty = new Armor();
        public static Armor LinenRags = new Armor() {
            ItemName = "Linen Rags",
            ItemLevel = 1,
            ItemSlot = Slot.Torso,
            ArmorType = ArmorType.Leather,
            Attributes = new PrimaryAttributes() { Strength = 0, Dexterity = 0, Intellect = 0, Constitution = 0, WillPower = 0 },
            SecondaryAttributes =new SecondaryAttributes() { ArmorRating = 0 }
        };
        public static Armor RunedSimpleRobe = new Armor() {
            ItemName = "Runed Simple Robe",
            ItemLevel = 2,
            ItemSlot = Slot.Torso,
            ArmorType = ArmorType.Cloth,
            Attributes = new PrimaryAttributes() { Strength = 1, Dexterity = 1, Intellect = 3, Constitution = 1, WillPower = 0 },
            SecondaryAttributes = new SecondaryAttributes() { ArmorRating = 4 }
        };
        public static Armor EnchantedElegantRobe = new Armor() {
            ItemName = "Enchanted Elegant Robe",
            ItemLevel = 5,
            ItemSlot = Slot.Torso,
            ArmorType = ArmorType.Cloth,
            Attributes = new PrimaryAttributes() { Strength = 1, Dexterity = 1, Intellect = 5, Constitution = 2, WillPower = 0 },
            SecondaryAttributes = new SecondaryAttributes() { ArmorRating = 8 }
        };
        public static Armor ArcanistsRobe = new Armor() {
            ItemName = "Arcanist's Robe",
            ItemLevel = 8,
            ItemSlot = Slot.Torso,
            ArmorType = ArmorType.Cloth,
            Attributes = new PrimaryAttributes() { Strength = 1, Dexterity = 1, Intellect = 7, Constitution = 3, WillPower = 0 },
            SecondaryAttributes = new SecondaryAttributes() { ArmorRating = 12 }
        };
        public static Armor HideArmor = new Armor() {
            ItemName = "Hide Armor",
            ItemLevel = 2,
            ItemSlot = Slot.Torso,
            ArmorType = ArmorType.Leather,
            Attributes = new PrimaryAttributes() { Strength = 1, Dexterity = 3, Intellect = 1, Constitution = 1, WillPower = 0 },
            SecondaryAttributes = new SecondaryAttributes() { ArmorRating = 4 }
        };
        public static Armor HuntersCuirass = new Armor() {
            ItemName = "Hunter's Cuirass",
            ItemLevel = 5,
            ItemSlot = Slot.Torso,
            ArmorType = ArmorType.Leather,
            Attributes = new PrimaryAttributes() { Strength = 1, Dexterity = 5, Intellect = 1, Constitution = 2, WillPower = 0 },
            SecondaryAttributes = new SecondaryAttributes() { ArmorRating = 8 }
        };
        public static Armor MarksmansBrigadine = new Armor() {
            ItemName = "Marksman's Brigadine",
            ItemLevel = 8,
            ItemSlot = Slot.Torso,
            ArmorType = ArmorType.Mail,
            Attributes = new PrimaryAttributes() { Strength = 1, Dexterity = 7, Intellect = 1, Constitution = 3, WillPower = 0 },
            SecondaryAttributes = new SecondaryAttributes() { ArmorRating = 12 }
        };
        public static Armor SteelMailShirt = new Armor() {
            ItemName = "Steel Mail Shirt",
            ItemLevel = 2,
            ItemSlot = Slot.Torso,
            ArmorType = ArmorType.Mail,
            Attributes = new PrimaryAttributes() { Strength = 3, Dexterity = 1, Intellect = 1, Constitution = 1, WillPower = 0 },
            SecondaryAttributes = new SecondaryAttributes() { ArmorRating = 4 }
        };
        public static Armor SteelBreastplate = new Armor() {
            ItemName = "Steel Breastplate",
            ItemLevel = 5,
            ItemSlot = Slot.Torso,
            ArmorType = ArmorType.Plate,
            Attributes = new PrimaryAttributes() { Strength = 5, Dexterity = 1, Intellect = 1, Constitution = 2, WillPower = 0 },
            SecondaryAttributes = new SecondaryAttributes() { ArmorRating = 8 }
        };
        public static Armor KnightsPlateArmor = new Armor() {
            ItemName = "Knight's Plate Armor",
            ItemLevel = 8,
            ItemSlot = Slot.Torso,
            ArmorType = ArmorType.Plate,
            Attributes = new PrimaryAttributes() { Strength = 7, Dexterity = 1, Intellect = 1, Constitution = 3, WillPower = 0 },
            SecondaryAttributes = new SecondaryAttributes() { ArmorRating = 12 }
        };
    }
}
