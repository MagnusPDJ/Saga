using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Configuration;
using System.Collections.Specialized;
using System.Security.Cryptography.X509Certificates;
using System.Threading;

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

        //Spillets udførelse ved opstart
        static void Main(string[] args) {

            //Danner en saves mappe hvis den ikke eksistere.
            if (!Directory.Exists("saves")) {
                Directory.CreateDirectory("saves");
            }

            //Kalder MainMenu metoden.
            MainMenu();

            //Spillets loop
            while (mainLoop) {
                Encounters.RandomEncounter();
            }
        }

        //Metode til at lave en MainMenu hvor man kan ændre settings eller starte spillet etc.
        public static void MainMenu() {
            Sounds.soundMainMenu.PlayLooping();
            while (true) {
                Console.Clear();
                Console.WriteLine("~~~~~~~~ Saga title ~~~~~~~~");
                Console.WriteLine("1.         Play");
                Console.WriteLine("2.       Settings");
                Console.WriteLine("3.       Quit Game");
                string input = PlayerPrompt();
                if (input == "1") {

                    //Kører Load() metoden og sætter spilleren ud fra hvad Load() returnere.
                    currentPlayer = Load(out bool newP);
                    Sounds.soundMainMenu.Stop();

                    //Tjekker for om det er 'new game' eller 'save game'. newP sættes til False i Load() metoden og hvorefter hvis 'new game' inputtes sættes newP til true
                    if (newP) {
                        Encounters.FirstEncounter();
                        Encounters.ShopEncounter();
                        break;
                    }
                    break;
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
                    PlayerPrompt();
                }
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
                Console.WriteLine("Choose a save!  ");
                Program.Print("-.-.-.-.-.-.-.-.-.-.-.-.-.-.-.-.",15);
                Program.Print("#: playername");
                foreach (Player p in players) {
                    Program.Print(p.id + ": " + p.name, 20);
                }
                Program.Print("<><><><><><><><><><><><><><><><>",15);
                Program.Print("Please input 'id:#' or 'playername'. For new game write 'new game'.",1);
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
                    } else if (data[0] == "new game") {
                        Player newPlayer = NewStart(idCount);
                        newP = true;
                        return newPlayer;
                    } else {
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
        static Player NewStart(int i) {
            Console.Clear();
            Player p = new Player();
            Console.WriteLine("//////////////");
            Print("Enter a name: ");
            string input;
            do {
                input = Console.ReadLine();
                if (input.Any(c => !char.IsLetter(c))) {
                    Console.WriteLine("Invalid name");
                } else {
                }
            } while (input.Any(c => !char.IsLetter(c)));
            p.name = input;
            Print("Pick a class: Mage  Archer  Warrior");
            bool flag = false;
            while(flag == false) {
                flag = true;
                string input1 = Console.ReadLine().ToLower();
                if (input1 == "mage") {
                    p.currentClass = Player.PlayerClass.Mage;
                } else if (input1 == "archer") {
                    p.currentClass = Player.PlayerClass.Archer;
                } else if (input1 == "warrior") {
                    p.currentClass = Player.PlayerClass.Warrior;
                } else {
                    Console.WriteLine("Please choose a listed class!");
                    flag = false;
                }
            }
            p.id = i;
            Console.Clear();
            Sounds.soundTypeWriter.PlayLooping();
            Print("You awake in a cold and dark room. You feel dazed and are having trouble remembering");
            Print("anything about your past.");
            if (String.IsNullOrWhiteSpace(p.name) == true) {
                Print("You can't even remember your own name...");
                p.name = "Adventurer";
            } else {
                Print($"You know your name is {p.name}.");
            }
            Sounds.soundTypeWriter.Stop();
            Console.ReadKey(true);
            Console.Clear();
            Sounds.soundTypeWriter.PlayLooping();
            Print("You grope around in the darkness until you find a door handle. You feel some resistance as");
            Print("you turn the handle, but the rusty lock breaks with little effort. You see your captor");
            Print("standing with his back to you outside the door.");
            Sounds.soundTypeWriter.Stop();
            return p;
        }

        //Metode til at 'Save and Quit' spillet.
        public static void Quit() {
            while (true) {
                Console.Clear();
                Console.WriteLine("Want to save? (Y/N)");
                string input = PlayerPrompt();
                if (input == "y") {
                    Save();
                    Console.WriteLine("Game has been saved!");
                    Console.ReadKey(true);
                    Environment.Exit(0);
                } else if (input == "n") {
                    Console.WriteLine("Game Over");
                    Console.ReadKey(true);
                    Environment.Exit(0);
                } else {
                    Console.WriteLine("Wrong Input");
                    PlayerPrompt();
                }
            }
        }

        //Metode til at ændre og gemme settings i en tilhørende configfil.
        private static void EditSettings() {
            var configFile = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
            var settings = configFile.AppSettings.Settings;
            Console.Clear();
            Print("            Settings", 20);
            Console.WriteLine("==================================");
            Console.WriteLine("");
            Print($"1. Toggle 'Press Enter continue': {settings["toggleReadLine"].Value}", 20);
            Print($"2. Toggle Slow-printing text:     {settings["toggleSlowPrint"].Value}", 20);
            Print($"3. System Volume:                 {Sounds.GetVolume()}", 20);
            while (true) {
                Console.Clear();
                Console.WriteLine("            Settings       ");
                Console.WriteLine("==================================");
                Console.WriteLine("                            ");
                Console.WriteLine($"1. Toggle 'Press Enter continue': {settings["toggleReadLine"].Value}");
                Console.WriteLine($"2. Toggle Slow-printing text:     {settings["toggleSlowPrint"].Value}");
                Console.WriteLine($"3. System Volume:                 {Sounds.GetVolume()}");
                Console.WriteLine("                            ");
                Console.WriteLine("=======Press Esc to go back=======");
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
                            Console.WriteLine($"Adjusting Volume (Between 0-100) - Volume {Sounds.GetVolume()}");
                            Console.WriteLine("Write (b)ack to return");
                            string input1 = Console.ReadLine();
                            if (input1 == "back" || input1 == "b") {
                                break;
                            }
                            else if (0 <= int.Parse(input1) && int.Parse(input1) <= 100) {
                                Sounds.SetVolume(int.Parse(input1));
                            }
                            else {
                                Console.WriteLine("Invalid. Please write a number between 100 and 0");
                                Console.ReadKey(true);
                            }
                        }
                        catch (FormatException) {
                            Console.WriteLine("Invalid. Please write a number between 100 and 0");
                            Console.ReadKey(true);
                        }
                    }
                } else if (input == "\u001b") {
                    configFile.Save(ConfigurationSaveMode.Minimal);
                    Print("SSettings saved! Please restart the game...", 20);
                    PlayerPrompt();
                    break;
                } else {
                    Console.WriteLine("No setting selected");
                    PlayerPrompt();
                }
            }
        }

        //Metode til at toggle ReadLine/ReadKey baseret på spiller settings.
        public static string PlayerPrompt() {
            if (Convert.ToBoolean(ConfigurationManager.AppSettings.Get("toggleReadLine")) == true) {
                return Console.ReadLine().ToLower();
            } else {
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
            } else {
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
                }
                else {
                    output += backgroundChar;
                }
            }
            return output;
        }
    }
}
