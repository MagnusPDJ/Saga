using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Saga
{
    public class Shop
    {
        //Metode til at kalde og Loade shoppen.
        public static void Loadshop(Player p) {
            AudioManager.soundMainMenu.Stop();
            AudioManager.soundTroldmandsKamp.Stop();
            AudioManager.soundKamp.Stop();
            AudioManager.soundShop.Play();
            Runshop(p);
        }

        //Metode til at køre shoppen.
        public static void Runshop(Player p) {
            int potionP;
            int armorP;
            int weaponP;
            //int difP;

            while (true) {
                //Sætter prisen i shoppen skaleret på spilleren.
                potionP = 20 + 10 * p.mods;
                armorP = 100 * (p.armorValue+1);
                weaponP = 100 * (p.weaponValue+1);

                Console.Clear();
                Console.WriteLine("         Gheed's Shop        ");
                Console.WriteLine("=============================");
                Console.WriteLine($"| (W)eapon Upgrade        $ {weaponP}");
                Console.WriteLine($"| (A)rmor Upgrade:        $ {armorP}");
                Console.WriteLine($"| (P)otions:              $ {potionP}");
                Console.WriteLine("|============================");
                Console.WriteLine($"| (S)ell    Potion      $ {potionP / 2}");
                Console.WriteLine($"|  Sell (5)xPotions     $ {(potionP/2)*5}");
                Console.WriteLine("=============================");
                Console.WriteLine("  (E)xit Shop  (Q)uit to menu");
                Console.WriteLine();
                Console.WriteLine();
                Console.WriteLine($"  {p.currentClass} {p.name}'s Stats");
                Console.WriteLine($"=============================");
                Console.WriteLine($"| Level: {p.level}");
                Console.Write("| EXP  ");
                Console.Write("[");
                Program.ProgressBar("+", " ", ((decimal)p.xp / (decimal)p.GetLevelUpValue()),20);
                Console.WriteLine("]");
                Console.WriteLine($"| Current Health:         {p.health}");
                Console.WriteLine($"| Gold:                  ${p.gold}");
                Console.WriteLine($"| Weapon Strength:        {p.weaponValue}");
                Console.WriteLine($"| Armor Rating:           {p.armorValue}");
                Console.WriteLine($"| Potions:                {p.potion}");
                Console.WriteLine("=============================");
                Console.WriteLine(" (U)se Potion (C)haracter screen");
                Console.WriteLine("");
                Console.WriteLine("Choose what to buy or sell");

                //Wait for input
                string input = Program.PlayerPrompt().ToLower();
                if (input.ToLower() == "p" || input == "potion") {
                    TryBuy("potion", potionP, p);
                } else if (input.ToLower() == "w" || input == "weapon") {
                    TryBuy("weapon", weaponP, p);
                } else if (input.ToLower() == "a" || input == "armor") {
                    TryBuy("armor", armorP, p);
                //} else if (input.ToLower() == "d" || input == "difficulty mod") {
                //    TryBuy("dif", difP, p);
                } else if (input.ToLower() == "s" || input == "sell" || input == "sell potion") {
                    TrySell("potion", potionP / 2, p);
                } else if (input.ToLower() == "5" || input== "5x" || input == "sell 5" || input == "sell 5x"|| input == "sell 5xpotions") {
                    TrySell("5x potion", potionP / 2, p);
                } else if (input.ToLower() == "u" || input == "use" || input == "heal") {
                    if (Program.currentPlayer.potion == 0) {
                        Program.Print("No potions left!", 20);
                    } else {
                        Program.Print("You used a potion", 20);
                        Program.currentPlayer.health += Program.currentPlayer.potionValue;
                        if (Program.currentPlayer.health > Program.currentPlayer.maxHealth) {
                            Program.currentPlayer.health = Program.currentPlayer.maxHealth;
                        }
                        Program.currentPlayer.potion -= 1;
                        if (Program.currentPlayer.health == Program.currentPlayer.maxHealth) {
                            Program.Print("You heal to max health!", 20);
                        }
                        else {
                            Program.Print($"You gain {Program.currentPlayer.potionValue} health", 20);
                        }
                    }
                    Program.PlayerPrompt();
                } else if (input == "c" || input == "character" || input == "character screen") {
                    Player.CharacterScreen();
                    Program.Print("Press to go back...",1);
                    Program.PlayerPrompt();
                } else if (input.ToLower() == "q" || input == "quit" || input == "quit game") {
                    Program.Quit();
                } else if (input.ToLower() == "e" || input == "exit") {
                    AudioManager.soundShop.Stop();
                    break;
                }
            }
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
                    case "dif":
                        p.mods++;
                        break;
                }
                p.gold -= cost;

            } else {
                Console.WriteLine("You don't have enough gold!");
                Program.PlayerPrompt();
            }
        }

        //Metode til at sælge til shoppen.
        static void TrySell(string item, int cost, Player p) {
                switch (item) {
                    case "potion":
                    if (p.potion > 0) {
                        p.potion--;
                        p.gold += cost;
                        break;
                    } else {
                        Console.WriteLine("You don't have any potions to sell!");
                        Program.PlayerPrompt();
                        break;
                    }
                case "5x potion":
                    if (p.potion >=5) {
                        p.potion -= 5;
                        p.gold += 5*cost;
                        break;
                    } else {
                        Console.WriteLine("You don't that many potions to sell!");
                        Program.PlayerPrompt();
                        break;
                    }
                }
        }
    }
}
