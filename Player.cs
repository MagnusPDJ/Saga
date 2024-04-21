using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Saga
{
    [Serializable]
    public class Player
    {
        //Player Stats
        public string name;
        public int id;
        public int gold = 0;
        public int level = 1;
        public int xp = 0;
        public int maxHealth = 10;
        public int health = 10;
        public int damage = 1;
        public int armorValue = 0 + equippedArmorValue;
        public int potion = 5;
        public int weaponValue = 0 + equippedWeaponValue;
        public int potionValue = 5;

        public int mods = 0;

        public string equippedWeapon = null;
        public static int equippedWeaponValue = 0;
        public string equippedArmor = null;
        public static int equippedArmorValue = 0;

        //Player classes that can be picked
        public enum PlayerClass {Mage, Archer, Warrior};
        public PlayerClass currentClass = PlayerClass.Warrior;

        //Monster liv skaleret på spilleren.
        public int GetHealth() {
            int upper = (2 * level+4);
            int lower = (level+1);
            return Program.rand.Next(lower, upper+1);
        }

        //Monster skade skaleret på spilleren.
        public int GetPower() {
            int upper = (2 * level + 2);
            int lower = (level);
            return Program.rand.Next(lower, upper+1);
        }

        //Guld drop skaleret på spilleren.
        public int GetGold() {
            int upper = (50 * level+100);
            int lower = (10 * level);
            return Program.rand.Next(lower, upper+1);
        }

        //Metode til at udregne exp fåen skaleret på spilleren.
        public int GetXP() {
            int upper = (20*level + 50);
            int lower = (15*level + 10);
            return Program.rand.Next(lower, upper+1);
        }

        //Metode til udregning af det exp det koster at level op.
        public int GetLevelUpValue() {
            return 100 * level + 400;
        }

        //Metode til at tjekke om lvl op er muligt
        public bool CanLevelUp() {
            if (xp >= GetLevelUpValue()) {
                return true;
            } else {
                return false;
            }
        }

        //Metode til at lvl spilleren op.
        public void LevelUp() {
            Sounds.soundLvlUp.Play();
            while(CanLevelUp()) {
                xp-=GetLevelUpValue();
                level++;
            }
            int h = 2+(level/3);
            maxHealth += h;
            Console.ForegroundColor = ConsoleColor.DarkBlue;
            Program.Print($"Congratulations! You are now level {level}! Your max health increased by {h}.");
            Console.ResetColor();
            Program.currentPlayer.health = maxHealth;
            Console.ReadKey(true);
        }

        //Metode til at kalde og gernerer en character screen som viser alle funktionelle variabler der er i brug.
        public static void CharacterScreen() { 
            Console.Clear();
            Console.WriteLine("~~~~~~~~~~Character screen~~~~~~~~~~");
            Program.Print($"Name: {Program.currentPlayer.name}\tClass: {Program.currentPlayer.currentClass}",10);
            Program.Print($"Level: {Program.currentPlayer.level}", 10);
            Program.Print($"EXP  [{Program.ProgressBarForPrint("+", " ", ((decimal)Program.currentPlayer.xp / (decimal)Program.currentPlayer.GetLevelUpValue()), 25)}]",10);
            Program.Print("------------------------------------",10);
            Console.WriteLine($"Max Health: { Program.currentPlayer.maxHealth}\t\tCurrent Health: {Program.currentPlayer.health}");
            Console.WriteLine($"Weapon Damage: {1+0 + ((Program.currentPlayer.currentClass == Player.PlayerClass.Warrior) ? 2 : 0)}-{1 + Program.currentPlayer.weaponValue+4+ ((Program.currentPlayer.currentClass == Player.PlayerClass.Warrior) ? 2 : 0)}\tArmor Rating: {Program.currentPlayer.armorValue}");
            Console.WriteLine($"Healing Potions: {Program.currentPlayer.potion}\tGold: ${Program.currentPlayer.gold}");
            Console.WriteLine($"Weapon: {Program.currentPlayer.equippedWeapon}\tWeapon damage: +{equippedWeaponValue}");
            Console.WriteLine($"Armor: {Program.currentPlayer.equippedArmor}\t\tArmor rating: +{equippedArmorValue}");
            Console.WriteLine("");
        }

        //Metode til at checke for om spilleren dør som kan kaldes hver gang spilleren tager skade.
        public static void DeathCode(string message) {
            if (Program.currentPlayer.health <= 0) {
                Sounds.soundGameOver.Play();
                Console.ForegroundColor = ConsoleColor.DarkRed;
                Program.Print(message,25);
                Console.ResetColor();
                Console.ReadKey(true);
                Program.MainMenu();   
            }
        }

        //Metode til at genere loot
        public static void Loot(string monster, string message) {
                Sounds.soundWin.Play();
                int g = Program.currentPlayer.GetGold();
                int x = Program.currentPlayer.GetXP() * ((monster == "Dark Wizard") ? + 2 : 0);
                int[] numbers = new[] { 0, 0, 0, 0, 0, 0, 0, 1, 1, 1, 2 };
                var pot = Program.rand.Next(0, numbers.Length);
                Program.Print(message, 15);
                Console.ForegroundColor = ConsoleColor.DarkGreen;
                Program.Print("You've gained "+ x + " experience points!",10);
                Console.ForegroundColor = ConsoleColor.DarkYellow;
                Program.Print("You loot " + g + " gold coins.", 15);
                Console.ResetColor();
                if (numbers[pot] != 0) {
                    Console.ForegroundColor = ConsoleColor.DarkGray;
                    Program.Print("You loot " + numbers[pot] + " healing potions", 20);
                    Console.ResetColor();
                    Program.currentPlayer.potion += numbers[pot];
                }
                Program.currentPlayer.gold += g;
                Program.currentPlayer.xp += x;
                Console.ReadKey(true);
        }
    }
}
