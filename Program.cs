using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Text.Json;
using System.Configuration;
using Saga.Character;
using Saga.Dungeon;
using Saga.Assets;
using Saga.Items.Loot;

namespace Saga
{
    public class Program {
        //Genere spilleren som objekt så den kan sættes senere.
        public static Player CurrentPlayer { get; set; }
        public static Loot CurrentLoot { get; set; }
        public static AudioManager SoundController { get; set; }
        //Sætter variablen til Lydniveauet fra configfilen.
        public static float VolumeLevel { get; set; } = float.Parse(ConfigurationManager.AppSettings.Get("volume"));
        //Genere et objekt som kan returnere tilfældige tal mm.
        public static Random Rand { get; set; } = new();
        //Gør savefilen nemmere at læse.
        public static JsonSerializerOptions Options { get; set; } = new() { WriteIndented = true };

        //Spillets udførelse ved opstart
        static void Main() {
            //Gør unicode karaktere "runer" læselige i consolen og indstiller vindue størrelse.
            Console.OutputEncoding = Encoding.UTF8;
            Console.SetWindowSize(100, 50);
            Console.SetBufferSize(100, 50);
            Console.Title = "Saga";

            //Sætter lydniveauet til variablen sat fra configfilen.
            SoundController = new() {
                Volume = VolumeLevel
            };

            //Danner en saves mappe hvis den ikke eksistere.
            if (!Directory.Exists("saves")) {
                Directory.CreateDirectory("saves");
            }
            //Kalder MainMenu metoden.
            MainMenu();
        }

        //Metode til at lave en MainMenu hvor man kan ændre settings eller starte spillet etc.
        public static void MainMenu() {
            SoundController.Play("mainmenu");
            HUDTools.MainMenu();
            while (true) {
                string input = HUDTools.PlayerPrompt();
                if (input == "1") {
                    Play();
                }
                else if (input == "2") {
                    EditSettings();
                    HUDTools.MainMenu();
                }
                else if (input == "3") {
                    Console.WriteLine("Come back soon!");
                    Environment.Exit(0);
                }
                else {
                    Console.WriteLine("Wrong Input");
                    HUDTools.PlayerPrompt();
                    HUDTools.ClearLastLine(3);
                }
            }
        }

        public static void Play() {
            CurrentPlayer = Load(out bool newP);
            NewStart(newP);
            if (CurrentPlayer != null) {
                if (CurrentPlayer.CurrentAct == Act.Act1) {
                    CurrentLoot ??= new Act1Loot();
                    while (CurrentPlayer.CurrentAct == Act.Act1) {
                        SoundController.Stop();                       
                        Encounters.Camp();
                    }
                }
            }
            {//else if (CurrentPlayer.CurrentAct == Act.Act2) {

                //} else if (CurrentPlayer.CurrentAct == Act.Act3) {

                //} else if (CurrentPlayer.CurrentAct == Act.Act4) {

                //} else if (CurrentPlayer.CurrentAct == Act.Act5) {

                //}
            }
        }

        //Metode til at køre start introduktionen
        public static void NewStart(bool newP) {
            if (newP) {
                CurrentLoot = new Act1Loot();
                CurrentPlayer.SetStartingGear();
                Encounters.FirstEncounter();
                Encounters.MeetGheed();
                Encounters.SecondEncounter();
                Encounters.FirstCamp();
            }
        }

        //Metode til at gemme spillet ved først at tjekke for om der er en eksisterende save med det korrekte navn, som så overskrives, eller så dannes en helt ny en.
        public static void Save() {
            string path = $"saves/{CurrentPlayer.Id}.player";
            FileStream file = File.Open(path, FileMode.OpenOrCreate);
            string toSave = JsonSerializer.Serialize(CurrentPlayer, Options);                    
            byte[] saveFile = new UTF8Encoding(true).GetBytes(toSave);
            file.Write(saveFile, 0, saveFile.Length);
            file.Close();
        }

        //Metode til at loade gemte karakterer, vise dem og vælge hvilken til at spille videre på eller lave en helt ny karakter.
        public static Player Load(out bool newP) {
            newP = false;
            string[] paths = Directory.GetFiles("saves");
            List<Player> players = [];
            foreach (string path in paths) {
                Player player = JsonSerializer.Deserialize<Player>(File.ReadAllText(path));                          
                players.Add(player);
            }
            int idCount = players.Count+1;
            Console.Clear();
            SoundController.Play("typewriter");
            HUDTools.LoadSaves(players);
            while (true) {
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
                            HUDTools.ClearLastLine(2);

                        } else {
                            Console.WriteLine("Your id needs to be a number! Press to continue!");
                            Console.ReadKey(true);
                            HUDTools.ClearLastLine(2);
                        }
                    } else if (data[0] == "delete") {
                        if (int.TryParse(data[1], out int id)) {
                            if (players.Count == 0) {
                                Console.WriteLine("There is no player with that id!");
                                Console.ReadKey(true);
                                HUDTools.ClearLastLine(2);
                            } else {
                                foreach (Player player in players) {
                                    if (player.Id == id) {
                                        File.Delete($"saves/{player.Id}.player");
                                        players.Remove(player);
                                        Console.WriteLine($"Save game {player.Name} - level {player.Level}, was deleted");
                                        Console.ReadKey(true);
                                        Console.Clear();
                                        HUDTools.LoadSaves(players);
                                        break;
                                    } else {
                                        Console.WriteLine("There is no player with that id!");
                                        Console.ReadKey(true);
                                        HUDTools.ClearLastLine(2);
                                    }
                                }
                            }
                        } else {
                            if (players.Count == 0) {
                                Console.WriteLine("There is no player with that id!");
                                Console.ReadKey(true);
                                HUDTools.ClearLastLine(2);
                            } else {
                                foreach (Player player in players) {
                                    if (data[1] == player.Name) {
                                        File.Delete($"saves/{player.Id}.player");
                                        players.Remove(player);
                                        Console.WriteLine($"Save game {player.Name} - level {player.Level}, was deleted");
                                        Console.ReadKey(true);
                                        Console.Clear();
                                        HUDTools.LoadSaves(players);
                                        break;
                                    } else {
                                        Console.WriteLine("There is no player with that name!");
                                        Console.ReadKey(true);
                                        HUDTools.ClearLastLine(2);
                                    }
                                }
                            }
                        }
                    } else if (data[0] == "new game") {
                        Player newPlayer = NewCharacter(idCount);
                        newP = true;
                        return newPlayer;
                    }
                    else if (data[0] == "back" || data[0] == "b") {
                        HUDTools.MainMenu();
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
                        HUDTools.ClearLastLine(2);
                    }
                } catch (IndexOutOfRangeException) {
                    Console.WriteLine("Your id needs to be a number! Press to continue!");
                    Console.ReadKey(true);
                    HUDTools.ClearLastLine(2);
                }
            }
        }

        //Metode til at genere ny karakter efter at have inputtet 'new game' i Load() metoden.
        static Player NewCharacter(int i) {
            Player p = CreateCharacter(PickName(), PickClass());
            p.Id = i;
            Console.Clear();
            Program.SoundController.Play("typewriter");
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
            return classes switch {
                3 => new Mage(name),
                2 => new Archer(name),
                _ => new Warrior(name),
            };
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
                while (true) {
                    input1 = HUDTools.PlayerPrompt();
                    if (input1 != "y" && input1 != "n") {
                        HUDTools.Print("Invalid input.", 3);
                        Console.ReadKey(true);
                        HUDTools.ClearLastLine(2);
                    } else {
                        break;
                    }
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
                    while (true) {
                        string input2 = HUDTools.PlayerPrompt();
                        if (input2 == "y") {
                            return 1;
                        } else if (input2 == "n") {
                            HUDTools.ClearLastLine(4);
                            break;
                        } else {
                            HUDTools.Print("Invalid input.", 3);
                            Console.ReadKey(true);
                            HUDTools.ClearLastLine(2);
                        }
                    }
                }
                else if (input1 == "2") {
                    HUDTools.Print($"You want to become a Archer?\n(Y/N)",5);
                    while (true) {
                        string input2 = HUDTools.PlayerPrompt();
                        if (input2 == "y") {
                            return 2;
                        } else if (input2 == "n") {
                            HUDTools.ClearLastLine(4);
                            break;
                        } else {
                            HUDTools.Print("Invalid input.", 3);
                            Console.ReadKey(true);
                            HUDTools.ClearLastLine(2);
                        }
                    }
                }
                else if (input1 == "3") {
                    HUDTools.Print($"You want to become a Mage?\n(Y/N)",5);
                    while (true) {
                        string input2 = HUDTools.PlayerPrompt();
                        if (input2 == "y") {
                            return 3;
                        } else if (input2 == "n") {
                            HUDTools.ClearLastLine(4);
                            break;
                        } else {
                            HUDTools.Print("Invalid input.", 3);
                            Console.ReadKey(true);
                            HUDTools.ClearLastLine(2);
                        }
                    }
                }
                else {
                    HUDTools.Print("Please choose a listed class!",3);
                    Console.ReadKey(true);
                    HUDTools.ClearLastLine(2);
                }
            }
        }
        
        //Metode til at 'Save and Quit' spillet.
        public static void Quit() {
            HUDTools.Print("Want to Quit? (Y)",10);
            string input = HUDTools.PlayerPrompt().ToLower();
            if (input == "y") {
                SoundController.Stop();
                SoundController.Play("laugh");
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
                        Console.WriteLine("Invalid input");
                        Console.ReadKey(true);
                        HUDTools.ClearLastLine(2);
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
                    HUDTools.ClearLastLine(1);
                    while (true) {
                        try {
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
                                HUDTools.ClearLastLine(3);
                            }
                        }
                        catch (FormatException) {
                            Console.WriteLine("Invalid. Please write a number between 1 and 0");
                            Console.ReadKey(true);
                            HUDTools.ClearLastLine(3);
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
 