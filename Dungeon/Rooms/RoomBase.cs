using Saga.Assets;
using Saga.Dungeon.Enemies;

namespace Saga.Dungeon.Rooms
{
    public abstract class RoomBase
    {
        public string Description { set; get; } = "";
        public string RoomName { set; get; } = "";
        public List<Exit> Exits { set; get; } = [];
        public bool Visited { set; get; } = false;

        public EnemyBase? Enemy { set; get; } = null;
        public bool EnemySpawned { set; get; } = false;
        public bool Cleared { set; get; } = false;
        public string CorpseDescription { set; get; } = "";

        public abstract void LoadRoom();
    }

    //Alle custom rum:

    //Rum til Puzzle One Encounter.
    public class HallwayRoom : RoomBase
    {
        public HallwayRoom(RoomBase[] rooms, int i) {
            RoomName = "Hallway";
            Description = "Behind you are the runes you crossed.";
            Exits = [new Exit() { keyString = "deeper", exitDescription = $"The hallway continues \u001b[96mdeeper\u001b[0m into the dark", valueRoom = rooms[i - 1] }];
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
    //Rum til et tilfældigt encounter.
    public class RandomEncounterRoom : RoomBase 
    {
        public override void LoadRoom() {
            string exit = "";
            Encounters.RandomEncounter();
            if (Program.RoomController.Ran == true) {
                Program.RoomController.Ran = false;
                Program.RoomController.ChangeRoom(Exits[0].keyString);
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