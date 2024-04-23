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
            switch (Program.currentPlayer.currentClass.ToString()) {
                case "Warrior":
                    Program.currentPlayer.equippedWeapon = "Rusty Sword";
                    Program.currentPlayer.equippedWeaponValue = 1;
                    Program.currentPlayer.equippedArmor = "Rags";
                    Program.currentPlayer.equippedArmorValue = 2;
                    break;
                case "Archer":
                    Program.currentPlayer.equippedWeapon = "Flimsy Bow";
                    Program.currentPlayer.equippedWeaponValue = 1;
                    Program.currentPlayer.equippedArmor = "Rags";
                    Program.currentPlayer.equippedArmorValue = 2;
                    break;
                case "Mage":
                    Program.currentPlayer.equippedWeapon = "Cracked Wand";
                    Program.currentPlayer.equippedWeaponValue = 1;
                    Program.currentPlayer.equippedArmor = "Rags";
                    Program.currentPlayer.equippedArmorValue = 2;
                    break;
            }
            Program.Print($"You throw open the door, grabbing a {Program.currentPlayer.equippedWeapon}, while charging toward your captor.");
            AudioManager.soundMainMenu.Stop();
            AudioManager.soundTaunt.Play();
            AudioManager.soundKamp.Play();
            Program.Print("He turns...");
            Program.PlayerPrompt();
            BasicCombat(false, "Human captor", 3, 5);
        }

        //Encounter som køres introducere shopkeeperen
        public static void ShopEncounter() {
            AudioManager.soundShop.Play();
            Console.Clear();
            AudioManager.soundTypeWriter.Play();
            if (Program.currentPlayer.currentClass == Player.PlayerClass.Mage) {
                Program.Print($"After dusting off your {Program.currentPlayer.equippedArmor} and tucking in your new wand, you find someone else captured.");
            } else if (Program.currentPlayer.currentClass==Player.PlayerClass.Archer) {
                Program.Print("After retrieving the last arrow from your captor's corpse, you find someone else captured.");
            } else {
                Program.Print("After cleaning the blood from your captor off your new sword, you find someone else captured.");
            }
            Program.Print("Freeing him from his shackles, he thanks you and gets up.");
            Program.Print("'Gheed is the name and trade is my game', he gives a wink.");
            Program.PlayerPrompt();
            Console.Clear();
            AudioManager.soundTypeWriter.Play();
            Program.Print("'If you go and clear some of the other rooms, I will look for my wares in these crates.'");
            Program.Print("'Then Run back to me, I will then have been able to set up a shop where you can spend ");
            Program.Print("some of that gold you are bound to have found,' he chuckles and rubs his hands at the thought.");
            Program.Print($"You nod and prepare your {Program.currentPlayer.equippedWeapon}, then you start walking down a dark corridor...");
            Program.PlayerPrompt();
        }

        //Encounter der "spawner" en random fjende som skal dræbes.
        public static void BasicFightEncounter() {
            Console.Clear();
            AudioManager.soundShop.Stop();
            AudioManager.soundMainMenu.Stop();
            AudioManager.soundTroldmandsKamp.Stop();
            AudioManager.soundKamp.Play();
            string n = GetName();
            switch (Program.rand.Next(0,2)) {
                case int x when (x == 0):
                    Program.Print($"You turn a corner and there you see a {n}...", 20);
                    break;
                case int x when (x == 1):
                    Program.Print($"You break down a door and find a {n} inside!", 20);
                    break;
            }
            Program.PlayerPrompt();
            BasicCombat(true, n, 0, 0);
        }

        //Encounter der "spawner" en specifik fjende som skal dræbes.
        public static void WizardEncounter() {
            Console.Clear();
            AudioManager.soundMainMenu.Stop();
            AudioManager.soundKamp.Stop();
            AudioManager.soundLaugh.Play();
            //Wizardbattlemusic
            Program.Print("The door slowly creaks open as you peer into the dark room. You see a tall man with a ",35);
            AudioManager.soundTroldmandsKamp.Play();
            Program.Print("long beard and pointy hat, looking at a large tome.");
            
            Program.PlayerPrompt();
            BasicCombat(false, "Dark Wizard", 6+2*Program.currentPlayer.level, 2+2*Program.currentPlayer.level+Program.currentPlayer.level/3);
        }

     //Encounter Tools
        //Metode til at vælge tilfældigt mellem encounters.
        public static void RandomEncounter() {
            switch (Program.rand.Next(0, 10+1)) {
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
            int t =1;

            if (random) {
                n = name;
                p = Program.currentPlayer.GetPower(n);
                h = Program.currentPlayer.GetHealth(n);
            } else {
                n = name;
                p = power;
                h = health;
            }
            Console.Clear();
            Program.Print($"Turn: {t}");
            Program.Print($"Fighting: {n}!", 20);
            Program.Print($"Strength: {p} / HP: {h}", 20);
            Program.Print("-----------------------", 20);
            while (h > 0) {
                Console.Clear();
                Console.WriteLine($"Turn: {t}");
                Console.WriteLine($"Fighting: {n}!");
                Console.WriteLine($"Strength: {p} / HP: {h}");
                Console.WriteLine("---------------------------");
                Console.WriteLine($"{Program.currentPlayer.currentClass} {Program.currentPlayer.name}'s Stats:");
                Console.WriteLine($"Health: {Program.currentPlayer.health}\t|| Healing Potions: {Program.currentPlayer.potion}");
                Console.WriteLine($"Level: {Program.currentPlayer.level}\t|| Gold: ${Program.currentPlayer.gold}");
                Console.Write("EXP  ");
                Console.Write("[");
                Program.ProgressBar("+", " ", (decimal)Program.currentPlayer.xp / (decimal)Program.currentPlayer.GetLevelUpValue(), 20);
                Console.WriteLine("]");
                Console.WriteLine("==========Actions==========");
                Console.WriteLine("| (A)ttack     (D)efend   |");
                Console.WriteLine("| (R)un        (H)eal     |");
                Console.WriteLine("| (C)haracter screen      |");
                Console.WriteLine("===========================");
                Console.WriteLine("Choose an action...");
                string input = Program.PlayerPrompt().ToLower();

                if (input.ToLower() == "a" || input == "attack") {
                    //Attack
                    if (Program.currentPlayer.currentClass == Player.PlayerClass.Warrior) {
                        Program.Print($"You swing your {Program.currentPlayer.equippedWeapon} and {n} retaliates.", 15);
                    } else if (Program.currentPlayer.currentClass== Player.PlayerClass.Mage) {
                        Program.Print($"You shoot an arcane missile from your {Program.currentPlayer.equippedWeapon} and {n} retaliates.", 10);
                    } else {
                        Program.Print($"You fire an arrow with your {Program.currentPlayer.equippedWeapon} and {n} retaliates.", 10);
                    }
                    int damage = p - Program.currentPlayer.TotalArmorValue();
                    if (damage < 0)
                        damage = 0;
                    int attack = Program.rand.Next((1+(Program.currentPlayer.TotalWeaponValue()) + ((Program.currentPlayer.currentClass == Player.PlayerClass.Warrior) ? 1 + Program.currentPlayer.level : 0))/2, 1+Program.currentPlayer.TotalWeaponValue()) + Program.rand.Next(0, 4) + ((Program.currentPlayer.currentClass==Player.PlayerClass.Warrior)?1+Program.currentPlayer.level:0);
                    Program.Print($"You lose {damage} health and you deal {attack} damage" ,20);
                    Program.currentPlayer.health -= damage;
                    h -= attack;
                    t++;
                } else if (input.ToLower() == "d" || input == "defend") {
                    //Defend
                    Program.Print($"You defend the incoming attack from {n}", 20);
                    int damage = (p / Program.currentPlayer.TotalArmorValue()) ;
                    if (damage < 0)
                        damage = 0;
                    int attack = Program.rand.Next(1+Program.currentPlayer.TotalWeaponValue() / 3, (4+Program.currentPlayer.TotalWeaponValue()) / 2);
                    Program.Print($"You lose {damage} health and you deal {attack} damage", 20);
                    Program.currentPlayer.health -= damage;
                    h -= attack;
                    t++;
                } else if (input.ToLower() == "r" || input == "run") {
                    //Run
                    if (Program.currentPlayer.currentClass != Player.PlayerClass.Archer && Program.rand.Next(0, 2) == 0 || n == "Human captor") {
                        Program.Print($"You try to sprint away from the {n}, it strikes and knocks you down", 20);
                        int damage = p - Program.currentPlayer.TotalArmorValue();
                        if (damage < 0)
                            damage = 0;
                        Program.Print($"You lose {damage} health and are unable to escape this round.", 20);
                        Program.currentPlayer.health -= damage;
                        t++;
                    } else {
                        if (Program.currentPlayer.currentClass == Player.PlayerClass.Archer) {
                            Program.Print($"You use your crazy ninja moves to evade the {n} and you successfully escape!");
                        } else {
                            Program.Print($"You barely manage to shake off the {n} and you successfully escape.");
                        }
                        Program.PlayerPrompt();
                        Shop.Loadshop(Program.currentPlayer);
                        break;
                    }
                } else if (input.ToLower() == "h" || input == "heal") {
                    //Heal
                    if (Program.currentPlayer.potion == 0) {
                        Program.Print("No potions left!", 20);
                        int damage = p - Program.currentPlayer.TotalArmorValue();
                        if (damage < 0)
                            damage = 0;
                        Program.Print($"The {n} attacks you while you fumble in your bags and lose {damage} health!", 20);
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
                        Program.currentPlayer.potion --;
                        if (Program.currentPlayer.health == Program.currentPlayer.maxHealth) {
                            Program.Print("You heal to max health!", 20);
                        } else {
                            Program.Print($"You gain {Program.currentPlayer.potionValue} health", 20);
                        }
                        Program.Print($"As you drink, the {n} strikes you.", 20);
                        int damage = (p / 2) - Program.currentPlayer.TotalArmorValue();
                        if (damage < 0)
                            damage = 0;
                        Program.Print($"You lose {damage} health", 20);
                        Program.currentPlayer.health -= damage;
                    }
                    t++;
                } else if (input.ToLower() == "c" || input == "character" || input == "character screen") {
                    Player.CharacterScreen();
                }
                //Død
                Player.DeathCode($"As the {n} menacingly comes down to strike, you are slain by the mighty {n}.");
                Program.Print("Press to continue...", 1);
                Program.PlayerPrompt();
            }
            if (h <= 0) {
                AudioManager.soundKamp.Stop();
                AudioManager.soundWin.Play();
                AudioManager.soundTroldmandsKamp.Stop();
                Player.Loot(n, $"You Won against the {n} on turn {t-1}!");
                if (Program.currentPlayer.CanLevelUp()) {
                    Program.currentPlayer.LevelUp();
                }
            }
        }
        
        //Monster navne/type låst efter level
        public static string GetName() {
            if (Program.currentPlayer.level < 3) {
                switch (Program.rand.Next(0, 2 + 1)) {
                    case 0:
                        return "Giant Rat";
                    case 1:
                        return "Grave Robber";
                    case 2:
                        return "Giant Bat";
                }
            } else if (Program.currentPlayer.level <= 5) {
                switch (Program.rand.Next(0, 4 + 1)) {
                    case 0:
                        return "Skeleton";
                    case 1:
                        return "Zombie";
                    case 2:
                        return "Giant Rat";
                    case 3:
                        return "Grave Robber";
                    case 4:
                        return "Giant Bat";
                }
            } else if (5 < Program.currentPlayer.level &&Program.currentPlayer.level <= 15) {
                switch (Program.rand.Next(0, 8 + 1)) {
                    case 0:
                        return "Skeleton";
                    case 1:
                        return "Zombie";
                    case 2:
                        return "Human Cultist";
                    case 3:
                        return "Grave Robber";
                    case 4:
                        return "Giant Bat";
                    case 5:
                        return "Human Rogue";
                    case 6:
                        return "Giant Rat";
                    case 7:
                        return "Bandit";
                    case 8:
                        return "Dire Wolf";
                }
            } switch(Program.rand.Next(0,6+1)) {
                case 0:
                    return "Human Cultist";
                case 1:
                    return "Skeleton";
                case 2:
                    return "Human Rogue";
                case 3:
                    return "Vampire";
                case 4:
                    return "Werewolf";
                case 5:
                    return "Dire Wolf";
                case 6:
                    return "Bandit";
            }
            return "";
        }
    }
}
