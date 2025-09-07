using Saga.Assets;
using Saga.Character;
using System.Collections.Generic;

namespace Saga.Dungeon
{
    public class Exit
    {
        public string keyString = "";
        public string exitDescription = "";
        public Room? valueRoom;
    }

    public abstract class Room
    {
        public string description = "";
        public string roomName = "";
        public Exit[] exits = [];
        public abstract void LoadRoom();

        public static Room CreateRandomBasicCombatRoom(Room[] rooms, int i) {
            (string, string) roomNameAndDescription = CreateRoomNameAndDescription();
            if (i == 0) {
                Room room = new RandomBasicCombatRoom()
                {
                    roomName = roomNameAndDescription.Item1,
                    description = roomNameAndDescription.Item2,
                    exits = [new Exit() { keyString = "home", exitDescription = $"This room is a dead end. You should \u001b[96mgo home\u001b[0m to your camp.", valueRoom = Rooms.Camp }],
                };
                return room;
            } else {
                (string, string) exit = CreateExit();
                Room room = new RandomBasicCombatRoom()
                {
                    roomName = roomNameAndDescription.Item1,
                    description = roomNameAndDescription.Item2,
                    exits = [new Exit() { keyString = $"{exit.Item1}", exitDescription = $"{exit.Item2}", valueRoom = rooms[i - 1]}],
                };
                return room;
            }
        }
        public static Room CreateRandomEncounterRoom(Room[] rooms, int i) {
            (string, string) roomNameAndDescription = CreateRoomNameAndDescription();
            if (i == 0) {
                Room room = new RandomEncounterRoom()
                {
                    roomName = roomNameAndDescription.Item1,
                    description = roomNameAndDescription.Item2,
                    exits = [new Exit() { keyString = "home", exitDescription = $"This room is a dead end. You should \u001b[96mgo home\u001b[0m to your camp.", valueRoom = Rooms.Camp }]
                };
                return room;
            } else {
                (string, string) exit = CreateExit();
                Room room = new RandomEncounterRoom()
                {
                    roomName = roomNameAndDescription.Item1,
                    description = roomNameAndDescription.Item2,
                    exits = [new Exit() { keyString = $"{exit.Item1}", exitDescription = $"{exit.Item2}", valueRoom = rooms[i-1]}]
                };
                return room;
            }
        }
        public static (string, string) CreateRoomNameAndDescription() {
            List<string> lines = HUDTools.ReadAllResourceLines("Saga.Dungeon.RoomNames.txt");
            lines.RemoveAt(0);
            int picked = Program.Rand.Next(lines.Count);
            return (lines[picked].Split(';')[0], lines[picked].Split(';')[1].Replace("\\n", "\n"));
        }
        public static (string, string) CreateExit() {
            List<string> lines = HUDTools.ReadAllResourceLines("Saga.Dungeon.RoomExits.txt");
            lines.RemoveAt(0);
            int picked = Program.Rand.Next(lines.Count);
            return (lines[picked].Split(';')[0], lines[picked].Split(';')[1].Replace("%", "\u001b[96m").Replace("¤", "\u001b[0m"));
        }
    }

    public class DungeonTemplate
    {
        public List<Room> rooms = [];
    }

    public class RoomController 
    {
        public InputAction[] InputRoomActions = [
            new Go("go"),     
            new Use("use"),
            new Look("look"),
            new DrinkHealingPotion("heal", "h"),
            new SeeCharacterScreen("character", "c"),
            new SeeInventory("inventory", "i"),
            new SeeQuestLog("questlog", "l"),
            new SeeSkillTree("skilltree", "k")
            ];
        public InputAction[] InputInvActions = [
            new Examine("examine"),
            new Equip("equip"),
            new UnEquip("unequip"),
            new Back("back", "b"),
            ];
        public Room currentRoom = Rooms.Camp;
        public DungeonTemplate currentDungeonInstance = new();
        public bool ran = false;

        public void ChangeRoom(string keystring, Room? room = null) {
            bool foundRoom = false;
            if (room != null) { 
                currentRoom = room;
                foundRoom = true;              
            }
            foreach (Exit exit in currentRoom.exits) {
                if (exit.keyString == keystring) {
                    if (exit.valueRoom != null) {
                        currentRoom = exit.valueRoom;
                    }
                    foundRoom = true;
                    break;
                }
            }
            if (foundRoom) {
                Program.CurrentPlayer.RegainMana();
                currentRoom.LoadRoom();
            }
        }
        public void ExploreDungeon() {
            Program.RoomController.currentDungeonInstance = GenerateDungeon();
            Program.CurrentPlayer.TimesExplored++;
            ChangeRoom("", currentDungeonInstance.rooms[^1]);
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
                rooms[i] = Program.Rand.Next(0, 100) switch
                {
                    int x when 55 <= x && x < 80 && i == 0 =>                           Encounters.ProgressTheStory(rooms, i),
                    int x when 80 <= x && x < 90 && i != 0 =>                           new HallwayRoom(rooms, i),
                    int x when 90 <= x && x < 100 && Program.CurrentPlayer.Level > 1 => Room.CreateRandomEncounterRoom(rooms, i),
                    _ =>                                                                Room.CreateRandomBasicCombatRoom(rooms, i),
                };
            }

            return [.. rooms];
        }
    }

    //Alle custom rum:

    //Det allerførste rum, hvor spillet starter.
    public class StartRoom : Room
    {
        public StartRoom() {
            roomName = "Jail cells";
            description = "You look around and see Gheed rummage through big wooden crates. You hear him counting.";
            exits = [new Exit() { keyString = "door", exitDescription = $"You see a big wooden \u001b[96mdoor\u001b[0m with rusted hinges and reinforced with iron plating", valueRoom = Rooms.HallwayStart }];
        }
        public override void LoadRoom() {
            string exit = "";
            Encounters.FirstEncounter();
            Encounters.MeetGheed();
            HUDTools.RoomHUD();
            while (exit == "") {
                exit = TextInput.PlayerPrompt(true);
            }
            Program.RoomController.ChangeRoom(exit);

        }
    }
    //Andet rum efter starten.
    public class HallwayRoomStart : Room
    {
        public HallwayRoomStart() {
            roomName = "Hallway";
            description = "";
            exits = [new Exit() { keyString = "deeper", exitDescription = $"The hallway continues \u001b[96mdeeper\u001b[0m into the dark", valueRoom = Rooms.Camp }];
        }
        public override void LoadRoom() {
            string exit = "";
            Encounters.SecondEncounter();
            if (Program.RoomController.ran == true) {
                Program.RoomController.ran = false;
                Program.RoomController.ChangeRoom(exits[0].keyString);
            } else {
                HUDTools.RoomHUD();
                while (exit == "") {
                    exit = TextInput.PlayerPrompt(true);
                }
                Program.RoomController.ChangeRoom(exit);
            }
        }
    }
    //Her har spilleren sin lejr og retinue holder til.
    public class CampRoom : Room
    {
        public CampRoom() {
            roomName = "Camp";
            description = "";
            exits = [new Exit() { keyString = "You will never leave", exitDescription = "here alive", valueRoom = null}];
        }
        public override void LoadRoom() {
            if (Program.CurrentPlayer.CurrentAct == Encounters.Act.Start) {
                Encounters.FirstCamp();
            }
            if (Program.CurrentPlayer.TimesExplored == 1) {
                Encounters.FirstReturn();
            }
                string choice = Encounters.Camp();
            Program.SoundController.Stop();
            if (choice == "quit") {
                Program.CurrentPlayer = new Warrior("Adventurer");
                Program.MainMenu();
            } else if (choice == "explore") {
                Program.RoomController.ExploreDungeon();
            }
        }
    }
    //Rum til Puzzle One Encounter.
    public class HallwayRoom : Room
    {
        public HallwayRoom(Room[] rooms, int i) {
            roomName = "Hallway";
            description = "Behind you are the runes you crossed.";
            exits = [new Exit() { keyString = "deeper", exitDescription = $"The hallway continues \u001b[96mdeeper\u001b[0m into the dark", valueRoom = rooms[i - 1] }];
        }
        public override void LoadRoom() {
            string exit = "";
            Encounters.PuzzleOneEncounter();
            HUDTools.RoomHUD();
            while (exit == "") {
                exit = TextInput.PlayerPrompt(true);
            }
            Program.SoundController.Stop();
            Program.RoomController.ChangeRoom(exit);
        }
    }
    //Rum med et tilfældigt monster.
    public class RandomBasicCombatRoom : Room 
    {
        public override void LoadRoom() {
            string exit = "";
            Encounters.RandomBasicCombatEncounter();
            if (Program.RoomController.ran == true) {
                Program.RoomController.ran = false;
                Program.RoomController.ChangeRoom(exits[0].keyString);
            } else {
                HUDTools.RoomHUD();
                while (exit == "") {
                    exit = TextInput.PlayerPrompt(true);
                }
                Program.RoomController.ChangeRoom(exit);
            }
        }
    }
    //Rum til et tilfældigt encounter.
    public class RandomEncounterRoom : Room 
    {
        public override void LoadRoom() {
            string exit = "";
            Encounters.RandomEncounter();
            if (Program.RoomController.ran == true) {
                Program.RoomController.ran = false;
                Program.RoomController.ChangeRoom(exits[0].keyString);
            } else {
                HUDTools.RoomHUD();
                while (exit == "") {
                    exit = TextInput.PlayerPrompt(true);
                }
                Program.RoomController.ChangeRoom(exit);
            }
        }
    }
    //Rum hvor spilleren møder Flemsha
    public class MeetFlemshaRoom : Room
    {
        public MeetFlemshaRoom() {
            roomName = "Old jail cells";
            description = "You look around the old jail. There is nothing of value. Flemsha seems to want you to leave first.";
            exits = [new Exit() { keyString = "home", exitDescription = $"This room is a dead end. You should \u001b[96mgo home\u001b[0m to your camp.", valueRoom = Rooms.Camp }];
        }
        public override void LoadRoom() {
            string exit = "";
            Encounters.MeetFlemsha();
            if (Program.RoomController.ran == true) {
                Program.RoomController.ran = false;
                Program.RoomController.ChangeRoom(exits[0].keyString);
            } else {
                HUDTools.RoomHUD();
                while (exit == "") {
                    exit = TextInput.PlayerPrompt(true);
                }
                Program.RoomController.ChangeRoom(exit);
            }
        }
    }

    public class Rooms {

        public readonly static CampRoom Camp = new();

        public readonly static HallwayRoomStart HallwayStart = new();

        public readonly static StartRoom StartRoom = new();
    }
}