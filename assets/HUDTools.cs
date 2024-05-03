using System;
using System.Configuration;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Saga.Dungeon;
using Saga.Items;
using Saga.Character;
using System.Text;
using System.Collections.Generic;

namespace Saga.assets
{
    public class HUDTools
    {
        //Get metode til at få teksten fra memory til filen.
        public static TextWriter Out { get; }

        //Metode til at toggle ReadLine/ReadKey baseret på spiller settings.
        public static string PlayerPrompt() {
            if (Convert.ToBoolean(ConfigurationManager.AppSettings.Get("toggleReadLine")) == true) {
                return Console.ReadLine().ToLower();
            }
            else {
                string x = Console.ReadKey().KeyChar.ToString().ToLower();
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
        public static void WriteCombatLog(string action, Encounters TurnTimer, int damage=0, int attack = 0, Enemy Monster=null) {
            if (!File.Exists("combatlog.txt")) {
                File.Create("combatlog.txt");
            }

            //Læser logfilen og gemmer det i memory
            string text = File.ReadAllText("combatlog.txt");

            // Åbner en text file navngivet "combatlog"  
            // at the location of your program 
            FileStream CombatLog = new FileStream("combatlog.txt", FileMode.Open);

            //Laver et objekt som kan skrive til Logfilen
            StreamWriter portal1 = new StreamWriter(CombatLog);

            // Standard Output stream is  
            // being saved to a Textwriter 
            TextWriter combatlogsave = Console.Out;

            //Klar gør objektet til at skrive til memory.
            Console.SetOut(portal1);

            //Skriver den gemte tekst.
            Console.Write($"{text}\n");

            //Skriver og tilføjer den nye tekst.
            if (Program.CurrentPlayer.BaseSecondaryAttributes.Awareness > Monster.awareness) {
                switch (action) {
                    case "attack":
                        Console.WriteLine($"Turn: {TurnTimer.turnTimer}\nYou attacked and dealt {attack} damage.");
                        break;
                    case "defend":
                        Console.WriteLine($"Turn: {TurnTimer.turnTimer}\nYou defended and lowered the next two attacks.");
                        break;
                    case "heal":
                        Console.WriteLine($"Turn: {TurnTimer.turnTimer}");
                        if (((Potion)Program.CurrentPlayer.Equipment[Slot.SLOT_POTION]).PotionQuantity == 0) {
                            Console.WriteLine($"You tried to drink a potion you didn't have.");
                        }
                        else {
                            if (Program.CurrentPlayer.BaseSecondaryAttributes.MaxHealth == Program.CurrentPlayer.Health) {
                                Console.WriteLine("You healed to max health by drinking a potion.");
                            }
                            else {
                                Console.WriteLine($"You gained {((Potion)Program.CurrentPlayer.Equipment[Slot.SLOT_POTION]).PotionPotency} health by drinking a potion.");
                            }
                        }
                        break;
                    case "run":
                        Console.WriteLine($"Turn: {TurnTimer.turnTimer}");
                        Console.WriteLine($"You tried to run but was unable to escape this turn.");
                        break;
                    case "enemysecond":
                        Console.WriteLine($"{Monster.name} attacked and dealt {damage} damage!");
                        break;
                }
            } else {
                switch (action) {
                    case "attack":
                        Console.WriteLine($"You attacked and dealt {attack} damage.");
                        break;
                    case "defend":
                        Console.WriteLine($"You defended and lowered the next two attacks.");
                        break;
                    case "heal":
                        if (((Potion)Program.CurrentPlayer.Equipment[Slot.SLOT_POTION]).PotionQuantity == 0) {
                            Console.WriteLine($"You tried to drink a potion you didn't have.");
                        }
                        else {
                            if (Program.CurrentPlayer.BaseSecondaryAttributes.MaxHealth == Program.CurrentPlayer.Health) {
                                Console.WriteLine("You healed to max health by drinking a potion.");
                            }
                            else {
                                Console.WriteLine($"You gained {((Potion)Program.CurrentPlayer.Equipment[Slot.SLOT_POTION]).PotionPotency} health by drinking a potion.");
                            }
                        }
                        break;
                    case "run":
                        Console.WriteLine($"You tried to run but was unable to escape this turn.");
                        break;
                    case "enemyfirst":
                        Console.WriteLine($"Turn: {TurnTimer.turnTimer}");
                        Console.WriteLine($"{Monster.name} attacked and dealt {damage} damage!");
                        break;
                }
            }

            //Skriver teksten i memory til filen.
            Console.SetOut(combatlogsave);

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
            //Print($"| (W)eapon Upgrade        $ {Shop.ShopPrice("weaponupgrade")}",5);
            //Print($"| (A)rmor Upgrade:        $ {Shop.ShopPrice("armorupgrade")}", 5);
            Print($"| (P)otions:              $ {Shop.ShopPrice("potion")}", 5);
            Print($"| Up(g)rade potion        $ {Shop.ShopPrice("potionupgrade")}", 5);
            Print("|============================", 1);
            Print($"| (S)ell    Potion      $ {Shop.ShopPrice("sellpotion")}", 5);
            Print($"|  Sell (5)xPotions     $ {Shop.ShopPrice("sellpotion5")}", 5);
            Print("=============================", 1);
            Print("  (E)xit Shop                \n\n", 1);
            Print($"  {Program.CurrentPlayer.currentClass} {Program.CurrentPlayer.Name}'s Stats", 5);
            Print($"=============================", 1);
            Print($"| Level: {Program.CurrentPlayer.Level}", 1);
            Print($"| EXP  [{ProgressBarForPrint("+", " ", ((decimal)Program.CurrentPlayer.Exp / (decimal)Program.CurrentPlayer.GetLevelUpValue()), 20)}]", 10);
            Print($"| Health:                 {Program.CurrentPlayer.Health}/{Program.CurrentPlayer.BaseSecondaryAttributes.MaxHealth}", 5);
            Print($"| Gold:                  ${Program.CurrentPlayer.Gold}", 5);
            //Print($"| Weapon Upgrades:        {Program.CurrentPlayer.weaponValue}", 5);
            //Print($"| Armor Upgrades:         {Program.CurrentPlayer.armorValue}", 5);
            Print($"| Potions:                {((Potion)Program.CurrentPlayer.Equipment[Slot.SLOT_POTION]).PotionQuantity}", 5);
            Print("=============================", 1);
            Print(" (U)se Potion (C)haracter screen\n", 5);
            Print("Choose what to buy or sell", 1);
        }
        public static void InstantShopHUD() {
            Console.Clear();
            Console.WriteLine("         Gheed's Shop        ");
            Console.WriteLine("=============================");
            //Console.WriteLine($"| (W)eapon Upgrade        $ {Shop.ShopPrice("weaponupgrade")}");
            //Console.WriteLine($"| (A)rmor Upgrade:        $ {Shop.ShopPrice("armorupgrade")}");
            Console.WriteLine($"| (P)otions:              $ {Shop.ShopPrice("potion")}");
            Console.WriteLine($"| Up(g)rade potion        $ {Shop.ShopPrice("potionupgrade")}");
            Console.WriteLine("|============================");
            Console.WriteLine($"| (S)ell    Potion      $ {Shop.ShopPrice("sellpotion")}");
            Console.WriteLine($"|  Sell (5)xPotions     $ {Shop.ShopPrice("sellpotion5")}");
            Console.WriteLine("=============================");
            Console.WriteLine("  (E)xit Shop                \n\n");
            Console.WriteLine($"  {Program.CurrentPlayer.currentClass} {Program.CurrentPlayer.Name}'s Stats");
            Console.WriteLine($"=============================");
            Console.WriteLine($"| Level: {Program.CurrentPlayer.Level}");
            Console.Write("| EXP  ");
            Console.Write("[");
            ProgressBar("+", " ", ((decimal)Program.CurrentPlayer.Exp / (decimal)Program.CurrentPlayer.GetLevelUpValue()), 20);
            Console.WriteLine("]");
            Console.WriteLine($"| Health:                 {Program.CurrentPlayer.Health}/{Program.CurrentPlayer.BaseSecondaryAttributes.MaxHealth}");
            Console.WriteLine($"| Gold:                  ${Program.CurrentPlayer.Gold}");
            //Console.WriteLine($"| Weapon Upgrades:        {Program.currentPlayer.weaponValue}");
            //Console.WriteLine($"| Armor Upgrades:         {Program.currentPlayer.armorValue}");
            Console.WriteLine($"| Potions:                {((Potion)Program.CurrentPlayer.Equipment[Slot.SLOT_POTION]).PotionQuantity}");
            Console.WriteLine("=============================");
            Console.WriteLine(" (U)se Potion (C)haracter screen\n");
            Console.WriteLine("Choose what to buy or sell");
        }
        public static void WriteStatsToConsole(string name, int level, PrimaryAttributes totalPrimaryAttributes, SecondaryAttributes baseSecondaryAttributes, (int, int) dpt) {
            StringBuilder stats = new StringBuilder("~~~~~~~~~~~~~~~~~~~ Character screen ~~~~~~~~~~~~~~~~~~~~~~~\n");

            stats.AppendFormat($" Name: {name}\t\t\tClass: {Program.CurrentPlayer.currentClass}\n");
            stats.AppendFormat($" Level: {level}\n");
            stats.AppendFormat($" EXP  [{ProgressBarForPrint("+", " ", ((decimal)Program.CurrentPlayer.Exp / (decimal)Program.CurrentPlayer.GetLevelUpValue()), 25)}] {Program.CurrentPlayer.Exp}/{Program.CurrentPlayer.GetLevelUpValue()}\n");
            stats.AppendFormat($"\n----------------- Primary Attributes -----------------------\n");
            stats.AppendFormat($" (S)trength: {totalPrimaryAttributes.Strength}\n");
            stats.AppendFormat($" (D)exterity: {totalPrimaryAttributes.Dexterity}\n");
            stats.AppendFormat($" (C)onstitution: {totalPrimaryAttributes.Constitution}\n");
            stats.AppendFormat($" (I)ntellect: {totalPrimaryAttributes.Intellect}\n");
            stats.AppendFormat($" (W)illpower: {totalPrimaryAttributes.WillPower}\n");
            stats.AppendFormat($"Attribute points to spend: {Program.CurrentPlayer.FreeAttributePoints}\n");
            stats.AppendFormat($"\n---------------- Secondary Attributes ----------------------\n");
            stats.AppendFormat($" Health: {Program.CurrentPlayer.Health} / {baseSecondaryAttributes.MaxHealth}\t\tDamage: {dpt.Item1}-{dpt.Item2}\n");
            stats.AppendFormat($" Mana: {Program.CurrentPlayer.Mana} / {baseSecondaryAttributes.MaxMana}\t\t\tAwareness: {baseSecondaryAttributes.Awareness}\n");
            stats.AppendFormat($" Armor Rating: {baseSecondaryAttributes.ArmorRating}\t\tElemental Resistance: {baseSecondaryAttributes.ElementalResistence}\n");

            Print(stats.ToString(),0);
        }
        public static void CharacterScreen() {
            //Metode til at kalde og gernerer en character screen som viser alle funktionelle variabler der er i brug.
            for (int i = Program.CurrentPlayer.FreeAttributePoints; i >= 0 ; i--) {
                Console.Clear();
                Program.CurrentPlayer.DisplayStats();
                Console.WriteLine("******************** Equipment *****************************");
                Console.WriteLine($" Gold: ${Program.CurrentPlayer.Gold}");
                Console.WriteLine($" Healing Potions: {((Potion)Program.CurrentPlayer.Equipment[Slot.SLOT_POTION]).PotionQuantity}\t\tPotion Strength: +{((Potion)Program.CurrentPlayer.Equipment[Slot.SLOT_POTION]).PotionPotency}");
                foreach (KeyValuePair<Slot, Item> entry in Program.CurrentPlayer.Equipment) {
                    if (entry.Key == Slot.SLOT_WEAPON) {
                        Console.WriteLine($" {entry.Value.ItemName}: +{((Weapon)entry.Value).WeaponAttributes.MinDamage}-{((Weapon)entry.Value).WeaponAttributes.MaxDamage} dmg");
                    }
                    else if (entry.Key == Slot.SLOT_POTION) {
                    }
                    else {
                        Console.Write($" {entry.Value.ItemName}:");
                        if (((Armor)entry.Value).Attributes.Strength > 0) {
                            Console.Write($" +{((Armor)entry.Value).Attributes.Strength} Strength");
                        }
                        if (((Armor)entry.Value).Attributes.Dexterity > 0) {
                            Console.Write($" +{((Armor)entry.Value).Attributes.Dexterity} Dexterity");
                        }
                        if (((Armor)entry.Value).Attributes.Intellect > 0) {
                            Console.Write($" +{((Armor)entry.Value).Attributes.Intellect} Intellect");
                        }
                        if (((Armor)entry.Value).Attributes.Constitution > 0) {
                            Console.Write($" +{((Armor)entry.Value).Attributes.Constitution} Constitution");
                        }
                        if (((Armor)entry.Value).Attributes.WillPower > 0) {
                            Console.WriteLine($" +{((Armor)entry.Value).Attributes.WillPower} Willpower");
                        }
                        if (entry.Value.ItemName == "Linen Rags") {
                            Console.WriteLine(" Offers no protection");
                        }
                    }
                }
                if (Program.CurrentPlayer.FreeAttributePoints > 0 && i != 0) {
                    Print("Allocate attribute point? Type the corresponding attribute name to spent 1 point, else type no",1);
                    while (true) {
                        string input = PlayerPrompt();
                        if (input == "s" || input == "strength") {
                            Program.CurrentPlayer.BasePrimaryAttributes.Strength++;
                            Program.CurrentPlayer.FreeAttributePoints--;
                            break;
                        }
                        else if (input == "d" || input == "dexterity") {
                            Program.CurrentPlayer.BasePrimaryAttributes.Dexterity++;
                            Program.CurrentPlayer.FreeAttributePoints--;
                            break;
                        }
                        else if (input == "i" || input == "intellect") {
                            Program.CurrentPlayer.BasePrimaryAttributes.Intellect++;
                            Program.CurrentPlayer.FreeAttributePoints--;
                            break;
                        }
                        else if (input == "c" || input == "constitution") {
                            Program.CurrentPlayer.BasePrimaryAttributes.Constitution++;
                            Program.CurrentPlayer.FreeAttributePoints--;
                            break;
                        }
                        else if (input == "w" || input == "willpower") {
                            Program.CurrentPlayer.BasePrimaryAttributes.WillPower++;
                            Program.CurrentPlayer.FreeAttributePoints--;
                            break;
                        }
                        else if (input == "n" || input == "no") {
                            i = 1;
                            break;
                        } else {
                            Print("Invalid input", 1);
                        }
                    }
                }           
            }
            Print("\nPress to go back...", 1);        
        }
        public static void TopCombatHUD(Enemy Monster, Encounters TurnTimer) {
            Console.Clear();
            Print($"Turn: {TurnTimer.turnTimer}",5);
            Print($"Fighting: {Monster.name}!", 10);
            Print($"Strength: {Monster.power} / HP: {Monster.health}", 10);
            if (Program.CurrentPlayer.BaseSecondaryAttributes.Awareness > Monster.awareness) {
                Print("---------------------------",5);
                Print("You go first!",10);
            }
            else {
                Print("The enemy go first!",10);
                Print("---------------------------",5);
            }
        }
        public static void FullCombatHUD(Enemy Monster, Encounters TurnTimer) {
            Console.Clear();
            Console.WriteLine($"Turn: {TurnTimer.turnTimer}");
            Console.WriteLine($"Fighting: {Monster.name}!");
            Console.WriteLine($"Strength: {Monster.power} / HP: {Monster.health}");
            if (Program.CurrentPlayer.BaseSecondaryAttributes.Awareness > Monster.awareness) {
                Console.WriteLine("---------------------------");
                Console.WriteLine("You go first!");
            } else {
                Console.WriteLine("The enemy go first!");
                Console.WriteLine("---------------------------");
            }           
            Console.WriteLine($"{Program.CurrentPlayer.currentClass} {Program.CurrentPlayer.Name}:");
            Console.WriteLine($"Health: {Program.CurrentPlayer.Health}/{Program.CurrentPlayer.BaseSecondaryAttributes.MaxHealth}\t|| Healing Potions: {((Potion)Program.CurrentPlayer.Equipment[Slot.SLOT_POTION]).PotionQuantity}");
            Console.WriteLine($"Level: {Program.CurrentPlayer.Level}\t|| Gold: ${Program.CurrentPlayer.Gold}");
            Console.Write("EXP  ");
            Console.Write("[");
            ProgressBar("+", " ", (decimal)Program.CurrentPlayer.Exp / (decimal)Program.CurrentPlayer.GetLevelUpValue(), 20);
            Console.WriteLine("]");
            Console.WriteLine("==========Actions==========");
            Console.WriteLine("| (A)ttack     (D)efend   |");
            Console.WriteLine("| (R)un        (H)eal     |");
            Console.WriteLine("===========Info============");
            Console.WriteLine("| (C)haracter screen      |");
            Console.WriteLine("|  Combat (L)og           |");
            Console.WriteLine("===========================");           
        }
        public static void TopCampHUD() {
            Console.Clear();
            Print("[][][][][][]  Camp   [][][][][][]", 5);
            Print($"{Program.CurrentPlayer.currentClass} {Program.CurrentPlayer.Name}:", 10);
            Print($"Health: {Program.CurrentPlayer.Health}/{Program.CurrentPlayer.BaseSecondaryAttributes.MaxHealth}\t|| Healing Potions: {((Potion)Program.CurrentPlayer.Equipment[Slot.SLOT_POTION]).PotionQuantity}", 10);
            Print($"Level: {Program.CurrentPlayer.Level}\t|| Gold: ${Program.CurrentPlayer.Gold}", 5);
            Print($"EXP  [{ProgressBarForPrint("+", " ", (decimal)Program.CurrentPlayer.Exp / (decimal)Program.CurrentPlayer.GetLevelUpValue(), 20)}]", 10);
        }
        public static void SlowCampHUD() {
            Console.Clear();
            Print("[][][][][][]  Camp   [][][][][][]",5);
            Print($"{Program.CurrentPlayer.currentClass} {Program.CurrentPlayer.Name}:",10);
            Print($"Health: {Program.CurrentPlayer.Health}/{Program.CurrentPlayer.BaseSecondaryAttributes.MaxHealth}\t|| Healing Potions: {((Potion)Program.CurrentPlayer.Equipment[Slot.SLOT_POTION]).PotionQuantity}",10);
            Print($"Level: {Program.CurrentPlayer.Level}\t|| Gold: ${Program.CurrentPlayer.Gold}", 10);
            Print($"EXP  [{ProgressBarForPrint("+", " ", (decimal)Program.CurrentPlayer.Exp / (decimal)Program.CurrentPlayer.GetLevelUpValue(), 20)}]",10);
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
            Console.WriteLine($"{Program.CurrentPlayer.currentClass} {Program.CurrentPlayer.Name}:");
            Console.WriteLine($"Health: {Program.CurrentPlayer.Health}/{Program.CurrentPlayer.BaseSecondaryAttributes.MaxHealth}\t|| Healing Potions: {((Potion)Program.CurrentPlayer.Equipment[Slot.SLOT_POTION]).PotionQuantity}");
            Console.WriteLine($"Level: {Program.CurrentPlayer.Level}\t|| Gold: ${Program.CurrentPlayer.Gold}");
            Console.Write("EXP  ");
            Console.Write("[");
            ProgressBar("+", " ", (decimal)Program.CurrentPlayer.Exp / (decimal)Program.CurrentPlayer.GetLevelUpValue(), 20);
            Console.WriteLine("]");
            Console.WriteLine("==============Actions=================");
            Console.WriteLine("0 (E)xplore          (S)leep (Save)  0");
            Console.WriteLine("0 (G)heed's shop     (H)eal          0");
            Console.WriteLine("0 (C)haracter screen                 0");
            Console.WriteLine("======================================");
            Console.WriteLine("  (Q)uit to Main Menu                 ");
            Console.WriteLine("Choose an action...");
        }
        public static void RespiteHUD() {
            Console.Clear();
            Console.WriteLine("You gain a moment of respite and a choice...");
            Console.WriteLine("Do you venture deeper or turn back to your camp?");
            Console.WriteLine($"{Program.CurrentPlayer.currentClass} {Program.CurrentPlayer.Name}:");
            Console.WriteLine($"Health: {Program.CurrentPlayer.Health}/{Program.CurrentPlayer.BaseSecondaryAttributes.MaxHealth}\t|| Healing Potions: {((Potion)Program.CurrentPlayer.Equipment[Slot.SLOT_POTION]).PotionQuantity}");
            Console.WriteLine($"Level: {Program.CurrentPlayer.Level}\t|| Gold: ${Program.CurrentPlayer.Gold}");
            Console.Write("EXP  ");
            Console.Write("[");
            ProgressBar("+", " ", (decimal)Program.CurrentPlayer.Exp / (decimal)Program.CurrentPlayer.GetLevelUpValue(), 20);
            Console.WriteLine("]");
            Console.WriteLine("==============Actions==============");
            Console.WriteLine("V (E)xplore            (R)eturn   V");
            Console.WriteLine("V (C)haracter screen   (H)eal     V");
            Console.WriteLine("===================================");
        }
    }
}
