using Saga.Assets;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Saga.Dungeon
{
    public class Exit
    {
        public string keyString;
        public string exitDescription;
        public Room valueRoom;
    }

    public class Room
    {
        public string description;
        public string roomName;
        public Exit[] exits;

    }

    public class RoomController 
    {
        public Room currentRoom;

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
                LoadRoom();
            }
        }

        public void LoadRoom() {
            string exit = "";
            if (currentRoom == Rooms.Start) {
                Encounters.FirstEncounter();
                Encounters.MeetGheed();
                Console.Clear();
                HUDTools.SmallCharacterInfo();
                while (exit == "") {
                    exit = TextInput.PlayerPrompt(true);
                }
                Program.RoomController.ChangeRoom(exit);
            }
        }
    }

    public class Rooms : Room {

        public static Room CreateRoom() {
            Room room = new();
            return room;
            //Not Implemented
        }

        public readonly static Room Start = new() {
            roomName = "Jail Cells",
            description = "You look around and see Gheed rummage through big wooden crates. You hear him counting.",
            exits = [new Exit() { keyString = "door", exitDescription = $"You see a big wooden \u001b[96mdoor\u001b[0m with rusted hinges and reinforced with iron plating", valueRoom = Hallway}]            
        };

        public readonly static Room Hallway = new() { 
            roomName = "Hallway",
            description = "",
            exits = [new Exit() { keyString = "deeper", exitDescription = $"The hallway continues \u001b[96mdeeper\u001b[0m into the dark", valueRoom = Camp}]
        };

        public readonly static Room Camp = new() {
            roomName = "Camp",
            description = "",
            exits = null
        };
    }
}
