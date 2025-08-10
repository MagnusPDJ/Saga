using Saga.Assets;
using Saga.Items;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using static System.Windows.Forms.LinkLabel;

namespace Saga.Dungeon
{
    public class Exit
    {
        public string keyString;
        public string exitDescription;
        public Room valueRoom;
    }

    public abstract class Room
    {
        public string description;
        public string roomName;
        public Exit[] exits;
        public abstract void LoadRoom();
    }

    public class DungeonTemplate
    {
        public List<Room> rooms = [];
    }

    public class RoomController 
    {
        public InputAction[] InputActions = [
            new Go("go"),
            new Examine("examine"),
            new Equip("equip"),
            new Use("use"),
            new Look("look"),
            new DrinkPotion("heal", "h"),
            new SeeCharacterScreen("character", "c"),
            new SeeInventory("inventory", "i"),
            new SeeQuestLog("questlog", "l")
            ];
        public Room currentRoom;
        public DungeonTemplate currentDungeonInstance;

        public void ChangeRoom(string keystring, Room room = null) {
            bool foundRoom = false;
            if (room != null) { 
                currentRoom = room;
                foundRoom = true;              
            }
            foreach (Exit exit in currentRoom.exits) {
                if (exit.keyString == keystring) {
                    currentRoom = exit.valueRoom;
                    foundRoom = true;
                    break;
                }
            }
            if (foundRoom) {
                currentRoom.LoadRoom();
            }
        }

        public void ExploreDungeon() {
            Program.RoomController.currentDungeonInstance = GenerateDungeon();
            Program.CurrentPlayer.TimesExplored++;
            ChangeRoom("",currentDungeonInstance.rooms[^1]);
        }

        public static DungeonTemplate GenerateDungeon() {
            var dungeon = new DungeonTemplate()
            {
                rooms = GenerateRooms()
            };

            return dungeon;
        }
        public static List<Room> GenerateRooms() {
            
            int dybde = Program.Rand.Next(1, 5 + 1);
            Room[] rooms = new Room[dybde];
            for (int i = 0; i < dybde; i++) {
                rooms[i] = CreateRandomRoom(rooms, i);
            }

            return [.. rooms];

        }

        public static Room CreateRandomRoom(Room[] rooms, int i) {
            (string, string) roomNameAndDescription = CreateRoomNameAndDescription();
            if (i == 0) {
                Room room = new RandomRoom()
                {
                    roomName = roomNameAndDescription.Item1,
                    description = roomNameAndDescription.Item2,
                    exits = [new Exit() { keyString = "home", exitDescription = $"This room is a dead end. You should \u001b[96mgo home\u001b[0m to your camp.", valueRoom = Rooms.Camp }],
                };
                return room;
            } else {
                (string, string) exit = CreateExit();
                Room room = new RandomRoom()
                {
                    roomName = roomNameAndDescription.Item1,
                    description = roomNameAndDescription.Item2,
                    exits = [new Exit() { keyString = $"{exit.Item1}", exitDescription = $"{exit.Item2}", valueRoom = rooms[i - 1] }],
                };
                return room;
            }       
        }

        public static (string, string) CreateRoomNameAndDescription() {
            List<string> lines = HUDTools.ReadAllResourceLines("Saga.Dungeon.RoomNames.txt");
            lines.RemoveAt(0);
            int picked = Program.Rand.Next(lines.Count);
            return (lines[picked].Split(';')[0], lines[picked].Split(';')[1]);
        }
        public static (string, string) CreateExit() {
            List<string> lines = HUDTools.ReadAllResourceLines("Saga.Dungeon.RoomExits.txt");
            lines.RemoveAt(0);
            int picked = Program.Rand.Next(lines.Count);
            return (lines[picked].Split(';')[0], lines[picked].Split(';')[1].Replace("%", "\u001b[96m").Replace("¤", "\u001b[0m"));
        }

    }
    public class StartRoom : Room
    {
        public StartRoom() {
            roomName = "Jail Cells";
            description = "You look around and see Gheed rummage through big wooden crates. You hear him counting.";
            exits = [new Exit() { keyString = "door", exitDescription = $"You see a big wooden \u001b[96mdoor\u001b[0m with rusted hinges and reinforced with iron plating", valueRoom = Rooms.Hallway }];
        }
        public override void LoadRoom() {
            string exit = "";
            Encounters.FirstEncounter();
            Encounters.MeetGheed();
            HUDTools.SmallCharacterInfo();
            while (exit == "") {
                exit = TextInput.PlayerPrompt(true);
            }
            Program.RoomController.ChangeRoom(exit);

        }
    }

    public class HallwayRoom : Room
    {
        public HallwayRoom() {
            roomName = "Hallway";
            description = "";
            exits = [new Exit() { keyString = "deeper", exitDescription = $"The hallway continues \u001b[96mdeeper\u001b[0m into the dark", valueRoom = Rooms.Camp }];
        }
        public override void LoadRoom() {
            string exit = "";
            Encounters.SecondEncounter();
            HUDTools.SmallCharacterInfo();
            while (exit == "") {
                exit = TextInput.PlayerPrompt(true);
            }
            Program.RoomController.ChangeRoom(exit);
        }
    }

    public class CampRoom : Room
    {
        public CampRoom() {
            roomName = "Camp";
            description = "";
            exits = [new Exit() { keyString = "You will never leave", exitDescription = "here alive", valueRoom = null}];
        }
        public override void LoadRoom() {
            if (Program.CurrentPlayer.CurrentAct == Character.Act.Start) {
                Encounters.FirstCamp();
            }
            if (Program.CurrentPlayer.TimesExplored == 1) {
                Encounters.FirstReturn();
            }
                string choice = Encounters.Camp();
            Program.SoundController.Stop();
            if (choice == "quit") {
                Program.MainMenu();
            } else if (choice == "explore") {
                Program.RoomController.ExploreDungeon();
            }
        }
    }

    public class RandomRoom : Room 
    {
        public override void LoadRoom() {
            string exit = "";
            Encounters.RandomEncounter();
            HUDTools.SmallCharacterInfo();
            while (exit == "") {
                exit = TextInput.PlayerPrompt(true);
            }
            Program.SoundController.Stop();
            Program.RoomController.ChangeRoom(exit);
        }
    }

    public class Rooms {

        public readonly static CampRoom Camp = new();

        public readonly static HallwayRoom Hallway = new();

        public readonly static StartRoom StartRoom = new();
    }
}
