using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

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
            if(!Directory.Exists("saves")) {
                Directory.CreateDirectory("saves");
            }

            //Kører Load() metoden og sætter spilleren ud fra hvad Load() returnere.
            currentPlayer = Load(out bool newP);

            //Tjekker for om det er 'new game' eller 'save game'. newP sættes til False i Load() metoden og hvorefter hvis 'new game' inputtes sættes newP til true.
            if (newP) {
                Encounters.FirstEncounter();
            }
            while (mainLoop) {
                Encounters.RandomEncounter();
            }
        }
        
        //Metode til at gemme spillet ved først at tjekke for om der er en eksisterende save med det korrekte navn, som så overskrives, eller så dannes helt en ny.
        public static void save() {
            BinaryFormatter binForm = new BinaryFormatter();
            string path = "saves/" + currentPlayer.id.ToString() + ".player";
            FileStream file = File.Open(path,FileMode.OpenOrCreate);
            binForm.Serialize(file, currentPlayer);
            file.Close();
        }

        //Metode til at loade gemte karaktere og vise dem og vælge hvilken til at spille videre på eller lave en helt ny karakter.
        public static Player Load(out bool newP) {
            newP = false;
            Console.Clear();
            string[] paths = Directory.GetFiles("saves");
            List<Player> players = new List<Player>();
            int idCount = 0;

            BinaryFormatter binForm = new BinaryFormatter();
            foreach (string p in paths) {
                FileStream file = File.Open(p, FileMode.Open);
                Player player = (Player)binForm.Deserialize(file);
                file.Close();
                players.Add(player);
            }

            idCount = players.Count;

            while (true) {
                Console.Clear();
                Console.WriteLine("Choose your player");
                foreach (Player p in players) {
                    Console.WriteLine(p.id + ": " + p.name);
                }
                Console.WriteLine("Please input player name or id (id:# or playername) or write 'new game'.");
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
                            Console.ReadKey();
                        } else {
                            Console.WriteLine("Your id needs to be a number! Press to continue!");
                            Console.ReadKey();
                        }
                    } else if (data[0] == "new game") {
                        Player newPlayer = NewStart(idCount);
                        newP = true;
                        return newPlayer;
                    } 
                    else {
                        foreach (Player player in players) {
                            if (player.name == data[0]) {
                                return player;
                            }
                            Console.WriteLine("There is no player with that name!");
                            Console.ReadKey();
                        }
                    }
                } catch(IndexOutOfRangeException) {
                    Console.WriteLine("Your id needs to be a number! Press to continue!");
                    Console.ReadKey();
                }
            }

        }

        //Metode til at genere ny karakter efter at have inputtet 'new game' i Load() metoden.
        static Player NewStart(int i) {
            Console.Clear();
            Player p = new Player();
            Console.WriteLine("Saga title");
            Console.Write("Enter a name: ");
            p.name = Console.ReadLine();
            p.id = i;
            Console.Clear();
            Console.WriteLine("You awake in a cold and dark room. You feel dazed and are having trouble remembering");
            Console.WriteLine("anything about your past.");
            if (String.IsNullOrWhiteSpace(p.name) == true) {
                Console.WriteLine("You can't even remember your own name...");
                p.name = "Adventurer";
            }
            else {
                Console.WriteLine("You know your name is " + p.name + ".");
            }
            Console.ReadKey();
            Console.Clear();
            Console.WriteLine("You grope around in the darkness until you find a door handle. You feel some resistance as");
            Console.WriteLine("you turn the handle, but the rusty lock breaks with little effort. You see your captor");
            Console.WriteLine("standing with his back to you outside the door");
            return p;
        }

        //Metode til at 'Save and Exit'.
        public static void Quit() {
            while (true) {
                Console.Clear();
                Console.WriteLine("Want to save? (Y/N)");
                string input = Console.ReadLine().ToLower();
                if (input == "y") {
                    save();
                    Console.WriteLine("Game has been saved!");
                    Console.ReadKey();
                    Environment.Exit(0);
                }
                else if (input == "n") {
                    Console.WriteLine("Game Over");
                    Console.ReadKey();
                    Environment.Exit(0);
                }
            }
        }
    }
}
