using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Saga
{
    public class Encounters
    {   
     //Encounter Generic
        

     //Encounters
        //Det Encounter som køres når en ny karakter startes for at introducere kamp.
        public static void FirstEncounter() {
            Console.WriteLine("You throw open the door, grapping a rusty sword, while charging toward your captor.");
            Console.WriteLine("He turns...");
            Console.ReadKey();
            BasicCombat(false, "Human captor", 1, 5);
        }

        //Encounter som køres når en ny karkter startes
        public static void ShopEncounter() {
            Console.Clear();
            Console.WriteLine("After cleaning the blood of your captor from your new sword, you find someone else captured.");
            Console.WriteLine("Freeing him from his shackles, he thanks you and gets up.");
            Console.WriteLine("'Gheed is the name and trade is my game', he gives a wink.");
            Program.PlayerPrompt();
            Console.Clear();
            Console.WriteLine("'If you go and clear some of the other rooms, I will look for my wares in these crates.'");
            Console.WriteLine("'Then Run back to me, I will then have been able to set up a shop where you can spend ");
            Console.WriteLine("some of that gold you are bound to have found,' he chuckles and rubs his hands at the thought.");
            Console.WriteLine("You nod and prepare your sword, then start walking down a dark corridor...");
            Program.PlayerPrompt();
        }

        //Encounter der "spawner" en random fjende som skal dræbes.
        public static void BasicFightEncounter() {
            Console.Clear();
            string n = GetName();
            switch (Program.rand.Next(0,2)) {
                case int x when (x == 0):
                    Console.WriteLine("You turn a corner and there you see a " + n + "...");
                    break;
                case int x when (x == 1):
                    Console.WriteLine("You break down a door and find a " + n + " inside!");
                    break;
            }
            Console.ReadKey();
            BasicCombat(true, n, 0, 0);
        }

        //Encounter der "spawner" en specifik fjende som skal dræbes.
        public static void WizardEncounter() {
            Console.Clear();
            Console.WriteLine("The door slowly creaks open as you peer into the dark room. You see a tall man with a ");
            Console.WriteLine("long beard and pointy hat, looking at a large tome.");
            Console.ReadKey();
            BasicCombat(false, "Dark Wizard", 4, 2);
        }


     //Encounter Tools
        //Metode til at vælge tilfældigt mellem encounters.
        public static void RandomEncounter() {
            switch (Program.rand.Next(0, 9)) {
                case int n when (n>0):
                    BasicFightEncounter();
                    break;
                case int n when (n==0):
                    WizardEncounter();
                    break;
            }
        }

        //Metode til at køre kamp.
        public static void BasicCombat(bool random, string name, int power, int health) {
            string n = "";
            int p = 0;
            int h = 0;

            if (random) {
                n = name;
                p = Program.currentPlayer.GetPower();
                h = Program.currentPlayer.GetHealth();
            } else {
                n = name;
                p = power;
                h = health;
            }
            while (h > 0) {
                Console.Clear();
                Console.WriteLine("Fighting: " + n + "!");
                Console.WriteLine("Strength: " + p + " / HP: " + h);
                Console.WriteLine("-----------------------");
                Console.WriteLine(Program.currentPlayer.name + "'s Stats:");
                Console.WriteLine( "Healing Potions: " + Program.currentPlayer.potion + " || Health: " + Program.currentPlayer.health);
                Console.WriteLine("Gold:           $" + Program.currentPlayer.gold + " || Armor:  " + Program.currentPlayer.armorValue);
                Console.WriteLine("=========Actions=======");
                Console.WriteLine("| (A)ttack (D)efend   |");
                Console.WriteLine("| (R)un    (H)eal     |");
                Console.WriteLine("=======================");
                
                Console.WriteLine("Choose an action...");
                string input = Program.PlayerPrompt();

                if (input.ToLower() == "a" || input == "attack") {
                    //Attack
                    Console.WriteLine("You attack and " + n + " retaliates.");
                    int damage = p - Program.currentPlayer.armorValue;
                    if (damage < 0)
                        damage = 0;
                    int attack = Program.rand.Next(1, 1+Program.currentPlayer.weaponValue) + Program.rand.Next(0, 4);
                    Console.WriteLine("You lose " + damage + " health and you deal " + attack + " damage");
                    Program.currentPlayer.health -= damage;
                    h -= attack;
                } else if (input.ToLower() == "d" || input == "defend") {
                    //Defend
                    Console.WriteLine("You defend the incoming attack from " + n);
                    int damage = 1+(p / 4) - Program.currentPlayer.armorValue;
                    if (damage < 0)
                        damage = 0;
                    int attack = Program.rand.Next(0, 4+Program.currentPlayer.weaponValue) / 2;
                    Console.WriteLine("You lose " + damage + " health and you deal " + attack + " damage");
                    Program.currentPlayer.health -= damage;
                    h -= attack;
                } else if (input.ToLower() == "r" || input == "run") {
                    //Run
                    if (Program.rand.Next(0, 2) == 0 || n == "Human captor") {
                        Console.WriteLine("As you sprint away from the " + n + ", it strikes you and knocks you down");
                        int damage = p - Program.currentPlayer.armorValue;
                        if (damage < 0)
                            damage = 0;
                        Console.WriteLine("You lose " + damage + " health and are unable to escape.");
                        Program.currentPlayer.health -= damage;

                    } else {
                        Console.WriteLine("You use your crazy ninja moves to evade the " + n + " and you successfully escape!");
                        Console.ReadKey();
                        Shop.Loadshop(Program.currentPlayer);
                        break;
                    }
                } else if (input.ToLower() == "h" || input == "heal") {
                    //Heal
                    if (Program.currentPlayer.potion == 0) {
                        Console.WriteLine("No potions left!");
                        int damage = p - Program.currentPlayer.armorValue;
                        if (damage < 0)
                            damage = 0;
                        Console.WriteLine("The " + n + " attacks you while you fumble in your bags and lose " + damage + " health!");
                        Program.currentPlayer.health -= damage;
                    } else {
                        Console.WriteLine("You use a potion");
                        Program.currentPlayer.health += Program.currentPlayer.potionValue;
                        if (Program.currentPlayer.health > Program.currentPlayer.maxHealth) {
                            Program.currentPlayer.health = Program.currentPlayer.maxHealth;
                        }
                        Program.currentPlayer.potion -= 1;
                        if (Program.currentPlayer.health == Program.currentPlayer.maxHealth) {
                            Console.WriteLine("You heal to max health!");
                        } else {
                            Console.WriteLine("You gain " + Program.currentPlayer.potionValue + " health");
                        }
                        Console.WriteLine("As you drink, the " + n + " strikes you.");
                        int damage = (p / 2) - Program.currentPlayer.armorValue;
                        if (damage < 0)
                            damage = 0;
                        Console.WriteLine("You lose " + damage + " health");
                        Program.currentPlayer.health -= damage;
                    }
                }
                if (Program.currentPlayer.health <= 0) {
                    //Death code
                    Console.WriteLine("As the " + n + " stands menacingly and comes down to strike. You have been slain by the mighty " + n);
                    Console.ReadKey(true);
                    System.Environment.Exit(0);
                }
                Console.WriteLine("Press to continue...");
                Console.ReadKey(true);
            }
            if (h <= 0)
            {
                //Loot
                int g = Program.currentPlayer.GetGold();
                int[] numbers = new[] { 0, 0, 0, 0, 0, 0, 0, 1, 1, 1, 2 };
                var pot = Program.rand.Next(0, numbers.Length);
                Console.WriteLine("You Won against the " + n + "! You loot " + g + " gold coins.");
                if (numbers[pot] != 0)
                {
                    Console.WriteLine("You loot " + numbers[pot] + " healing potions");
                    Program.currentPlayer.potion += numbers[pot];
                }
                Program.currentPlayer.gold += g;
                Console.ReadKey(true);
            }
        }

        //Monster navne
        public static string GetName() {
            switch(Program.rand.Next(0, 10)) {
                case 0:
                    return "Skeleton";
                case 1:
                    return "Zombie";
                case 2:
                    return "Human Cultist";
                case 3:
                    return "Grave Robber";
                case 4:
                    return "Bat";
                case 5:
                    return "Human Rogue";
                case 6:
                    return "Vampire";
                case 7:
                    return "Werewolf";
                case 8:
                    return "Wolf";
                case 9:
                    return "Bandit";

            }
            return "Rat";

        }

    }
}
