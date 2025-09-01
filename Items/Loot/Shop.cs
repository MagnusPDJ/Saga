using Saga.Assets;
using Saga.Character;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;

namespace Saga.Items.Loot
{
    public class Shop
    {
        public List<IEquipable> Forsale { get; set; } = [];

        //Metode til at genere ny shop ved tilbagekomst til camp
        public static Shop SetForsale() {
            var items = new List<IEquipable> {
                CreateRandomArmor(-3),
                CreateRandomArmor(-2),
                CreateRandomArmor(-1),
                CreateRandomWeapon(-3),
                CreateRandomWeapon(-2),
                CreateRandomWeapon(-1)
            };

            foreach (var item in items) {
                if (item is IWeapon weapon) {
                    weapon.SetWeaponAttributes();
                }
                if (item is IArmor armor) {
                    armor.SetPrimaryAttributes();
                    armor.SetSecondaryAttributes();
                }
                item.ItemPrice = item.CalculateItemPrice();
            }

            Shop shop = new() {
                Forsale = items
            };
            return shop;
        }
        public List<IEquipable> GetForsale() {
            return Forsale;
        }

        //Metode til at kalde og Loade shoppen.
        public static void Loadshop(Player p, Shop shop) {
            Program.SoundController.Play("shop");
            Runshop(p, shop);
            Program.SoundController.Stop();
        }
        //Metode til at køre shoppen.
        public static void Runshop(Player p, Shop shop) {
            bool sell = false;
            bool buy = true;
            while (buy) {
                HUDTools.BuyShopHUD(shop);
                //Wait for input
                string input = TextInput.PlayerPrompt();
                if (input == "u" || input == "use" || input == "heal") {
                    (Program.CurrentPlayer.Equipment.Potion as IConsumable)?.Consume();
                    TextInput.PressToContinue();
                } else if (input == "c" || input == "character" || input == "character screen") {
                    HUDTools.CharacterScreen();
                    TextInput.PressToContinue();
                } else if (input == "s" || input == "switch" || input == "sell") {
                    sell = true;
                } else if (input == "i" || input == "inventory") {
                    while (true) {
                        HUDTools.InventoryScreen();
                        string input2 = TextInput.PlayerPrompt(false);
                        if (input2 == "back") {
                            break;
                        }
                    }
                } else if (input == "e" || input == "exit") {
                    break;
                } else if (input == "q" || input == "questlog") {
                    HUDTools.QuestLogHUD();
                    TextInput.PressToContinue();
                } else if (input.Any(c => char.IsNumber(c)) && 0 < int.Parse(input) && int.Parse(input) <= shop.Forsale.Count) {
                    HUDTools.Print($"You sure you want to buy item # {input}? (Y/N)", 4);
                    string input2 = TextInput.PlayerPrompt();
                    if (input2 == "y") {
                        TryBuyItem(int.Parse(input) - 1, shop.Forsale[int.Parse(input) - 1].CalculateItemPrice(), shop ,p);
                        TextInput.PressToContinue();
                    }
                }
                while (sell) {
                    HUDTools.SellShopHUD();
                    string input1 = TextInput.PlayerPrompt();
                    if (input1 == "s" || input1 == "switch" || input1 == "buy") {
                        sell = false;
                    } else if (input1 == "e" || input1 == "exit") {
                        buy = false;
                        break;
                    } else if (input1 == "p" || input1 == "potion" || input1 == "sell potion") {
                        TrySellPotion("potion", ShopPrice("sellpotion"), p);
                    } else if (input1 == "f" || input1 == "5x" || input1 == "sell 5" || input1 == "sell 5x" || input1 == "sell 5xpotions") {
                        TrySellPotion("5x potion", ShopPrice("sellpotion5"), p);
                    } else if (input1 == "i" || input1 == "inventory") {
                        while (true) {
                            HUDTools.InventoryScreen();
                            string input2 = TextInput.PlayerPrompt(false);
                            if (input2 == "back") {
                                break;
                            }
                        }
                    } else if (input1 == "q" || input == "questlog") {
                        HUDTools.QuestLogHUD();
                        TextInput.PressToContinue();
                    } else if (input1 == "c" || input == "character" || input == "character screen") {
                        HUDTools.CharacterScreen();
                        TextInput.PressToContinue();
                    } else if (input1 == "u" || input == "use" || input == "heal") {
                        (Program.CurrentPlayer.Equipment.Potion as IConsumable)?.Consume();
                        TextInput.PressToContinue();
                    } else if (input1.Any(c => char.IsNumber(c))) {
                        if (input1 == "0") {
                            HUDTools.Print($"You sure you want to sell item # {10}? (Y/N)", 4);
                        } else {
                            HUDTools.Print($"You sure you want to sell item # {input1}? (Y/N)", 4);
                        }
                        string input2 = TextInput.PlayerPrompt();
                        if (input2 == "y") {
                            TrySellItem(int.Parse(input1) - 1, ShopPrice(input1), p);
                            TextInput.PressToContinue();
                        }
                    } 
                }
            }
        }
        //Metode til at genere priser i shoppen.
        public static int ShopPrice(string item) {
            int potionP = Program.CurrentPlayer.Equipment.Potion?.CalculateItemPrice() ?? 0;
            int sellPotionP = potionP / 2;
            switch (item) {
                default:
                    return 0;
                case "sellpotion":
                    return sellPotionP;
                case "sellpotion5":
                    int sellPotionP5 = sellPotionP * 5;
                    return sellPotionP5;
                case "1":
                    if (Program.CurrentPlayer.Inventory[0] == null) {
                        return -1;
                    }
                    else {
                        var itemItem = Program.CurrentPlayer.Inventory[0];
                        return Convert.ToInt32(itemItem.CalculateItemPrice()*0.6);
                    }
                case "2":
                    if (Program.CurrentPlayer.Inventory[1] == null) {
                        return -1;
                    }
                    else {
                        var itemItem = Program.CurrentPlayer.Inventory[1];
                        return Convert.ToInt32(itemItem.CalculateItemPrice() * 0.6);
                    }
                case "3":
                    if (Program.CurrentPlayer.Inventory[2] == null) {
                        return -1;
                    }
                    else {
                        var itemItem = Program.CurrentPlayer.Inventory[2];
                        return Convert.ToInt32(itemItem.CalculateItemPrice() * 0.6);
                    }
                case "4":
                    if (Program.CurrentPlayer.Inventory[3] == null) {
                        return -1;
                    }
                    else {
                        var itemItem = Program.CurrentPlayer.Inventory[3];
                        return Convert.ToInt32(itemItem.CalculateItemPrice() * 0.6);
                    }
                case "5":
                    if (Program.CurrentPlayer.Inventory[4] == null) {
                        return -1;
                    }
                    else {
                        var itemItem = Program.CurrentPlayer.Inventory[4];
                        return Convert.ToInt32(itemItem.CalculateItemPrice() * 0.6);
                    }
                case "6":
                    if (Program.CurrentPlayer.Inventory[5] == null) {
                        return -1;
                    }
                    else {
                        var itemItem = Program.CurrentPlayer.Inventory[5];
                        return Convert.ToInt32(itemItem.CalculateItemPrice() * 0.6);
                    }
                case "7":
                    if (Program.CurrentPlayer.Inventory[6] == null) {
                        return -1;
                    }
                    else {
                        var itemItem = Program.CurrentPlayer.Inventory[6];
                        return Convert.ToInt32(itemItem.CalculateItemPrice() * 0.6);
                    }
                case "8":
                    if (Program.CurrentPlayer.Inventory[7] == null) {
                        return -1;
                    }
                    else {
                        var itemItem = Program.CurrentPlayer.Inventory[7];
                        return Convert.ToInt32(itemItem.CalculateItemPrice() * 0.6);
                    }
                case "9":
                    if (Program.CurrentPlayer.Inventory[8] == null) {
                        return -1;
                    }
                    else {
                        var itemItem = Program.CurrentPlayer.Inventory[8];
                        return Convert.ToInt32(itemItem.CalculateItemPrice() * 0.6);
                    }
                case "0": case "10":
                    if (Program.CurrentPlayer.Inventory[9] == null) {
                        return -1;
                    }
                    else {
                        var itemItem = Program.CurrentPlayer.Inventory[10];
                        return Convert.ToInt32(itemItem.CalculateItemPrice() * 0.6);
                    }
            }
        }
        //Metode til at købe fra shoppen.
        static void TryBuyItem(int index, int cost, Shop s, Player p) {
            if (p.Gold >= cost) {
                int index1 = Array.FindIndex(p.Inventory, i => i == null || p.Inventory.Length == 0);
                p.Inventory.SetValue(s.Forsale[index], index1);
                s.Forsale.RemoveAt(index);
                p.Gold -= cost;
                HUDTools.Print("Item bought!", 3);
            } else {
                HUDTools.Print("You don't have enough gold!");
            }
        }
        //Metode til at sælge til shoppen.
        static void TrySellPotion(string item, int price, Player p) {
            switch (item) {
                case "potion":
                if ((p.Equipment.Potion is IConsumable consumable) && consumable.PotionQuantity > 0) {
                    consumable.PotionQuantity--;
                    p.Gold += price;
                    break;
                } else {
                    Console.WriteLine("You don't have any potions to sell!");
                    TextInput.PressToContinue();
                    break;
                }
                case "5x potion":
                if ((p.Equipment.Potion is IConsumable consumable1) && consumable1.PotionQuantity >= 5) {
                    consumable1.PotionQuantity -= 5;
                    p.Gold += price;
                    break;
                } else {
                    Console.WriteLine("You don't that many potions to sell!");
                    TextInput.PressToContinue();
                    break;
                }
            }
        }
        static void TrySellItem(int index, int price, Player p) {
            if (price == 0) {
                HUDTools.Print("The shopkeeper doesn't want that item...",3);
            }
            else if (price == -1) {
                if (index == -1) {
                    HUDTools.Print($"You don't have an item in slot #{10}");
                } else {
                    HUDTools.Print($"You don't have an item in slot #{index + 1}");
                }                   
            }
            else {
                p.Inventory.SetValue(null, index);
                p.Gold += price;
                HUDTools.Print("Item sold!", 3);
            }
        }
        public static IEquipable CreateRandomArmor(int level, int armorType = -1, int slot = -1) {
            if (armorType == -1) armorType = Program.Rand.Next(4);
            if (slot == -1) slot = Program.Rand.Next(7);
            ArmorBase item = new() {
                ItemLevel = Math.Max(1, Program.CurrentPlayer.Level + level),
                ItemSlot = (Slot)slot,
                ArmorType = (ArmorType)armorType,
                ItemName = RandomArmorName((ArmorType)armorType, (Slot)slot),
                ItemDescription = "A piece from Gheed's collection.\nYou start to wonder where he gets his items from...",
            };
            return (IEquipable)item;
        }
        public static IEquipable CreateRandomWeapon(int level) {
            int weapon = Program.Rand.Next(10);
            IWeapon item;
            string itemName;
            switch (weapon) {
                default:
                    itemName = RandomWeaponName(new OneHandedAxe());
                    item = new OneHandedAxe() {
                        ItemLevel = Math.Max(1, Program.CurrentPlayer.Level + level),
                        ItemDescription = "A piece from Gheed's collection.\nYou start to wonder where he gets his items from...",
                        ItemName = itemName,
                        AttackDescription = $"You swing your {itemName}",
                    };
                    return item;
                case 1:
                    itemName = RandomWeaponName(new OneHandedSword());
                    item = new OneHandedSword() {
                        ItemLevel = Math.Max(1, Program.CurrentPlayer.Level + level),
                        ItemDescription = "A piece from Gheed's collection.\nYou start to wonder where he gets his items from...",
                        ItemName = itemName,
                        AttackDescription = $"You swing your {itemName}",
                    };
                    return item;
                case 2:
                    itemName = RandomWeaponName(new TwoHandedSword());
                    item = new TwoHandedSword() {
                        ItemLevel = Math.Max(1, Program.CurrentPlayer.Level + level),
                        ItemDescription = "A piece from Gheed's collection.\nYou start to wonder where he gets his items from...",
                        ItemName = itemName,
                        AttackDescription = $"You swing your {itemName}",
                    };
                    return item;
                case 3:
                    itemName = RandomWeaponName(new OneHandedMace());
                    item = new OneHandedMace() {
                        ItemLevel = Math.Max(1, Program.CurrentPlayer.Level + level),
                        ItemDescription = "A piece from Gheed's collection.\nYou start to wonder where he gets his items from...",
                        ItemName = itemName,
                        AttackDescription = $"You swing your {itemName}",
                    };
                    return item;
                case 4:
                    itemName = RandomWeaponName(new Bow());
                    item = new Bow() {
                        ItemLevel = Math.Max(1, Program.CurrentPlayer.Level + level),
                        ItemDescription = "A piece from Gheed's collection.\nYou start to wonder where he gets his items from...",
                        ItemName = itemName,
                        AttackDescription = $"You fire an arrow from your {itemName}",
                    };
                    return item;
                case 5:
                    itemName = RandomWeaponName(new Dagger());
                    item = new Dagger() {
                        ItemLevel = Math.Max(1, Program.CurrentPlayer.Level + level),
                        ItemDescription = "A piece from Gheed's collection.\nYou start to wonder where he gets his items from...",
                        ItemName = itemName,
                        AttackDescription = $"You stab with your {itemName}",
                    };
                    return item;
                case 6:
                    itemName = RandomWeaponName(new Crossbow());
                    item = new Crossbow() {
                        ItemLevel = Math.Max(1, Program.CurrentPlayer.Level + level),
                        ItemDescription = "A piece from Gheed's collection.\nYou start to wonder where he gets his items from...",
                        ItemName = itemName,
                        AttackDescription = $"You fire a bolt with your {itemName}",
                    };
                    return item;
                case 7:
                    itemName = RandomWeaponName(new Staff());
                    item = new Staff() {
                        ItemLevel = Math.Max(1, Program.CurrentPlayer.Level + level),
                        ItemDescription = "A piece from Gheed's collection.\nYou start to wonder where he gets his items from...",
                        ItemName = itemName,
                        AttackDescription = $"You swing your {itemName}",
                    };
                    return item;
                case 8:
                    itemName = RandomWeaponName(new Wand());
                    item = new Wand() {
                        ItemLevel = Math.Max(1, Program.CurrentPlayer.Level + level),
                        ItemDescription = "A piece from Gheed's collection.\nYou start to wonder where he gets his items from...",
                        ItemName = itemName,
                        AttackDescription = $"You stab with your {itemName}",
                    };
                    return item;
                case 9:
                    itemName = RandomWeaponName(new Tome());
                    item = new Tome() {
                        ItemLevel = Math.Max(1, Program.CurrentPlayer.Level + level),
                        ItemDescription = "A piece from Gheed's collection.\nYou start to wonder where he gets his items from...",
                        ItemName = itemName,
                        AttackDescription = $"You bash with your {itemName}",
                    };
                    return item;
            }
        }
        public static string RandomArmorName(ArmorType type, Slot slot) {
            string name = "Fine Hat";
            int rand = Program.Rand.Next(5);
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
        public static string RandomWeaponName(IWeapon weapon) {
            string name1 = "Sturdy";
            string name2 = "Stick";
            int rand = Program.Rand.Next(10);
            switch (weapon) {
                case OneHandedAxe:
                    name2 = "axe";
                    name1 = WeaponNameList(weapon, rand);
                    break;
                case OneHandedSword:
                    name2 = "sword";
                    name1 = WeaponNameList(weapon, rand);
                    break;
                case TwoHandedSword:
                    name2 = "sword";
                    name1 = WeaponNameList(weapon, rand);
                    break;
                case OneHandedMace:
                    rand = Program.Rand.Next(2);
                    if (rand == 0) name2 = "hammer";
                    else name2 = "mace";
                    name1 = WeaponNameList(weapon, rand);
                    break;
                case Bow:
                    name2 = "bow";
                    name1 = WeaponNameList(weapon, rand);
                    break;
                case Dagger:
                    name2 = "dagger";
                    name1 = WeaponNameList(weapon, rand);
                    break;
                case Crossbow:
                    name2 = "crossbow";
                    name1 = WeaponNameList(weapon, rand);
                    break;
                case Staff:
                    name2 = "staff";
                    name1 = WeaponNameList(weapon, rand);
                    break;
                case Wand:
                    name2 = "wand";
                    name1 = WeaponNameList(weapon, rand);
                    break;
                case Tome:
                    name2 = "tome";
                    name1 = WeaponNameList(weapon, rand);
                    break;
            }
            return $"{name1}{name2}";
        }
        public static string WeaponNameList(IWeapon weapon, int rand) {
            string name = "";
            switch (weapon) {
                default:
                case OneHandedAxe:
                    switch (rand) {
                        case 0:
                            name = "Hand ";
                            break;
                        case 1:
                            name = "Broad ";
                            break;
                        case 2:
                            name = "Splitting ";
                            break;
                        case 3:
                            name = "Felling ";
                            break;
                        case 4:
                            name = "Battle ";
                            break;
                        case 5:
                            name = "Dane ";
                            break;
                        case 6:
                            name = "Double ";
                            break;
                        case 7:
                            name = "Cleaving ";
                            break;
                        case 8:
                            name = "Dagger-";
                            break;
                        case 9:
                            name = "Balanced ";
                            break;
                    }
                    return name;
                case OneHandedSword:
                    switch (rand) {
                        case 0:
                            name = "Short ";
                            break;
                        case 1:
                            name = "Runed ";
                            break;
                        case 2:
                            name = "Bastard ";
                            break;
                        case 3:
                            name = "Arming ";
                            break;
                        case 4:
                            name = "Sharp ";
                            break;
                        case 5:
                            name = "Decorated ";
                            break;
                        case 6:
                            name = "Sabre ";
                            break;
                        case 7:
                            name = "Falchion ";
                            break;
                        case 8:
                            name = "Broad";
                            break;
                        case 9:
                            name = "Dueling ";
                            break;
                    }
                    return name;
                case TwoHandedSword:
                    switch (rand) {
                        case 0:
                            name = "Zweihander ";
                            break;
                        case 1:
                            name = "Long";
                            break;
                        case 2:
                            name = "Bastard ";
                            break;
                        case 3:
                            name = "Arming ";
                            break;
                        case 4:
                            name = "Great";
                            break;
                        case 5:
                            name = "Claymore ";
                            break;
                        case 6:
                            name = "Butcher ";
                            break;
                        case 7:
                            name = "Executioner's ";
                            break;
                        case 8:
                            name = "Ornate ";
                            break;
                        case 9:
                            name = "Katana ";
                            break;
                    }
                    return name;
                case OneHandedMace:
                    switch (rand) {
                        case 0:
                            name = "War";
                            break;
                        case 1:
                            name = "Crushing ";
                            break;
                        case 2:
                            name = "Maul ";
                            break;
                        case 3:
                            name = "Piercing ";
                            break;
                        case 4:
                            name = "Blunt ";
                            break;
                        case 5:
                            name = "Club ";
                            break;
                        case 6:
                            name = "Mace ";
                            break;
                        case 7:
                            name = "Light ";
                            break;
                        case 8:
                            name = "Heavy ";
                            break;
                        case 9:
                            name = "Balanced ";
                            break;
                    }
                    return name;
                case Bow:
                    switch (rand) {
                        case 0:
                            name = "Short";
                            break;
                        case 1:
                            name = "Long";
                            break;
                        case 2:
                            name = "Recurve ";
                            break;
                        case 3:
                            name = "Straight ";
                            break;
                        case 4:
                            name = "Reflex ";
                            break;
                        case 5:
                            name = "Composite ";
                            break;
                        case 6:
                            name = "Hunting ";
                            break;
                        case 7:
                            name = "Flat";
                            break;
                        case 8:
                            name = "Hickory ";
                            break;
                        case 9:
                            name = "Ash ";
                            break;
                    }
                    return name;
                case Dagger:
                    switch (rand) {
                        case 0:
                            name = "Sharp ";
                            break;
                        case 1:
                            name = "Piercing ";
                            break;
                        case 2:
                            name = "Rondel ";
                            break;
                        case 3:
                            name = "Dirk ";
                            break;
                        case 4:
                            name = "Stiletto ";
                            break;
                        case 5:
                            name = "Poignard ";
                            break;
                        case 6:
                            name = "Parrying ";
                            break;
                        case 7:
                            name = "Bollock ";
                            break;
                        case 8:
                            name = "Hunting ";
                            break;
                        case 9:
                            name = "Seax ";
                            break;
                    }
                    return name;
                case Crossbow:
                    switch (rand) {
                        case 0:
                            name = "Recurve ";
                            break;
                        case 1:
                            name = "Light ";
                            break;
                        case 2:
                            name = "Siege ";
                            break;
                        case 3:
                            name = "Compound ";
                            break;
                        case 4:
                            name = "Repeating ";
                            break;
                        case 5:
                            name = "Quickdraw ";
                            break;
                        case 6:
                            name = "Penetrator ";
                            break;
                        case 7:
                            name = "Hunting ";
                            break;
                        case 8:
                            name = "Pulley ";
                            break;
                        case 9:
                            name = "Heavy ";
                            break;
                    }
                    return name;
                case Staff:
                    switch (rand) {
                        case 0:
                            name = "Quarter";
                            break;
                        case 1:
                            name = "Short ";
                            break;
                        case 2:
                            name = "Long ";
                            break;
                        case 3:
                            name = "Composite ";
                            break;
                        case 4:
                            name = "War ";
                            break;
                        case 5:
                            name = "Battle ";
                            break;
                        case 6:
                            name = "Runic ";
                            break;
                        case 7:
                            name = "Ancient ";
                            break;
                        case 8:
                            name = "Lunar ";
                            break;
                        case 9:
                            name = "Sun ";
                            break;
                    }
                    return name;
                case Wand:
                    switch (rand) {
                        case 0:
                            name = "Ashwood ";
                            break;
                        case 1:
                            name = "Elmwood ";
                            break;
                        case 2:
                            name = "Juniperwood ";
                            break;
                        case 3:
                            name = "Oakwood ";
                            break;
                        case 4:
                            name = "Walnut ";
                            break;
                        case 5:
                            name = "Beechwood ";
                            break;
                        case 6:
                            name = "Cedarwood ";
                            break;
                        case 7:
                            name = "Chestnut ";
                            break;
                        case 8:
                            name = "Yew ";
                            break;
                        case 9:
                            name = "Willow ";
                            break;
                    }
                    return name;
                case Tome:
                    switch (rand) {
                        case 0:
                            name = "Big ";
                            break;
                        case 1:
                            name = "Heavy ";
                            break;
                        case 2:
                            name = "Old ";
                            break;
                        case 3:
                            name = "Ancient ";
                            break;
                        case 4:
                            name = "Rune ";
                            break;
                        case 5:
                            name = "Mage's ";
                            break;
                        case 6:
                            name = "Spell";
                            break;
                        case 7:
                            name = "Magic ";
                            break;
                        case 8:
                            name = "Cursed ";
                            break;
                        case 9:
                            name = "Power ";
                            break;
                    }
                    return name;
            }
        }
    }
}
