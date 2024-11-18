using System;
using System.Collections.Generic;
using System.Linq;
using Saga.assets;
using Saga.Character;
using Saga.Items.Loot;

namespace Saga.Items
{
    public class Shop
    {
        public List<Item> Forsale { get; set; }      
        public static Shop SetForsale() {
            Shop shop = new Shop();
            shop.Forsale = new List<Item> { ArmorLootTable.CreateRandomArmor(-1), ArmorLootTable.CreateRandomArmor(0) , ArmorLootTable.CreateRandomArmor(1), WeaponLootTable.CreateRandomWeapon(-1),WeaponLootTable.CreateRandomWeapon(0),WeaponLootTable.CreateRandomWeapon(1) };        
            return shop;
        }
        //Metode til at kalde og Loade shoppen.
        public static void Loadshop(Player p, Shop shop) {
            AudioManager.soundShop.Play();
            Runshop(p, shop);
            AudioManager.soundShop.Stop();
        }

        //Metode til at køre shoppen.
        public static void Runshop(Player p, Shop shop) {
            bool sell = false;
            bool buy = true;
            while (buy) {
                HUDTools.BuyShopHUD(shop);
                //Wait for input
                string input = HUDTools.PlayerPrompt();
                if (input == "p" || input == "potion") {
                    TryBuy("potion", ShopPrice("potion"), p);
                } else if (input == "w" || input == "weapon") {
                    TryBuy("weapon", ShopPrice("weaponupgrade"), p);
                } else if (input == "a" || input == "armor") {
                    TryBuy("armor", ShopPrice("armorupgrade"), p);
                } else if (input == "g" || input == "upgrade potion") {
                    TryBuy("upgradepotion", ShopPrice("potionupgrade"), p);
                } else if (input == "u" || input == "use" || input == "heal") {
                    Program.CurrentPlayer.Heal();
                    HUDTools.PlayerPrompt();
                } else if (input == "c" || input == "character" || input == "character screen") {
                    HUDTools.CharacterScreen();
                    HUDTools.PlayerPrompt();
                } else if (input == "s" || input == "switch" || input == "sell") {
                    sell = true;
                }
                else if (input == "i" || input == "inventory") {
                    HUDTools.InventoryScreen();
                }
                else if (input == "e" || input == "exit") {
                    break;
                }
                else if (input.Any(c => char.IsNumber(c))) {
                    HUDTools.Print($"You sure you want to buy item # {input}? (Y/N)", 4);
                    string input3 = HUDTools.PlayerPrompt();
                    if (input3 == "y") {
                        TryBuyItem(int.Parse(input), shop.Forsale[int.Parse(input)].CalculateItemPrice(), shop ,p);
                        HUDTools.PlayerPrompt();
                    }
                }
                while (sell) {
                    HUDTools.SellShopHUD();
                    string input1 = HUDTools.PlayerPrompt();
                    if (input1 == "s" || input1 == "switch" || input1 == "buy") {
                        sell = false;
                    }
                    else if (input1 == "e" || input1 == "exit") {
                        buy = false;
                        break;
                    }
                    else if (input1 == "p" || input1 == "potion" || input1 == "sell potion") {
                        TrySell("potion", ShopPrice("sellpotion"), p);
                    }
                    else if (input1 == "5" || input1 == "5x" || input1 == "sell 5" || input1 == "sell 5x" || input1 == "sell 5xpotions") {
                        TrySell("5x potion", ShopPrice("sellpotion5"), p);
                    }
                    else if (input1 == "i" || input1 == "inventory") {
                        HUDTools.InventoryScreen();
                    }
                    else if (input1.Any(c => char.IsNumber(c))) {
                        HUDTools.Print($"You sure you want to sell item # {input1}? (Y/N)",4);
                        string input3  = HUDTools.PlayerPrompt();
                        if (input3 == "y") {
                            TrySellItem(int.Parse(input1), ShopPrice(input1), p);
                            HUDTools.PlayerPrompt();
                        }
                    }
                }
            }
        }

        //Metode til at genere priser i shoppen.
        public static int ShopPrice(string item) {
            int potionP = Program.CurrentPlayer.CurrentHealingPotion.CalculateItemPrice();
            int sellPotionP = potionP / 2;
            switch (item) {
                default:
                    return 0;
                case "potion":                    
                    return potionP;
                case "potionupgrade":
                    int potionupgradeP = 200 * Program.CurrentPlayer.CurrentHealingPotion.PotionPotency;
                    return potionupgradeP;
                case "sellpotion":
                    return sellPotionP;
                case "sellpotion5":
                    int sellPotionP5 = sellPotionP * 5;
                    return sellPotionP5;
                case "0":
                    if (Program.CurrentPlayer.Inventory[0] == null) {
                        return 0;
                    }
                    else {
                        var itemItem = Program.CurrentPlayer.Inventory[0];
                        return Convert.ToInt32(itemItem.CalculateItemPrice()*0.6);
                    }
                case "1":
                    if (Program.CurrentPlayer.Inventory[1] == null) {
                        return 0;
                    }
                    else {
                        var itemItem = Program.CurrentPlayer.Inventory[1];
                        return Convert.ToInt32(itemItem.CalculateItemPrice() * 0.6);
                    }
                case "2":
                    if (Program.CurrentPlayer.Inventory[2] == null) {
                        return 0;
                    }
                    else {
                        var itemItem = Program.CurrentPlayer.Inventory[2];
                        return Convert.ToInt32(itemItem.CalculateItemPrice() * 0.6);
                    }
                case "3":
                    if (Program.CurrentPlayer.Inventory[3] == null) {
                        return 0;
                    }
                    else {
                        var itemItem = Program.CurrentPlayer.Inventory[3];
                        return Convert.ToInt32(itemItem.CalculateItemPrice() * 0.6);
                    }
                case "4":
                    if (Program.CurrentPlayer.Inventory[4] == null) {
                        return 0;
                    }
                    else {
                        var itemItem = Program.CurrentPlayer.Inventory[4];
                        return Convert.ToInt32(itemItem.CalculateItemPrice() * 0.6);
                    }
                case "5":
                    if (Program.CurrentPlayer.Inventory[5] == null) {
                        return 0;
                    }
                    else {
                        var itemItem = Program.CurrentPlayer.Inventory[5];
                        return Convert.ToInt32(itemItem.CalculateItemPrice() * 0.6);
                    }
                case "6":
                    if (Program.CurrentPlayer.Inventory[6] == null) {
                        return 0;
                    }
                    else {
                        var itemItem = Program.CurrentPlayer.Inventory[6];
                        return Convert.ToInt32(itemItem.CalculateItemPrice() * 0.6);
                    }
                case "7":
                    if (Program.CurrentPlayer.Inventory[7] == null) {
                        return 0;
                    }
                    else {
                        var itemItem = Program.CurrentPlayer.Inventory[7];
                        return Convert.ToInt32(itemItem.CalculateItemPrice() * 0.6);
                    }
                case "8":
                    if (Program.CurrentPlayer.Inventory[8] == null) {
                        return 0;
                    }
                    else {
                        var itemItem = Program.CurrentPlayer.Inventory[8];
                        return Convert.ToInt32(itemItem.CalculateItemPrice() * 0.6);
                    }
                case "9":
                    if (Program.CurrentPlayer.Inventory[9] == null) {
                        return 0;
                    }
                    else {
                        var itemItem = Program.CurrentPlayer.Inventory[10];
                        return Convert.ToInt32(itemItem.CalculateItemPrice() * 0.6);
                    }
            }
        }
        //Metode til at købe fra shoppen.
        static void TryBuy(string item, int cost, Player p) {
            if (p.Gold >= cost) {
                switch (item) {
                    case "potion":
                        p.CurrentHealingPotion.PotionQuantity++;
                        break;
                    //case "weapon":
                    //    p.weaponValue++; 
                    //    break;
                    //case "armor":
                    //    p.armorValue++; 
                    //    break;
                    case "upgradepotion":
                        p.CurrentHealingPotion.PotionPotency += 5;
                        break;
                }
                p.Gold -= cost;

            } else {
                Console.WriteLine("You don't have enough gold!");
                HUDTools.PlayerPrompt();
            }
        }

        //Metode til at sælge til shoppen.
        static void TrySell(string item, int price, Player p) {
            switch (item) {
                case "potion":
                if (p.CurrentHealingPotion.PotionQuantity > 0) {
                    p.CurrentHealingPotion.PotionQuantity--;
                    p.Gold += price;
                    break;
                } else {
                    Console.WriteLine("You don't have any potions to sell!");
                    HUDTools.PlayerPrompt();
                    break;
                }
                case "5x potion":
                if (p.CurrentHealingPotion.PotionQuantity >= 5) {
                    p.CurrentHealingPotion.PotionQuantity -= 5;
                    p.Gold += price;
                    break;
                } else {
                    Console.WriteLine("You don't that many potions to sell!");
                    HUDTools.PlayerPrompt();
                    break;
                }
            }
        }
        static void TrySellItem(int index, int price, Player p) {
            if (price == 0) {
                HUDTools.Print("Invalid Input",3);
                HUDTools.PlayerPrompt();
            }
            else {
                p.Inventory.SetValue(null, index);
                p.Gold += price;
                HUDTools.Print("Item sold!", 3);
            }
        }
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
    }
}
