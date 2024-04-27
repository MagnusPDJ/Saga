using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Text;
using System.Threading.Tasks;

namespace Saga
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
                if (input.ToLower() == "p" || input == "potion") {
                    TryBuy("potion", ShopPrice("potion"), p);
                } else if (input.ToLower() == "w" || input == "weapon") {
                    TryBuy("weapon", ShopPrice("weaponupgrade"), p);
                } else if (input.ToLower() == "a" || input == "armor") {
                    TryBuy("armor", ShopPrice("armorupgrade"), p);
                } else if (input.ToLower() == "g" || input == "upgrade potion") {
                    TryBuy("upgradepotion", ShopPrice("potionupgrade"), p);
                } else if (input.ToLower() == "s" || input == "sell" || input == "sell potion") {
                    TrySell("potion", ShopPrice("sellpotion"), p);
                } else if (input.ToLower() == "5" || input== "5x" || input == "sell 5" || input == "sell 5x"|| input == "sell 5xpotions") {
                    TrySell("5x potion", ShopPrice("sellpotion5"), p);
                } else if (input.ToLower() == "u" || input == "use" || input == "heal") {
                    Player.Heal();
                    HUDTools.PlayerPrompt();
                } else if (input == "c" || input == "character" || input == "character screen") {
                    HUDTools.CharacterScreen();
                    HUDTools.PlayerPrompt();
                } else if (input.ToLower() == "e" || input == "exit") {
                    break;
                }
            }
        }

        //Metode til at genere priser i shoppen.
        public static int ShopPrice(string item) {
            int potionP = 20 + 10 * Program.currentPlayer.level;
            int sellPotionP = potionP / 2;
            switch (item) {
                case "potion":                    
                    return potionP;
                case "armorupgrade":
                    int armorP = 100 * (Program.currentPlayer.armorValue + 1);
                    return armorP;
                case "weaponupgrade":
                    int weaponP = 100 * (Program.currentPlayer.weaponValue + 1);
                    return weaponP;
                case "potionupgrade":
                    int potionupgradeP = 200 * Program.currentPlayer.potionValue;
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
            if (p.gold >= cost) {
                switch (item) {
                    case "potion":
                        p.potion++;
                        break;
                    case "weapon":
                        p.weaponValue++; 
                        break;
                    case "armor":
                        p.armorValue++; 
                        break;
                    case "upgradepotion":
                        p.potionValue += 5;
                        break;
                }
                p.gold -= cost;

            } else {
                Console.WriteLine("You don't have enough gold!");
                HUDTools.PlayerPrompt();
            }
        }

        //Metode til at sælge til shoppen.
        static void TrySell(string item, int price, Player p) {
            switch (item) {
                case "potion":
                if (p.potion > 0) {
                    p.potion--;
                    p.gold += price;
                    break;
                } else {
                    Console.WriteLine("You don't have any potions to sell!");
                    HUDTools.PlayerPrompt();
                    break;
                }
                case "5x potion":
                if (p.potion >=5) {
                    p.potion -= 5;
                    p.gold += price;
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
