using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Saga
{
    public class Encounters
    {   
        static Random rand = new Random();
        //Encounter Generic
        

        //Encounters
        public static void FirstEncounter() {
            Console.WriteLine("You throw open the door, grapping a rusty sword, while charging toward your captor.");
            Console.WriteLine("He turns...");
            Console.ReadKey();
            BasicCombat(false, "Human captor", 1, 5);
        }

        public static void BasicFightEncounter() {
            Console.Clear();
            Console.WriteLine("You turn the corner and there you see a foe...");
            Console.ReadKey();
            BasicCombat(true, "", 0, 0);
        }

        public static void WizardEncounter() {
            Console.Clear();
            Console.WriteLine("The door slowly creaks open as you peer into the dark room. You see a tall man with a ");
            Console.WriteLine("long beard and pointy hat, looking at a large tome.");
            Console.ReadKey();
            BasicCombat(false, "Dark Wizard", 4, 2);
        }


        //Encounter Tools
        public static void RandomEncounter() {
            switch (rand.Next(0, 9)) {
                case int n when (n>0):
                    BasicFightEncounter();
                    break;
                case int n when (n==0):
                    WizardEncounter();
                    break;
            }
        }

        public static void BasicCombat(bool random, string name, int power, int health) {
            string n = "";
            int p = 0;
            int h = 0;

            if (random) {
                n = GetName();
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
                string input = Console.ReadLine();
                if (input.ToLower() == "a" || input.ToLower() == "attack") {
                    //Attack
                    Console.WriteLine("You attack and " + n + " retaliates.");
                    int damage = p - Program.currentPlayer.armorValue;
                    if (damage < 0)
                        damage = 0;
                    int attack = rand.Next(1, 1+Program.currentPlayer.weaponValue) + rand.Next(0, 4);
                    Console.WriteLine("You lose " + damage + " health and you deal " + attack + " damage");
                    Program.currentPlayer.health -= damage;
                    h -= attack;
                }
                else if (input.ToLower() == "d" || input.ToLower() == "defend") {
                    //Defend
                    Console.WriteLine("You defend the incoming attack from " + n);
                    int damage = 1+(p / 4) - Program.currentPlayer.armorValue;
                    if (damage < 0)
                        damage = 0;
                    int attack = rand.Next(0, 4+Program.currentPlayer.weaponValue) / 2;
                    Console.WriteLine("You lose " + damage + " health and you deal " + attack + " damage");
                    Program.currentPlayer.health -= damage;
                    h -= attack;
                }
                else if (input.ToLower() == "r" || input.ToLower() == "run") {
                    //Run
                    if (rand.Next(0, 2) == 0) {
                        Console.WriteLine("As you sprint away from the " + n + ", it strikes you and knocks you down");
                        int damage = p - Program.currentPlayer.armorValue;
                        if (damage < 0)
                            damage = 0;
                        Console.WriteLine("You lose " + damage + " health and are unable to escape.");
                        Program.currentPlayer.health -= damage;
                        Console.ReadKey();

                    } else {
                        Console.WriteLine("You use your crazy ninja moves to evade the " + n + " and you successfully escape!");
                        Console.ReadKey();
                        Shop.Loadshop(Program.currentPlayer);
                        break;
                    }


                }
                else if (input.ToLower() == "h" || input.ToLower() == "heal") {
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
                        Console.WriteLine("You gain " + Program.currentPlayer.potionValue + " health");
                        Program.currentPlayer.health += Program.currentPlayer.potionValue;
                        Program.currentPlayer.potion -= 1;
                        Console.WriteLine("As you drink, the " + n + " strikes you.");
                        int damage = (p / 2) - Program.currentPlayer.armorValue;
                        if (damage < 0)
                            damage = 0;
                        Console.WriteLine("You lose " + damage + " health");
                    }
                }
                if (Program.currentPlayer.health <= 0) {
                    //Death code
                    Console.WriteLine("As the " + n + " stands menacingly and comes down to strike. You have been slain by the mighty " + n);
                    Console.ReadKey();
                    System.Environment.Exit(0);
                }
                Console.WriteLine("Press to continue...");
                Console.ReadKey();
            }
            if (h <= 0)
            {
                int g = Program.currentPlayer.GetGold();
                int[] numbers = new[] { 0, 0, 0, 0, 0, 0, 0, 1, 1, 1, 2 };
                var pot = rand.Next(0, numbers.Length);
                Console.WriteLine("You Won against " + n + "! You loot " + g + " gold coins.");
                if (numbers[pot] != 0)
                {
                    Console.WriteLine("You loot " + numbers[pot] + " healing potions");
                    Program.currentPlayer.potion += numbers[pot];
                }
                Program.currentPlayer.gold += g;
                Console.ReadKey();
            }
        }

        public static string GetName() {
            switch(rand.Next(0, 10)) {
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
