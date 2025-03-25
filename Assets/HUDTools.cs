using System;
using System.Configuration;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Saga.Dungeon;
using Saga.Items;
using Saga.Character;

namespace Saga.Assets
{
    public class HUDTools
    {
        //Get metode til at få teksten fra memory til filen.
        public static TextWriter Out { get; }

        //Metode til at "Slow-print" tekst, med indbygget toggle setting.
        public static void Print(string text, int time = 40) {
            if (Convert.ToBoolean(ConfigurationManager.AppSettings.Get("toggleSlowPrint")) == true) {
                Task t = Task.Run(() => {
                    foreach (char c in text) {
                        if (Console.KeyAvailable && Console.ReadKey(true).Key == ConsoleKey.Spacebar) {
                            time = 0;
                        }
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

        // Metode til at skrive en logfil til kamp
        public static void WriteCombatLog(string action, Encounters TurnTimer, int damage=0, int attack = 0, Enemy Monster=null) {
            if (!File.Exists("combatlog.txt")) {
                File.Create("combatlog.txt");
            }

            //Læser logfilen og gemmer det i memory
            string text = File.ReadAllText("combatlog.txt");

            // Åbner en text file navngivet "combatlog"  
            // at the location of your program 
            FileStream CombatLog = new("combatlog.txt", FileMode.Open);

            //Laver et objekt som kan skrive til Logfilen
            StreamWriter portal1 = new(CombatLog);

            // Standard Output stream is  
            // being saved to a Textwriter 
            TextWriter combatlogsave = Console.Out;

            //Klar gør objektet til at skrive til memory.
            Console.SetOut(portal1);

            //Skriver den gemte tekst.
            Console.Write($"{text}\n");

            //Skriver og tilføjer den nye tekst.
            if (Program.CurrentPlayer.TotalSecondaryAttributes.Awareness > Monster.Awareness) {
                switch (action) {
                    case "attack":
                        Console.WriteLine($"Turn: {TurnTimer.TurnTimer}\nYou attacked and dealt {attack} damage.");
                        break;
                    case "defend":
                        Console.WriteLine($"Turn: {TurnTimer.TurnTimer}\nYou defended and lowered the next two attacks.");
                        break;
                    case "heal":
                        Console.WriteLine($"Turn: {TurnTimer.TurnTimer}");
                        if (Program.CurrentPlayer.CurrentHealingPotion.PotionQuantity == 0) {
                            Console.WriteLine($"You tried to drink a potion you didn't have.");
                        }
                        else {
                            if (Program.CurrentPlayer.TotalSecondaryAttributes.MaxHealth == Program.CurrentPlayer.Health) {
                                Console.WriteLine("You healed to max health by drinking a potion.");
                            }
                            else {
                                Console.WriteLine($"You gained {Program.CurrentPlayer.CurrentHealingPotion.PotionPotency} health by drinking a potion.");
                            }
                        }
                        break;
                    case "run":
                        Console.WriteLine($"Turn: {TurnTimer.TurnTimer}");
                        Console.WriteLine($"You tried to run but was unable to escape this turn.");
                        break;
                    case "enemysecond":
                        Console.WriteLine($"{Monster.Name} attacked and dealt {damage} damage!");
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
                        if (Program.CurrentPlayer.CurrentHealingPotion.PotionQuantity == 0) {
                            Console.WriteLine($"You tried to drink a potion you didn't have.");
                        }
                        else {
                            if (Program.CurrentPlayer.TotalSecondaryAttributes.MaxHealth == Program.CurrentPlayer.Health) {
                                Console.WriteLine("You healed to max health by drinking a potion.");
                            }
                            else {
                                Console.WriteLine($"You gained {Program.CurrentPlayer.CurrentHealingPotion.PotionPotency} health by drinking a potion.");
                            }
                        }
                        break;
                    case "run":
                        Console.WriteLine($"You tried to run but was unable to escape this turn.");
                        break;
                    case "enemyfirst":
                        Console.WriteLine($"Turn: {TurnTimer.TurnTimer}");
                        Console.WriteLine($"{Monster.Name} attacked and dealt {damage} damage!");
                        break;
                }
            }

            //Skriver teksten i memory til filen.
            Console.SetOut(combatlogsave);

            //Lukker objektet og filen igen.
            portal1.Close();
        }

        //Skriver Loggen i consolen.
        public static void GetLog() {
            string text = File.ReadAllText("combatlog.txt");
            Console.WriteLine(text);
        }

        //Rydder Log filen så den er klar til brug igen.
        public static void ClearLog() {
            File.WriteAllText("combatlog.txt", String.Empty);
        }

        //Read all lines fra embedded resource til en lise.
        public static List<string> ReadAllResourceLines(string resourceName) {
            using Stream stream = Assembly.GetEntryAssembly()
                .GetManifestResourceStream(resourceName);
            using StreamReader reader = new(stream);
            return [.. EnumerateLines(reader)];
        }
        static IEnumerable<string> EnumerateLines(StreamReader reader) {
            string line;
            while ((line = reader.ReadLine()) != null) {
                yield return line;
            }
        }

        //Read all text fra embedded reource til en string.
        public static string ReadAllResourceText(string resourceName) {
            using Stream stream = Assembly.GetEntryAssembly()
                .GetManifestResourceStream(resourceName);
            using StreamReader reader = new(stream);
            return reader.ReadToEnd();
        }

        //Clears the last x lines of the console screen.
        public static void ClearLastLine(int x) {
            Console.SetCursorPosition(0, Console.CursorTop - x);
            for (int i = 0; i < x; i++) {
                Console.WriteLine(new string(' ', Console.BufferWidth));
            }
            Console.SetCursorPosition(0, Console.CursorTop - x);
        }

        public static void ClearLastText((int, int) startCursorPosition) {
            (int, int) endCursor = Console.GetCursorPosition();
            Console.SetCursorPosition(0, startCursorPosition.Item2);
            int x = endCursor.Item2 - startCursorPosition.Item2;
            for (int i = 0; i < x; i++) {
                Console.WriteLine(new string(' ', Console.BufferWidth));
            }
            Console.SetCursorPosition(0, startCursorPosition.Item2);
        }

        //Centerer teksten.
        public static void WriteCenterLine(string input) {
            Console.WriteLine(String.Format("{0," + ((Console.WindowWidth / 2) + (input.Length / 2)) + "}", input));
        }

        //HUDS
        public static void MainMenu() {
            Console.Clear();
            WriteCenterLine("########         ##         ##########         ##       ");
            WriteCenterLine("########        ####        ##########        ####      ");
            WriteCenterLine("##             ##  ##       ##               ##  ##     ");
            WriteCenterLine("##            ##    ##      ##              ##    ##    ");
            WriteCenterLine("########     ##      ##     ##   #####     ##      ##   ");
            WriteCenterLine("########    ############    ##   #####    ############  ");
            WriteCenterLine("      ##   ##############   ##      ##   ############## ");
            WriteCenterLine("      ##   ##          ##   ##      ##   ##          ## ");
            WriteCenterLine("########  ##            ##  ##########  ##            ##");
            WriteCenterLine("########  ##            ##  ##########  ##            ##");
            WriteCenterLine("~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~\n");
            WriteCenterLine("1.          Play  \n");
            WriteCenterLine("2.        Settings\n");
            WriteCenterLine("3.       Quit Game\n");
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
            Console.WriteLine($"=======Press Esc to go back=======");
        }
        public static void LoadSaves(List<Player> players) {
            Print("Choose a save! ('back' for main menu) ", 10);
            Print("-.-.-.-.-.-.-.-.-.-.-.-.-.-.-.-.", 3);
            Print("#: playername");
            foreach (Player p in players) {
                Print($"{p.Id}: {p.Name} - Class: {p.CurrentClass} - Level: {p.Level}", 5);
            }
            Print("<><><><><><><><><><><><><><><><>", 3);
            Print("To load a save write 'id:#' or 'playername'.\nFor new game write 'new game'.\nTo delete a save write 'delete:playername'.\n", 1);
        }
        public static void BuyShopHUD(Shop shop) {
            Console.Clear();
            Console.WriteLine("         Gheed's Shop        ");
            Console.WriteLine("=======================================================");
            Console.WriteLine($"| Items for sale:");
            foreach (Item item in shop.Forsale) {
                if (item == null) {
                }
                else if (item.ItemSlot == Slot.Weapon) {
                    Console.WriteLine($"| ({1 + shop.Forsale.IndexOf(item)}) Ilvl: {item.ItemLevel}, {((Weapon)item).WeaponType}, {item.ItemName}, $ {item.CalculateItemPrice()}, +{((Weapon)item).WeaponAttributes.MinDamage}-{((Weapon)item).WeaponAttributes.MaxDamage} dmg");
                }
                else {
                    Console.Write($"| ({1 + shop.Forsale.IndexOf(item)}) Ilvl: {item.ItemLevel}, {item.ItemSlot}, {((Armor)item).ArmorType}, {item.ItemName}, $ {item.CalculateItemPrice()},");
                    if (((Armor)item).SecondaryAttributes.ArmorRating > 0) {
                        Console.Write($" +{((Armor)item).SecondaryAttributes.ArmorRating} Armor Rating");
                    }
                    if (((Armor)item).Attributes.Strength > 0) {
                        Console.Write($", +{((Armor)item).Attributes.Strength} Str");
                    }
                    if (((Armor)item).Attributes.Dexterity > 0) {
                        Console.Write($", +{((Armor)item).Attributes.Dexterity} Dex");
                    }
                    if (((Armor)item).Attributes.Intellect > 0) {
                        Console.Write($", +{((Armor)item).Attributes.Intellect} Int");
                    }
                    if (((Armor)item).Attributes.Constitution > 0) {
                        Console.Write($", +{((Armor)item).Attributes.Constitution} Const");
                    }
                    if (((Armor)item).Attributes.WillPower > 0) {
                        Console.Write($", +{((Armor)item).Attributes.WillPower} Wp");
                    }
                    Console.WriteLine($"");
                }
            }
            Console.WriteLine("=======================================================");
            Console.WriteLine(" (S)witch to sell  (E)xit Shop\n\n");
            Console.WriteLine($"  {Program.CurrentPlayer.CurrentClass} {Program.CurrentPlayer.Name}'s Stats");
            Console.WriteLine($"==============================");
            Console.WriteLine($"| Level: {Program.CurrentPlayer.Level}");
            Console.Write("| EXP  ");
            Console.Write("[");
            ProgressBar("+", " ", ((decimal)Program.CurrentPlayer.Exp / (decimal)Program.CurrentPlayer.GetLevelUpValue()), 20);
            Console.WriteLine("]");
            Console.WriteLine($"| Health:                 {Program.CurrentPlayer.Health}/{Program.CurrentPlayer.TotalSecondaryAttributes.MaxHealth}");
            Console.WriteLine($"| Gold:                  ${Program.CurrentPlayer.Gold}");
            Console.WriteLine($"| Potions:                {Program.CurrentPlayer.CurrentHealingPotion.PotionQuantity}");
            Console.WriteLine($"| Items in inventory:");
            foreach (Item item in Program.CurrentPlayer.Inventory) {
                if (item == null) {
                }
                else if (item.ItemSlot == Slot.Weapon) {
                    Console.WriteLine($"| {item.ItemName}: +{((Weapon)item).WeaponAttributes.MinDamage}-{((Weapon)item).WeaponAttributes.MaxDamage} dmg");
                }
                else if (item.ItemSlot != Slot.Quest){
                    Console.Write($"| {item.ItemName}:");
                    if (((Armor)item).SecondaryAttributes.ArmorRating > 0) {
                        Console.Write($" +{((Armor)item).SecondaryAttributes.ArmorRating} Armor Rating");
                    }
                    if (((Armor)item).Attributes.Strength > 0) {
                        Console.Write($", +{((Armor)item).Attributes.Strength} Str");
                    }
                    if (((Armor)item).Attributes.Dexterity > 0) {
                        Console.Write($", +{((Armor)item).Attributes.Dexterity} Dex");
                    }
                    if (((Armor)item).Attributes.Intellect > 0) {
                        Console.Write($", +{((Armor)item).Attributes.Intellect} Int");
                    }
                    if (((Armor)item).Attributes.Constitution > 0) {
                        Console.Write($", +{((Armor)item).Attributes.Constitution} Const");
                    }
                    if (((Armor)item).Attributes.WillPower > 0) {
                        Console.Write($", +{((Armor)item).Attributes.WillPower} Wp");
                    }
                    if (item.ItemName == "Linen Rags") {
                        Console.Write(" Offers no protection");
                    }
                    Console.WriteLine("");
                }
                else if (item.ItemSlot == Slot.Quest) {
                    Console.WriteLine($"| \u001b[96mQuest Item - {item.ItemName} #{((QuestItem)item).Amount}\u001b[0m");
                }
            }
            Console.WriteLine("==============================");
            Console.WriteLine(" (U)se Potion (C)haracter screen\n (I)nventory (Q)uestlog\n");
            Console.WriteLine("Choose what to buy");
        }
        public static void SellShopHUD() {
            Console.Clear();
            Console.WriteLine("         Gheed's Shop        ");
            Console.WriteLine("=======================================================");
            Console.WriteLine($"| Items in inventory:");
            foreach (Item item in Program.CurrentPlayer.Inventory) {
                if (item == null) {
                }
                else if (item.ItemSlot == Slot.Weapon) {
                    Console.WriteLine($"| ({1 + Array.IndexOf(Program.CurrentPlayer.Inventory, item)}) {item.ItemName}: +{((Weapon)item).WeaponAttributes.MinDamage}-{((Weapon)item).WeaponAttributes.MaxDamage} dmg,\t $ {Shop.ShopPrice((1 + Array.IndexOf(Program.CurrentPlayer.Inventory, item)).ToString())}");
                }
                else if (item.ItemSlot != Slot.Quest) {
                    Console.Write($"| ({1 + Array.IndexOf(Program.CurrentPlayer.Inventory, item)}) {item.ItemName}: ");
                    if (((Armor)item).SecondaryAttributes.ArmorRating > 0) {
                        Console.Write($" +{((Armor)item).SecondaryAttributes.ArmorRating} Armor Rating");
                    }
                    if (((Armor)item).Attributes.Strength > 0) {
                        Console.Write($", +{((Armor)item).Attributes.Strength} Str");
                    }
                    if (((Armor)item).Attributes.Dexterity > 0) {
                        Console.Write($", +{((Armor)item).Attributes.Dexterity} Dex");
                    }
                    if (((Armor)item).Attributes.Intellect > 0) {
                        Console.Write($", +{((Armor)item).Attributes.Intellect} Int");
                    }
                    if (((Armor)item).Attributes.Constitution > 0) {
                        Console.Write($", +{((Armor)item).Attributes.Constitution} Cons");
                    }
                    if (((Armor)item).Attributes.WillPower > 0) {
                        Console.WriteLine($", +{((Armor)item).Attributes.WillPower} Wp");
                    }
                    if (item.ItemName == "Linen Rags") {
                        Console.Write(" Offers no protection");
                    }
                    Console.WriteLine($"\t $ {Shop.ShopPrice((1 + Array.IndexOf(Program.CurrentPlayer.Inventory, item)).ToString())}");
                } 
                else if (item.ItemSlot == Slot.Quest) {
                    Console.WriteLine($"| \u001b[96mQuest Item - {item.ItemName} #{((QuestItem)item).Amount}\u001b[0m");
                }
            }
            Console.WriteLine($"|  Sell     (P)otion     $ {Shop.ShopPrice("sellpotion")}");
            if (Program.CurrentPlayer.CurrentHealingPotion.PotionQuantity >= 5) {
                Console.WriteLine($"|  Sell (F)ive Potions   $ {Shop.ShopPrice("sellpotion5")}");
            }
            Console.WriteLine("=======================================================");
            Console.WriteLine(" (S)witch to Buy   (E)xit Shop\n\n");
            Console.WriteLine($"  {Program.CurrentPlayer.CurrentClass} {Program.CurrentPlayer.Name}'s Stats");
            Console.WriteLine($"==============================");
            Console.WriteLine($"| Level: {Program.CurrentPlayer.Level}");
            Console.Write("| EXP  ");
            Console.Write("[");
            ProgressBar("+", " ", ((decimal)Program.CurrentPlayer.Exp / (decimal)Program.CurrentPlayer.GetLevelUpValue()), 20);
            Console.WriteLine("]");
            Console.WriteLine($"| Health:                 {Program.CurrentPlayer.Health}/{Program.CurrentPlayer.TotalSecondaryAttributes.MaxHealth}");
            Console.WriteLine($"| Gold:                  ${Program.CurrentPlayer.Gold}");
            Console.WriteLine($"| Potions:                {Program.CurrentPlayer.CurrentHealingPotion.PotionQuantity}");
            Console.WriteLine("==============================");
            Console.WriteLine(" (U)se Potion (C)haracter screen\n (I)nventory (Q)uestlog\n");
            Console.WriteLine("Choose what to sell");
        }
        public static void WriteStatsToConsole(string name, int level, PrimaryAttributes totalPrimaryAttributes, SecondaryAttributes baseSecondaryAttributes, (int, int) dpt) {
            StringBuilder stats = new("~~~~~~~~~~~~~~~~~~~ Character screen ~~~~~~~~~~~~~~~~~~~~~~~\n");

            stats.AppendFormat($" Name: {name}\t\t\tClass: {Program.CurrentPlayer.CurrentClass}\n");
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
            stats.AppendFormat($" Armor Rating: {baseSecondaryAttributes.ArmorRating}\t\tElemental Resistance: {baseSecondaryAttributes.ElementalResistence}");

            Print(stats.ToString(),0);
        }
        public static void CharacterScreen() {
            //Metode til at kalde og gernerer en character screen som viser alle funktionelle variabler der er i brug.
            for (int i = Program.CurrentPlayer.FreeAttributePoints; i >= 0 ; i--) {
                Console.Clear();
                Program.CurrentPlayer.DisplayStats();
                if (Program.CurrentPlayer.FreeAttributePoints > 0 && i != 0) {
                    Print("Allocate attribute point? Type the corresponding (A)ttribute abbr. to spent 1 point, else (N)o",1);
                    while (true) {
                        string input = TextInput.PlayerPrompt(true);
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
        public static void InventoryScreen() {
            while (true) {
                Console.Clear();
                Console.WriteLine("******************** Equipment *****************************");
                foreach (KeyValuePair<Slot, Item> entry in Program.CurrentPlayer.Equipment) {
                    if (entry.Key == Slot.Weapon) {
                        Console.WriteLine($" {entry.Value.ItemSlot} - {entry.Value.ItemName}: +{((Weapon)entry.Value).WeaponAttributes.MinDamage}-{((Weapon)entry.Value).WeaponAttributes.MaxDamage} dmg");
                    } else {
                        Console.Write($" {entry.Value.ItemSlot} - {entry.Value.ItemName}:");
                        if (((Armor)entry.Value).SecondaryAttributes.ArmorRating > 0) {
                            Console.Write($" +{((Armor)entry.Value).SecondaryAttributes.ArmorRating} Armor Rating");
                        }
                        if (((Armor)entry.Value).Attributes.Strength > 0) {
                            Console.Write($", +{((Armor)entry.Value).Attributes.Strength} Str");
                        }
                        if (((Armor)entry.Value).Attributes.Dexterity > 0) {
                            Console.Write($", +{((Armor)entry.Value).Attributes.Dexterity} Dex");
                        }
                        if (((Armor)entry.Value).Attributes.Intellect > 0) {
                            Console.Write($", +{((Armor)entry.Value).Attributes.Intellect} Int");
                        }
                        if (((Armor)entry.Value).Attributes.Constitution > 0) {
                            Console.Write($", +{((Armor)entry.Value).Attributes.Constitution} Const");
                        }
                        if (((Armor)entry.Value).Attributes.WillPower > 0) {
                            Console.Write($", +{((Armor)entry.Value).Attributes.WillPower} Wp");
                        }
                        if (entry.Value.ItemName == "Linen Rags") {
                            Console.Write(" Offers no protection");
                        }
                        Console.WriteLine("");
                    }
                }
                Console.WriteLine("\n@@@@@@@@@@@@@@@@@ Inventory @@@@@@@@@@@@@@@@@@@");
                Console.WriteLine($" Gold: ${Program.CurrentPlayer.Gold}");
                Console.WriteLine($" Healing Potions: {Program.CurrentPlayer.CurrentHealingPotion.PotionQuantity}\t\tPotion Strength: +{Program.CurrentPlayer.CurrentHealingPotion.PotionPotency}");
                foreach (Item item in Program.CurrentPlayer.Inventory) {
                    if (item == null) {
                        Console.WriteLine("\u001b[90m Empty slot\u001b[0m");
                    } else if (item.ItemSlot == Slot.Weapon) {
                        Console.WriteLine($" {item.ItemSlot} - {item.ItemName}: +{((Weapon)item).WeaponAttributes.MinDamage}-{((Weapon)item).WeaponAttributes.MaxDamage} dmg");
                    } else if (item.ItemSlot != Slot.Quest) {
                        Console.Write($" {item.ItemSlot} - {item.ItemName}:");
                        if (((Armor)item).SecondaryAttributes.ArmorRating > 0) {
                            Console.Write($" +{((Armor)item).SecondaryAttributes.ArmorRating} Armor Rating");
                        }
                        if (((Armor)item).Attributes.Strength > 0) {
                            Console.Write($" +{((Armor)item).Attributes.Strength} Str");
                        }
                        if (((Armor)item).Attributes.Dexterity > 0) {
                            Console.Write($" +{((Armor)item).Attributes.Dexterity} Dex");
                        }
                        if (((Armor)item).Attributes.Intellect > 0) {
                            Console.Write($" +{((Armor)item).Attributes.Intellect} Int");
                        }
                        if (((Armor)item).Attributes.Constitution > 0) {
                            Console.Write($" +{((Armor)item).Attributes.Constitution} Const");
                        }
                        if (((Armor)item).Attributes.WillPower > 0) {
                            Console.Write($" +{((Armor)item).Attributes.WillPower} Wp");
                        }
                        if (item.ItemName == "Linen Rags") {
                            Console.Write(" Offers no protection");
                        }
                        Console.WriteLine("");
                    } else if (item.ItemSlot == Slot.Quest) {
                        Console.WriteLine($"\u001b[96m Quest Item - {item.ItemName} #{((QuestItem)item).Amount}\u001b[0m");
                    }
                }
                Print($"\nTo equip item write 'equip_Itemname', to unequip item write 'unequip_Itemname'\nTo examine item write examine_Itemname else (b)ack\n", 1);
                string[] input = Console.ReadLine().ToLower().Split('_');
                if (input[0] == "equip") {
                    if (Program.CurrentPlayer.Inventory.All(x => x == null)) {
                        Console.WriteLine("\nNo items in inventory...");
                    } else {
                        var item = Program.CurrentPlayer.Inventory.FirstOrDefault(x => x?.ItemName.ToLower() == input[1]);
                        if (item != null) {
                            if (item.ItemSlot == Slot.Quest) {
                                Print("\nYou cannot equip this item...", 3);
                            } else if (item.ItemSlot == Slot.Weapon) {
                                Print($"\n{Program.CurrentPlayer.Equip((Weapon)item)}", 3);
                            } else if (item.ItemSlot != Slot.Weapon) {
                                Print($"\n{Program.CurrentPlayer.Equip((Armor)item)}", 3);
                            }
                        } else {
                            Console.WriteLine("\nNo such item in inventory...");
                        }
                    }
                    TextInput.PressToContinue();
                } else if (input[0] == "unequip") {
                    var wat = Program.CurrentPlayer.Equipment.FirstOrDefault(x => x.Value.ItemName.Equals(input[1], StringComparison.CurrentCultureIgnoreCase));
                    if (wat.Value == null) {
                        Console.WriteLine("\nNo such item equipped...");
                    } else {
                        Print($"\n{Program.CurrentPlayer.UnEquip(wat.Key, wat.Value)}", 3);
                    }
                    TextInput.PressToContinue();
                } else if (input[0] == "examine") {
                    var wat = Program.CurrentPlayer.Equipment.FirstOrDefault(x => x.Value.ItemName.Equals(input[1], StringComparison.CurrentCultureIgnoreCase));
                    var item = Program.CurrentPlayer.Inventory.FirstOrDefault(x => x?.ItemName.ToLower() == input[1]);
                    if (wat.Value == null && item == null) {
                        Console.WriteLine("\nNo such item exists...");
                    } else if (wat.Value != null) {
                        if (wat.Value.ItemSlot == Slot.Quest) {
                            Print($"\n{wat.Value.ItemDescription}", 3);
                        } else if (wat.Value.ItemSlot == Slot.Weapon) {
                            Print($"\nThis is a weapon of type {((Weapon)wat.Value).WeaponType}.\n{wat.Value.ItemDescription}", 3);
                        } else {
                            Print($"\nThis is an armor of type {((Armor)wat.Value).ArmorType}.\n{wat.Value.ItemDescription}", 3);
                        }
                    } else {
                        if (item.ItemSlot == Slot.Quest) {
                            Print($"\n{item.ItemDescription}", 3);
                        } else if (item.ItemSlot == Slot.Weapon) {
                            Print($"\nThis is a weapon of type {((Weapon)item).WeaponType}.\n{item.ItemDescription}", 3);
                        } else {
                            Print($"\nThis is an armor of type {((Armor)item).ArmorType}.\n{item.ItemDescription}", 3);
                        }
                    }
                    TextInput.PressToContinue();

                } else if (input[0] == "b" || input[0] == "back") {
                    break;
                }
            }
        }
        public static void TopCombatHUD(Enemy Monster, Encounters TurnTimer) {
            Console.Clear();
            Print($"Turn: {TurnTimer.TurnTimer}",5);
            Print($"Fighting: {Monster.Name}!", 10);
            Print($"Strength: {Monster.Power} / HP: {Monster.Health}", 10);
            if (Program.CurrentPlayer.TotalSecondaryAttributes.Awareness > Monster.Awareness) {
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
            Console.WriteLine($"Turn: {TurnTimer.TurnTimer}");
            Console.WriteLine($"Fighting: {Monster.Name}!");
            Console.WriteLine($"Strength: {Monster.Power} / HP: {Monster.Health}");
            if (Program.CurrentPlayer.TotalSecondaryAttributes.Awareness > Monster.Awareness) {
                Console.WriteLine("---------------------------");
                Console.WriteLine("You go first!");
            } else {
                Console.WriteLine("The enemy goes first!");
                Console.WriteLine("---------------------------");
            }           
            Console.WriteLine($"{Program.CurrentPlayer.CurrentClass} {Program.CurrentPlayer.Name}:");
            Console.WriteLine($"Health: {Program.CurrentPlayer.Health}/{Program.CurrentPlayer.TotalSecondaryAttributes.MaxHealth}\t|| Healing Potions: {Program.CurrentPlayer.CurrentHealingPotion.PotionQuantity}");
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
            Console.WriteLine("| (Q)uestlog              |");
            Console.WriteLine("===========================");           
        }
        public static void SmallCharacterInfo() {
            Console.WriteLine($"{Program.CurrentPlayer.CurrentClass} {Program.CurrentPlayer.Name}:");
            Console.WriteLine($"Health: {Program.CurrentPlayer.Health}/{Program.CurrentPlayer.TotalSecondaryAttributes.MaxHealth}\t|| Healing Potions: {Program.CurrentPlayer.CurrentHealingPotion.PotionQuantity}");
            Console.WriteLine($"Level: {Program.CurrentPlayer.Level}\t|| Gold: ${Program.CurrentPlayer.Gold}");
            Console.Write("EXP  ");
            Console.Write("[");
            ProgressBar("+", " ", (decimal)Program.CurrentPlayer.Exp / (decimal)Program.CurrentPlayer.GetLevelUpValue(), 20);
            Console.WriteLine("]");
            Console.WriteLine("==============Actions==============");
            Console.WriteLine("V (C)haracter screen   (H)eal     V");
            Console.WriteLine("V (I)nventory          Quest(L)og V");
            Console.WriteLine("===================================\n");
        }
        public static void TopCampHUD() {
            Console.Clear();
            Print("[][][][][][]  Camp   [][][][][][]", 5);
            Print($"{Program.CurrentPlayer.CurrentClass} {Program.CurrentPlayer.Name}:", 10);
            Print($"Health: {Program.CurrentPlayer.Health}/{Program.CurrentPlayer.TotalSecondaryAttributes.MaxHealth}\t|| Healing Potions: {Program.CurrentPlayer.CurrentHealingPotion.PotionQuantity}", 10);
            Print($"Level: {Program.CurrentPlayer.Level}\t|| Gold: ${Program.CurrentPlayer.Gold}", 5);
            Print($"EXP  [{ProgressBarForPrint("+", " ", (decimal)Program.CurrentPlayer.Exp / (decimal)Program.CurrentPlayer.GetLevelUpValue(), 20)}]", 10);
        }
        public static void FullCampHUD() {
            Console.Clear();
            Console.WriteLine("[][][][][][]  Camp   [][][][][][]");
            Console.WriteLine($"{Program.CurrentPlayer.CurrentClass} {Program.CurrentPlayer.Name}:");
            Console.WriteLine($"Health: {Program.CurrentPlayer.Health}/{Program.CurrentPlayer.TotalSecondaryAttributes.MaxHealth}\t|| Healing Potions: {Program.CurrentPlayer.CurrentHealingPotion.PotionQuantity}");
            Console.WriteLine($"Level: {Program.CurrentPlayer.Level}\t|| Gold: ${Program.CurrentPlayer.Gold}");
            Console.Write("EXP  ");
            Console.Write("[");
            ProgressBar("+", " ", (decimal)Program.CurrentPlayer.Exp / (decimal)Program.CurrentPlayer.GetLevelUpValue(), 20);
            Console.WriteLine("]");
            Console.WriteLine("==============Actions=================");
            Console.WriteLine("0 (E)xplore          (S)leep (Save)  0");
            Console.WriteLine("0 (G)heed's shop     (H)eal          0");
            Console.WriteLine("0 (C)haracter screen (I)nventory     0");
            Console.WriteLine("0 Quest(L)og         (T)alk to NPC's 0");
            Console.WriteLine("======================================");
            Console.WriteLine("  (Q)uit to Main Menu                 ");
            Console.WriteLine("Choose an action...\n");
        }
        public static void RespiteHUD() {
            Console.Clear();
            Console.WriteLine("You gain a moment of respite and a choice...");
            Console.WriteLine("Do you venture deeper or turn back to your camp?");
            Console.WriteLine($"{Program.CurrentPlayer.CurrentClass} {Program.CurrentPlayer.Name}:");
            Console.WriteLine($"Health: {Program.CurrentPlayer.Health}/{Program.CurrentPlayer.TotalSecondaryAttributes.MaxHealth}\t|| Healing Potions: {Program.CurrentPlayer.CurrentHealingPotion.PotionQuantity}");
            Console.WriteLine($"Level: {Program.CurrentPlayer.Level}\t|| Gold: ${Program.CurrentPlayer.Gold}");
            Console.Write("EXP  ");
            Console.Write("[");
            ProgressBar("+", " ", (decimal)Program.CurrentPlayer.Exp / (decimal)Program.CurrentPlayer.GetLevelUpValue(), 20);
            Console.WriteLine("]");
            Console.WriteLine("==============Actions==============");
            Console.WriteLine("V (E)xplore            (R)eturn   V");
            Console.WriteLine("V (C)haracter screen   (H)eal     V");
            Console.WriteLine("V (I)nventory          Quest(L)og V");
            Console.WriteLine("===================================");
            Console.WriteLine("Choose an action...");
        }
        public static void QuestLogHUD() {
            Console.Clear();
            Console.WriteLine($"{Program.CurrentPlayer.CurrentClass} {Program.CurrentPlayer.Name}:");
            Console.WriteLine($"Health: {Program.CurrentPlayer.Health}/{Program.CurrentPlayer.TotalSecondaryAttributes.MaxHealth}\t|| Level: {Program.CurrentPlayer.Level}");
            Console.Write("EXP  ");
            Console.Write("[");
            ProgressBar("+", " ", (decimal)Program.CurrentPlayer.Exp / (decimal)Program.CurrentPlayer.GetLevelUpValue(), 20);
            Console.WriteLine("]");
            Console.WriteLine("\n@@@@@@@@@@@@@@@@@ Quest Items @@@@@@@@@@@@@@@@@@@");
            int i = 0;
            foreach (Item item in Program.CurrentPlayer.Inventory) {
                if (item == null) {
                    i++;
                    if (i == 10) {
                        Console.WriteLine("You don't have any quest items...\n");
                    }
                    continue;
                }
                if (item.ItemSlot == Slot.Quest) {
                    Console.WriteLine($"\u001b[96m Quest Item - {item.ItemName} #{((QuestItem)item).Amount}\u001b[0m");
                }
            }
            Console.WriteLine("¤¤¤¤¤¤¤¤¤¤¤¤¤¤¤¤¤¤¤¤ Quests ¤¤¤¤¤¤¤¤¤¤¤¤¤¤¤¤¤¤¤¤");
            if (Program.CurrentPlayer.QuestLog.Count == 0) {
                Console.WriteLine("You don't have any active quests...");
            } else {
                foreach (Quest quest in Program.CurrentPlayer.QuestLog) {
                    Console.WriteLine($"\u001b[96m{quest.Name}:\u001b[0m");
                    if (!quest.Completed) {
                        Console.WriteLine(quest.Objective);
                    } else if (quest.Completed) {
                        Console.WriteLine(quest.TurnIn);
                    }
                    Console.WriteLine("Rewards:");
                    if (quest.Gold > 0 && quest.Potions > 0) {
                        Console.WriteLine($"{quest.Potions} healing potions, {quest.Gold} gold pieces and {quest.Exp} experience points.");
                    } else if (quest.Gold == 0 && quest.Potions > 0) {
                        Console.WriteLine($"{quest.Potions} healing potions and {quest.Exp} experience points.");
                    } else if (quest.Gold > 0 && quest.Potions == -1) {
                        Console.WriteLine($"{quest.Gold} gold pieces and {quest.Exp} experience points.");
                    }                 
                    if (quest.Item != null) {
                        if (quest.Item.ItemSlot == Slot.Weapon) {
                            Console.WriteLine($" {quest.Item.ItemSlot} - {quest.Item.ItemName}: +{((Weapon)quest.Item).WeaponAttributes.MinDamage}-{((Weapon)quest.Item).WeaponAttributes.MaxDamage} dmg");
                        } else if (quest.Item.ItemSlot != Slot.Quest) {
                            Console.Write($" {quest.Item.ItemSlot} - {quest.Item.ItemName}:");
                            if (((Armor)quest.Item).SecondaryAttributes.ArmorRating > 0) {
                                Console.Write($" +{((Armor)quest.Item).SecondaryAttributes.ArmorRating} Armor Rating");
                            }
                            if (((Armor)quest.Item).Attributes.Strength > 0) {
                                Console.Write($" +{((Armor)quest.Item).Attributes.Strength} Str");
                            }
                            if (((Armor)quest.Item).Attributes.Dexterity > 0) {
                                Console.Write($" +{((Armor)quest.Item).Attributes.Dexterity} Dex");
                            }
                            if (((Armor)quest.Item).Attributes.Intellect > 0) {
                                Console.Write($" +{((Armor)quest.Item).Attributes.Intellect} Int");
                            }
                            if (((Armor)quest.Item).Attributes.Constitution > 0) {
                                Console.Write($" +{((Armor)quest.Item).Attributes.Constitution} Const");
                            }
                            if (((Armor)quest.Item).Attributes.WillPower > 0) {
                                Console.Write($" +{((Armor)quest.Item).Attributes.WillPower} Wp");
                            }
                            if (quest.Item.ItemName == "Linen Rags") {
                                Console.Write(" Offers no protection");
                            }
                            Console.WriteLine("");
                        } else if (quest.Item.ItemSlot == Slot.Quest) {
                            Console.WriteLine($"\u001b[96m Quest Item - {quest.Item.ItemName}\u001b[0m");
                        }
                    }
                }
            }
            Print("\nPress to go back...", 1);
        }
        public static void TalkToNpcHUD() {
            Console.Clear();
            Print("Who would you like to talk to?", 20);
            if (Program.CurrentPlayer.NpcsInCamp.Count > 0) {
                foreach (NonPlayableCharacters npc in Program.CurrentPlayer.NpcsInCamp) {
                    Print($"({1 + Program.CurrentPlayer.NpcsInCamp.IndexOf(npc)}) - {npc.Name}", 20);
                }
            } 
            else {
                Print("There are no one in your camp :(");
            }
            Print("\nPress the number to talk to that NPC else write (b)ack", 10);
        }
    }
}
