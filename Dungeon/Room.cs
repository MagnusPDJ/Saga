using Saga.Assets;
using Saga.Character;
using Saga.Dungeon.Enemies;

namespace Saga.Dungeon
{
    public interface IExit
    {
        string ExitTemplateDescription { get; set; }
    }

    public class Exit : IExit
    {
        public string keyString = "";
        public string exitDescription = "";
        public required Room valueRoom;
        public bool hasPreviousRoom = false;
        public string ExitTemplateDescription { get; set; } = "";
    }

    public abstract class Room
    {
        public string description = "";
        public string roomName = "";
        public List<Exit> exits = [];
        public bool Visited = false;

        public EnemyBase? enemy = null;
        public bool MonsterSpawned = false;
        public bool Cleared = false;
        public string corpseDescription = "";

        public abstract void LoadRoom();
    }

    // Simple generic dungeon room used by the generator:
    public class DungeonRoom : Room
    {
        public DungeonRoom(string name, string desc)
        {
            roomName = name;
            description = desc;
            exits = [];
        }

        public override void LoadRoom()
        {
            if (!Visited) Visited = true;
            string exit = "";
            HUDTools.RoomHUD();
            while (exit == "")
            {
                exit = TextInput.PlayerPrompt(true);
            }
            Program.RoomController.ChangeRoom(exit);
        }
    }

    public class DungeonInstance
    {
        public int Level { get; set; } = 1;
        public double DifficultyMultiplier { get; set; } = 1.0;
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
        public DungeonInstance currentDungeonInstance = new();
        public bool ran = false;

        public void ChangeRoom(string keystring, Room? room = null) {
            bool foundRoom = false;
            Room previousRoom = new DungeonRoom("", "");
            if (room != null) {
                currentRoom = room;
                foundRoom = true;
            }
            if (keystring == "home") {
                currentRoom = Rooms.Camp;
                foundRoom = true;
            }
            if (!foundRoom) {
                previousRoom = currentRoom;
                foreach (Exit exit in currentRoom.exits) {
                    if (exit.keyString == keystring) {
                        currentRoom = exit.valueRoom;
                        foundRoom = true;
                        break;
                    }
                }
            }
            if (foundRoom) {
                Program.CurrentPlayer.RegainMana();

                //Update exit descriptions for all exits in current room
                foreach (var ex in currentRoom.exits) {
                    if (ex.valueRoom != null && ex.ExitTemplateDescription != null) {
                        if (ex.valueRoom == previousRoom) {
                            string destName = ex.valueRoom.roomName;
                            ex.exitDescription = $"[\u001b[96mback\u001b[0m] {ex.ExitTemplateDescription.Replace("{0}", destName)}";
                            ex.hasPreviousRoom = true;
                        } else {
                            string destName = ex.valueRoom.Visited ? ex.valueRoom.roomName : "UNKNOWN";
                            ex.exitDescription = $"[\u001b[96m{ex.keyString}\u001b[0m] {ex.ExitTemplateDescription.Replace("{0}", destName)}";
                        }
                    }
                }

                currentRoom.LoadRoom();
            }
        }

        public void ExploreDungeon() {
            Program.RoomController.currentDungeonInstance = DungeonGenerator.GenerateDungeon();
            Program.CurrentPlayer.TimesExplored++;
            ChangeRoom("", currentDungeonInstance.rooms[0]);
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
            RandomBasicCombatEncounter();
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
        public void RandomBasicCombatEncounter() {
            Console.Clear();
            Program.SoundController.Play("kamp");
            EnemyBase random = SpawnManager.SpawnEnemyInRoom(Program.CurrentPlayer, roomName);
            HUDTools.RoomHUD();
            HUDTools.ClearLastLine(1);
            switch (Program.Rand.Next(0, 2)) {
                case int x when (x == 0):
                    HUDTools.Print($"You turn a corner and there you see a {random.Name}...", 10);
                    break;
                case int x when (x == 1):
                    HUDTools.Print($"You break down a door and find a {random.Name} inside!", 10);
                    break;
            }
            TextInput.PressToContinue();
            new CombatController(Program.CurrentPlayer, random).Combat();
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