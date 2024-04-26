using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Configuration;
using System.Threading;
using static Saga.Player;

namespace Saga
{
    internal class Program
    {
        //Genere spilleren som objekt så den kan sættes senere.
        public static Player currentPlayer = new Player();

        //Sætter Game Loopet til true så man kan spille indefinitely.
        public static bool mainLoop = true;

        //Genere et objekt som kan returnere tilfældige tal mm.
        public static Random rand = new Random();

        //Sætter variablen til Lydniveauet fra configfilen.
        public static float volumeLevel = float.Parse(ConfigurationManager.AppSettings.Get("volume"));
        //Laver et objekt til at sætte lyden når spillet åbnes.
        public static AudioManager soundVolumeController = new AudioManager(Properties.Resources.mainmenu);

        //Spillets udførelse ved opstart
        static void Main(string[] args) {

            //Gør unicode karaktere "runer" læselige i consolen
            Console.OutputEncoding = Encoding.UTF8;

            //Sætter lydniveauet til variablen sat fra configfilen.
            soundVolumeController.Volume = volumeLevel;

            //Danner en saves mappe hvis den ikke eksistere.
            if (!Directory.Exists("saves")) {
            Directory.CreateDirectory("saves");
            }

            //Kalder MainMenu metoden.
            MainMenu();
        }

        //Metode til at lave en MainMenu hvor man kan ændre settings eller starte spillet etc.
        public static void MainMenu() {
            AudioManager.soundMainMenu.Play();
            while (true) {
                Console.Clear();
                Console.WriteLine("~~~~~~~~ Saga title ~~~~~~~~");
                Console.WriteLine("1.         Play");
                Console.WriteLine("2.       Settings");
                Console.WriteLine("3.       Quit Game");
                string input = HUDTools.PlayerPrompt();
                if (input == "1") {
                    Play();
                }
                else if (input == "2") {
                    EditSettings();
                }
                else if (input == "3") {
                    Console.WriteLine("Come back soon!");
                    Environment.Exit(0);
                }
                else {
                    Console.WriteLine("Wrong Input");
                    HUDTools.PlayerPrompt();
                }
            }
        }

        public static void Play() {
            currentPlayer = Load(out bool newP);
            NewStart(newP);

            //Spillets loop
            while (mainLoop) {
                AudioManager.soundMainMenu.Stop();
                AudioManager.soundShop.Stop();
                Encounters.Camp();
            }
        }
        
        //Metode til at køre start introduktionen
        public static void NewStart(bool newP) {
            if (newP) {
                SetStartingGear(currentPlayer);
                Encounters.FirstEncounter();
                Encounters.FirstShopEncounter();
                Encounters.RandomBasicCombatEncounter();
                Encounters.FirstCamp();
            }
        }

        //Metode til at gemme spillet ved først at tjekke for om der er en eksisterende save med det korrekte navn, som så overskrives, eller så dannes en helt ny en.
        public static void Save() {
            BinaryFormatter binForm = new BinaryFormatter();
            string path = $"saves/{currentPlayer.id}.player";
            FileStream file = File.Open(path,FileMode.OpenOrCreate);
            binForm.Serialize(file, currentPlayer);
            file.Close();
        }

        //Metode til at loade gemte karakterer, vise dem og vælge hvilken til at spille videre på eller lave en helt ny karakter.
        public static Player Load(out bool newP) {
            newP = false;
            Console.Clear();
            string[] paths = Directory.GetFiles("saves");
            List<Player> players = new List<Player>();
            BinaryFormatter binForm = new BinaryFormatter();
            foreach (string p in paths) {
                FileStream file = File.Open(p, FileMode.Open);
                Player player = (Player)binForm.Deserialize(file);
                file.Close();
                players.Add(player);
            }
            int idCount = players.Count;
            while (true) {
                Console.Clear();
                AudioManager.soundTypeWriter.Play();
                HUDTools.Print("Choose a save!  ",15);
                HUDTools.Print("-.-.-.-.-.-.-.-.-.-.-.-.-.-.-.-.",5);
                HUDTools.Print("#: playername");
                foreach (Player p in players) {
                    HUDTools.Print($"{p.id}: {p.name} - Level: {p.level}", 10);
                }
                HUDTools.Print("<><><><><><><><><><><><><><><><>",5);
                HUDTools.Print("To load a save write 'id:#' or 'playername'.\nFor new game write 'new game'.\nTo delete a save write 'delete:playername'.",1);
                string[] data = Console.ReadLine().Split(':');
                try {
                    if (data[0] == "id") {
                        if (int.TryParse(data[1], out int id)) {
                            foreach (Player player in players) {
                                if (player.id == id) {
                                    return player;
                                }
                            }
                            Console.WriteLine("There is no player with that id!");
                            Console.ReadKey(true);
                        } else {
                            Console.WriteLine("Your id needs to be a number! Press to continue!");
                            Console.ReadKey(true);
                        }
                    } else if (data[0] == "delete") {
                        if (int.TryParse(data[1], out int id)) {
                            foreach (Player player in players) {
                                if (player.id == id) {
                                    File.Delete($"saves/{player.id}.player");
                                    players.Remove(player);
                                    Console.WriteLine($"Save game {player.name} - level {player.level}, was deleted");
                                    Console.ReadKey(true);
                                    break;
                                } else {
                                    Console.WriteLine("There is no player with that id!");
                                    Console.ReadKey(true);
                                }
                            }
                        } else {
                            foreach (Player player in players) {
                                if (data[1] == player.name) {
                                    File.Delete($"saves/{player.id}.player");
                                    players.Remove(player);
                                    Console.WriteLine($"Save game {player.name} - level {player.level}, was deleted");
                                    Console.ReadKey(true);
                                    break;
                                } else {
                                    Console.WriteLine("There is no player with that name!");
                                    Console.ReadKey(true);
                                }
                            }
                        }
                    } else if (data[0] == "new game") {
                        Player newPlayer = NewCharacter(idCount);
                        newP = true;
                        return newPlayer;
                    }
                    else {
                        foreach (Player player in players) {
                            if (player.name == data[0]) {
                                return player;
                            }
                        }
                        Console.WriteLine("There is no player with that name!");
                        Console.ReadKey(true);
                    }
                } catch(IndexOutOfRangeException) {
                    Console.WriteLine("Your id needs to be a number! Press to continue!");
                    Console.ReadKey(true);
                }
            }
        }

        //Metode til at genere ny karakter efter at have inputtet 'new game' i Load() metoden.
        static Player NewCharacter(int i) {
            Console.Clear();
            Player p = new Player();
            PickName(p);
            PickClass(p);
            p.id = i;
            Console.Clear();
            AudioManager.soundTypeWriter.Play();
            HUDTools.Print("You awake in a cold and dark room. You feel dazed and are having trouble remembering");
            HUDTools.Print("anything about your past.");
            if (String.IsNullOrWhiteSpace(p.name) == true) {
                HUDTools.Print("You can't even remember your own name...");
                p.name = "Adventurer";
            } else {
                HUDTools.Print($"You know your name is {p.name}.");
            }
            Console.ReadKey(true);
            return p;
        }

        //Metode til at vælge navn.
        public static void PickName(Player p) {
            string input;
            string input1;
            do {
                Console.Clear();
                Console.WriteLine("//////////////");
                HUDTools.Print("Enter a name: ",10);
                do {
                    input = Console.ReadLine();
                    if (input.Any(c => !char.IsLetter(c) && !char.IsWhiteSpace(c)) && !input.Contains('-') && !input.Contains('\u0027')) {
                        Console.WriteLine("Invalid name");
                    }
                    else if (input.Length >= 30) {
                        Console.WriteLine("Name is too long. Max 30 characters!");
                    }
                    else {
                    }
                } while (input.Any(c => !char.IsLetter(c) && !char.IsWhiteSpace(c)) && !input.Contains('-') && !input.Contains('\u0027') || input.Length >= 30);
                Console.Clear();
                HUDTools.Print($"This is your name?\n{input}.\n(Y/N)");
                input1 = Console.ReadLine().ToLower();
            } while (input1 != "y");
            p.name = input;
        }

        //Metode til at vælge Class.
        public static void PickClass(Player p) {
            HUDTools.Print("Pick a class: Mage  Archer  Warrior");
            bool flag = false;
            while (flag == false) {
                flag = true;
                string input1 = Console.ReadLine().ToLower();
                if (input1 == "mage") {
                    p.currentClass = PlayerClass.Mage;
                }
                else if (input1 == "archer") {
                    p.currentClass = PlayerClass.Archer;
                }
                else if (input1 == "warrior") {
                    p.currentClass = PlayerClass.Warrior;
                }
                else {
                    Console.WriteLine("Please choose a listed class!");
                    flag = false;
                }
            }
        }

        //Metode til at sætte start udstyr
        public static void SetStartingGear(Player p) {
            switch (p.currentClass) {
                case PlayerClass.Warrior:
                    Program.currentPlayer.equippedWeapon = "Rusty Sword";
                    Program.currentPlayer.equippedWeaponValue = 1;
                    Program.currentPlayer.equippedArmor = "Linen Rags";
                    Program.currentPlayer.equippedArmorValue = 1;
                    break;
                case PlayerClass.Archer:
                    Program.currentPlayer.equippedWeapon = "Flimsy Bow";
                    Program.currentPlayer.equippedWeaponValue = 1;
                    Program.currentPlayer.equippedArmor = "Linen Rags";
                    Program.currentPlayer.equippedArmorValue = 1;
                    break;
                case PlayerClass.Mage:
                    Program.currentPlayer.equippedWeapon = "Cracked Wand";
                    Program.currentPlayer.equippedWeaponValue = 1;
                    Program.currentPlayer.equippedArmor = "Linen Rags";
                    Program.currentPlayer.equippedArmorValue = 1;
                    break;
            }
        }
        
        //Metode til at 'Save and Quit' spillet.
        public static void Quit() {
            HUDTools.Print("Want to Quit? (Y/N)",10);
            string input = HUDTools.PlayerPrompt().ToLower();
            if (input == "y") {
                while (true) {
                    AudioManager.soundCampFire.Stop();
                    AudioManager.soundCampMusic.Stop();
                    AudioManager.soundLaugh.Play();
                    Console.WriteLine("Want to save? (Y/N)");
                    string input1 = HUDTools.PlayerPrompt().ToLower();
                    if (input1 == "y") {
                        Save();
                        Console.WriteLine("Game has been saved!");
                        Console.ReadKey(true);
                        break;
                    }
                    else if (input1 == "n") {
                        break;
                    }
                    else {
                        Console.WriteLine("Wrong Input");
                        HUDTools.PlayerPrompt();
                    }
                }
                MainMenu();
            }
        }

        //Metode til at ændre og gemme settings i en tilhørende configfil.
        private static void EditSettings() {
            var configFile = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
            var settings = configFile.AppSettings.Settings;
            while (true) {
                HUDTools.InstantSettings();
                string input = Console.ReadKey().KeyChar.ToString();
                if (input == "1") {
                    if (settings["toggleReadLine"].Value == "true") {
                        settings["toggleReadLine"].Value = "false";
                    } else {
                        settings["toggleReadLine"].Value = "true";
                    }
                } else if (input == "2") {
                    if (settings["toggleSlowPrint"].Value == "true") {
                        settings["toggleSlowPrint"].Value = "false";
                    } else {
                        settings["toggleSlowPrint"].Value = "true";
                    }
                } else if (input == "3") {
                    while (true) {
                        try {
                            Console.Clear();
                            Console.WriteLine($"Adjusting Volume (Between 0,0-1,0) - Volume {settings["volume"].Value}");
                            Console.WriteLine("Write (b)ack to return");
                            string input1 = Console.ReadLine();
                            if (input1 == "back" || input1 == "b") {
                                break;
                            }
                            else if (0 <= float.Parse(input1) && float.Parse(input1) <= 1) {
                                settings["volume"].Value = input1;
                            }
                            else {
                                Console.WriteLine("Invalid. Please write a number between 1 and 0");
                                Console.ReadKey(true);
                            }
                        }
                        catch (FormatException) {
                            Console.WriteLine("Invalid. Please write a number between 1 and 0");
                            Console.ReadKey(true);
                        }
                    }
                } else if (input == "\u001b") {
                    configFile.Save(ConfigurationSaveMode.Minimal);
                    HUDTools.Print("SSettings saved! Please restart the game...", 20);
                    HUDTools.PlayerPrompt();
                    break;
                } else {
                    Console.WriteLine("\nNo setting selected");
                    HUDTools.PlayerPrompt();
                }
                configFile.Save(ConfigurationSaveMode.Minimal);
            }
        }
    }
}
 