using Saga.Assets;
using Saga.Dungeon.Enemies;

namespace Saga.Dungeon.Rooms
{
    public abstract class RoomBase
    {
        public string description = "";
        public string roomName = "";
        public List<Exit> exits = [];
        public bool Visited = false;

        public EnemyBase? enemy = null;
        public bool EnemySpawned = false;
        public bool Cleared = false;
        public string corpseDescription = "";

        public abstract void LoadRoom();
    }

    //Alle custom rum:

    //Rum til Puzzle One Encounter.
    public class HallwayRoom : RoomBase
    {
        public HallwayRoom(RoomBase[] rooms, int i) {
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
    public class RandomBasicCombatRoom : RoomBase 
    {
        public override void LoadRoom() {
            string exit = "";
            // RandomBasicCombatEncounter();
            if (Program.RoomController.Ran == true) {
                Program.RoomController.Ran = false;
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
    public class RandomEncounterRoom : RoomBase 
    {
        public override void LoadRoom() {
            string exit = "";
            Encounters.RandomEncounter();
            if (Program.RoomController.Ran == true) {
                Program.RoomController.Ran = false;
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
    public class MeetFlemshaRoom : RoomBase
    {
        public MeetFlemshaRoom() {
            roomName = "Old jail cells";
            description = "You look around the old jail. There is nothing of value. Flemsha seems to want you to leave first.";
            exits = [new Exit() { keyString = "home", exitDescription = $"This room is a dead end. You should \u001b[96mgo home\u001b[0m to your camp.", valueRoom = RoomController.Camp }];
        }
        public override void LoadRoom() {
            string exit = "";
            Encounters.MeetFlemsha();
            if (Program.RoomController.Ran == true) {
                Program.RoomController.Ran = false;
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
}