using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Saga
{
    internal class HUDTools
    {
        //Get metode til at få teksten fra memory til filen.
        public static TextWriter Out { get; }

        //Metode til at toggle ReadLine/ReadKey baseret på spiller settings.
        public static string PlayerPrompt() {
            if (Convert.ToBoolean(ConfigurationManager.AppSettings.Get("toggleReadLine")) == true) {
                return Console.ReadLine().ToLower();
            }
            else {
                string x = Console.ReadKey().KeyChar.ToString();
                Console.WriteLine("");
                return x;
            }
        }

        //Metode til at "Slow-print" tekst, med indbygget toggle setting.
        public static void Print(string text, int time = 40) {
            if (Convert.ToBoolean(ConfigurationManager.AppSettings.Get("toggleSlowPrint")) == true) {
                Task t = Task.Run(() => {
                    foreach (char c in text) {
                        Console.Write(c);
                        Thread.Sleep(time);
                    }
                    Console.WriteLine();
                });
                t.Wait();
            }
            else {
                Task t = Task.Run(() => {
                    foreach (char c in text) {
                        Console.Write(c);
                        Thread.Sleep(0);
                    }
                    Console.WriteLine();
                });
                t.Wait();
            }
        }

        //En metode til at printe en progress bar til f.eks. lvl progress (Måske redundant?).
        public static void ProgressBar(string fillerChar, string backgroundChar, decimal value, int size) {
            int dif = (int)(value * size);
            for (int i = 0; i < size; i++) {
                if (i < dif) {
                    Console.Write(fillerChar);
                }
                else {
                    Console.Write(backgroundChar);
                }
            }
        }

        //Samme metode men til brug sammen med slow print metoden.
        public static string ProgressBarForPrint(string fillerChar, string backgroundChar, decimal value, int size) {
            int dif = (int)(value * size);
            string output = "";
            for (int i = 0; i < size; i++) {
                if (i < dif) {
                    output += fillerChar;
                }
                else {
                    output += backgroundChar;
                }
            }
            return output;
        }

        // Metode til at skrive en logfil 
        public static void WriteCombatLog(string action, int t, int damage, int attack) {
            if (!File.Exists("combatlog.txt")) {
                File.Create("combatlog.txt");
            }

            //Læser logfilen og gemmer det i memory
            string text = File.ReadAllText("combatlog.txt");

            // Åbner en text file navngivet "Geeks"  
            // at the location of your program 
            FileStream geeks1 = new FileStream("combatlog.txt", FileMode.Open);

            //Laver et objekt som kan skrive til Logfilen
            StreamWriter portal1 = new StreamWriter(geeks1);

            // Standard Output stream is  
            // being saved to a Textwriter 
            TextWriter geeksave = Console.Out;

            //Klar gør objektet til at skrive til memory.
            Console.SetOut(portal1);

            //Skriver den gemte tekst.
            Console.Write($"{text}\n");

            //Skriver og tilføjer den nye tekst.
            switch (action) {
                case "attack":
                    Console.WriteLine($"Turn: {t}\nYou attacked and lost {damage} health and you dealt {attack} damage.");
                    break;
                case "defend":
                    Console.WriteLine($"Turn: {t}\nYou defended and lost {damage} health and you dealt {attack} damage.");
                    break;
                case "heal":
                    Console.WriteLine($"Turn: {t}");
                    if (Program.currentPlayer.potion == 0) {
                        Console.WriteLine($"You tried to drink a potion you didn't have and lost {damage} health.");
                    }
                    else {
                        if (Program.currentPlayer.health == Program.currentPlayer.maxHealth) {
                            Console.WriteLine("You healed to max health by drinking a potion.");
                        }
                        else {
                            Console.WriteLine($"You gained {Program.currentPlayer.potionValue + ((Program.currentPlayer.currentClass == Player.PlayerClass.Mage) ? 3 + Program.currentPlayer.level : 0)} health by drinking a potion.");
                        }
                        Console.WriteLine($"You lost {damage} health while drinking the potion.");
                    }
                    break;
                case "run":
                    Console.WriteLine($"Turn: {t}");
                    Console.WriteLine($"You tried to run. You lost {damage} health and was unable to escape this turn.");
                    break;
            }
            //Skriver teksten i memory til filen.
            Console.SetOut(geeksave);

            //Lukker objektet og filen igen.
            portal1.Close();
        }

        //Skriver Loggen i consolen.
        public static void GetCombatLog() {
            string text = File.ReadAllText("combatlog.txt");
            Console.WriteLine(text);
        }

        //Rydder Log filen så den er klar til brug igen.
        public static void ClearCombatLog() {
            File.WriteAllText("combatlog.txt", String.Empty);
        }

        //HUDS
        public static void SlowSettings() {
            Console.Clear();
            var configFile = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
            var settings = configFile.AppSettings.Settings;
            Print("            Settings", 10);
            Print("==================================\n",5);
            Print($"1. Toggle 'Press Enter continue': {settings["toggleReadLine"].Value}", 10);
            Print($"2. Toggle Slow-printing text:     {settings["toggleSlowPrint"].Value}", 10);
            Print($"3. Game Volume:                   {settings["volume"].Value}\n", 10);
            Print("=======Press Esc to go back=======",5);
        }
        public static void InstantSettings() {
            Console.Clear();
            var configFile = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
            var settings = configFile.AppSettings.Settings;
            Console.WriteLine("            Settings       ");
            Console.WriteLine("==================================\n");
            Console.WriteLine($"1. Toggle 'Press Enter continue': {settings["toggleReadLine"].Value}");
            Console.WriteLine($"2. Toggle Slow-printing text:     {settings["toggleSlowPrint"].Value}");
            Console.WriteLine($"3. Game Volume:                   {settings["volume"].Value}\n");
            Console.WriteLine("=======Press Esc to go back=======");
        }
        public static void SlowShopHUD() {
            Console.Clear();
            Print("         Gheed's Shop        ",10);
            Print("=============================",5);
            Print($"| (W)eapon Upgrade        $ {Shop.ShopPrice("weaponupgrade")}",5);
            Print($"| (A)rmor Upgrade:        $ {Shop.ShopPrice("armorupgrade")}", 5);
            Print($"| (P)otions:              $ {Shop.ShopPrice("potion")}", 5);
            Print($"| Up(g)rade potion        $ {Shop.ShopPrice("potionupgrade")}", 5);
            Print("|============================", 1);
            Print($"| (S)ell    Potion      $ {Shop.ShopPrice("sellpotion")}", 5);
            Print($"|  Sell (5)xPotions     $ {Shop.ShopPrice("sellpotion5")}", 5);
            Print("=============================", 1);
            Print("  (E)xit Shop                \n\n", 1);
            Print($"  {Program.currentPlayer.currentClass} {Program.currentPlayer.name}'s Stats", 5);
            Print($"=============================", 1);
            Print($"| Level: {Program.currentPlayer.level}", 1);
            Print($"| EXP  [{ProgressBarForPrint("+", " ", ((decimal)Program.currentPlayer.xp / (decimal)Program.currentPlayer.GetLevelUpValue()), 20)}]", 10);
            Print($"| Health:                 {Program.currentPlayer.health}/{Program.currentPlayer.maxHealth}", 5);
            Print($"| Gold:                  ${Program.currentPlayer.gold}", 5);
            Print($"| Weapon Upgrades:        {Program.currentPlayer.weaponValue}", 5);
            Print($"| Armor Upgrades:         {Program.currentPlayer.armorValue}", 5);
            Print($"| Potions:                {Program.currentPlayer.potion}", 5);
            Print("=============================", 1);
            Print(" (U)se Potion (C)haracter screen\n", 5);
            Print("Choose what to buy or sell", 1);
        }
        public static void InstantShopHUD() {
            Console.Clear();
            Console.WriteLine("         Gheed's Shop        ");
            Console.WriteLine("=============================");
            Console.WriteLine($"| (W)eapon Upgrade        $ {Shop.ShopPrice("weaponupgrade")}");
            Console.WriteLine($"| (A)rmor Upgrade:        $ {Shop.ShopPrice("armorupgrade")}");
            Console.WriteLine($"| (P)otions:              $ {Shop.ShopPrice("potion")}");
            Console.WriteLine($"| Up(g)rade potion        $ {Shop.ShopPrice("potionupgrade")}");
            Console.WriteLine("|============================");
            Console.WriteLine($"| (S)ell    Potion      $ {Shop.ShopPrice("sellpotion")}");
            Console.WriteLine($"|  Sell (5)xPotions     $ {Shop.ShopPrice("sellpotion5")}");
            Console.WriteLine("=============================");
            Console.WriteLine("  (E)xit Shop                \n\n");
            Console.WriteLine($"  {Program.currentPlayer.currentClass} {Program.currentPlayer.name}'s Stats");
            Console.WriteLine($"=============================");
            Console.WriteLine($"| Level: {Program.currentPlayer.level}");
            Console.Write("| EXP  ");
            Console.Write("[");
            ProgressBar("+", " ", ((decimal)Program.currentPlayer.xp / (decimal)Program.currentPlayer.GetLevelUpValue()), 20);
            Console.WriteLine("]");
            Console.WriteLine($"| Health:                 {Program.currentPlayer.health}/{Program.currentPlayer.maxHealth}");
            Console.WriteLine($"| Gold:                  ${Program.currentPlayer.gold}");
            Console.WriteLine($"| Weapon Upgrades:        {Program.currentPlayer.weaponValue}");
            Console.WriteLine($"| Armor Upgrades:         {Program.currentPlayer.armorValue}");
            Console.WriteLine($"| Potions:                {Program.currentPlayer.potion}");
            Console.WriteLine("=============================");
            Console.WriteLine(" (U)se Potion (C)haracter screen\n");
            Console.WriteLine("Choose what to buy or sell");
        }
        public static void CharacterScreen() {
            //Metode til at kalde og gernerer en character screen som viser alle funktionelle variabler der er i brug.
            Console.Clear();
            Console.WriteLine("~~~~~~~~~~~~~~Character screen~~~~~~~~~~~~~~");
            Print($"Name: {Program.currentPlayer.name}\t\tClass: {Program.currentPlayer.currentClass}", 5);
            Print($"Level: {Program.currentPlayer.level}", 5);
            Print($"EXP  [{ProgressBarForPrint("+", " ", ((decimal)Program.currentPlayer.xp / (decimal)Program.currentPlayer.GetLevelUpValue()), 25)}] {Program.currentPlayer.xp}/{Program.currentPlayer.GetLevelUpValue()}", 10);
            Print("-----------------Stats----------------------", 5);
            Console.WriteLine($"Max Health: {Program.currentPlayer.maxHealth}\t\tCurrent Health: {Program.currentPlayer.health}");
            Console.WriteLine($"Weapon Damage: {1 + (Program.currentPlayer.TotalWeaponValue() + 0 + ((Program.currentPlayer.currentClass == Player.PlayerClass.Warrior) ? 1 + Program.currentPlayer.level : 0)) / 2}-{1 + Program.currentPlayer.TotalWeaponValue() + 4 + ((Program.currentPlayer.currentClass == Player.PlayerClass.Warrior) ? 1 + Program.currentPlayer.level : 0)}\tTotal Armor Rating: {Program.currentPlayer.TotalArmorValue()}");
            Console.WriteLine("\n*****************Equipment********************\n");
            Console.WriteLine($"Gold: ${Program.currentPlayer.gold}");
            Console.WriteLine($"Healing Potions: {Program.currentPlayer.potion}\tPotion Strength: +{Program.currentPlayer.potionValue}");
            Console.WriteLine($"Weapon upgrades: {Program.currentPlayer.weaponValue}\tWeapon: {Program.currentPlayer.equippedWeapon} (+{Program.currentPlayer.equippedWeaponValue} dmg)");
            Console.WriteLine($"Armor upgrades:  {Program.currentPlayer.armorValue}\tArmor: {Program.currentPlayer.equippedArmor} (+{Program.currentPlayer.equippedArmorValue} armor)");
            Print("Press to go back...", 1);
        }
        public static void TopBasicCombatHUD(string name, int power, int health, int turn) {
            Console.Clear();
            Print($"Turn: {turn}",5);
            Print($"Fighting: {name}!", 10);
            Print($"Strength: {power} / HP: {health}", 10);
            Print("-----------------------", 5);
        }
        public static void FullBasicCombatHUD(string name, int power, int health, int turn) {
            Console.Clear();
            Console.WriteLine($"Turn: {turn}");
            Console.WriteLine($"Fighting: {name}!");
            Console.WriteLine($"Strength: {power} / HP: {health}");
            Console.WriteLine("---------------------------");
            Console.WriteLine($"{Program.currentPlayer.currentClass} {Program.currentPlayer.name}:");
            Console.WriteLine($"Health: {Program.currentPlayer.health}/{Program.currentPlayer.maxHealth}\t|| Healing Potions: {Program.currentPlayer.potion}");
            Console.WriteLine($"Level: {Program.currentPlayer.level}\t|| Gold: ${Program.currentPlayer.gold}");
            Console.Write("EXP  ");
            Console.Write("[");
            ProgressBar("+", " ", (decimal)Program.currentPlayer.xp / (decimal)Program.currentPlayer.GetLevelUpValue(), 20);
            Console.WriteLine("]");
            Console.WriteLine("==========Actions==========");
            Console.WriteLine("| (A)ttack     (D)efend   |");
            Console.WriteLine("| (R)un        (H)eal     |");
            Console.WriteLine("===========Info============");
            Console.WriteLine("| (C)haracter screen      |");
            Console.WriteLine("|  Combat (L)og           |");
            Console.WriteLine("===========================");
            Console.WriteLine("Choose an action...");
        }
        public static void TopCampHUD() {
            Console.Clear();
            Print("[][][][][][]  Camp   [][][][][][]", 5);
            Print($"{Program.currentPlayer.currentClass} {Program.currentPlayer.name}:", 10);
            Print($"Health: {Program.currentPlayer.health}/{Program.currentPlayer.maxHealth}\t|| Healing Potions: {Program.currentPlayer.potion}", 10);
            Print($"Level: {Program.currentPlayer.level}\t|| Gold: ${Program.currentPlayer.gold}", 5);
            Print($"EXP  [{ProgressBarForPrint("+", " ", (decimal)Program.currentPlayer.xp / (decimal)Program.currentPlayer.GetLevelUpValue(), 20)}]", 10);
        }
        public static void SlowCampHUD() {
            Console.Clear();
            Print("[][][][][][]  Camp   [][][][][][]",5);
            Print($"{Program.currentPlayer.currentClass} {Program.currentPlayer.name}:",10);
            Print($"Health: {Program.currentPlayer.health}/{Program.currentPlayer.maxHealth}\t|| Healing Potions: {Program.currentPlayer.potion}",10);
            Print($"Level: {Program.currentPlayer.level}\t|| Gold: ${Program.currentPlayer.gold}", 10);
            Print($"EXP  [{ProgressBarForPrint("+", " ", (decimal)Program.currentPlayer.xp / (decimal)Program.currentPlayer.GetLevelUpValue(), 20)}]",10);
            Print("==============Actions=================",5);
            Print("0 (E)xplore          (S)leep (Save)  0", 10);
            Print("0 (G)heed's shop     (H)eal          0", 10);
            Print("0 (C)haracter screen                 0", 10);
            Print("======================================", 5);
            Print("  (Q)uit to Main Menu                 ", 10);
            Print("Choose an action...", 1);
        }
        public static void InstantCampHUD() {
            Console.Clear();
            Console.WriteLine("[][][][][][]  Camp   [][][][][][]");
            Console.WriteLine($"{Program.currentPlayer.currentClass} {Program.currentPlayer.name}:");
            Console.WriteLine($"Health: {Program.currentPlayer.health}/{Program.currentPlayer.maxHealth}\t|| Healing Potions: {Program.currentPlayer.potion}");
            Console.WriteLine($"Level: {Program.currentPlayer.level}\t|| Gold: ${Program.currentPlayer.gold}");
            Console.Write("EXP  ");
            Console.Write("[");
            HUDTools.ProgressBar("+", " ", (decimal)Program.currentPlayer.xp / (decimal)Program.currentPlayer.GetLevelUpValue(), 20);
            Console.WriteLine("]");
            Console.WriteLine("==============Actions=================");
            Console.WriteLine("0 (E)xplore          (S)leep (Save)  0");
            Console.WriteLine("0 (G)heed's shop     (H)eal          0");
            Console.WriteLine("0 (C)haracter screen                 0");
            Console.WriteLine("======================================");
            Console.WriteLine("  (Q)uit to Main Menu                 ");
            Console.WriteLine("Choose an action...");
        }
    }
}
