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
            int upper = (2 * level+5);
            int lower = (level+2);
            return Program.rand.Next(lower, upper);
        }

        //Monster skade skaleret på spilleren.
        public int GetPower() {
            int upper = (2 * level + 3);
            int lower = (level + 1);
            return Program.rand.Next(lower, upper);
        }

        //Guld drop skaleret på spilleren.
        public int GetGold() {
            int upper = (15 * level + 50);
            int lower = (10 * level + 10);
            return Program.rand.Next(lower, upper);
        }

        //Metode til at udregne exp fåen skaleret på spilleren.
        public int GetXP() {
            int upper = (20*level + 50);
            int lower = (15*level + 10);
            return Program.rand.Next(lower, upper);
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
            Program.Print("Congratulations! You are now level " + level + "! Your max health increased by " + h);
            Console.ResetColor();
            Program.currentPlayer.health = maxHealth;
            Console.ReadKey(true);
        }

        public static void CharacterScreen() { 
            Console.Clear();
            Console.WriteLine("~~~~~~~~~~Character screen~~~~~~~~~~");
            Program.Print("Name: " + Program.currentPlayer.name + "\tClass: " + Program.currentPlayer.currentClass);
            Console.WriteLine("Level: " + Program.currentPlayer.level);
            Program.Print("EXP  ["+Program.ProgressBarForPrint("+", " ", ((decimal)Program.currentPlayer.xp / (decimal)Program.currentPlayer.GetLevelUpValue()), 25)+"]");
            Program.Print("------------------------------------");
            Console.WriteLine("Max Health: " + Program.currentPlayer.maxHealth + "\t\tCurrent Health: " + Program.currentPlayer.health);
            Console.WriteLine("Healing Potions: " + Program.currentPlayer.potion + "\tGold:  $" + Program.currentPlayer.gold);
            Console.WriteLine("Weapon: " + Program.currentPlayer.equippedWeapon + "\tWeapon damage: +" + equippedWeaponValue);
            Console.WriteLine("Armor: " + Program.currentPlayer.equippedArmor + "\t\tArmor rating: +" + equippedArmorValue);
            Console.WriteLine("");
        }

    }
}
