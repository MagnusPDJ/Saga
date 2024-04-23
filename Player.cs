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
        public int weaponValue = 0;
        public int armorValue = 0;
        public int potion = 5;
        public int potionValue = 5;

        public string equippedWeapon = null;
        public int equippedWeaponValue = 0;
        public string equippedArmor = null;
        public int equippedArmorValue = 0;


        //Player classes that can be picked
        public enum PlayerClass {Mage, Archer, Warrior};
        public PlayerClass currentClass = PlayerClass.Warrior;
       
        //Metode til at get total weapon.
        public int TotalWeaponValue() {
            return Program.currentPlayer.weaponValue + Program.currentPlayer.equippedWeaponValue;
        }
        //Metode til at get total armor.
        public int TotalArmorValue() {
            return Program.currentPlayer.armorValue + Program.currentPlayer.equippedArmorValue;
        }

        //Monster liv skaleret på spilleren.
        public int GetHealth(string name) {
            int baseModUp = 3 + 2 * level;
            int baseModLower = 4 + level;
            switch (name) {
                case "Vampire":
                    int upper6 = (7+baseModUp);
                    int lower6 = (4+baseModLower);
                    return Program.rand.Next(lower6, upper6 + 1);
                case "Werewolf":
                    int upper7 = (6+baseModUp);
                    int lower7 = (4+baseModLower);
                    return Program.rand.Next(lower7, upper7 + 1);
                case "Dire Wolf":
                    int upper8 = (5+baseModUp);
                    int lower8 = (4+baseModLower);
                    return Program.rand.Next(lower8, upper8 + 1);
                case "Human Cultist":
                    int upper2 = (4+baseModUp);
                    int lower2 = (3+baseModLower);
                    return Program.rand.Next(lower2, upper2 + 1);
                case "Human Rogue":
                    int upper5 = (3+baseModUp);
                    int lower5 = (2+baseModLower);
                    return Program.rand.Next(lower5, upper5 + 1);
                case "Bandit":
                    int upper9 = (3+baseModUp+3);
                    int lower9 = (2+baseModLower+2);
                    return Program.rand.Next(lower9, upper9 + 1);
                case "Skeleton":
                    int upper = (2+baseModUp);
                    int lower = (2+baseModLower);
                    return Program.rand.Next(lower, upper + 1);
                case "Zombie":
                    int upper1 = (2+baseModUp+2);
                    int lower1 = (2+baseModLower+1);
                    return Program.rand.Next(lower1, upper1 + 1);
                case "Grave Robber":
                    int upper3 = (1+baseModUp);
                    int lower3 = (1+baseModLower);
                    return Program.rand.Next(lower3, upper3 + 1);
                case "Giant Bat":
                    int upper4 = (baseModUp-1);
                    int lower4 = (baseModLower-2);
                    return Program.rand.Next(lower4, upper4 + 1);
                case "Giant Rat":
                    int upper0 = (baseModUp);
                    int lower0 = (baseModLower);
                    return Program.rand.Next(lower0, upper0 + 1);
            }
            return 0;
        }

        //Monster skade skaleret på spilleren.
        public int GetPower(string name) {
            int baseModUp = 3 + 2 * level;
            int baseModLower = 4 + level;
            switch (name) {
                case "Vampire":
                    int upper6 = (7+baseModUp);
                    int lower6 = (4+baseModLower);
                    return Program.rand.Next(lower6, upper6 + 1);
                case "Werewolf":
                    int upper7 = (6+baseModUp);
                    int lower7 = (4+baseModLower);
                    return Program.rand.Next(lower7, upper7 + 1);
                case "Dire Wolf":
                    int upper8 = (5+baseModUp);
                    int lower8 = (4+baseModLower);
                    return Program.rand.Next(lower8, upper8 + 1);
                case "Human Cultist":
                    int upper2 = (4+baseModUp);
                    int lower2 = (3+baseModLower);
                    return Program.rand.Next(lower2, upper2 + 1);
                case "Human Rogue":
                    int upper5 = (2+baseModUp+3);
                    int lower5 = (2+baseModLower+1);
                    return Program.rand.Next(lower5, upper5 + 1);
                case "Bandit":
                    int upper9 = (2+baseModUp);
                    int lower9 = (2+baseModLower);
                    return Program.rand.Next(lower9, upper9 + 1);
                case "Skeleton":
                    int upper = (baseModUp+2);
                    int lower = (baseModLower+level-level/3);
                    return Program.rand.Next(lower, upper + 1);
                case "Zombie":
                    int upper1 = (baseModUp);
                    int lower1 = (baseModLower);
                    return Program.rand.Next(lower1, upper1 + 1);
                case "Grave Robber":
                    int upper3 = (baseModUp-level/4);
                    int lower3 = (baseModLower-level/4);
                    return Program.rand.Next(lower3, upper3 + 1);
                case "Giant Bat":
                    int upper4 = (baseModUp-1-level/2-level/5);
                    int lower4 = (baseModLower-1-level/2);
                    return Program.rand.Next(lower4, upper4 + 1);
                case "Giant Rat":
                    int upper0 = (baseModUp-3-level/2-level/5);
                    int lower0 = (baseModLower-3-level/2);
                    return Program.rand.Next(lower0, upper0 + 1);
            }
            return 0;
        }

        //Guld drop skaleret på spilleren.
        public int GetGold() {
            int upper = (50 * level+100);
            int lower = (20 * level);
            return Program.rand.Next(lower, upper+1);
        }

        //Metode til at udregne exp fåen skaleret på spilleren.
        public int GetXP() {
            int upper = (20*level + 50);
            int lower = (10*level + 10);
            return Program.rand.Next(lower, upper+1);
        }

        //Metode til udregning af det exp det koster at level op.
        public int GetLevelUpValue() {
            return Convert.ToInt32(5000000/(1+10000*Math.Pow(1.2,1-level)));
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
            AudioManager.soundLvlUp.Play();
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
            Program.PlayerPrompt();
        }

        //Metode til at kalde og gernerer en character screen som viser alle funktionelle variabler der er i brug.
        public static void CharacterScreen() { 
            Console.Clear();
            Console.WriteLine("~~~~~~~~~~~~Character screen~~~~~~~~~~~~");
            Program.Print($"Name: {Program.currentPlayer.name}\tClass: {Program.currentPlayer.currentClass}",10);
            Program.Print($"Level: {Program.currentPlayer.level}", 10);
            Program.Print($"EXP  [{Program.ProgressBarForPrint("+", " ", ((decimal)Program.currentPlayer.xp / (decimal)Program.currentPlayer.GetLevelUpValue()), 25)}] {Program.currentPlayer.xp}/{Program.currentPlayer.GetLevelUpValue()}",10);
                Program.Print("---------------Stats--------------------",10);
            Console.WriteLine($"Max Health: { Program.currentPlayer.maxHealth}\t\tCurrent Health: {Program.currentPlayer.health}");
            Console.WriteLine($"Weapon Damage: {1+(Program.currentPlayer.TotalWeaponValue() + 0 + ((Program.currentPlayer.currentClass == Player.PlayerClass.Warrior) ? 1 + Program.currentPlayer.level : 0))/2}-{1 + Program.currentPlayer.TotalWeaponValue()+4+ ((Program.currentPlayer.currentClass == Player.PlayerClass.Warrior) ? 1 + Program.currentPlayer.level : 0)}\tTotal Armor Rating: {Program.currentPlayer.TotalArmorValue()}");
            Console.WriteLine("\n**************Equipment*****************\n");
            Console.WriteLine($"Healing Potions: {Program.currentPlayer.potion}\t\tGold: ${Program.currentPlayer.gold}");
            Console.WriteLine($"Weapon: {Program.currentPlayer.equippedWeapon} (+{Program.currentPlayer.equippedWeaponValue} dmg)\tWeapon upgrades: {Program.currentPlayer.weaponValue}");
            Console.WriteLine($"Armor: {Program.currentPlayer.equippedArmor} (+{Program.currentPlayer.equippedArmorValue} armor)\t\tArmor upgrades: {Program.currentPlayer.armorValue}");
            Console.WriteLine("");
        }

        //Metode til at checke for om spilleren dør som kan kaldes hver gang spilleren tager skade.
        public static void DeathCode(string message) {
                AudioManager.soundKamp.Stop();
                AudioManager.soundTroldmandsKamp.Stop();
                AudioManager.soundGameOver.Play();
                Console.ForegroundColor = ConsoleColor.DarkRed;
                Program.Print(message,20);
                Console.ResetColor();
                Program.PlayerPrompt();
                Program.MainMenu();
        }

        //Metode til at genere loot
        public static void Loot(int xpModifier, int goldModifier, string message) {
                int g = Program.currentPlayer.GetGold()*goldModifier;
                int x = Program.currentPlayer.GetXP() * xpModifier;
                int[] numbers = new[] { 0, 0, 0, 0, 0, 0, 0, 1, 1, 1, 2 };
                var pot = Program.rand.Next(0, numbers.Length);
                Program.Print(message, 15);
                Console.ForegroundColor = ConsoleColor.DarkGreen;
                Program.Print($"You've gained {x} experience points!",10);
                Console.ForegroundColor = ConsoleColor.DarkYellow;
                Program.Print($"You loot {g} gold coins.", 15);
                Console.ResetColor();
                if (numbers[pot] != 0) {
                    Console.ForegroundColor = ConsoleColor.DarkGray;
                    Program.Print($"You loot {numbers[pot]} healing potions", 20);
                    Console.ResetColor();
                    Program.currentPlayer.potion += numbers[pot];
                }
                Program.currentPlayer.gold += g;
                Program.currentPlayer.xp += x;
                Program.PlayerPrompt();
        }
    }
}
