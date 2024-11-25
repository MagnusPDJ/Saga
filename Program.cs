using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Configuration;
using Saga.Character;
using Saga.Dungeon;
using Saga.assets;
using Saga.Items.Loot;

namespace Saga
{
    public class Program {
        //Genere spilleren som objekt så den kan sættes senere.
        public static Player CurrentPlayer { get; set; }
        public static Loot CurrentLoot { get; set; }

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
                HUDTools.MainMenu();
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
            CurrentPlayer = Load(out bool newP);
            if (CurrentPlayer == null) {
            } else {
                NewStart(newP);
                if (CurrentPlayer.CurrentAct == Act.Act1) {
                    if (CurrentLoot == null) {
                        CurrentLoot = new Act1Loot();
                    }                   
                    while (CurrentPlayer.CurrentAct == Act.Act1) {
                        AudioManager.soundMainMenu.Stop();
                        AudioManager.soundShop.Stop();
                        Encounters.Camp();
                    }
                } else if (CurrentPlayer.CurrentAct == Act.Act2) {

                } else if (CurrentPlayer.CurrentAct == Act.Act3) {

                } else if (CurrentPlayer.CurrentAct == Act.Act4) {

                } else if (CurrentPlayer.CurrentAct == Act.Act5) {

                }
            }
        }

        //Metode til at køre start introduktionen
        public static void NewStart(bool newP) {
            if (newP) {
                CurrentLoot = new Act1Loot();
                CurrentPlayer.SetStartingGear();
                Encounters.FirstEncounter();
                Encounters.FirstShopEncounter();
                Encounters.SecondEncounter();
                Encounters.FirstCamp();
            }
        }

        //Metode til at gemme spillet ved først at tjekke for om der er en eksisterende save med det korrekte navn, som så overskrives, eller så dannes en helt ny en.
        public static void Save() {
            BinaryFormatter binForm = new BinaryFormatter();
            string path = $"saves/{CurrentPlayer.Id}.player";
            FileStream file = File.Open(path, FileMode.OpenOrCreate);
            binForm.Serialize(file, CurrentPlayer);
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
                HUDTools.Print("Choose a save! ('back' for main menu) ", 15);
                HUDTools.Print("-.-.-.-.-.-.-.-.-.-.-.-.-.-.-.-.", 5);
                HUDTools.Print("#: playername");
                foreach (Player p in players) {
                    HUDTools.Print($"{p.Id}: {p.Name} - Class: {p.CurrentClass} - Level: {p.Level}", 10);
                }
                HUDTools.Print("<><><><><><><><><><><><><><><><>", 5);
                HUDTools.Print("To load a save write 'id:#' or 'playername'.\nFor new game write 'new game'.\nTo delete a save write 'delete:playername'.", 1);
                string[] data = Console.ReadLine().Split(':');
                try {
                    if (data[0] == "id") {
                        if (int.TryParse(data[1], out int id)) {
                            foreach (Player player in players) {
                                if (player.Id == id) {
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
                                if (player.Id == id) {
                                    File.Delete($"saves/{player.Id}.player");
                                    players.Remove(player);
                                    Console.WriteLine($"Save game {player.Name} - level {player.Level}, was deleted");
                                    Console.ReadKey(true);
                                    break;
                                } else {
                                    Console.WriteLine("There is no player with that id!");
                                    Console.ReadKey(true);
                                }
                            }
                        } else {
                            foreach (Player player in players) {
                                if (data[1] == player.Name) {
                                    File.Delete($"saves/{player.Id}.player");
                                    players.Remove(player);
                                    Console.WriteLine($"Save game {player.Name} - level {player.Level}, was deleted");
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
                    else if (data[0] == "back") {
                        return null;
                    }
                    else {
                        foreach (Player player in players) {
                            if (player.Name == data[0]) {
                                return player;
                            }
                        }
                        Console.WriteLine("There is no player with that name!");
                        Console.ReadKey(true);
                    }
                } catch (IndexOutOfRangeException) {
                    Console.WriteLine("Your id needs to be a number! Press to continue!");
                    Console.ReadKey(true);
                }
            }
        }

        //Metode til at genere ny karakter efter at have inputtet 'new game' i Load() metoden.
        static Player NewCharacter(int i) {
            Player p = CreateCharacter(PickName(), PickClass());
            p.Id = i;
            Console.Clear();
            AudioManager.soundTypeWriter.Play();
            HUDTools.Print("You awake in a cold and dark room. You feel dazed and are having trouble remembering");
            HUDTools.Print("anything about your past.");
            if (string.IsNullOrWhiteSpace(p.Name) == true) {
                HUDTools.Print("You can't even remember your own name...");
                p.Name = "Adventurer";
            } else {
                HUDTools.Print($"You know your name is {p.Name}.");
            }
            Console.ReadKey(true);
            return p;
        }

        public static Player CreateCharacter(string name = "Adventurer", int classes = 1) { 
            switch (classes) {
                default:
                case 1:
                    return new Warrior(name);
                case 2:
                    return new Archer(name);
                case 3:
                    return new Mage(name);
            }
        }
   
        //Metode til at vælge navn.
        public static string PickName() {
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
                HUDTools.Print($"This is your name?\n{input}.\n(Y/N)",10);
                input1 = HUDTools.PlayerPrompt();
                if (input1 != "y" && input1 != "n") {
                    HUDTools.Print("Invalid input.", 3);
                }
            } while (input1 != "y");
            return input;
        }

        //Metode til at vælge Class.
        public static int PickClass() {
            HUDTools.Print("Pick a class, enter a # 1-3:\n1. Warrior\n2. Archer\n3. Mage",20);
            while (true) {
                string input1 = HUDTools.PlayerPrompt();
                if (input1 == "1") {
                    HUDTools.Print($"You want to become a Warrior?\n(Y/N)", 5);
                    string input2 = HUDTools.PlayerPrompt();
                    if (input2 == "y") {
                        return 1;
                    }
                    else {
                        HUDTools.Print("Enter a # 1-3:",5);
                    }
                }
                else if (input1 == "2") {
                    HUDTools.Print($"You want to become a Archer?\n(Y/N)",5);
                    string input2 = HUDTools.PlayerPrompt();
                    if (input2 == "y") {
                        return 2;
                    }
                    else {
                        HUDTools.Print("Enter a # 1-3:",5);
                    }
                }
                else if (input1 == "3") {
                    HUDTools.Print($"You want to become a Mage?\n(Y/N)",5);
                    string input2 = HUDTools.PlayerPrompt();
                    if (input2 == "y") {
                        return 3;
                    }
                    else {
                        HUDTools.Print("Enter a # 1-3:", 5);
                    }
                }
                else {
                    HUDTools.Print("Please choose a listed class!",3);
                }
            }
        }
        
        //Metode til at 'Save and Quit' spillet.
        public static void Quit() {
            HUDTools.Print("Want to Quit? (Y)",10);
            string input = HUDTools.PlayerPrompt().ToLower();
            if (input == "y") {
                AudioManager.soundCampFire.Stop();
                AudioManager.soundCampMusic.Stop();
                AudioManager.soundLaugh.Play();
                Console.WriteLine("Want to save? (Y/N)");
                while (true) {
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
                string input = Console.ReadKey(true).KeyChar.ToString();
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
                    HUDTools.Print("Settings saved! Please restart the game...", 20);
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
 