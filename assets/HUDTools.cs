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
using System.Linq;
using System.Reflection;

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

        //Read all lines fra embedded resource
        public static List<string> ReadAllResourceLines(string resourceName) {
            using (Stream stream = Assembly.GetEntryAssembly()
                .GetManifestResourceStream(resourceName))
            using (StreamReader reader = new StreamReader(stream)) {
                return EnumerateLines(reader).ToList();
            }
        }
        static IEnumerable<string> EnumerateLines(TextReader reader) {
            string line;

            while ((line = reader.ReadLine()) != null) {
                yield return line;
            }
        }


        //HUDS
        public static void MainMenu() {
            Console.Clear();
            Console.WriteLine("########         ##         ##########         ##       ");
            Console.WriteLine("########        ####        ##########        ####      ");
            Console.WriteLine("##             ##  ##       ##               ##  ##     ");
            Console.WriteLine("##            ##    ##      ##              ##    ##    ");
            Console.WriteLine("########     ##      ##     ##   #####     ##      ##   ");
            Console.WriteLine("########    ############    ##   #####    ############  ");
            Console.WriteLine("      ##   ##############   ##      ##   ############## ");
            Console.WriteLine("      ##   ##          ##   ##      ##   ##          ## ");
            Console.WriteLine("########  ##            ##  ##########  ##            ##");
            Console.WriteLine("########  ##            ##  ##########  ##            ##");
            Console.WriteLine("~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~\n");
            Console.WriteLine("\t\t1.         Play\n");
            Console.WriteLine("\t\t2.       Settings\n");
            Console.WriteLine("\t\t3.       Quit Game\n");
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
        public static void BuyShopHUD(Shop shop) {
            Console.Clear();
            Console.WriteLine("         Gheed's Shop        ");
            Console.WriteLine("=======================================================");
            Console.WriteLine($"| (P)otions:              $ {Shop.ShopPrice("potion")}");
            Console.WriteLine($"| Up(g)rade potion        $ {Shop.ShopPrice("potionupgrade")}");
            Console.WriteLine($"|\n| Items for sale:");
            foreach (Item item in shop.Forsale) {
                if (item == null) {
                }
                else if (item.ItemSlot == Slot.Weapon) {
                    Console.WriteLine($"| ({shop.Forsale.IndexOf(item)}) Ilvl: {item.ItemLevel}, {((Weapon)item).WeaponType}, {item.ItemName}, $ {item.CalculateItemPrice()}, +{((Weapon)item).WeaponAttributes.MinDamage}-{((Weapon)item).WeaponAttributes.MaxDamage} dmg");
                }
                else {
                    Console.Write($"| ({shop.Forsale.IndexOf(item)}) Ilvl: {item.ItemLevel}, {item.ItemSlot}, {((Armor)item).ArmorType}, {item.ItemName}, $ {item.CalculateItemPrice()},");
                    if (((Armor)item).SecondaryAttributes.ArmorRating > 0) {
                        Console.Write($" +{((Armor)item).SecondaryAttributes.ArmorRating} Armor Rating");
                    }
                    if (((Armor)item).Attributes.Strength > 0) {
                        Console.Write($", +{((Armor)item).Attributes.Strength} Strength");
                    }
                    if (((Armor)item).Attributes.Dexterity > 0) {
                        Console.Write($", +{((Armor)item).Attributes.Dexterity} Dexterity");
                    }
                    if (((Armor)item).Attributes.Intellect > 0) {
                        Console.Write($", +{((Armor)item).Attributes.Intellect} Intellect");
                    }
                    if (((Armor)item).Attributes.Constitution > 0) {
                        Console.Write($", +{((Armor)item).Attributes.Constitution} Constitution");
                    }
                    if (((Armor)item).Attributes.WillPower > 0) {
                        Console.Write($", +{((Armor)item).Attributes.WillPower} Willpower");
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
                else {
                    Console.Write($"| {item.ItemName}:");
                    if (((Armor)item).SecondaryAttributes.ArmorRating > 0) {
                        Console.Write($" +{((Armor)item).SecondaryAttributes.ArmorRating} Armor Rating");
                    }
                    if (((Armor)item).Attributes.Strength > 0) {
                        Console.Write($", +{((Armor)item).Attributes.Strength} Strength");
                    }
                    if (((Armor)item).Attributes.Dexterity > 0) {
                        Console.Write($", +{((Armor)item).Attributes.Dexterity} Dexterity");
                    }
                    if (((Armor)item).Attributes.Intellect > 0) {
                        Console.Write($", +{((Armor)item).Attributes.Intellect} Intellect");
                    }
                    if (((Armor)item).Attributes.Constitution > 0) {
                        Console.Write($", +{((Armor)item).Attributes.Constitution} Constitution");
                    }
                    if (((Armor)item).Attributes.WillPower > 0) {
                        Console.Write($", +{((Armor)item).Attributes.WillPower} Willpower");
                    }
                    if (item.ItemName == "Linen Rags") {
                        Console.Write(" Offers no protection");
                    }
                    Console.WriteLine("");
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
                    Console.WriteLine($"| ({Array.IndexOf(Program.CurrentPlayer.Inventory, item)}) {item.ItemName}: +{((Weapon)item).WeaponAttributes.MinDamage}-{((Weapon)item).WeaponAttributes.MaxDamage} dmg,\t $ {Shop.ShopPrice(Array.IndexOf(Program.CurrentPlayer.Inventory, item).ToString())}");
                }
                else {
                    Console.Write($"| ({Array.IndexOf(Program.CurrentPlayer.Inventory, item)}) {item.ItemName}: ");
                    if (((Armor)item).SecondaryAttributes.ArmorRating > 0) {
                        Console.Write($" +{((Armor)item).SecondaryAttributes.ArmorRating} Armor Rating");
                    }
                    if (((Armor)item).Attributes.Strength > 0) {
                        Console.Write($", +{((Armor)item).Attributes.Strength} Strength");
                    }
                    if (((Armor)item).Attributes.Dexterity > 0) {
                        Console.Write($", +{((Armor)item).Attributes.Dexterity} Dexterity");
                    }
                    if (((Armor)item).Attributes.Intellect > 0) {
                        Console.Write($", +{((Armor)item).Attributes.Intellect} Intellect");
                    }
                    if (((Armor)item).Attributes.Constitution > 0) {
                        Console.Write($", +{((Armor)item).Attributes.Constitution} Constitution");
                    }
                    if (((Armor)item).Attributes.WillPower > 0) {
                        Console.WriteLine($", +{((Armor)item).Attributes.WillPower} Willpower");
                    }
                    if (item.ItemName == "Linen Rags") {
                        Console.Write(" Offers no protection");
                    }
                    Console.WriteLine($"\t $ {Shop.ShopPrice(Array.IndexOf(Program.CurrentPlayer.Inventory, item).ToString())}");
                }
            }
            Console.WriteLine($"|  Sell    (P)otion     $ {Shop.ShopPrice("sellpotion")}");
            if (Program.CurrentPlayer.CurrentHealingPotion.PotionQuantity >= 5) {
                Console.WriteLine($"|  Sell (5)xPotions     $ {Shop.ShopPrice("sellpotion5")}");
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
            StringBuilder stats = new StringBuilder("~~~~~~~~~~~~~~~~~~~ Character screen ~~~~~~~~~~~~~~~~~~~~~~~\n");

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
        public static void InventoryScreen() {
            while (true) {
                Console.Clear();
                Console.WriteLine("******************** Equipment *****************************");
                foreach (KeyValuePair<Slot, Item> entry in Program.CurrentPlayer.Equipment) {
                    if (entry.Key == Slot.Weapon) {
                        Console.WriteLine($" {entry.Value.ItemSlot} - {entry.Value.ItemName}: +{((Weapon)entry.Value).WeaponAttributes.MinDamage}-{((Weapon)entry.Value).WeaponAttributes.MaxDamage} dmg");
                    }
                    else {
                        Console.Write($" {entry.Value.ItemSlot} - {entry.Value.ItemName}:");
                        if (((Armor)entry.Value).SecondaryAttributes.ArmorRating > 0) {
                            Console.Write($" +{((Armor)entry.Value).SecondaryAttributes.ArmorRating} Armor Rating");
                        }
                        if (((Armor)entry.Value).Attributes.Strength > 0) {
                            Console.Write($", +{((Armor)entry.Value).Attributes.Strength} Strength");
                        }
                        if (((Armor)entry.Value).Attributes.Dexterity > 0) {
                            Console.Write($", +{((Armor)entry.Value).Attributes.Dexterity} Dexterity");
                        }
                        if (((Armor)entry.Value).Attributes.Intellect > 0) {
                            Console.Write($", +{((Armor)entry.Value).Attributes.Intellect} Intellect");
                        }
                        if (((Armor)entry.Value).Attributes.Constitution > 0) {
                            Console.Write($", +{((Armor)entry.Value).Attributes.Constitution} Constitution");
                        }
                        if (((Armor)entry.Value).Attributes.WillPower > 0) {
                            Console.Write($", +{((Armor)entry.Value).Attributes.WillPower} Willpower");
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
                        Console.ForegroundColor = ConsoleColor.DarkGray;
                        Console.WriteLine(" Empty slot");
                        Console.ResetColor();
                    }
                    else if (item.ItemSlot == Slot.Weapon) {
                        Console.WriteLine($" {item.ItemSlot} - {item.ItemName}: +{((Weapon)item).WeaponAttributes.MinDamage}-{((Weapon)item).WeaponAttributes.MaxDamage} dmg");
                    }
                    else if (item.ItemSlot != Slot.Quest) {
                        Console.Write($" {item.ItemSlot} - {item.ItemName}:");
                        if (((Armor)item).SecondaryAttributes.ArmorRating > 0) {
                            Console.Write($" +{((Armor)item).SecondaryAttributes.ArmorRating} Armor Rating");
                        }
                        if (((Armor)item).Attributes.Strength > 0) {
                            Console.Write($" +{((Armor)item).Attributes.Strength} Strength");
                        }
                        if (((Armor)item).Attributes.Dexterity > 0) {
                            Console.Write($" +{((Armor)item).Attributes.Dexterity} Dexterity");
                        }
                        if (((Armor)item).Attributes.Intellect > 0) {
                            Console.Write($" +{((Armor)item).Attributes.Intellect} Intellect");
                        }
                        if (((Armor)item).Attributes.Constitution > 0) {
                            Console.Write($" +{((Armor)item).Attributes.Constitution} Constitution");
                        }
                        if (((Armor)item).Attributes.WillPower > 0) {
                            Console.Write($" +{((Armor)item).Attributes.WillPower} Willpower");
                        }
                        if (item.ItemName == "Linen Rags") {
                            Console.Write(" Offers no protection");
                        }
                        Console.WriteLine("");
                    }
                    else if ( item.ItemSlot == Slot.Quest) {
                        Console.ForegroundColor = ConsoleColor.Cyan;
                        Console.WriteLine($" Quest Item - {item.ItemName}");
                        Console.ResetColor();
                    }
                }
                Print($"\nTo equip item write 'equip_Itemname', to unequip item write 'unequip_Itemname'\nTo examine item write examine_Itemname else (b)ack", 1);
                string[] input = Console.ReadLine().ToLower().Split('_');
                if (input[0] == "equip") {
                    if (Program.CurrentPlayer.Inventory.All(x => x == null)) {
                        Console.WriteLine("No items in inventory...");                    
                    } else {
                        var item = Program.CurrentPlayer.Inventory.FirstOrDefault(x => x?.ItemName.ToLower() == input[1]);
                        if (item != null) {
                            if (item.ItemSlot == Slot.Quest) {
                                Print("You cannot equip this item...", 3);
                            } else if (item.ItemSlot == Slot.Weapon) {
                                Print(Program.CurrentPlayer.Equip((Weapon)item), 3);
                            } else if (item.ItemSlot != Slot.Weapon) {
                                Print(Program.CurrentPlayer.Equip((Armor)item), 3);
                            }
                        } else {
                            Console.WriteLine("No such item in inventory...");
                        }
                    }
                    PlayerPrompt();
                } else if (input[0] == "unequip") {
                    var wat = Program.CurrentPlayer.Equipment.FirstOrDefault(x => x.Value.ItemName.ToLower() == input[1]);
                    if (wat.Value == null) {
                        Console.WriteLine("No such item equipped...");                       
                    } else {
                        Print(Program.CurrentPlayer.UnEquip(wat.Key, wat.Value), 3);                       
                    }
                    PlayerPrompt();
                } else if (input[0] == "examine") {
                    var wat = Program.CurrentPlayer.Equipment.FirstOrDefault(x => x.Value.ItemName.ToLower() == input[1]);
                    var item = Program.CurrentPlayer.Inventory.FirstOrDefault(x => x?.ItemName.ToLower() == input[1]);
                    if (wat.Value == null && item == null) {
                        Console.WriteLine("No such item exists...");
                    } else if (item != null && item.ItemSlot == Slot.Weapon) {
                        Print($"This is a weapon of type {((Weapon)item).WeaponType}.", 3);
                    } else if (wat.Value != null && wat.Value.ItemSlot == Slot.Weapon) {
                        Print($"This is a weapon of type {((Weapon)wat.Value).WeaponType}.", 3);
                    } else if (item != null && item.ItemSlot == Slot.Quest) {
                        Print(((QuestItem)item).ItemDescription, 3);
                    } else if (item != null && item.ItemSlot != Slot.Quest && item.ItemSlot != Slot.Weapon) {
                        Print($"This is an armor of type {((Armor)item).ArmorType}.", 3);
                    } else if (wat.Value != null && wat.Value.ItemSlot != Slot.Quest && wat.Value.ItemSlot != Slot.Weapon) {
                        Print($"This is an armor of type {((Armor)wat.Value).ArmorType}.", 3);
                    }
                    PlayerPrompt();
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
            Console.WriteLine("Choose an action...");
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
                    Console.ForegroundColor = ConsoleColor.Cyan;
                    Console.WriteLine($" Quest Item - {item.ItemName}");
                    Console.ResetColor();
                }
            }
            Console.WriteLine("¤¤¤¤¤¤¤¤¤¤¤¤¤¤¤¤¤¤¤¤ Quests ¤¤¤¤¤¤¤¤¤¤¤¤¤¤¤¤¤¤¤¤");
            if (Program.CurrentPlayer.QuestLog.Count() == 0) {
                Console.WriteLine("You don't have any active quests...");
            } else {
                foreach (Quest quest in Program.CurrentPlayer.QuestLog) {
                    Console.ForegroundColor = ConsoleColor.Cyan;
                    Console.WriteLine($"{quest.Name}:");
                    Console.ResetColor();
                    if (!quest.Completed) {
                        Console.WriteLine(quest.Objective);
                    } else if (quest.Completed) {
                        Console.WriteLine(quest.TurnIn);
                    }
                    Console.WriteLine("Rewards:");
                    Console.WriteLine($"{quest.Gold} gold pieces and {quest.Exp} experience points.");
                    if (quest.Item != null) {
                        if (quest.Item.ItemSlot == Slot.Weapon) {
                            Console.WriteLine($" {quest.Item.ItemSlot} - {quest.Item.ItemName}: +{((Weapon)quest.Item).WeaponAttributes.MinDamage}-{((Weapon)quest.Item).WeaponAttributes.MaxDamage} dmg");
                        } else if (quest.Item.ItemSlot != Slot.Quest) {
                            Console.Write($" {quest.Item.ItemSlot} - {quest.Item.ItemName}:");
                            if (((Armor)quest.Item).SecondaryAttributes.ArmorRating > 0) {
                                Console.Write($" +{((Armor)quest.Item).SecondaryAttributes.ArmorRating} Armor Rating");
                            }
                            if (((Armor)quest.Item).Attributes.Strength > 0) {
                                Console.Write($" +{((Armor)quest.Item).Attributes.Strength} Strength");
                            }
                            if (((Armor)quest.Item).Attributes.Dexterity > 0) {
                                Console.Write($" +{((Armor)quest.Item).Attributes.Dexterity} Dexterity");
                            }
                            if (((Armor)quest.Item).Attributes.Intellect > 0) {
                                Console.Write($" +{((Armor)quest.Item).Attributes.Intellect} Intellect");
                            }
                            if (((Armor)quest.Item).Attributes.Constitution > 0) {
                                Console.Write($" +{((Armor)quest.Item).Attributes.Constitution} Constitution");
                            }
                            if (((Armor)quest.Item).Attributes.WillPower > 0) {
                                Console.Write($" +{((Armor)quest.Item).Attributes.WillPower} Willpower");
                            }
                            if (quest.Item.ItemName == "Linen Rags") {
                                Console.Write(" Offers no protection");
                            }
                            Console.WriteLine("");
                        } else if (quest.Item.ItemSlot == Slot.Quest) {
                            Console.ForegroundColor = ConsoleColor.Cyan;
                            Console.WriteLine($" Quest Item - {quest.Item.ItemName}");
                            Console.ResetColor();
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
                    Print($"({Program.CurrentPlayer.NpcsInCamp.IndexOf(npc)}) - {npc.Name}");
                }
            } 
            else {
                Print("There are no one in your camp :(");
            }
            Print("\nPress the number to talk to that NPC else write (b)ack", 10);
        }
    }
}
