using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Saga
{
    internal class Program
    {
        public static Player currentPlayer = new Player();
        public static bool mainLoop = true;


        //
        static void Main(string[] args) {
            Start();
            Encounters.FirstEncounter();
            while (mainLoop) {
                Encounters.RandomEncounter();
            }
        }


        static void Start() {
            Console.WriteLine("Saga title");
            Console.Write("Enter a name: ");
            currentPlayer.name = Console.ReadLine();
            Console.Clear();
            Console.WriteLine("You awake in a cold and dark room. You feel dazed and are having trouble remembering");
            Console.WriteLine("anything about your past.");
            if (currentPlayer.name == "") {
                Console.WriteLine("You can't even remember your own name...");
                currentPlayer.name = "Adventurer";
            } else {
                Console.WriteLine("You know your name is " + currentPlayer.name + ".");
            }
            Console.ReadKey();
            Console.Clear();
            Console.WriteLine("You grope around in the darkness until you find a door handle. You feel some resistance as");
            Console.WriteLine("you turn the handle, but the rusty lock breaks with little effort. You see your captor");
            Console.WriteLine("standing with his back to you outside the door");
        }

    }
}
