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
        public static Player currentPlayer = new Player();
        public static bool mainLoop = true;

<<<<<<< HEAD
=======

        //
>>>>>>> bbca3fa97f77b19a4e52c92c1107b8721f7f1f8a
        static void Main(string[] args) {
            if(!Directory.Exists("saves")) {
                Directory.CreateDirectory("saves");
            }
            currentPlayer = Load(out bool newP);
            if (newP) {
                Encounters.FirstEncounter();
            }
            while (mainLoop) {
                Encounters.RandomEncounter();
            }
        }

        static Player NewStart(int i) {
            Player p = new Player();
            Console.WriteLine("Saga title");
            Console.Write("Enter a name: ");
            p.name = Console.ReadLine();
            p.id = i;
            Console.Clear();
            Console.WriteLine("You awake in a cold and dark room. You feel dazed and are having trouble remembering");
            Console.WriteLine("anything about your past.");
            if (currentPlayer.name == "") {
                Console.WriteLine("You can't even remember your own name...");
                currentPlayer.name = "Adventurer";
            } else {
                Console.WriteLine("You know your name is " + p.name + ".");
            }
            Console.ReadKey();
            Console.Clear();
            Console.WriteLine("You grope around in the darkness until you find a door handle. You feel some resistance as");
            Console.WriteLine("you turn the handle, but the rusty lock breaks with little effort. You see your captor");
            Console.WriteLine("standing with his back to you outside the door");
            return p;
        }
        
        public static void save() {
            BinaryFormatter binForm = new BinaryFormatter();
            string path = "saves/" + currentPlayer.id.ToString();
            FileStream file = File.Open(path,FileMode.OpenOrCreate);
            binForm.Serialize(file, currentPlayer);
            file.Close();
        }
        public static Player Load(out bool newP) {
            newP = false;
            Console.Clear();
            string[] paths = Directory.GetDirectories("saves");
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
    }
}
