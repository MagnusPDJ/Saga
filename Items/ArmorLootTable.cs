using Saga.Character;
using System;

namespace Saga.Items
{
    [Serializable]
    public class ArmorLootTable : Armor
    {
        public static Armor LinenRags = new Armor() {
            ItemName = "Linen Rags",
            ItemLevel = 1,
            ItemSlot = Slot.SLOT_BODY,
            ArmorType = ArmorType.ARMOR_LEATHER,
            Attributes = new PrimaryAttributes() { Strength = 0, Dexterity = 0, Intellect = 0, Constitution = 0, WillPower = 0 }
        };
        public static Armor SimpleRobe = new Armor() {
            ItemName = "Simple Robe",
            ItemLevel = 1,
            ItemSlot = Slot.SLOT_BODY,
            ArmorType = ArmorType.ARMOR_CLOTH,
            Attributes = new PrimaryAttributes() { Strength = 1, Dexterity = 1, Intellect = 3, Constitution = 1, WillPower = 0 }
        };
        public static Armor ElegantRobe = new Armor() {
            ItemName = "Elegant Robe",
            ItemLevel = 1,
            ItemSlot = Slot.SLOT_BODY,
            ArmorType = ArmorType.ARMOR_CLOTH,
            Attributes = new PrimaryAttributes() { Strength = 1, Dexterity = 1, Intellect = 5, Constitution = 2, WillPower = 0 }
        };
        public static Armor ArcanistsRobe = new Armor() {
            ItemName = "Arcanist's Robe",
            ItemLevel = 1,
            ItemSlot = Slot.SLOT_BODY,
            ArmorType = ArmorType.ARMOR_CLOTH,
            Attributes = new PrimaryAttributes() { Strength = 1, Dexterity = 1, Intellect = 7, Constitution = 3, WillPower = 0 }
        };
        public static Armor HideArmor = new Armor() {
            ItemName = "Hide Armor",
            ItemLevel = 1,
            ItemSlot = Slot.SLOT_BODY,
            ArmorType = ArmorType.ARMOR_LEATHER,
            Attributes = new PrimaryAttributes() { Strength = 1, Dexterity = 3, Intellect = 1, Constitution = 1, WillPower = 0 }
        };
        public static Armor LeatherCuirass = new Armor() {
            ItemName = "Leather Cuirass",
            ItemLevel = 1,
            ItemSlot = Slot.SLOT_BODY,
            ArmorType = ArmorType.ARMOR_LEATHER,
            Attributes = new PrimaryAttributes() { Strength = 1, Dexterity = 5, Intellect = 1, Constitution = 2, WillPower = 0 }
        };
        public static Armor MarksmansBrigadine = new Armor() {
            ItemName = "Marksman's Brigadine",
            ItemLevel = 1,
            ItemSlot = Slot.SLOT_BODY,
            ArmorType = ArmorType.ARMOR_MAIL,
            Attributes = new PrimaryAttributes() { Strength = 1, Dexterity = 7, Intellect = 1, Constitution = 3, WillPower = 0 }
        };
        public static Armor MailShirt = new Armor() {
            ItemName = "Mail Shirt",
            ItemLevel = 1,
            ItemSlot = Slot.SLOT_BODY,
            ArmorType = ArmorType.ARMOR_MAIL,
            Attributes = new PrimaryAttributes() { Strength = 3, Dexterity = 1, Intellect = 1, Constitution = 1, WillPower = 0 }
        };
        public static Armor Breastplate = new Armor() {
            ItemName = "Breastplate",
            ItemLevel = 1,
            ItemSlot = Slot.SLOT_BODY,
            ArmorType = ArmorType.ARMOR_PLATE,
            Attributes = new PrimaryAttributes() { Strength = 5, Dexterity = 1, Intellect = 1, Constitution = 2, WillPower = 0 }
        };
        public static Armor KnightsPlateArmor = new Armor() {
            ItemName = "Knight's Plate Armor",
            ItemLevel = 1,
            ItemSlot = Slot.SLOT_BODY,
            ArmorType = ArmorType.ARMOR_PLATE,
            Attributes = new PrimaryAttributes() { Strength = 7, Dexterity = 1, Intellect = 1, Constitution = 3, WillPower = 0 }
        };
    }
}
