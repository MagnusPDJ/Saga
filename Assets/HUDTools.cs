using Saga.Character;
using Saga.Character.DmgLogic;
using Saga.Dungeon;
using Saga.Dungeon.Monsters;
using Saga.Dungeon.Quests;
using Saga.Items;
using Saga.Items.Loot;
using System.Configuration;
using System.Reflection;
using System.Text;

namespace Saga.Assets
{
    public class HUDTools
    {
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
        public static void WriteCombatLog(string action, Enemy monster, int damage=0, int attack = 0) {
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
            if (Program.CurrentPlayer.DerivedStats.Initiative > monster.Initiative) {
                switch (action) {
                    case "attack":
                        Console.WriteLine($"Turn: \nYou attacked and dealt {attack} damage.");
                        break;
                    case "defend":
                        Console.WriteLine($"Turn: \nYou defended and lowered the next two attacks.");
                        break;
                    case "heal":
                        Console.WriteLine($"Turn: ");
                        var potion = Array.Find(Program.CurrentPlayer.Equipment.Potion, p => p is IItem { ItemName: "Healing Potion" });
                        if (potion != null) {
                            if (potion.PotionQuantity == 0) {
                                Console.WriteLine($"You tried to drink a potion you didn't have.");
                            } else {
                                if (Program.CurrentPlayer.DerivedStats.MaxHealth == Program.CurrentPlayer.Health) {
                                    Console.WriteLine("You healed to max health by drinking a potion.");
                                } else {
                                    int mageBonus = (Program.CurrentPlayer.CurrentClass == "Mage" ? 1 + Program.CurrentPlayer.Level * 2 : 0);
                                    Console.WriteLine($"You gained {potion.PotionPotency + mageBonus} health by drinking a potion.");
                                }
                            }
                        }                       
                        break;
                    case "run":
                        Console.WriteLine($"Turn: ");
                        Console.WriteLine($"You tried to run but was unable to escape this turn.");
                        break;
                    case "enemysecond":
                        Console.WriteLine($"{monster.Name} attacked and dealt {damage} damage!");
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
                        var potion = Array.Find(Program.CurrentPlayer.Equipment.Potion, p => p is IItem { ItemName: "Healing Potion" });
                        if (potion != null) {
                            if (potion.PotionQuantity == 0) {
                                Console.WriteLine($"You tried to drink a potion you didn't have.");
                            } else {
                                if (Program.CurrentPlayer.DerivedStats.MaxHealth == Program.CurrentPlayer.Health) {
                                    Console.WriteLine("You healed to max health by drinking a potion.");
                                } else {
                                    int mageBonus = (Program.CurrentPlayer.CurrentClass == "Mage" ? 1 + Program.CurrentPlayer.Level * 2 : 0);
                                    Console.WriteLine($"You gained {potion.PotionPotency + mageBonus} health by drinking a potion.");
                                }
                            }
                        }
                        break;
                    case "run":
                        Console.WriteLine($"You tried to run but was unable to escape this turn.");
                        break;
                    case "enemyfirst":
                        Console.WriteLine($"Turn: ");
                        Console.WriteLine($"{monster.Name} attacked and dealt {damage} damage!");
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

        //Read all lines fra embedded resource til en liste.
        public static List<string> ReadAllResourceLines(string resourceName) {
            ArgumentNullException.ThrowIfNull(resourceName);

            Assembly? assembly = Assembly.GetEntryAssembly() ?? throw new InvalidOperationException("No entry assembly found.");
            using Stream? stream = assembly.GetManifestResourceStream(resourceName) ?? throw new FileNotFoundException($"Resource '{resourceName}' not found in assembly.");
            using StreamReader reader = new(stream);
            return [.. EnumerateLines(reader)];
        }
        static IEnumerable<string> EnumerateLines(StreamReader reader) {
            string? line;
            while ((line = reader.ReadLine()) != null) {
                yield return line;
            }
        }

        //Read all text fra embedded reource til en string.
        public static string ReadAllResourceText(string resourceName) {
            ArgumentNullException.ThrowIfNull(resourceName);

            Assembly? assembly = Assembly.GetEntryAssembly() ?? throw new InvalidOperationException("No entry assembly found.");
            using Stream stream = assembly.GetManifestResourceStream(resourceName) ?? throw new FileNotFoundException($"Resource '{resourceName}' not found in assembly.");
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
        public static void Settings() {
            Console.Clear();
            var configFile = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
            var settings = configFile.AppSettings.Settings;
            Console.WriteLine("               Settings               ");
            Console.WriteLine("======================================\n");
            Console.WriteLine($" 1. Toggle automatically ending turn: {settings["toggleAutoEndturn"].Value}");
            Console.WriteLine($" 2. Toggle slow-printing text:        {settings["toggleSlowPrint"].Value}");
            Console.WriteLine($" 3. Game Volume:                      {settings["volume"].Value}\n");
            Console.WriteLine($"======== Press Esc to go back =======");
        }
        public static void LoadSaves(List<Player> players) {
            Print(" Choose a save! ('back' for main menu) ", 0);
            Print("-.-.-.-.-.-.-.-.-.-.-.-.-.-.-.-.", 0);
            Print("#: playername", 0);
            foreach (Player p in players) {
                Print($" {p.Id}: {p.Name} - Class: {p.CurrentClass} - Level: {p.Level}", 5);
            }
            Print("<><><><><><><><><><><><><><><><>", 0);
            Print(" To load a save write 'id:#' or 'playername'.\n For new game write 'new game'.\n To delete a save write 'delete:playername'.\n", 1);
        }
        public static void BuyShopHUD(Shop shop) {
            Console.Clear();
            Console.WriteLine("         Gheed's Shop        ");
            Console.WriteLine("=======================================================");
            Console.WriteLine($"| Items for sale:");
            foreach (IEquipable item in shop.GetForsale()) {
                if (item == null) {
                }
                else if (item.ItemSlot == Slot.Right_Hand) {
                    Console.WriteLine($"| ({1 + shop.GetForsale().IndexOf(item)}) Ilvl: {item.ItemLevel}, {((IWeapon)item).WeaponCategory}, {item.ItemName}, $ {item.CalculateItemPrice()}, +{((IWeapon)item).WeaponAttributes.MinDamage}-{((IWeapon)item).WeaponAttributes.MaxDamage} dmg");
                }
                else {
                    Console.Write($"| ({1 + shop.GetForsale().IndexOf(item)}) Ilvl: {item.ItemLevel}, {item.ItemSlot}, {((ArmorBase)item).ArmorType}, {item.ItemName}, $ {item.CalculateItemPrice()},");
                    if (((ArmorBase)item).SecondaryAffixes.ArmorRating > 0) {
                        Console.Write($" +{((ArmorBase)item).SecondaryAffixes.ArmorRating} Armor Rating");
                    }
                    if (((ArmorBase)item).PrimaryAffixes.Strength > 0) {
                        Console.Write($", +{((ArmorBase)item).PrimaryAffixes.Strength} Str");
                    }
                    if (((ArmorBase)item).PrimaryAffixes.Dexterity > 0) {
                        Console.Write($", +{((ArmorBase)item).PrimaryAffixes.Dexterity} Dex");
                    }
                    if (((ArmorBase)item).PrimaryAffixes.Intellect > 0) {
                        Console.Write($", +{((ArmorBase)item).PrimaryAffixes.Intellect} Int");
                    }
                    if (((ArmorBase)item).PrimaryAffixes.Constitution > 0) {
                        Console.Write($", +{((ArmorBase)item).PrimaryAffixes.Constitution} Const");
                    }
                    if (((ArmorBase)item).PrimaryAffixes.WillPower > 0) {
                        Console.Write($", +{((ArmorBase)item).PrimaryAffixes.WillPower} Wp");
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
            ProgressBar("+", " ", (decimal)Program.CurrentPlayer.Exp / (decimal)Program.CurrentPlayer.GetLevelUpValue(), 20);
            Console.WriteLine("]");
            Console.WriteLine($"| Health:                 {Program.CurrentPlayer.Health}/{Program.CurrentPlayer.DerivedStats.MaxHealth}");
            Console.WriteLine($"| Gold:                  ${Program.CurrentPlayer.Gold}");
            Console.WriteLine($"| Healing Potions:        {Array.Find(Program.CurrentPlayer.Equipment.Potion, (p => p is IItem { ItemName: "Healing Potion" }))?.PotionQuantity}");
            Console.WriteLine($"| Items in inventory:");
            foreach (IItem item in Program.CurrentPlayer.Inventory) {
                if (item == null) {
                } else if (item is IQuestItem item1) {
                    Console.WriteLine($"| \u001b[96mQuest Item - {item.ItemName} #{item1.Amount}\u001b[0m");
                } else if (((IEquipable)item).ItemSlot == Slot.Right_Hand) {
                    Console.WriteLine($"| {item.ItemName}: +{((IWeapon)item).WeaponAttributes.MinDamage}-{((IWeapon)item).WeaponAttributes.MaxDamage} dmg");
                } else if (item is IArmor) {
                    Console.Write($"| {item.ItemName}:");
                    if (((ArmorBase)item).SecondaryAffixes.ArmorRating > 0) {
                        Console.Write($" +{((ArmorBase)item).SecondaryAffixes.ArmorRating} Armor Rating");
                    }
                    if (((ArmorBase)item).PrimaryAffixes.Strength > 0) {
                        Console.Write($", +{((ArmorBase)item).PrimaryAffixes.Strength} Str");
                    }
                    if (((ArmorBase)item).PrimaryAffixes.Dexterity > 0) {
                        Console.Write($", +{((ArmorBase)item).PrimaryAffixes.Dexterity} Dex");
                    }
                    if (((ArmorBase)item).PrimaryAffixes.Intellect > 0) {
                        Console.Write($", +{((ArmorBase)item).PrimaryAffixes.Intellect} Int");
                    }
                    if (((ArmorBase)item).PrimaryAffixes.Constitution > 0) {
                        Console.Write($", +{((ArmorBase)item).PrimaryAffixes.Constitution} Const");
                    }
                    if (((ArmorBase)item).PrimaryAffixes.WillPower > 0) {
                        Console.Write($", +{((ArmorBase)item).PrimaryAffixes.WillPower} Wp");
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
            foreach (IItem item in Program.CurrentPlayer.Inventory) {
                if (item == null) {
                } else if (item is IQuestItem item1) {
                    Console.WriteLine($"| \u001b[96mQuest Item - {item.ItemName} #{item1.Amount}\u001b[0m");
                } else if (((IEquipable)item).ItemSlot == Slot.Right_Hand) {
                    Console.WriteLine($"| ({1 + Array.IndexOf(Program.CurrentPlayer.Inventory, item)}) {item.ItemName}: +{((IWeapon)item).WeaponAttributes.MinDamage}-{((IWeapon)item).WeaponAttributes.MaxDamage} dmg,\t $ {Shop.ShopPrice((1 + Array.IndexOf(Program.CurrentPlayer.Inventory, item)).ToString())}");
                } else if (item is IArmor) {
                    Console.Write($"| ({1 + Array.IndexOf(Program.CurrentPlayer.Inventory, item)}) {item.ItemName}: ");
                    if (((ArmorBase)item).SecondaryAffixes.ArmorRating > 0) {
                        Console.Write($" +{((ArmorBase)item).SecondaryAffixes.ArmorRating} Armor Rating");
                    }
                    if (((ArmorBase)item).PrimaryAffixes.Strength > 0) {
                        Console.Write($", +{((ArmorBase)item).PrimaryAffixes.Strength} Str");
                    }
                    if (((ArmorBase)item).PrimaryAffixes.Dexterity > 0) {
                        Console.Write($", +{((ArmorBase)item).PrimaryAffixes.Dexterity} Dex");
                    }
                    if (((ArmorBase)item).PrimaryAffixes.Intellect > 0) {
                        Console.Write($", +{((ArmorBase)item).PrimaryAffixes.Intellect} Int");
                    }
                    if (((ArmorBase)item).PrimaryAffixes.Constitution > 0) {
                        Console.Write($", +{((ArmorBase)item).PrimaryAffixes.Constitution} Cons");
                    }
                    if (((ArmorBase)item).PrimaryAffixes.WillPower > 0) {
                        Console.WriteLine($", +{((ArmorBase)item).PrimaryAffixes.WillPower} Wp");
                    }
                    if (item.ItemName == "Linen Rags") {
                        Console.Write(" Offers no protection");
                    }
                    Console.WriteLine($"\t $ {Shop.ShopPrice((1 + Array.IndexOf(Program.CurrentPlayer.Inventory, item)).ToString())}");
                }
            }
                Console.WriteLine($"|  Sell Healing (P)otion        $ {Shop.ShopPrice("sellpotion")}");
            var potion = Array.Find(Program.CurrentPlayer.Equipment.Potion, p => p is IItem { ItemName: "Healing Potion" });
            if (potion is not null && potion.PotionQuantity >= 5) {
                Console.WriteLine($"|  Sell (F)ive Healing Potions  $ {Shop.ShopPrice("sellpotion5")}");
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
            Console.WriteLine($"| Health:                 {Program.CurrentPlayer.Health}/{Program.CurrentPlayer.DerivedStats.MaxHealth}");
            Console.WriteLine($"| Gold:                  ${Program.CurrentPlayer.Gold}");
            Console.WriteLine($"| Healing Potions:        {potion?.PotionQuantity}");
            Console.WriteLine("==============================");
            Console.WriteLine(" (U)se Healing Potion (C)haracter screen\n (I)nventory (Q)uestlog\n");
            Console.WriteLine("Choose what to sell");
        }
        public static void DisplayStats(Player player) {
            StringBuilder stats = new("~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~ Character screen ~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~\n");
            //                        "                                                                                                    "
                  stats.AppendFormat($" Name: {player.Name}\t\t\tClass: {player.CurrentClass}\n");
                  stats.AppendFormat($" Level: {player.Level}\t\t\tTimes Explored: {player.TimesExplored}\n");
                  stats.AppendFormat($" EXP  [{ProgressBarForPrint("+", " ", (decimal)Program.CurrentPlayer.Exp / (decimal)Program.CurrentPlayer.GetLevelUpValue(), 25)}] {Program.CurrentPlayer.Exp}/{Program.CurrentPlayer.GetLevelUpValue()}\n");
                stats.AppendFormat($"\n---------------------------------------------- Stats -----------------------------------------------\n");
                  stats.AppendFormat($" ┌(S)trength ⤵            {player.Attributes.Strength    }\t=> Elemental Resistance:  {player.DerivedStats.ElementalResistance}\n");
                  stats.AppendFormat($" │      (C)onstitution⤵   {player.Attributes.Constitution}\t=> Health:                {player.Health} / {player.DerivedStats.MaxHealth}\n");
                  stats.AppendFormat($" │(D)exterity ⇵           {player.Attributes.Dexterity   }\t=> Armor Rating:          {player.DerivedStats.ArmorRating}\n");
                  stats.AppendFormat($" │      (A)wareness ⤵     {player.Attributes.Awareness   }\t=> Initiative:            {player.DerivedStats.Initiative}\n");
                  stats.AppendFormat($" │(I)ntellect ⇵           {player.Attributes.Intellect   }\t=> Magical Resistance:    {player.DerivedStats.MagicalResistance}\n");                 
                  stats.AppendFormat($" └>     (W)illpower ⤵     {player.Attributes.WillPower   }\t=> Mana:                  {player.Mana} / {player.DerivedStats.MaxMana}\n");
                  stats.AppendFormat($"                 Virtue   {player.Attributes.Virtue      }\t=> Action Points:         {player.DerivedStats.ActionPoints}\n");
                  stats.AppendFormat($"  Attribute points to spend: {Program.CurrentPlayer.FreeAttributePoints}\n\n");
                  stats.AppendFormat($" Attack Speed:      {player.DerivedStats.AttackSpeed}\t(Constitution and Awareness)\n");
                  stats.AppendFormat($" Casting Speed:     {player.DerivedStats.CastingSpeed}\t(Willpower and Awareness)\n");
                  stats.AppendFormat($" Mana Regen:        {player.DerivedStats.ManaRegenRate}\t(Constitution and Wilpower)\n");
                stats.AppendFormat($"\n----------------------------------------- Equipment Stats ------------------------------------------\n");
                   stats.AppendFormat(" Weapon Attributes:\n");      
                  stats.AppendFormat($"  Weapon Damage:  {(player.Equipment.Right_Hand as IWeapon)?.WeaponAttributes.MinDamage}-" +
                                                     $"{(player.Equipment.Right_Hand as IWeapon)?.WeaponAttributes.MaxDamage}" +
                                                     $"{(Program.CurrentPlayer.CurrentClass == "Warrior" && (Program.CurrentPlayer.Equipment.Right_Hand as IWeapon is IPhysical) ?
                                                     $"(+{Program.CurrentPlayer.Level} warrior bonus)" :
                                                     "")},\t+Attack Speed:  {player.Equipment.BonusAttackSpeed},\t+Casting Speed:  {player.Equipment.BonusCastingSpeed}.\n");
                stats.AppendFormat($"\n Primary Attributes:\n");
                  stats.AppendFormat($"  +Str:  {player.Equipment.BonusStr},  +Dex:  {player.Equipment.BonusDex},  +Int:  {player.Equipment.BonusInt},  +Con:  {player.Equipment.BonusCon},  +Awa:  {player.Equipment.BonusAwa}, +Wp:  {player.Equipment.BonusWP},  +Virtue:  {player.Equipment.BonusVirtue},\n");
               stats.AppendFormat($"\n Secondary Attributes:\n");
            stats.AppendFormat($"  +Max Health:  {player.Equipment.BonusHealth     },\t+Max Mana:              {player.Equipment.BonusMana        },\t+Mana Regen:          {player.Equipment.BonusManaRegenRate}.\n");
            stats.AppendFormat($"  +Initiative:  {player.Equipment.BonusInitiative },\t+Action Points:         {player.Equipment.BonusActionPoints},\n");
            stats.AppendFormat($"  +Armor:       {player.Equipment.BonusArmorRating},\t+Elemental Resistance:  {player.Equipment.BonusElementRes  },\t+Magical Resistance:  {player.Equipment.BonusMagicRes}.\n");
            
            Print(stats.ToString(),0);
        }
        public static void CharacterScreen() {
            //Metode til at kalde og gernerer en character screen som viser alle funktionelle variabler der er i brug.
            for (int i = Program.CurrentPlayer.FreeAttributePoints; i >= 0 ; i--) {
                Console.Clear();
                DisplayStats(Program.CurrentPlayer);
                i = Program.CurrentPlayer.SpendAttributePoint(i);           
            }       
        }
        public static void ShowSkillTree() {
            //Console.Clear();
            //Console.WriteLine("************************************* Skill Tree ***************************************************\n" +
            //                  "                                    ┌───────────┐\r\n" +
            //                  "                                    │  [X] ROOT │\r\n" +
            //                  "             ┌──────────────────────┴─────┬─────┴───────────────────────┐\r\n" +
            //                  "        ┌────┴────┐                       │                        ┌────┴─────┐\r\n" +
            //                  "        │[*]Attack│                       │                        │[X]Defense│\r\n" +
            //                  "        └────┬────┘                       │                        └──────┬───┘\r\n" +
            //                  "     ┌───────┴────────────┐               │                      ┌────────┴──────┐\r\n" +
            //                  " ┌───┴────┐           ┌───┴─────┐         │                ┌─────┴─────┐    ┌────┴────┐\r\n" +
            //                  " │[ ]Sword│           │[ ]Ranged│         │                │ [*]Armor  │    │[*]Shield│\r\n" +
            //                  " └───┬────┘           └───┬─────┘         │                └─────┬─────┘    └────┬────┘\r\n" +
            //                  "     │                    │               │                      │               │\r\n" +
            //                  "┌────┴────┐         ┌─────┴──────┐        │                ┌─────┴────┐      ┌───┴────┐\r\n" +
            //                  "│ [ ]Slash│         │[ ]Precision│        │                │[ ]Fortify│      │[ ]Block│\r\n" +
            //                  "└────┬────┘         └─────┬──────┘        │                └─────┬────┘      └───┬────┘\r\n" +
            //                  "     │                    │               │                      │               │\r\n" +
            //                  "┌────┴──────┐      ┌──────┴──┐            │               ┌──────┴─────┐     ┌───┴────────┐\r\n" +
            //                  "│[ ]Critical│      │[ ]Volley│            │               │[ ]Iron Skin│     │[ ]Iron Will│\r\n" +
            //                  "└───────────┘      └─────────┘            │               └────────────┘     └────────────┘\r\n" +
            //                  "             ┌────────────────────────────┴────────────────────────────┐\r\n" +
            //                  "        ┌────┴────┐                                               ┌────┴─────┐\r\n" +
            //                  "        │[ ]Magic │                                               │[ ]Utility│\r\n" +
            //                  "        └────┬────┘                                               └────┬─────┘\r\n" +
            //                  "     ┌───────┴─────────┐                                   ┌───────────┴─────────┐\r\n" +
            //                  " ┌───┴────┐        ┌───┴────┐                         ┌────┴─────┐         ┌─────┴─────┐\r\n" +
            //                  " │[ ]Fire │        │[ ]Frost│                         │[ ]Stealth│         │[ ]Crafting│\r\n" +
            //                  " └───┬────┘        └───┬────┘                         └────┬─────┘         └─────┬─────┘\r\n" +
            //                  "     │                 │                                   │                     │\r\n" +
            //                  "┌────┴──────┐      ┌───┴────────┐                   ┌──────┴────┐        ┌───────┴──┐\r\n" +
            //                  "│[ ]Fireball│      │[ ]Ice Shard│                   │[ ]Backstab│        │[ ]Alchemy│\r\n" +
            //                  "└────┬──────┘      └───┬────────┘                   └──────┬────┘        └───────┬──┘\r\n" +
            //                  "     │                 │                                   │                     │\r\n" +
            //                  "┌────┴─────┐       ┌───┴───────┐                      ┌────┴─────────┐     ┌─────┴────┐\r\n" +
            //                  "│[ ]Inferno│       │[ ]Blizzard│                      │[ ]Assassinate│     │[ ]Enchant│\r\n" +
            //                  "└──────────┘       └───────────┘                      └──────────────┘     └──────────┘\r\n" +
            //                  "****************************************************************************************************\n" +
            //                  "Legend:\r\n" +
            //                  "[X] = unlocked skill\r\n" +
            //                  "[*] = available to learn\r\n" +
            //                  "[ ] = locked\r\n" +
            //                  "Commands: \r\n" +
            //                  "- learn <SkillName>   → Unlock a skill\r\n" +
            //                  "- info <SkillName>    → View skill details\r\n" +
            //                  "- (b)ack              → Return");
            Console.Clear();
            Print($"Learned Skills:", 0);
            for (int i = 0; i < Program.CurrentPlayer.LearnedSkills.Count; i++) {
                var s = Program.CurrentPlayer.LearnedSkills[i];
                Print($"{i + 1}: {s.Name} ({s.Tier.Min}/{s.Tier.Max}) - {s.Description}", 0);
            }
            var availableSkills = Program.CurrentPlayer.SkillTree.GetAvailableSkills(Program.CurrentPlayer.Level);
            if (availableSkills.Count != 0) {
                Print("Available Skills:", 0);
                for (int i = 0; i < availableSkills.Count; i++) {
                    var s = availableSkills[i];
                    Print($"{i + 1}: {s.Name} - {s.Description} (Requires Level {s.LevelRequired}, {s.Tier.Min}/{s.Tier.Max})", 0);
                }
            }
            Print("\nEnter the number of the skill to unlock, or '(b)ack' to exit.", 10);
        }
        public static void InventoryScreen() {
            Console.Clear();
            Console.WriteLine("******************** Equipment *****************************");
            foreach (var slot in Program.CurrentPlayer.Equipment.AsEnumerable()) {
                if (slot.Value is IWeapon weapon) {
                    if (weapon is ITwoHanded) {
                        if (slot.Key != "Left_Hand") { 
                            Console.WriteLine($" Both hands - {weapon.ItemName}: +{weapon.WeaponAttributes.MinDamage}-{weapon.WeaponAttributes.MaxDamage} dmg"); 
                        }                        
                    } else {
                        Console.WriteLine($" {weapon.ItemSlot} - {weapon.ItemName}: +{weapon.WeaponAttributes.MinDamage}-{weapon.WeaponAttributes.MaxDamage} dmg");
                    }
                } else if (slot.Value is IArmor armor) {
                    Console.Write($" {armor.ItemSlot} - {armor.ItemName}:");
                    if (armor.SecondaryAffixes.ArmorRating > 0) {
                        Console.Write($" +{armor.SecondaryAffixes.ArmorRating} Armor Rating");
                    }
                    if (armor.PrimaryAffixes.Strength > 0) {
                        Console.Write($", +{armor.PrimaryAffixes.Strength} Str");
                    }
                    if (armor.PrimaryAffixes.Dexterity > 0) {
                        Console.Write($", +{armor.PrimaryAffixes.Dexterity} Dex");
                    }
                    if (armor.PrimaryAffixes.Intellect > 0) {
                        Console.Write($", +{armor.PrimaryAffixes.Intellect} Int");
                    }
                    if (armor.PrimaryAffixes.Constitution > 0) {
                        Console.Write($", +{armor.PrimaryAffixes.Constitution} Const");
                    }
                    if (armor.PrimaryAffixes.WillPower > 0) {
                        Console.Write($", +{armor.PrimaryAffixes.WillPower} Wp");
                    }
                    if (armor.PrimaryAffixes.Awareness > 0) {
                        Console.Write($", +{armor.PrimaryAffixes.Awareness} Awa");
                    }
                    if (armor.ItemName == "Linen Rags") {
                        Console.Write(" Offers no protection");
                    }
                    Console.WriteLine("");
                }
            }
            Console.WriteLine("\n@@@@@@@@@@@@@@@@@ Inventory @@@@@@@@@@@@@@@@@@@");
            Console.WriteLine($" Gold: ${Program.CurrentPlayer.Gold}");
            var hPotion = Array.Find(Program.CurrentPlayer.Equipment.Potion, p => p is IItem { ItemName: "Healing Potion" });
            var mPotion = Array.Find(Program.CurrentPlayer.Equipment.Potion, p => p is IItem { ItemName: "Mana Potion" });
            Console.WriteLine($" Healing Potions: {hPotion?.PotionQuantity ?? 0}\t\t{(hPotion is not null ? $"Potion Strength: +{hPotion.PotionPotency}" : "No healing potion equipped.")}  {(Program.CurrentPlayer.CurrentClass == "Mage" && hPotion is not null ? $"(+{1 + Program.CurrentPlayer.Level * 2} Mage bonus)" : "")}");
            Console.WriteLine($" Mana Potions:    {mPotion?.PotionQuantity ?? 0}\t\t{(mPotion is not null ? $"Potion Strength: +{mPotion.PotionPotency}" : "No mana potion equipped.")}");
            foreach (IItem item in Program.CurrentPlayer.Inventory) {
                if (item == null) {
                    Console.WriteLine("\u001b[90m Empty slot\u001b[0m");
                }  else if (item is IQuestItem item1) {
                    Console.WriteLine($"\u001b[96m Quest Item - {item.ItemName} #{item1.Amount}\u001b[0m");
                } else if (((IEquipable)item).ItemSlot == Slot.Right_Hand) {
                    if (item is ITwoHanded twoHanded) {
                        Console.WriteLine($" Both hands - {twoHanded.ItemName}: +{twoHanded.WeaponAttributes.MinDamage}-{twoHanded.WeaponAttributes.MaxDamage} dmg");
                    } else {
                        Console.WriteLine($" {((IEquipable)item).ItemSlot} - {item.ItemName}: +{((IWeapon)item).WeaponAttributes.MinDamage}-{((IWeapon)item).WeaponAttributes.MaxDamage} dmg");
                    }
                } else if (item is IArmor armor) {
                    Console.Write($" {armor.ItemSlot} - {armor.ItemName}:");
                    if (armor.SecondaryAffixes.ArmorRating > 0) {
                        Console.Write($" +{armor.SecondaryAffixes.ArmorRating} Armor Rating");
                    }
                    if (armor.PrimaryAffixes.Strength > 0) {
                        Console.Write($", +{armor.PrimaryAffixes.Strength} Str");
                    }
                    if (armor.PrimaryAffixes.Dexterity > 0) {
                        Console.Write($", +{armor.PrimaryAffixes.Dexterity} Dex");
                    }
                    if (armor.PrimaryAffixes.Intellect > 0) {
                        Console.Write($", +{armor.PrimaryAffixes.Intellect} Int");
                    }
                    if (armor.PrimaryAffixes.Constitution > 0) {
                        Console.Write($", +{armor.PrimaryAffixes.Constitution} Const");
                    }
                    if (armor.PrimaryAffixes.WillPower > 0) {
                        Console.Write($", +{armor.PrimaryAffixes.WillPower} Wp");
                    }
                    if (armor.PrimaryAffixes.Awareness > 0) {
                        Console.Write($", +{armor.PrimaryAffixes.Awareness} Awa");
                    }
                    if (item.ItemName == "Linen Rags") {
                        Console.Write(" Offers no protection");
                    }
                    Console.WriteLine("");
                }
            }
            Print($"\nTo equip item write 'equip Itemname', to unequip item write 'unequip Itemname'\nTo examine item write examine Itemname else (b)ack\n", 0);
        }
        public static void CombatHUD(Enemy Monster, CombatController combatController) {
            Console.Clear();
            Console.WriteLine($" Turn: {combatController.Turn} \tLocation: {Program.RoomController.currentRoom.roomName}\n");
            Console.WriteLine($" Fighting: {Monster.Name}!");
            Console.WriteLine($" Strength: {Monster.Power} <> Enemy health: {Monster.Health}/{Monster.MaxHealth}");
            if (Program.CurrentPlayer.DerivedStats.Initiative > Monster.Initiative) {
                Console.WriteLine("\n----------------------------------------------------------------------------------------------------");
                Console.WriteLine("  You go first!\n");
            } else {
                Console.WriteLine("\n  The enemy goes first!");
                  Console.WriteLine("----------------------------------------------------------------------------------------------------\n");
            }           
            Console.WriteLine($"\t{Program.CurrentPlayer.CurrentClass} {Program.CurrentPlayer.Name}:");
            Console.WriteLine($"\t     Your Health:   {Program.CurrentPlayer.Health}/{Program.CurrentPlayer.DerivedStats.MaxHealth}  | | Healing Potions: {Array.Find(Program.CurrentPlayer.Equipment.Potion, (p => p is IItem { ItemName: "Healing Potion" }))?.PotionQuantity ?? 0}");
            Console.WriteLine($"\t          Mana:     {Program.CurrentPlayer.Mana}/{Program.CurrentPlayer.DerivedStats.MaxMana    }  | | Mana Potions:    {Array.Find(Program.CurrentPlayer.Equipment.Potion, (p => p is IItem { ItemName: "Mana Potion" }))?.PotionQuantity ?? 0}");
            Console.WriteLine($"\t   Action Points:   {combatController.GetRemainingActionPoints()                             }\t   | | Gold: ${Program.CurrentPlayer.Gold}");
            Console.WriteLine($"\tLevel: {Program.CurrentPlayer.Level}");
                 Console.Write("\tEXP  ");Console.Write("[");ProgressBar("+", " ", (decimal)Program.CurrentPlayer.Exp / (decimal)Program.CurrentPlayer.GetLevelUpValue(), 25);Console.WriteLine("]");
            Console.WriteLine($"\n ============== Actions ============|=============== Info ==============");
            Console.WriteLine($" |  (1) Quick Cast: {(Program.CurrentPlayer.SkillTree.QuickCast != string.Empty? Program.CurrentPlayer.SkillTree.QuickCast : "\t\t   ")} |  (C)haracter screen              |");
            Console.WriteLine($" |  (2) Attack     (3) Heal         |   Combat (L)og                   |");
            Console.WriteLine($" |  (4) Run        (5) Skills       |  (Q)uestlog                      |");
            Console.WriteLine($" =============== Info ==============|===================================");
            Console.WriteLine($"  Choose an action...\n");
        }
        public static void RoomHUD() {
            Console.Clear();
            Console.WriteLine($" Location:\t{Program.RoomController.currentRoom.roomName}");
            Console.WriteLine($" {Program.CurrentPlayer.CurrentClass} {Program.CurrentPlayer.Name}:");
            Console.WriteLine($" Health: {Program.CurrentPlayer.Health}/{Program.CurrentPlayer.DerivedStats.MaxHealth}\t|| Healing Potions: {Array.Find(Program.CurrentPlayer.Equipment.Potion, (p => p is IItem { ItemName: "Healing Potion" }))?.PotionQuantity ?? 0}");
            Console.WriteLine($" Mana:   {Program.CurrentPlayer.Mana}/{Program.CurrentPlayer.DerivedStats.MaxMana}\t|| Mana Potions:    {Array.Find(Program.CurrentPlayer.Equipment.Potion, (p => p is IItem { ItemName: "Mana Potion" }))?.PotionQuantity ?? 0}");
            Console.WriteLine($" Level: {Program.CurrentPlayer.Level}\t|| Gold: ${Program.CurrentPlayer.Gold}");
            Console.Write(" EXP  ");
            Console.Write("[");
            ProgressBar("+", " ", (decimal)Program.CurrentPlayer.Exp / (decimal)Program.CurrentPlayer.GetLevelUpValue(), 20);
            Console.WriteLine("]");
            Console.WriteLine(" ==============Actions==============");
            Console.WriteLine(" V (C)haracter screen   (H)eal     V");
            Console.WriteLine(" V (I)nventory          Quest(L)og V");
            Console.WriteLine(" V S(K)illtree                     V");
            Console.WriteLine(" ===================================\n");
            Console.WriteLine(" Write an action:");
        }
        public static void FullCampHUD() {
            Console.Clear();
            Console.WriteLine("[][][][][][][][]  Camp   [][][][][][][][]");
            Console.WriteLine($" {Program.CurrentPlayer.CurrentClass} {Program.CurrentPlayer.Name}:");
            Console.WriteLine($" Health: {Program.CurrentPlayer.Health}/{Program.CurrentPlayer.DerivedStats.MaxHealth}\t|| Healing Potions: {Array.Find(Program.CurrentPlayer.Equipment.Potion, (p => p is IItem { ItemName: "Healing Potion" }))?.PotionQuantity}");
            Console.WriteLine($" Mana:   {Program.CurrentPlayer.Mana}/{Program.CurrentPlayer.DerivedStats.MaxMana}\t|| Mana Potions:    {Array.Find(Program.CurrentPlayer.Equipment.Potion, (p => p is IItem { ItemName: "Mana Potion" }))?.PotionQuantity ?? 0}");
            Console.WriteLine($" Level: {Program.CurrentPlayer.Level}\t|| Gold: ${Program.CurrentPlayer.Gold}");
            Console.Write(" EXP  ");
            Console.Write("[");
            ProgressBar("+", " ", (decimal)Program.CurrentPlayer.Exp / (decimal)Program.CurrentPlayer.GetLevelUpValue(), 20);
            Console.WriteLine("]");
            Console.WriteLine(" ==============Actions=================");
            Console.WriteLine(" 0 (E)xplore          (S)leep (Save)  0");
            Console.WriteLine(" 0 (G)heed's shop     (H)eal          0");
            Console.WriteLine(" 0 (C)haracter screen (I)nventory     0");
            Console.WriteLine(" 0 Quest(L)og         (T)alk to NPC's 0");
            Console.WriteLine(" 0 S(K)illtree                        0");
            Console.WriteLine(" ======================================");
            Console.WriteLine("   (Q)uit to Main Menu                 ");
            Console.WriteLine(" Choose an action...\n");
        }
        public static void QuestLogHUD() {
            Console.Clear();
            Console.WriteLine($" {Program.CurrentPlayer.CurrentClass} {Program.CurrentPlayer.Name}:");
            Console.WriteLine($" Health: {Program.CurrentPlayer.Health}/{Program.CurrentPlayer.DerivedStats.MaxHealth}\t|| Level: {Program.CurrentPlayer.Level}");
            Console.Write(" EXP  ");
            Console.Write("[");
            ProgressBar("+", " ", (decimal)Program.CurrentPlayer.Exp / (decimal)Program.CurrentPlayer.GetLevelUpValue(), 20);
            Console.WriteLine("]");
            Console.WriteLine("\n@@@@@@@@@@@@@@@@@ Quest Items @@@@@@@@@@@@@@@@@@@");
            int i = 0;
            foreach (IItem item in Program.CurrentPlayer.Inventory) {
                if (item is null || item is not IQuestItem) {
                    i++;
                } else if (item is IQuestItem item1) {
                    Console.WriteLine($" \u001b[96m Quest Item - {item.ItemName} #{item1.Amount}\u001b[0m");
                    continue;
                }
                if (i == 10) {
                    Console.WriteLine(" You don't have any quest items...\n");
                }
            }
            Console.WriteLine("¤¤¤¤¤¤¤¤¤¤¤¤¤¤¤¤¤¤¤¤ Quests ¤¤¤¤¤¤¤¤¤¤¤¤¤¤¤¤¤¤¤¤");
            if (Program.CurrentPlayer.QuestLog.Count == 0) {
                Console.WriteLine(" You don't have any active quests...\n");
            } else {
                foreach (Quest quest in Program.CurrentPlayer.QuestLog) {
                    Console.WriteLine($" \u001b[96m{quest.Name}:\u001b[0m{(quest.QuestType == Dungeon.Quests.Type.Elimination ? $"\t{quest.Amount}/{quest.Requirements[quest.Target]} " : "")}");
                    if (!quest.Completed) {
                        Console.WriteLine($" {quest.Objective}");
                    } else if (quest.Completed) {
                        Console.WriteLine($" {quest.TurnIn}");
                    }
                    Console.WriteLine(" Rewards:");
                    if (quest.Gold > 0 && quest.Potions > 0) {
                        Console.WriteLine($" {quest.Potions} healing potions, {quest.Gold} gold pieces and {quest.Exp} experience points.");
                    } else if (quest.Gold == 0 && quest.Potions > 0) {
                        Console.WriteLine($" {quest.Potions} healing potions and {quest.Exp} experience points.");
                    } else if (quest.Gold > 0 && quest.Potions == -1) {
                        Console.WriteLine($" {quest.Gold} gold pieces and {quest.Exp} experience points.");
                    }                 
                    if (quest.Item != null) {
                        if (((IEquipable)quest.Item).ItemSlot == Slot.Right_Hand) {
                            Console.WriteLine($" {((IEquipable)quest.Item).ItemSlot} - {quest.Item.ItemName}: +{((IWeapon)quest.Item).WeaponAttributes.MinDamage}-{((IWeapon)quest.Item).WeaponAttributes.MaxDamage} dmg");
                        } else if (quest.Item is not IQuestItem) {
                            Console.Write($" {((IEquipable)quest.Item).ItemSlot} - {quest.Item.ItemName}:");
                            if (((ArmorBase)quest.Item).SecondaryAffixes.ArmorRating > 0) {
                                Console.Write($" +{((ArmorBase)quest.Item).SecondaryAffixes.ArmorRating} Armor Rating");
                            }
                            if (((ArmorBase)quest.Item).PrimaryAffixes.Strength > 0) {
                                Console.Write($", +{((ArmorBase)quest.Item).PrimaryAffixes.Strength} Str");
                            }
                            if (((ArmorBase)quest.Item).PrimaryAffixes.Dexterity > 0) {
                                Console.Write($", +{((ArmorBase)quest.Item).PrimaryAffixes.Dexterity} Dex");
                            }
                            if (((ArmorBase)quest.Item).PrimaryAffixes.Intellect > 0) {
                                Console.Write($", +{((ArmorBase)quest.Item).PrimaryAffixes.Intellect} Int");
                            }
                            if (((ArmorBase)quest.Item).PrimaryAffixes.Constitution > 0) {
                                Console.Write($", +{((ArmorBase)quest.Item).PrimaryAffixes.Constitution} Const");
                            }
                            if (((ArmorBase)quest.Item).PrimaryAffixes.WillPower > 0) {
                                Console.Write($", +{((ArmorBase)quest.Item).PrimaryAffixes.WillPower} Wp");
                            }
                            if (quest.Item.ItemName == "Linen Rags") {
                                Console.Write(" Offers no protection");
                            }
                            Console.WriteLine("");
                        } else if (quest.Item is IQuestItem) {
                            Console.WriteLine($" \u001b[96m Quest Item - {quest.Item.ItemName}\u001b[0m");
                        }
                    }
                }
            }
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
        public static void PickClassHUD() {
            Print($"=== Pick a class ===", 3);
            Print($"1. Warrior:\n" +
                           $"   The warrior class has a bonus to damage (based on level) making them strong in the early game.\n" +
                           $"   Their primary stat is strength. A warrior is trained to use swords, axes and maces.\n" +
                           $"   They can wear mail and plate armor.", 15);
            Print($"2. Archer:\n" +
                           $"   The archer class can always escape enemies and therefore can choose their battles more carefully.\n" +
                           $"   Their primary stat is dexterity. An archer is trained to use daggers, bows and crossbows.\n" +
                           $"   They can wear leather and mail.", 15);
            Print($"3. Mage:\n" +
                           $"   The mage class uses magic to amplify healing potion potency (based on level), but are usually\n" +
                           $"   weaker in the early game. Their primary stat is intellect. A mage is trained to use tomes, wands\n" +
                           $"   and staves. They can wear cloth and leather.\n", 15);
            Print($"Enter a number from 1 to 3:", 3);
        }
    }
}
