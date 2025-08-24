using System;
using Saga.Character;

namespace Saga.Items.Loot
{
    public class ArmorLootTable : Armor
    {
        public static Item CreateRandomArmor(int level, int armorType = -1, int slot = -1) {
            if (slot == -1) slot = Program.Rand.Next(7);
            if (armorType == -1) armorType = Program.Rand.Next(4);

            int constitution = 0;
            int strength = 0;
            int dexterity = 0;
            int intellect = 0;
            int willpower = 0;
            int armorRating = 1;

            //Roll for stats:
            for (int i = 0; i<6; i++) {

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
                    armorRating = Program.Rand.Next(Program.CurrentPlayer.Level/2, 2 + Math.Max(3, Program.CurrentPlayer.Level + level));
                }
            }

            Item item = new Armor() {
                ItemLevel = Math.Max(1, Program.CurrentPlayer.Level + level),
                ItemSlot = (Slot)slot,
                ArmorType = (ArmorType)armorType,
                Attributes = new PrimaryAttributes() { Constitution = constitution, Strength = strength, Dexterity = dexterity, Intellect = intellect, WillPower = willpower },
                SecondaryAttributes = new SecondaryAttributes() { ArmorRating = armorRating },
                ItemName = RandomArmorName((ArmorType)armorType, (Slot)slot),
                ItemDescription = "A piece from Gheed's collection.\nYou start to wonder where he gets his items from...",
            };
            return item;
        }

        public readonly static Armor LinenRags = new() {
            ItemName = "Linen Rags",
            ItemLevel = 1,
            ItemSlot = Slot.Torso,
            ArmorType = ArmorType.Cloth,
            Attributes = new PrimaryAttributes() { Strength = 0, Dexterity = 0, Intellect = 0, Constitution = 0, WillPower = 0 },
            SecondaryAttributes =new SecondaryAttributes() { ArmorRating = 0 },
            ItemDescription = "Clothes that mark you a prisoner but without them, you'd be naked.",
        };
        public readonly static Armor RunedSimpleRobe = new() {
            ItemName = "Runed Simple Robe",
            ItemLevel = 2,
            ItemSlot = Slot.Torso,
            ArmorType = ArmorType.Cloth,
            Attributes = new PrimaryAttributes() { Strength = 1, Dexterity = 1, Intellect = 3, Constitution = 1, WillPower = 0 },
            SecondaryAttributes = new SecondaryAttributes() { ArmorRating = 4 }
        };
        public readonly static Armor EnchantedElegantRobe = new() {
            ItemName = "Enchanted Elegant Robe",
            ItemLevel = 5,
            ItemSlot = Slot.Torso,
            ArmorType = ArmorType.Cloth,
            Attributes = new PrimaryAttributes() { Strength = 1, Dexterity = 1, Intellect = 5, Constitution = 2, WillPower = 0 },
            SecondaryAttributes = new SecondaryAttributes() { ArmorRating = 8 }
        };
        public readonly static Armor ArcanistsRobe = new() {
            ItemName = "Arcanist's Robe",
            ItemLevel = 8,
            ItemSlot = Slot.Torso,
            ArmorType = ArmorType.Cloth,
            Attributes = new PrimaryAttributes() { Strength = 1, Dexterity = 1, Intellect = 7, Constitution = 3, WillPower = 0 },
            SecondaryAttributes = new SecondaryAttributes() { ArmorRating = 12 }
        };
        public readonly static Armor HideArmor = new() {
            ItemName = "Hide Armor",
            ItemLevel = 2,
            ItemSlot = Slot.Torso,
            ArmorType = ArmorType.Leather,
            Attributes = new PrimaryAttributes() { Strength = 1, Dexterity = 3, Intellect = 1, Constitution = 1, WillPower = 0 },
            SecondaryAttributes = new SecondaryAttributes() { ArmorRating = 4 }
        };
        public readonly static Armor HuntersCuirass = new() {
            ItemName = "Hunter's Cuirass",
            ItemLevel = 5,
            ItemSlot = Slot.Torso,
            ArmorType = ArmorType.Leather,
            Attributes = new PrimaryAttributes() { Strength = 1, Dexterity = 5, Intellect = 1, Constitution = 2, WillPower = 0 },
            SecondaryAttributes = new SecondaryAttributes() { ArmorRating = 8 }
        };
        public readonly static Armor MarksmansBrigadine = new() {
            ItemName = "Marksman's Brigadine",
            ItemLevel = 8,
            ItemSlot = Slot.Torso,
            ArmorType = ArmorType.Mail,
            Attributes = new PrimaryAttributes() { Strength = 1, Dexterity = 7, Intellect = 1, Constitution = 3, WillPower = 0 },
            SecondaryAttributes = new SecondaryAttributes() { ArmorRating = 12 }
        };
        public readonly static Armor SteelMailShirt = new() {
            ItemName = "Steel Mail Shirt",
            ItemLevel = 2,
            ItemSlot = Slot.Torso,
            ArmorType = ArmorType.Mail,
            Attributes = new PrimaryAttributes() { Strength = 3, Dexterity = 1, Intellect = 1, Constitution = 1, WillPower = 0 },
            SecondaryAttributes = new SecondaryAttributes() { ArmorRating = 4 }
        };
        public readonly static Armor SteelBreastplate = new() {
            ItemName = "Steel Breastplate",
            ItemLevel = 5,
            ItemSlot = Slot.Torso,
            ArmorType = ArmorType.Plate,
            Attributes = new PrimaryAttributes() { Strength = 5, Dexterity = 1, Intellect = 1, Constitution = 2, WillPower = 0 },
            SecondaryAttributes = new SecondaryAttributes() { ArmorRating = 8 }
        };
        public readonly static Armor KnightsPlateArmor = new() {
            ItemName = "Knight's Plate Armor",
            ItemLevel = 8,
            ItemSlot = Slot.Torso,
            ArmorType = ArmorType.Plate,
            Attributes = new PrimaryAttributes() { Strength = 7, Dexterity = 1, Intellect = 1, Constitution = 3, WillPower = 0 },
            SecondaryAttributes = new SecondaryAttributes() { ArmorRating = 12 }
        };
    }
}
