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
            Sounds.soundTypeWriter.PlayLooping();
            switch (Program.currentPlayer.currentClass.ToString()) {
                case "Warrior":
                    Program.currentPlayer.equippedWeapon = "Rusty sword";
                    Program.currentPlayer.equippedArmor = "Rags";
                    break;
                case "Archer":
                    Program.currentPlayer.equippedWeapon = "Flimsy bow";
                    Program.currentPlayer.equippedArmor = "Rags";
                    break;
                case "Mage":
                    Program.currentPlayer.equippedWeapon = "Gnarled wand";
                    Program.currentPlayer.equippedArmor = "Rags";
                    break;
            }
            Program.Print("You throw open the door, grabbing a "+ Program.currentPlayer.equippedWeapon +", while charging toward your captor.");
            Program.Print("He turns...");
            Sounds.soundTypeWriter.Stop();
            Console.ReadKey();
            Sounds.soundKamp.PlayLooping();
            BasicCombat(false, "Human captor", 1, 5);
        }

        //Encounter som køres introducere shopkeeperen
        public static void ShopEncounter() {
            Console.Clear();
            Sounds.soundTypeWriter.PlayLooping();
            if (Program.currentPlayer.currentClass == Player.PlayerClass.Mage) {
                Program.Print("After dusting off your" + Program.currentPlayer.equippedArmor + "and tucking in your new wand, you find someone else captured.");
            } else if (Program.currentPlayer.currentClass==Player.PlayerClass.Archer) {
                Program.Print("After retrieving the last arrow from your captors corpse, you find someone else captured.");
            } else {
                Program.Print("After cleaning the blood of your captor from your new sword, you find someone else captured.");
            }
            Program.Print("Freeing him from his shackles, he thanks you and gets up.");
            Program.Print("'Gheed is the name and trade is my game', he gives a wink.");
            Sounds.soundTypeWriter.Stop();
            Program.PlayerPrompt();
            Sounds.soundTypeWriter.PlayLooping();
            Console.Clear();
            Program.Print("'If you go and clear some of the other rooms, I will look for my wares in these crates.'");
            Program.Print("'Then Run back to me, I will then have been able to set up a shop where you can spend ");
            Program.Print("some of that gold you are bound to have found,' he chuckles and rubs his hands at the thought.");
            Program.Print("You nod and prepare your " + Program.currentPlayer.equippedWeapon + ", then you start walking down a dark corridor...");
            Sounds.soundTypeWriter.Stop();
            Program.PlayerPrompt();
        }

        //Encounter der "spawner" en random fjende som skal dræbes.
        public static void BasicFightEncounter() {
            Console.Clear();
            Sounds.soundKamp.PlayLooping();
            string n = GetName();
            switch (Program.rand.Next(0,2)) {
                case int x when (x == 0):
                    Program.Print("You turn a corner and there you see a " + n + "...", 20);
                    break;
                case int x when (x == 1):
                    Program.Print("You break down a door and find a " + n + " inside!", 20);
                    break;
            }
            Console.ReadKey();
            BasicCombat(true, n, 0, 0);
        }

        //Encounter der "spawner" en specifik fjende som skal dræbes.
        public static void WizardEncounter() {
            Console.Clear();
            Sounds.soundTypeWriter.PlayLooping();
            Program.Print("The door slowly creaks open as you peer into the dark room. You see a tall man with a ");
            Program.Print("long beard and pointy hat, looking at a large tome.");
            Sounds.soundTypeWriter.Stop();
            Console.ReadKey();
            Sounds.soundMainMenu.PlayLooping();
            BasicCombat(false, "Dark Wizard", 4+Program.currentPlayer.level, 2+Program.currentPlayer.level);
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
            string n;
            int p;
            int h;

            if (random) {
                n = name;
                p = Program.currentPlayer.GetPower();
                h = Program.currentPlayer.GetHealth();
            } else {
                n = name;
                p = power;
                h = health;
            }
            Console.Clear();
            Program.Print("Fighting: " + n + "!", 20);
            Program.Print("Strength: " + p + " / HP: " + h, 20);
            Program.Print("-----------------------", 20);
            while (h > 0) {
                Console.Clear();
                Console.WriteLine("Fighting: " + n + "!");
                Console.WriteLine("Strength: " + p + " / HP: " + h);
                Console.WriteLine("-----------------------");
                Console.WriteLine(Program.currentPlayer.currentClass + " " + Program.currentPlayer.name + "'s Stats:");
                Console.WriteLine("Health: " + Program.currentPlayer.health + "\t|| Healing Potions: " + Program.currentPlayer.potion);
                Console.WriteLine("Armor:  " + Program.currentPlayer.armorValue + "\t|| Gold: $" + Program.currentPlayer.gold);
                Console.WriteLine("Level: " + Program.currentPlayer.level);
                Console.Write("EXP  ");
                Console.Write("[");
                Program.ProgressBar("+", " ", ((decimal)Program.currentPlayer.xp / (decimal)Program.currentPlayer.GetLevelUpValue()), 20);
                Console.WriteLine("]");
                Console.WriteLine("===========Actions=========");
                Console.WriteLine("| (A)ttack     (D)efend   |");
                Console.WriteLine("| (R)un        (H)eal     |");
                Console.WriteLine("===========================");
                Console.WriteLine(" (C)haracter screen        ");
                Console.WriteLine("");
                Console.WriteLine("Choose an action...");
                string input = Program.PlayerPrompt().ToLower();

                if (input.ToLower() == "a" || input == "attack") {
                    //Attack
                    if (Program.currentPlayer.currentClass == Player.PlayerClass.Warrior) {
                        Program.Print("You swing your " + Program.currentPlayer.equippedWeapon + " and " + n + " retaliates.", 15);
                    } else if (Program.currentPlayer.currentClass== Player.PlayerClass.Mage) {
                        Program.Print("You shoot an arcane missile from your " + Program.currentPlayer.equippedWeapon + " and " + n + " retaliates.", 10);
                    } else {
                        Program.Print("You fire an arrow with your " + Program.currentPlayer.equippedWeapon + " and " + n + " retaliates.", 10);
                    }
                    int damage = p - Program.currentPlayer.armorValue;
                    if (damage < 0)
                        damage = 0;
                    int attack = Program.rand.Next(1, 1+Program.currentPlayer.weaponValue) + Program.rand.Next(0, 4) + ((Program.currentPlayer.currentClass==Player.PlayerClass.Warrior)?2:0);
                    Program.Print("You lose " + damage + " health and you deal " + attack + " damage" ,20);
                    Program.currentPlayer.health -= damage;
                    h -= attack;
                } else if (input.ToLower() == "d" || input == "defend") {
                    //Defend
                    Program.Print("You defend the incoming attack from " + n, 20);
                    int damage = 1+(p / 4) - Program.currentPlayer.armorValue;
                    if (damage < 0)
                        damage = 0;
                    int attack = Program.rand.Next(0, 4+Program.currentPlayer.weaponValue) / 2;
                    Program.Print("You lose " + damage + " health and you deal " + attack + " damage", 20);
                    Program.currentPlayer.health -= damage;
                    h -= attack;
                } else if (input.ToLower() == "r" || input == "run") {
                    //Run
                    if (Program.currentPlayer.currentClass != Player.PlayerClass.Archer && Program.rand.Next(0, 2) == 0 || n == "Human captor") {
                        Program.Print("You try to sprint away from the " + n + ", it strikes and knocks you down", 20);
                        int damage = p - Program.currentPlayer.armorValue;
                        if (damage < 0)
                            damage = 0;
                        Program.Print("You lose " + damage + " health and are unable to escape.", 20);
                        Program.currentPlayer.health -= damage;

                    } else {
                        if (Program.currentPlayer.currentClass == Player.PlayerClass.Archer) {
                            Program.Print("You use your crazy ninja moves to evade the " + n + " and you successfully escape!");
                        } else {
                            Program.Print("You barely manage to shake off the " + n + " and you successfully escape.");
                        }
                        
                        Console.ReadKey();
                        Shop.Loadshop(Program.currentPlayer);
                        break;
                    }
                } else if (input.ToLower() == "h" || input == "heal") {
                    //Heal
                    if (Program.currentPlayer.potion == 0) {
                        Program.Print("No potions left!", 20);
                        int damage = p - Program.currentPlayer.armorValue;
                        if (damage < 0)
                            damage = 0;
                        Program.Print("The " + n + " attacks you while you fumble in your bags and lose " + damage + " health!", 20);
                        Program.currentPlayer.health -= damage;
                    } else {
                        if (Program.currentPlayer.currentClass == Player.PlayerClass.Mage) {
                            Program.Print("You use a potion amplified by your magic", 30);
                        } else {
                            Program.Print("You use a potion", 20);
                        }
                        Program.currentPlayer.health += Program.currentPlayer.potionValue + ((Program.currentPlayer.currentClass==Player.PlayerClass.Mage)?+4:0);
                        if (Program.currentPlayer.health > Program.currentPlayer.maxHealth) {
                            Program.currentPlayer.health = Program.currentPlayer.maxHealth;
                        }
                        Program.currentPlayer.potion -= 1;
                        if (Program.currentPlayer.health == Program.currentPlayer.maxHealth) {
                            Program.Print("You heal to max health!", 20);
                        } else {
                            Program.Print("You gain " + Program.currentPlayer.potionValue + " health", 20);
                        }
                        Program.Print("As you drink, the " + n + " strikes you.", 20);
                        int damage = (p / 2) - Program.currentPlayer.armorValue;
                        if (damage < 0)
                            damage = 0;
                        Program.Print("You lose " + damage + " health", 20);
                        Program.currentPlayer.health -= damage;
                    }
                } else if (input.ToLower() == "c" || input == "character" || input == "character screen") {
                    Player.CharacterScreen();
                }

                if (Program.currentPlayer.health <= 0) {
                    //Death code
                    Sounds.soundGameOver.Play();
                    Console.ForegroundColor = ConsoleColor.DarkRed;
                    Program.Print("As the " + n + " menacingly comes down to strike, you are slain by the mighty " + n, 25);
                    Console.ReadKey(true);
                    Console.ResetColor();
                    System.Environment.Exit(0);
                }
                Program.Print("Press to continue...", 1);
                Console.ReadKey(true);
            }
            if (h <= 0)
            {
                //Loot
                Sounds.soundWin.Play();
                int g = Program.currentPlayer.GetGold();
                int x = Program.currentPlayer.GetXP() + ((n == "Dark Wizard")?+Program.currentPlayer.GetXP():0);
                int[] numbers = new[] { 0, 0, 0, 0, 0, 0, 0, 1, 1, 1, 2 };
                var pot = Program.rand.Next(0, numbers.Length);
                Program.Print("You Won against the " + n + "! " + x + " experience points gained.", 15);
                Console.ForegroundColor = ConsoleColor.DarkYellow;
                Program.Print("You loot " + g + " gold coins.", 15);
                Console.ResetColor();
                if (numbers[pot] != 0)
                {
                    Program.Print("You loot " + numbers[pot] + " healing potions", 20);
                    Program.currentPlayer.potion += numbers[pot];
                }
                Program.currentPlayer.gold += g;
                Program.currentPlayer.xp += x;
                Console.ReadKey(true);
                if (Program.currentPlayer.CanLevelUp()) {
                    Program.currentPlayer.LevelUp();
                }
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
