using System;
using Saga.assets;
using Saga.Character;

namespace Saga.Items
{
    public class Shop
    {
        //Metode til at kalde og Loade shoppen.
        public static void Loadshop(Player p) {
            AudioManager.soundShop.Play();
            Runshop(p);
            AudioManager.soundShop.Stop();
        }

        //Metode til at køre shoppen.
        public static void Runshop(Player p) {
            while (true) {
                HUDTools.InstantShopHUD();
                //Wait for input
                string input = HUDTools.PlayerPrompt().ToLower();
                if (input == "p" || input == "potion") {
                    TryBuy("potion", ShopPrice("potion"), p);
                } else if (input == "w" || input == "weapon") {
                    TryBuy("weapon", ShopPrice("weaponupgrade"), p);
                } else if (input == "a" || input == "armor") {
                    TryBuy("armor", ShopPrice("armorupgrade"), p);
                } else if (input == "g" || input == "upgrade potion") {
                    TryBuy("upgradepotion", ShopPrice("potionupgrade"), p);
                } else if (input == "s" || input == "sell" || input == "sell potion") {
                    TrySell("potion", ShopPrice("sellpotion"), p);
                } else if (input == "5" || input== "5x" || input == "sell 5" || input == "sell 5x"|| input == "sell 5xpotions") {
                    TrySell("5x potion", ShopPrice("sellpotion5"), p);
                } else if (input == "u" || input == "use" || input == "heal") {
                    Program.CurrentPlayer.Heal();
                    HUDTools.PlayerPrompt();
                } else if (input == "c" || input == "character" || input == "character screen") {
                    HUDTools.CharacterScreen();
                    HUDTools.PlayerPrompt();
                } else if (input == "e" || input == "exit") {
                    break;
                }
            }
        }

        //Metode til at genere priser i shoppen.
        public static int ShopPrice(string item) {
            int potionP = 20 + 10 * Program.CurrentPlayer.Level;
            int sellPotionP = potionP / 2;
            switch (item) {
                case "potion":                    
                    return potionP;
                //case "armorupgrade":
                //    int armorP = 100 * (Program.currentPlayer.armorValue + 1);
                //    return armorP;
                //case "weaponupgrade":
                //    int weaponP = 100 * (Program.currentPlayer.weaponValue + 1);
                //    return weaponP;
                case "potionupgrade":
                    int potionupgradeP = 200 * ((Potion)Program.CurrentPlayer.Equipment[Slot.SLOT_POTION]).PotionPotency;
                    return potionupgradeP;
                case "sellpotion":
                    return sellPotionP;
                case "sellpotion5":
                    int sellPotionP5 = sellPotionP * 5;
                    return sellPotionP5;
            }
            return 0;
        }


        //Metode til at købe fra shoppen.
        static void TryBuy(string item, int cost, Player p) {
            if (p.Gold >= cost) {
                switch (item) {
                    case "potion":
                        ((Potion)p.Equipment[Slot.SLOT_POTION]).PotionQuantity++;
                        break;
                    //case "weapon":
                    //    p.weaponValue++; 
                    //    break;
                    //case "armor":
                    //    p.armorValue++; 
                    //    break;
                    case "upgradepotion":
                        ((Potion)p.Equipment[Slot.SLOT_POTION]).PotionPotency += 5;
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
                if (((Potion)p.Equipment[Slot.SLOT_POTION]).PotionQuantity > 0) {
                    ((Potion)p.Equipment[Slot.SLOT_POTION]).PotionQuantity--;
                    p.Gold += price;
                    break;
                } else {
                    Console.WriteLine("You don't have any potions to sell!");
                    HUDTools.PlayerPrompt();
                    break;
                }
                case "5x potion":
                if (((Potion)p.Equipment[Slot.SLOT_POTION]).PotionQuantity >= 5) {
                    ((Potion)p.Equipment[Slot.SLOT_POTION]).PotionQuantity -= 5;
                    p.Gold += price;
                    break;
                } else {
                    Console.WriteLine("You don't that many potions to sell!");
                    HUDTools.PlayerPrompt();
                    break;
                }
            }
        }
    }
}
