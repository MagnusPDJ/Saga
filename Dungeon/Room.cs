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

        public void ChangeRoom() {
            //Not implemented
        }

        public void LoadRoom() {
            //Not implemented
        }
    }

    public class Rooms : Room {

        public static Room CreateRoom() {
            Room room = new();
            return room;
            //Not Implemented
        }

        public readonly static Room IntroductionRoom = new() {
            roomName = "Jail Cells",
            description = "",
            exits = [new Exit() { keyString = "door", exitDescription = "A big wooden _door_ with rusted hinges and reinforced with iron plating", valueRoom = Hallway}]            
        };

        public readonly static Room Hallway = new() { 
            roomName = "Hallway",
            description = "",
            exits = [new Exit() { keyString = "deeper", exitDescription = "The hallway continues _deeper_ into the dark", valueRoom = Camp}]
        };

        public readonly static Room Camp = new() {
            roomName = "Camp",
            description = "",
            exits = null
        };
    }
}
