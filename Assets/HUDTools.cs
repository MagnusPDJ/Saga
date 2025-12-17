using Saga.Character;
using Saga.Character.Buffs;
using Saga.Character.DmgLogic;
using Saga.Character.Skills;
using Saga.Dungeon.Enemies;
using Saga.Dungeon.People;
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
                Task t = Task.Run(() =>
                {
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
            } else {
                Task t = Task.Run(() =>
                {
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
                } else {
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
                } else {
                    output += backgroundChar;
                }
            }
            return output;
        }

        // Metode til at skrive en logfil til kamp
        public static void WriteCombatLog(string action, EnemyBase monster, int damage = 0, int attack = 0) {
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
                        var potion = Array.Find(Program.CurrentPlayer.Equipment.Potions, p => p is IItem { ItemName: "Healing Potion" });
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
                        var potion = Array.Find(Program.CurrentPlayer.Equipment.Potions, p => p is IItem { ItemName: "Healing Potion" });
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

        // Formats a dictionary into wrapped inline text (safe under 100 width)
        public static string FormatDictionary<TKey>(Dictionary<TKey, int> dict, int maxLineWidth = 40, int indent = 58)
            where TKey : notnull {
            var sb = new StringBuilder();
            int currentWidth = 0;
            string indentSpaces = new(' ', indent);

            foreach (var kvp in dict) {
                string text = $"{kvp.Key}: {kvp.Value}%, ";
                if (currentWidth + text.Length > maxLineWidth) {
                    sb.AppendLine();
                    sb.Append(indentSpaces);
                    currentWidth = 0;
                }
                sb.Append(text);
                currentWidth += text.Length;
            }

            return sb.ToString().Trim().TrimEnd(',');
        }

        // Helper: determine whether an item should be presented as a quest item for the current player
        // returns (isQuestItem, displayAmount)
        private static (bool, int) GetQuestItemInfo(IItem? item) {
            if (item == null) return (false, 0);
            var player = Program.CurrentPlayer;
            if (player == null || player.QuestLog == null || player.QuestLog.Count == 0) return (false, 0);

            // Quest is relevant if any active (not completed) quest requires this ItemId
            var relevantQuests = player.QuestLog.Where(q => !q.Completed && q.Requirements != null && q.Requirements.ContainsKey(item.ItemId)).ToList();
            if (relevantQuests.Count == 0) return (false, 0);

            // Determine display amount:
            if (item is ICraftingItem citem) {
                return (true, citem.Amount);
            }
            if (item is IQuestItem qitem) {
                return (true, qitem.Amount);
            }

            // For non-stackable items, show 1 (or if multiple quests require different amounts, keep 1)
            // If player carries multiple separate entries in inventory for the same ItemId (non-stackable),
            // this method returns 1 for display. Stackable items should implement ICraftingItem or IQuestItem.
            return (true, 1);
        }

        //Tilføjer spaces før og efter en string, bruges til at bygge HUDS med varierende string længder.
        // \u001b[0m Reset color
        // \u001b[31m rød
        // \u001b[34m blå
        // \u001b[32m grøn
        // \u001b[33m gul
        // \u001b[90m grå
        private static string AddSpacesToEnds(string input, string side, int width) {
            int inputWidth = input.Replace("\u001b[0m", "").Replace("\u001b[31m", "").Replace("\u001b[32m", "").Replace("\u001b[33m", "").Replace("\u001b[34m", "").Replace("\u001b[90m", "").Length;
            if (side == "Left") {
                for (int i = 0; i < width - inputWidth; i++) {
                    input = " " + input;
                }
            } else if (side == "Right") {
                for (int i = 0; i < width - inputWidth; i++) {
                    input += " ";
                }
            } else if (side == "Both") {
                for (int i = 0; i < width - inputWidth; i++) {
                    input = " " + input;
                    i++;
                    if (i >= width - inputWidth) {
                        break;
                    }
                    input += " ";
                }
            }
            return input;
        }
        private static string DisplaySkill(int branch, int index) {
            ISkill skill;
            try {
                skill = Program.CurrentPlayer.SkillTree.Skills[branch][index];
            } catch (Exception) {
                return AddSpacesToEnds($"\u001b[90m*No skill*\u001b[0m", "Both", 20);
            }
            if (skill.IsUnlocked) {
                if (skill.Tier.Min == skill.Tier.Max) {
                    return AddSpacesToEnds($"\u001b[32m[X]{skill.Name}({skill.Tier.Min}/{skill.Tier.Max})\u001b[0m", "Both", 20);
                } else {
                    return AddSpacesToEnds($"\u001b[34m[*]{skill.Name}({skill.Tier.Min}/{skill.Tier.Max})\u001b[0m", "Both", 20);
                }                    
            } else if (Program.CurrentPlayer.Level >= skill.LevelRequired) {
                return AddSpacesToEnds($"\u001b[33m[*]{skill.Name}\u001b[0m", "Both", 20);
            } else {
                return AddSpacesToEnds($"[ ]{skill.Name}", "Both", 20);
            }
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
            WriteCenterLine(" ~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~\n");
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
                } else if (item.ItemSlot == Slot.Right_Hand) {
                    Console.WriteLine($"| ({1 + shop.GetForsale().IndexOf(item)}) Ilvl: {item.ItemLevel}, {((IWeapon)item).WeaponCategory}, {item.ItemName}, $ {item.CalculateItemPrice()}, +{((IWeapon)item).WeaponAttributes.MinDamage}-{((IWeapon)item).WeaponAttributes.MaxDamage} dmg");
                } else {
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
            Console.WriteLine($"| Gold:                  ${Program.CurrentPlayer.Equipment.GetGoldAmount()}");
            Console.WriteLine($"| Healing Potions:        {Array.Find(Program.CurrentPlayer.Equipment.Potions, (p => p is IItem { ItemName: "Healing Potion" }))?.PotionQuantity}");
            Console.WriteLine($"| Items in inventory:");
            foreach (var (item, i) in Program.CurrentPlayer.Inventory.Select((item, i) => (item, i))) {
                if (item == null) {
                } else {
                    var (isQuest, amount) = GetQuestItemInfo(item);
                    if (isQuest) {
                        Console.WriteLine($"| - \u001b[96mQuest Item - {item.ItemName} #{amount}\u001b[0m");
                    } else if (item is ICraftingItem cItem) {
                        Console.WriteLine($"| - Crafting Item - {cItem.ItemName} #{cItem.Amount}");
                    } else if ((item is IEquipable eItem) && eItem.ItemSlot == Slot.Right_Hand) {
                        Console.WriteLine($"| - {item.ItemName}: +{((IWeapon)item).WeaponAttributes.MinDamage}-{((IWeapon)item).WeaponAttributes.MaxDamage} dmg");
                    } else if (item is IArmor) {
                        Console.Write($"| - {item.ItemName}:");
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
            foreach (var (item, i) in Program.CurrentPlayer.Inventory.Select((item, i) => (item, i))) {
                if (item == null) {
                } else {
                    var (isQuest, amount) = GetQuestItemInfo(item);
                    if (isQuest) {
                        Console.WriteLine($"| ({i + 1}) \u001b[96mQuest Item - {item.ItemName} #{amount}\u001b[0m");
                    } else if (item is ICraftingItem cItem) {
                        Console.WriteLine($"| ({i + 1}) Crafting Item - {cItem.ItemName} #{cItem.Amount}");
                    } else if ((item is IEquipable eItem) && eItem.ItemSlot == Slot.Right_Hand) {
                        Console.WriteLine($"| ({i + 1}) {item.ItemName}: +{((IWeapon)item).WeaponAttributes.MinDamage}-{((IWeapon)item).WeaponAttributes.MaxDamage} dmg");
                    } else if (item is IArmor) {
                        Console.Write($"| ({i + 1}) {item.ItemName}:");
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
            }
            Console.WriteLine($"|  Sell Healing (P)otion        $ {Shop.ShopPrice("sellpotion")}");
            var potion = Array.Find(Program.CurrentPlayer.Equipment.Potions, p => p is IItem { ItemName: "Healing Potion" });
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
            Console.WriteLine($"| Gold:                  ${Program.CurrentPlayer.Equipment.GetGoldAmount()}");
            Console.WriteLine($"| Healing Potions:        {potion?.PotionQuantity}");
            Console.WriteLine("==============================");
            Console.WriteLine(" (U)se Healing Potion (C)haracter screen\n (I)nventory (Q)uestlog\n");
            Console.WriteLine("Choose what to sell");
        }
        public static void DisplayStats(Player player) {
            StringBuilder stats = new("~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~ Character screen ~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~\n");
            stats.AppendFormat($" Name: {player.Name}\t\t\tClass: {player.CurrentClass}\n");
            stats.AppendFormat($" Level: {player.Level}\t\t\tTimes Explored: {player.TimesExplored}\n");
            stats.AppendFormat($" EXP  [{ProgressBarForPrint("+", " ", (decimal)player.Exp / (decimal)player.GetLevelUpValue(), 25)}] {player.Exp}/{player.GetLevelUpValue()}\n");
            stats.AppendFormat($"\n---------------------------------------------- Stats -----------------------------------------------\n");
            stats.AppendFormat($" ┌(S)trength ⤵           {player.Attributes.Strength}\t=> Elemental Resistance:  {FormatDictionary(player.DerivedStats.ElementalResistance)}\n");
            stats.AppendFormat($" │      (C)onstitution⤵  {player.Attributes.Constitution}\t=> Health:                {player.Health} / {player.DerivedStats.MaxHealth}\n");
            stats.AppendFormat($" │(D)exterity ⇵          {player.Attributes.Dexterity}\t=> Physical Resistance:   {FormatDictionary(player.DerivedStats.PhysicalResistance)}\n");
            stats.AppendFormat($" │      (A)wareness ⤵    {player.Attributes.Awareness}\t=> Initiative:            {player.DerivedStats.Initiative}\n");
            stats.AppendFormat($" │(I)ntellect ⇵          {player.Attributes.Intellect}\t=> Magical Resistance:    {FormatDictionary(player.DerivedStats.MagicalResistance)}\n");
            stats.AppendFormat($" └>     (W)illpower ⤵    {player.Attributes.WillPower}\t=> Mana:                  {player.Mana} / {player.DerivedStats.MaxMana}\n");
            stats.AppendFormat($"                 Virtue  {player.Attributes.Virtue}\t=> Action Points:         {player.DerivedStats.ActionPoints}\n");
            stats.AppendFormat($"  Attribute points to spend: {player.FreeAttributePoints}\n\n");
            stats.AppendFormat($" Attack Speed:   {player.DerivedStats.AttackSpeed}\t(Constitution and Awareness)\n");
            stats.AppendFormat($" Casting Speed:  {player.DerivedStats.CastingSpeed}\t(Willpower and Awareness)\n");
            stats.AppendFormat($" Mana Regen:     {player.DerivedStats.ManaRegenRate}\t(Constitution and Wilpower)\n");
            stats.AppendFormat($"\n----------------------------------------- Equipment Stats ------------------------------------------\n");
            stats.AppendFormat($" Armor Rating:  {player.Equipment.ArmorRating}\n");
            stats.AppendFormat($"\n Weapon Attributes:\n");
            stats.AppendFormat($"  Weapon Damage:  {(player.Equipment.Right_Hand as IWeapon)?.WeaponAttributes.MinDamage}-{(player.Equipment.Right_Hand as IWeapon)?.WeaponAttributes.MaxDamage} {(player.CurrentClass == "Warrior" && (player.Equipment.Right_Hand as IWeapon is IPhysical) ? $"(+{player.Level} warrior bonus)" : "")},\t+Attack Speed:  {player.Equipment.BonusAttackSpeed},\t+Casting Speed:  {player.Equipment.BonusCastingSpeed}.\n");
            stats.AppendFormat($"\n Primary Attributes:\n");
            stats.AppendFormat($"  +Str:  {player.Equipment.BonusStr},  +Dex:  {player.Equipment.BonusDex},  +Int:  {player.Equipment.BonusInt},  +Con:  {player.Equipment.BonusCon},  +Awa:  {player.Equipment.BonusAwa}, +Wp:  {player.Equipment.BonusWP},  +Virtue:  {player.Equipment.BonusVirtue},\n");
            stats.AppendFormat($"\n Secondary Attributes:\n");
            stats.AppendFormat($"  +Max Health:  {player.Equipment.BonusHealth},\t+Max Mana:       {player.Equipment.BonusMana},\t+Mana Regen:  {player.Equipment.BonusManaRegenRate},\n");
            stats.AppendFormat($"  +Initiative:  {player.Equipment.BonusInitiative},\t+Action Points:  {player.Equipment.BonusActionPoints}\n");
            stats.AppendFormat($"  +Physical Resistance:  {FormatDictionary(player.Equipment.BonusPhysicalRes, indent: 22)}\n");
            stats.AppendFormat($"  +Elemental Resistance: {FormatDictionary(player.Equipment.BonusElementRes, indent: 22)}\n");
            stats.AppendFormat($"  +Magical Resistance:   {FormatDictionary(player.Equipment.BonusMagicRes, indent: 22)}\n");

            Print(stats.ToString(), 0);
        }
        public static void CharacterScreen() {
            //Metode til at kalde og gernerer en character screen som viser alle funktionelle variabler der er i brug.
            for (int i = Program.CurrentPlayer.FreeAttributePoints; i >= 0; i--) {
                Console.Clear();
                DisplayStats(Program.CurrentPlayer);
                i = Program.CurrentPlayer.SpendAttributePoint(i);
            }
        }
        public static void ConstructSkillTree(Player player) {
            StringBuilder stats = new("******************************************** Skill Tree ********************************************\n");
            stats.AppendFormat($"                                       ┌────────────────────┐\r\n");
            stats.AppendFormat($"                                       │{DisplaySkill(0,0) }│\r\n");
            stats.AppendFormat($"                         ┌─────────────┴──────────┬─────────┴──────────────┐\r\n");
            stats.AppendFormat($"              ┌──────────┴─────────┐              │             ┌──────────┴─────────┐\r\n");
            stats.AppendFormat($"              │{DisplaySkill(1, 0)}│              │             │{DisplaySkill(2, 0)}│\r\n");
            stats.AppendFormat($"              └──────────┬─────────┘              │             └──────────┬─────────┘\r\n");
            stats.AppendFormat($"             ┌───────────┴───────────┐            │            ┌───────────┴───────────┐\r\n");
            stats.AppendFormat($"  ┌──────────┴─────────┐   ┌─────────┴──────────┐ │ ┌──────────┴─────────┐   ┌─────────┴──────────┐\r\n");
            stats.AppendFormat($"  │{DisplaySkill(1, 1)}│   │{DisplaySkill(1, 4)}│ │ │{DisplaySkill(2, 1)}│   │{DisplaySkill(2, 4)}│\r\n");
            stats.AppendFormat($"  └──────────┬─────────┘   └─────────┬──────────┘ │ └──────────┬─────────┘   └─────────┬──────────┘\r\n");
            stats.AppendFormat($"  ┌──────────┴─────────┐   ┌─────────┴──────────┐ │ ┌──────────┴─────────┐   ┌─────────┴──────────┐\r\n");
            stats.AppendFormat($"  │{DisplaySkill(1, 2)}│   │{DisplaySkill(1, 5)}│ │ │{DisplaySkill(2, 2)}│   │{DisplaySkill(2, 5)}│\r\n");
            stats.AppendFormat($"  └──────────┬─────────┘   └─────────┬──────────┘ │ └──────────┬─────────┘   └─────────┬──────────┘\r\n");
            stats.AppendFormat($"  ┌──────────┴─────────┐   ┌─────────┴──────────┐ │ ┌──────────┴─────────┐   ┌─────────┴──────────┐\r\n");
            stats.AppendFormat($"  │{DisplaySkill(1, 3)}│   │{DisplaySkill(1, 6)}│ │ │{DisplaySkill(2, 3)}│   │{DisplaySkill(2, 6)}│\r\n");
            stats.AppendFormat($"  └────────────────────┘   └────────────────────┘ │ └────────────────────┘   └────────────────────┘\r\n");
            stats.AppendFormat($"                         ┌────────────────────────┴────────────────────────┐\r\n");
            stats.AppendFormat($"              ┌──────────┴─────────┐                             ┌─────────┴──────────┐\r\n");
            stats.AppendFormat($"              │{DisplaySkill(3, 0)}│                             │{DisplaySkill(4, 0)}│\r\n");
            stats.AppendFormat($"              └──────────┬─────────┘                             └─────────┬──────────┘\r\n");
            stats.AppendFormat($"             ┌───────────┴───────────┐                         ┌───────────┴───────────┐\r\n");
            stats.AppendFormat($"  ┌──────────┴─────────┐   ┌─────────┴──────────┐   ┌──────────┴─────────┐   ┌─────────┴──────────┐\r\n");
            stats.AppendFormat($"  │{DisplaySkill(3, 1)}│   │{DisplaySkill(3, 4)}│   │{DisplaySkill(4, 1)}│   │{DisplaySkill(4, 4)}│\r\n");
            stats.AppendFormat($"  └──────────┬─────────┘   └─────────┬──────────┘   └──────────┬─────────┘   └─────────┬──────────┘\r\n");
            stats.AppendFormat($"  ┌──────────┴─────────┐   ┌─────────┴──────────┐   ┌──────────┴─────────┐   ┌─────────┴──────────┐\r\n");
            stats.AppendFormat($"  │{DisplaySkill(3, 2)}│   │{DisplaySkill(3 ,5)}│   │{DisplaySkill(4, 2)}│   │{DisplaySkill(4, 5)}│\r\n");
            stats.AppendFormat($"  └──────────┬─────────┘   └─────────┬──────────┘   └──────────┬─────────┘   └─────────┬──────────┘\r\n");
            stats.AppendFormat($"  ┌──────────┴─────────┐   ┌─────────┴──────────┐   ┌──────────┴─────────┐   ┌─────────┴──────────┐\r\n");
            stats.AppendFormat($"  │{DisplaySkill(3, 3)}│   │{DisplaySkill(3, 6)}│   │{DisplaySkill(4 ,3)}│   │{DisplaySkill(4, 6)}│\r\n");
            stats.AppendFormat($"  └────────────────────┘   └────────────────────┘   └────────────────────┘   └────────────────────┘\r\n");
            stats.AppendFormat($" Bound Quickcast: {player.SkillTree.QuickCast}.\r\n");
            stats.AppendFormat($"****************************************************************************************************\n");
            stats.AppendFormat(" Legend:  [X] = Maxed skill   [*] = Available to learn/upgrade   [ ] = Locked\r");
            Console.WriteLine(stats.ToString());
        }
        public static void ShowSkillTree(Player player) {
            Console.Clear();
            ConstructSkillTree(player);
            Console.WriteLine(" Commands:   learn <SkillName>     → Unlock/Upgrade a skill");
            Console.WriteLine("  -          info <SkillName>      → View skill details");
            Console.WriteLine("  -          quickcast <SkillName> → Rebind skill to quickcast");
            Console.WriteLine("  -          (b)ack                → To Return");
            Console.WriteLine($" Available skill points: {player.FreeSkillPoints}\n");
        }
        public static void ShowSkillsCombat(Player player, CombatController cController) {
            Console.Clear();
            Console.WriteLine($" Health:        \u001b[31m{player.Health}/{player.DerivedStats.MaxHealth}\u001b[0m");
            Console.WriteLine($" Mana:          \u001b[34m{player.Mana}/{player.DerivedStats.MaxMana}\u001b[0m");
            Console.WriteLine($" Action Points: \u001b[32m{cController.GetRemainingAP()}/{player.DerivedStats.ActionPoints}\u001b[0m");
            Console.WriteLine("-.-.-.-.-.-.-.-.-.-.-.-.-.-.-.-.-.-.-.-");
            Console.WriteLine($" Bound Quickcast: {player.SkillTree.QuickCast}");
            Console.WriteLine("-.-.-.-.-.-.-.-.-.-.-.-.-.-.-.-.-.-.-.-");
            Console.WriteLine($" Available Skills:");
            List<ISkill> learnedSkills = Program.CurrentPlayer.SkillTree.GetLearnedSkills();
            for (int i = 0; i < learnedSkills.Count; i++) {
                var s = learnedSkills[i];
                Print($"  {i + 1}: {s.Name} ({s.Tier.Min}/{s.Tier.Max}) - \u001b[32m{((IActiveSkill)s).ActionPointCost / (((IActiveSkill)s).SpeedType == "Casting Speed" ? player.DerivedStats.CastingSpeed : player.DerivedStats.AttackSpeed)} AP\u001b[0m {(s.ManaCost > 0 ? $"& \u001b[34m{s.ManaCost} Mana\u001b[0m" : "")}", 0);
            }
            Print("\n Enter the number of the skill to use, 'q' to rebind quickcast, or '(b)ack'.", 10);
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
            int i = 1;
            foreach (IConsumable potion in Program.CurrentPlayer.Equipment.Potions) {
                if (potion == null) {
                    Console.WriteLine($"\u001b[90m Potion slot {i} - Empty\u001b[0m");
                } else if (potion is IItem iPotion) {
                    Console.WriteLine($" Potion slot {i} - {iPotion.ItemName}: +{potion.PotionPotency} {(potion.PotionType == PotionType.Healing && Program.CurrentPlayer.CurrentClass == "Mage" ? $"(+{1 + Program.CurrentPlayer.Level * 2} Mage bonus)" : "")}, Quantity: {potion.PotionQuantity}");
                }
                i++;
            }
            Console.WriteLine("\n@@@@@@@@@@@@@@@@@ Inventory @@@@@@@@@@@@@@@@@@@");
            Console.WriteLine($" {Program.CurrentPlayer.Equipment.Pouch.ItemName}: ${Program.CurrentPlayer.Equipment.GetGoldAmount()}");
            foreach (IItem item in Program.CurrentPlayer.Inventory) {
                if (item == null) {
                    Console.WriteLine("\u001b[90m Empty slot\u001b[0m");
                } else {
                    var (isQuest, amount) = GetQuestItemInfo(item);
                    if (isQuest) {
                        Console.WriteLine($"\u001b[96m Quest Item - {item.ItemName} #{amount}\u001b[0m");
                    } else if (item is IConsumable conItem) {
                        Console.WriteLine($" {item.ItemName}: +{conItem.PotionPotency} {(conItem.PotionType == PotionType.Healing && Program.CurrentPlayer.CurrentClass == "Mage" ? $"(+{1 + Program.CurrentPlayer.Level * 2} Mage bonus)" : "")}, Quantity: {conItem.PotionQuantity}");
                    } else if (item is ICraftingItem cItem) {
                        Console.WriteLine($" Crafting Item - {cItem.ItemName} #{cItem.Amount}");
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
            }
            Print($"\n To equip item write 'equip Itemname', to unequip item write 'unequip Itemname'\n To examine item write examine Itemname else (b)ack\n", 0);
        }
        public static void CombatHUD(Player player, EnemyBase Monster, CombatController cController) {
            List<ISkill> learnedSkills = player.SkillTree.GetLearnedSkills();
            var quickCastSkill = learnedSkills.Find(x => x.Name == player.SkillTree.QuickCast) as IActiveSkill;
            var basicAttackSkill = learnedSkills.Find(x => x.Name == "Basic Attack") as IActiveSkill;
            Console.Clear();
            Console.WriteLine(">>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>   In Combat!   <<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<");
            Console.WriteLine($" Turn: {cController.Turn} \tLocation: {Program.RoomController.CurrentRoom.RoomName}\n");
            Console.WriteLine($" Fighting: {Monster.Name}!");
            Console.WriteLine($" Power: {Monster.Power}\tAttack: {Monster.Attack}\tEnemy health: {Monster.Health}/{Monster.MaxHealth}");
            if (player.DerivedStats.Initiative >= Monster.Initiative) {
                Console.WriteLine("\n----------------------------------------------------------------------------------------------------");
                Console.WriteLine("  You go first!\n");
            } else {
                Console.WriteLine("\n  The enemy goes first!");
                Console.WriteLine("----------------------------------------------------------------------------------------------------\n");
            }
            Console.WriteLine($"\t\t{player.Name} the {player.CurrentClass}:");
            WriteCenterLine($"              {AddSpacesToEnds($"  Your Health: \u001b[31m{player.Health}/{player.DerivedStats.MaxHealth}\u001b[0m", "Right", 25)}| | {AddSpacesToEnds($"{(player.Equipment.Potions[0] as IItem)?.ItemName ?? "Potion slot 1 - empty"}: {player.Equipment.Potions[0]?.PotionQuantity ?? 0}", "Right", 25)}");
            WriteCenterLine($"              {AddSpacesToEnds($"       Mana:   \u001b[34m{player.Mana}/{player.DerivedStats.MaxMana}\u001b[0m", "Right", 25)}| | {AddSpacesToEnds($"{(player.Equipment.Potions[1] as IItem)?.ItemName ?? "Potion slot 2 - empty"}: {player.Equipment.Potions[1]?.PotionQuantity ?? 0}", "Right", 25)}");
            WriteCenterLine($"              {AddSpacesToEnds($"Action Points: \u001b[32m{cController.GetRemainingAP()}/{player.DerivedStats.ActionPoints}\u001b[0m", "Right", 25)}| | {AddSpacesToEnds($"{(player.Equipment.Potions[2] as IItem)?.ItemName ?? "Potion slot 1 - empty"}: {player.Equipment.Potions[2]?.PotionQuantity ?? 0}", "Right", 25)}");
            WriteCenterLine($"              {AddSpacesToEnds($"       Gold:   \u001b[33m${player.Equipment.GetGoldAmount()}\u001b[0m", "Right", 25)}| | {AddSpacesToEnds($"{(player.Equipment.Potions[3] as IItem)?.ItemName ?? "Potion slot 1 - empty"}: {player.Equipment.Potions[3]?.PotionQuantity ?? 0}", "Right", 25)}");
            WriteCenterLine($"Level: {player.Level}                                                            ");
            WriteCenterLine("EXP  " + "[" + ProgressBarForPrint("+", " ", (decimal)player.Exp / (decimal)player.GetLevelUpValue(), 25) + "]                                  \n");
            Console.WriteLine($"       ======================= Actions =====================|======== Info ========");
            Console.WriteLine($"       |  {AddSpacesToEnds($"(1) Quick Cast: {player.SkillTree.QuickCast} (\u001b[32m{quickCastSkill?.ActionPointCost / (quickCastSkill?.SpeedType == "Casting Speed" ? player.DerivedStats.CastingSpeed : player.DerivedStats.AttackSpeed)} AP\u001b[0m {(quickCastSkill?.ManaCost > 0 ? $"& \u001b[34m{quickCastSkill?.ManaCost} Mana\u001b[0m" : "")})", "Right", 50)}|  (C)haracter screen |");
            Console.WriteLine($"       |  (2) Basic Attack {AddSpacesToEnds($"(\u001b[32m{basicAttackSkill?.ActionPointCost / player.DerivedStats.AttackSpeed} AP\u001b[0m)", "Right", 9)}(4) Skills              |   Combat (L)og      |");
            Console.WriteLine($"       |  (3) Drink Potion (\u001b[32m{(player.BuffedStats.ActiveBuffs.Find(x => x.Name == "Haste" && !((HasteBuff)x).PotionDrunk) != null ? 0 : 1)} AP\u001b[0m)   (5) Run                 |  (Q)uestlog         |");
            Console.WriteLine($"       =====================================================|======================");
            Console.WriteLine($"          {(CombatController.AutoEndturn == false ? "(E)nd Turn." : "")}");
            Console.WriteLine($"\n   Choose an action...\n");
        }
        public static void RoomHUD() {
            Console.Clear();
            Console.WriteLine($" Location:\t{Program.RoomController.CurrentRoom.RoomName}");
            Console.WriteLine($" {Program.CurrentPlayer.CurrentClass} {Program.CurrentPlayer.Name}:");
            Console.WriteLine($" Health: \u001b[31m{Program.CurrentPlayer.Health}/{Program.CurrentPlayer.DerivedStats.MaxHealth}\u001b[0m\t|| {(Program.CurrentPlayer.Equipment.Potions[0] as IItem)?.ItemName ?? "Potion slot 1 - empty"}: {Program.CurrentPlayer.Equipment.Potions[0]?.PotionQuantity ?? 0}");
            Console.WriteLine($" Mana:   \u001b[34m{Program.CurrentPlayer.Mana}/{Program.CurrentPlayer.DerivedStats.MaxMana}\u001b[0m\t|| {(Program.CurrentPlayer.Equipment.Potions[1] as IItem)?.ItemName ?? "Potion slot 2 - empty"}: {Program.CurrentPlayer.Equipment.Potions[1]?.PotionQuantity ?? 0}");
            Console.WriteLine($" Gold:   \u001b[33m${Program.CurrentPlayer.Equipment.GetGoldAmount()}\u001b[0m\t|| {(Program.CurrentPlayer.Equipment.Potions[2] as IItem)?.ItemName ?? "Potion slot 3 - empty"}: {Program.CurrentPlayer.Equipment.Potions[2]?.PotionQuantity ?? 0}");
            Console.WriteLine($" Level: {Program.CurrentPlayer.Level}\t|| {(Program.CurrentPlayer.Equipment.Potions[1] as IItem)?.ItemName ?? "Potion slot 4 - empty"}: {Program.CurrentPlayer.Equipment.Potions[3]?.PotionQuantity ?? 0}");
            Console.Write(" EXP  ");
            Console.Write("[");
            ProgressBar("+", " ", (decimal)Program.CurrentPlayer.Exp / (decimal)Program.CurrentPlayer.GetLevelUpValue(), 20);
            Console.WriteLine("]");
            Console.WriteLine(" ==================Actions================");
            Console.WriteLine(" V Look around          Go #             V");
            Console.WriteLine(" V (D)rink Potion       Go back/home     V");
            Console.WriteLine(" ===================Menus=================");
            Console.WriteLine(" V (C)haracter screen   S(K)illtree      V");
            Console.WriteLine(" V (I)nventory          Quest(L)og       V");
            Console.WriteLine(" =========================================\n");
            Console.WriteLine(" Write an action:");
        }
        public static void FullCampHUD() {
            Console.Clear();
            Console.WriteLine("[][][][][][][][]  Camp   [][][][][][][][]");
            Console.WriteLine($" {Program.CurrentPlayer.CurrentClass} {Program.CurrentPlayer.Name}:");
            Console.WriteLine($" Health: \u001b[31m{Program.CurrentPlayer.Health}/{Program.CurrentPlayer.DerivedStats.MaxHealth}\u001b[0m\t|| {(Program.CurrentPlayer.Equipment.Potions[0] as IItem)?.ItemName ?? "Potion slot 1 - empty"}: {Program.CurrentPlayer.Equipment.Potions[0]?.PotionQuantity ?? 0}");
            Console.WriteLine($" Mana:   \u001b[34m{Program.CurrentPlayer.Mana}/{Program.CurrentPlayer.DerivedStats.MaxMana}\u001b[0m\t|| {(Program.CurrentPlayer.Equipment.Potions[1] as IItem)?.ItemName ?? "Potion slot 2 - empty"}: {Program.CurrentPlayer.Equipment.Potions[1]?.PotionQuantity ?? 0}");
            Console.WriteLine($" Gold:   \u001b[33m${Program.CurrentPlayer.Equipment.GetGoldAmount()}\u001b[0m\t|| {(Program.CurrentPlayer.Equipment.Potions[2] as IItem)?.ItemName ?? "Potion slot 3 - empty"}: {Program.CurrentPlayer.Equipment.Potions[2]?.PotionQuantity ?? 0}");
            Console.WriteLine($" Level: {Program.CurrentPlayer.Level}\t|| {(Program.CurrentPlayer.Equipment.Potions[3] as IItem)?.ItemName ?? "Potion slot 4 - empty"}: {Program.CurrentPlayer.Equipment.Potions[3]?.PotionQuantity ?? 0}");
            Console.Write(" EXP  ");
            Console.Write("[");
            ProgressBar("+", " ", (decimal)Program.CurrentPlayer.Exp / (decimal)Program.CurrentPlayer.GetLevelUpValue(), 20);
            Console.WriteLine("]");
            Console.WriteLine(" ==============Actions=================");
            Console.WriteLine(" 0 (E)xplore          (S)leep (Save)  0");
            Console.WriteLine(" 0 (G)heed's shop     (D)rink Potion  0");
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
                if (item is null) {
                    i++;
                    if (i == 10) {
                        Console.WriteLine(" You don't have any quest items...\n");
                    }
                    continue;
                }
                var (isQuest, amount) = GetQuestItemInfo(item);
                if (!isQuest) {
                    i++;
                } else {
                    Console.WriteLine($" \u001b[96m Quest Item - {item.ItemName} #{amount}\u001b[0m");
                    continue;
                }
            }
            Console.WriteLine("¤¤¤¤¤¤¤¤¤¤¤¤¤¤¤¤¤¤¤¤ Quests ¤¤¤¤¤¤¤¤¤¤¤¤¤¤¤¤¤¤¤¤¤");
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
                    if (quest.Gold > 0 && quest.Potions is not null) {
                        Console.WriteLine($" {quest.Potions[0].Item2} {quest.Potions[0].Item1} potions, {quest.Gold} gold pieces and {quest.Exp} experience points.");
                    } else if (quest.Gold == 0 && quest.Potions is not null) {
                        Console.WriteLine($" {quest.Potions[0].Item2} {quest.Potions[0].Item1} potions and {quest.Exp} experience points.");
                    } else if (quest.Gold > 0 && quest.Potions is null) {
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
            } else {
                Print("There are no one in your camp :(");
            }
            Print("\nPress the number to talk to that NPC else write (b)ack", 10);
        }
        public static void PickClassHUD() {
            Print($"==== Pick a class ====", 3);
            Print($" 1. Warrior:\n" +
                           $"    The warrior class has a bonus to damage (based on level) making them strong in the early game.\n" +
                           $"    Their primary stat is strength. A warrior is trained to use swords, axes and maces.\n" +
                           $"    They can wear mail and plate armor.", 15);
            Print($" 2. Archer:\n" +
                           $"    The archer class can always escape enemies and can choose their battles more carefully.\n" +
                           $"    Their primary stat is dexterity. An archer is trained to use daggers, bows and crossbows.\n" +
                           $"    They can wear leather and mail.", 15);
            Print($" 3. Mage:\n" +
                           $"    The mage class uses magic to amplify healing potion potency (based on level), but are usually\n" +
                           $"    weaker in the early game.\n" +
                           $"    Their primary stat is intellect. A mage is trained to use tomes, wands and staves.\n" +
                           $"    They can wear cloth and leather.\n", 15);
            Print($" Enter a number from 1 to 3:", 3);
        }
    }
}
