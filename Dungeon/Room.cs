using System;
using Saga.Assets;
using Windows.Devices.Lights;

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
            exits = [new Exit() { keyString = "e", exitDescription = "", valueRoom = null }];
        }
        public override void LoadRoom() {
            if (Program.CurrentPlayer.CurrentAct == Character.Act.Start) {
                Encounters.FirstCamp();
            }     
            Encounters.Camp();
        }
    }

    public class Rooms {

        public readonly static CampRoom Camp = new();

        public readonly static HallwayRoom Hallway = new();

        public readonly static StartRoom StartRoom = new();
    }
}
