using Saga.Character;
using System;

namespace Saga.Items
{
    //Lists all available armor types
    public enum ArmorType
    {
        Cloth,
        Leather,
        Mail,
        Plate
    }
    [Serializable]
    public class Armor : Item
    {
        public ArmorType ArmorType { get; set; }
        public PrimaryAttributes Attributes { get; set; }
        public SecondaryAttributes SecondaryAttributes { get; set; }

        public override int CalculateItemPrice() {
            ItemPrice = Convert.ToInt32(ItemLevel*30+SecondaryAttributes.ArmorRating*95+(Math.Pow(Attributes.Strength,1/1000)*55+Math.Pow(Attributes.Dexterity,1/1000)*55+ Math.Pow(Attributes.Intellect,1/1000)*55+ Math.Pow(Attributes.Constitution,1/1000)*40+ Math.Pow(Attributes.WillPower,1/1000)*40) *(Attributes.Strength+Attributes.Dexterity+Attributes.Intellect+Attributes.Constitution+Attributes.WillPower));
            return ItemPrice;
        }
        public static string RandomArmorName(ArmorType type, Slot slot) {
            string name = "Fine Hat";
            int rand = Program.rand.Next(5);
            switch (type) {
                case ArmorType.Cloth:
                    name = ArmorNameList(type, slot, rand);
                    break;
                case ArmorType.Leather:
                    name = ArmorNameList(type, slot, rand);
                    break;
                case ArmorType.Mail:
                    name = ArmorNameList(type, slot, rand);
                    break;
                case ArmorType.Plate:
                    name = ArmorNameList(type, slot, rand);
                    break;           
            }
            return name;
        }
        public static string ArmorNameList(ArmorType type, Slot slot, int rand) {
            string name = "";
            switch (type) {
                default:
                case ArmorType.Cloth:
                    switch (slot) {
                        default:
                        case Slot.Headgear:
                            switch (rand) {
                                case 0:
                                    name = "Handsewn Hat";
                                    break;
                                case 1:
                                    name = "Wool Cap";
                                    break;
                                case 2:
                                    name = "Runed Hood";
                                    break;
                                case 3:
                                    name = "Pointy Hat";
                                    break;
                                case 4:
                                    name = "Runed Cap";
                                    break;
                            }
                            return name;
                        case Slot.Torso:
                            switch (rand) {
                                case 0:
                                    name = "Simple Robe";
                                    break;
                                case 1:
                                    name = "Elegant Robe";
                                    break;
                                case 2:
                                    name = "Mage Robe";
                                    break;
                                case 3:
                                    name = "Scholar Robe";
                                    break;
                                case 4:
                                    name = "Satin Robe";
                                    break;
                            }
                            return name;
                        case Slot.Legs:
                            switch (rand) {
                                case 0:
                                    name = "Woolen Trousers";
                                    break;
                                case 1:
                                    name = "Linen Breeches";
                                    break;
                                case 2:
                                    name = "Pantaloons";
                                    break;
                                case 3:
                                    name = "Runebound Leggings";
                                    break;
                                case 4:
                                    name = "Patterned Kilt";
                                    break;
                            }
                            return name;
                        case Slot.Feet:
                            switch (rand) {
                                case 0:
                                    name = "Slippers";
                                    break;
                                case 1:
                                    name = "Sandals";
                                    break;
                                case 2:
                                    name = "Shoes";
                                    break;
                                case 3:
                                    name = "Socks";
                                    break;
                                case 4:
                                    name = "Footwraps";
                                    break;
                            }
                            return name;
                        case Slot.Bracers:
                            switch (rand) {
                                case 0:
                                    name = "Runecloth";
                                    break;
                                case 1:
                                    name = "Armbands";
                                    break;
                                case 2:
                                    name = "Bracelets";
                                    break;
                                case 3:
                                    name = "Wraps";
                                    break;
                                case 4:
                                    name = "Runed Cuffs";
                                    break;
                            }
                            return name;
                        case Slot.Shoulders:
                            switch (rand) {
                                case 0:
                                    name = "Shoulderpads";
                                    break;
                                case 1:
                                    name = "Mantle";
                                    break;
                                case 2:
                                    name = "Spaulders";
                                    break;
                                case 3:
                                    name = "Amice";
                                    break;
                                case 4:
                                    name = "Shawl";
                                    break;
                            }
                            return name;
                        case Slot.Belt:
                            switch (rand) {
                                case 0:
                                    name = "Light Belt";
                                    break;
                                case 1:
                                    name = "Linen Strap";
                                    break;
                                case 2:
                                    name = "Elvish Robe";
                                    break;
                                case 3:
                                    name = "Sash";
                                    break;
                                case 4:
                                    name = "Cord";
                                    break;
                            }
                            return name;
                        case Slot.Cape:
                            switch (rand) {
                                case 0:
                                    name = "";
                                    break;
                                case 1:
                                    name = "";
                                    break;
                                case 2:
                                    name = "";
                                    break;
                                case 3:
                                    name = "";
                                    break;
                                case 4:
                                    name = "";
                                    break;
                            }
                            return name;
                        case Slot.Gloves:
                            switch (rand) {
                                case 0:
                                    name = "";
                                    break;
                                case 1:
                                    name = "";
                                    break;
                                case 2:
                                    name = "";
                                    break;
                                case 3:
                                    name = "";
                                    break;
                                case 4:
                                    name = "";
                                    break;
                            }
                            return name;
                    }
                case ArmorType.Leather:
                    switch (slot) {
                        default:
                        case Slot.Headgear:
                            switch (rand) {
                                case 0:
                                    name = "Leather Helm";
                                    break;
                                case 1:
                                    name = "Leather Cap";
                                    break;
                                case 2:
                                    name = "Leather Cowl";
                                    break;
                                case 3:
                                    name = "Hide Helmet";
                                    break;
                                case 4:
                                    name = "Pelt Cap";
                                    break;
                            }
                            return name;
                        case Slot.Torso:
                            switch (rand) {
                                case 0:
                                    name = "Chestguard";
                                    break;
                                case 1:
                                    name = "Tunic";
                                    break;
                                case 2:
                                    name = "Jerkin";
                                    break;
                                case 3:
                                    name = "Cuirass";
                                    break;
                                case 4:
                                    name = "Brigadine";
                                    break;
                            }
                            return name;
                        case Slot.Legs:
                            switch (rand) {
                                case 0:
                                    name = "Trousers";
                                    break;
                                case 1:
                                    name = "Breeches";
                                    break;
                                case 2:
                                    name = "Hide Leggings";
                                    break;
                                case 3:
                                    name = "Legguards";
                                    break;
                                case 4:
                                    name = "Britches";
                                    break;
                            }
                            return name;
                        case Slot.Feet:
                            switch (rand) {
                                case 0:
                                    name = "Boots";
                                    break;
                                case 1:
                                    name = "Leather Sandals";
                                    break;
                                case 2:
                                    name = "Hide Shoes";
                                    break;
                                case 3:
                                    name = "Treads";
                                    break;
                                case 4:
                                    name = "Striders";
                                    break;
                            }
                            return name;
                        case Slot.Bracers:
                            switch (rand) {
                                case 0:
                                    name = "Wristguards";
                                    break;
                                case 1:
                                    name = "Bindings";
                                    break;
                                case 2:
                                    name = "Vambraces";
                                    break;
                                case 3:
                                    name = "Hide Bracers";
                                    break;
                                case 4:
                                    name = "Hide Cuffs";
                                    break;
                            }
                            return name;
                        case Slot.Shoulders:
                            switch (rand) {
                                case 0:
                                    name = "Hide Shoulderpads";
                                    break;
                                case 1:
                                    name = "Pelt Mantle";
                                    break;
                                case 2:
                                    name = "Pelt Spaulders";
                                    break;
                                case 3:
                                    name = "Hide Mantle";
                                    break;
                                case 4:
                                    name = "Leather Mantle";
                                    break;
                            }
                            return name;
                        case Slot.Belt:
                            switch (rand) {
                                case 0:
                                    name = "Light Waistband";
                                    break;
                                case 1:
                                    name = "Leather Strap";
                                    break;
                                case 2:
                                    name = "Light WaistGuard";
                                    break;
                                case 3:
                                    name = "Leather Sash";
                                    break;
                                case 4:
                                    name = "Leather Cord";
                                    break;
                            }
                            return name;
                        case Slot.Cape:
                            switch (rand) {
                                case 0:
                                    name = "";
                                    break;
                                case 1:
                                    name = "";
                                    break;
                                case 2:
                                    name = "";
                                    break;
                                case 3:
                                    name = "";
                                    break;
                                case 4:
                                    name = "";
                                    break;
                            }
                            return name;
                        case Slot.Gloves:
                            switch (rand) {
                                case 0:
                                    name = "";
                                    break;
                                case 1:
                                    name = "";
                                    break;
                                case 2:
                                    name = "";
                                    break;
                                case 3:
                                    name = "";
                                    break;
                                case 4:
                                    name = "";
                                    break;
                            }
                            return name;
                    }
                case ArmorType.Mail:
                    switch (slot) {
                        default:
                        case Slot.Headgear:
                            switch (rand) {
                                case 0:
                                    name = "Mailcoif";
                                    break;
                                case 1:
                                    name = "Helm";
                                    break;
                                case 2:
                                    name = "Kettle Hat";
                                    break;
                                case 3:
                                    name = "Skullcap";
                                    break;
                                case 4:
                                    name = "Aventail";
                                    break;
                            }
                            return name;
                        case Slot.Torso:
                            switch (rand) {
                                case 0:
                                    name = "Mail Shirt";
                                    break;
                                case 1:
                                    name = "Chain Mail";
                                    break;
                                case 2:
                                    name = "Hauberk";
                                    break;
                                case 3:
                                    name = "Scale Mail";
                                    break;
                                case 4:
                                    name = "Ring Mail";
                                    break;
                            }
                            return name;
                        case Slot.Legs:
                            switch (rand) {
                                case 0:
                                    name = "Mail Trousers";
                                    break;
                                case 1:
                                    name = "Greaves";
                                    break;
                                case 2:
                                    name = "Chain Leggings";
                                    break;
                                case 3:
                                    name = "Tassets";
                                    break;
                                case 4:
                                    name = "Chausses";
                                    break;
                            }
                            return name;
                        case Slot.Feet:
                            switch (rand) {
                                case 0:
                                    name = "Heavy Boots";
                                    break;
                                case 1:
                                    name = "Chain Boots";
                                    break;
                                case 2:
                                    name = "Mail Shoes";
                                    break;
                                case 3:
                                    name = "Chain Treads";
                                    break;
                                case 4:
                                    name = "Heavy Treads";
                                    break;
                            }
                            return name;
                        case Slot.Bracers:
                            switch (rand) {
                                case 0:
                                    name = "Chain Armguards";
                                    break;
                                case 1:
                                    name = "Mail Wraps";
                                    break;
                                case 2:
                                    name = "Heavy Armbands";
                                    break;
                                case 3:
                                    name = "Mail Armguards";
                                    break;
                                case 4:
                                    name = "Chain Wraps";
                                    break;
                            }
                            return name;
                        case Slot.Shoulders:
                            switch (rand) {
                                case 0:
                                    name = "Soldier's Mantle";
                                    break;
                                case 1:
                                    name = "Mail Pauldrons";
                                    break;
                                case 2:
                                    name = "Chain Mantle";
                                    break;
                                case 3:
                                    name = "Soldier's Pauldrons";
                                    break;
                                case 4:
                                    name = "Heavy Shoulderpads";
                                    break;
                            }
                            return name;
                        case Slot.Belt:
                            switch (rand) {
                                case 0:
                                    name = "Heavy Belt";
                                    break;
                                case 1:
                                    name = "Heavy Waistband";
                                    break;
                                case 2:
                                    name = "Mail Cord";
                                    break;
                                case 3:
                                    name = "Girdle";
                                    break;
                                case 4:
                                    name = "Clasp";
                                    break;
                            }
                            return name;
                        case Slot.Cape:
                            switch (rand) {
                                case 0:
                                    name = "";
                                    break;
                                case 1:
                                    name = "";
                                    break;
                                case 2:
                                    name = "";
                                    break;
                                case 3:
                                    name = "";
                                    break;
                                case 4:
                                    name = "";
                                    break;
                            }
                            return name;
                        case Slot.Gloves:
                            switch (rand) {
                                case 0:
                                    name = "";
                                    break;
                                case 1:
                                    name = "";
                                    break;
                                case 2:
                                    name = "";
                                    break;
                                case 3:
                                    name = "";
                                    break;
                                case 4:
                                    name = "";
                                    break;
                            }
                            return name;
                    }
                case ArmorType.Plate:
                    switch (slot) {
                        default:
                        case Slot.Headgear:
                            switch (rand) {
                                case 0:
                                    name = "Full Helm";
                                    break;
                                case 1:
                                    name = "Sallet Helm";
                                    break;
                                case 2:
                                    name = "Great Helm";
                                    break;
                                case 3:
                                    name = "Bascinet Helmet";
                                    break;
                                case 4:
                                    name = "Barbute Helmet";
                                    break;
                            }
                            return name;
                        case Slot.Torso:
                            switch (rand) {
                                case 0:
                                    name = "Breastplate";
                                    break;
                                case 1:
                                    name = "Chestpiece";
                                    break;
                                case 2:
                                    name = "Vanguard";
                                    break;
                                case 3:
                                    name = "Plate Armor";
                                    break;
                                case 4:
                                    name = "Battleplate";
                                    break;
                            }
                            return name;
                        case Slot.Legs:
                            switch (rand) {
                                case 0:
                                    name = "Plate Greaves";
                                    break;
                                case 1:
                                    name = "Heavy Chausses";
                                    break;
                                case 2:
                                    name = "Plate Pantaloons";
                                    break;
                                case 3:
                                    name = "Plate Leggings";
                                    break;
                                case 4:
                                    name = "Heavy Tassets";
                                    break;
                            }
                            return name;
                        case Slot.Feet:
                            switch (rand) {
                                case 0:
                                    name = "Sabatons";
                                    break;
                                case 1:
                                    name = "Plate Boots";
                                    break;
                                case 2:
                                    name = "Plate Shoes";
                                    break;
                                case 3:
                                    name = "Solleret";
                                    break;
                                case 4:
                                    name = "Plate Treads";
                                    break;
                            }
                            return name;
                        case Slot.Bracers:
                            switch (rand) {
                                case 0:
                                    name = "Heavy Vambraces";
                                    break;
                                case 1:
                                    name = "Plate Armbands";
                                    break;
                                case 2:
                                    name = "Armplates";
                                    break;
                                case 3:
                                    name = "Wristplates";
                                    break;
                                case 4:
                                    name = "Plate Bracers";
                                    break;
                            }
                            return name;
                        case Slot.Shoulders:
                            switch (rand) {
                                case 0:
                                    name = "Plate Shoulderpads";
                                    break;
                                case 1:
                                    name = "Knight's Mantle";
                                    break;
                                case 2:
                                    name = "Plate Spaulders";
                                    break;
                                case 3:
                                    name = "Knigth's Spaulders";
                                    break;
                                case 4:
                                    name = "Pauldron";
                                    break;
                            }
                            return name;
                        case Slot.Belt:
                            switch (rand) {
                                case 0:
                                    name = "Plate Girdle";
                                    break;
                                case 1:
                                    name = "Plate Clasp";
                                    break;
                                case 2:
                                    name = "Faulds";
                                    break;
                                case 3:
                                    name = "Waistplate";
                                    break;
                                case 4:
                                    name = "Heavy Cord";
                                    break;
                            }
                            return name;
                        case Slot.Cape:
                            switch (rand) {
                                case 0:
                                    name = "";
                                    break;
                                case 1:
                                    name = "";
                                    break;
                                case 2:
                                    name = "";
                                    break;
                                case 3:
                                    name = "";
                                    break;
                                case 4:
                                    name = "";
                                    break;
                            }
                            return name;
                        case Slot.Gloves:
                            switch (rand) {
                                case 0:
                                    name = "";
                                    break;
                                case 1:
                                    name = "";
                                    break;
                                case 2:
                                    name = "";
                                    break;
                                case 3:
                                    name = "";
                                    break;
                                case 4:
                                    name = "";
                                    break;
                            }
                            return name;
                    }
            }
        }
    }
}
