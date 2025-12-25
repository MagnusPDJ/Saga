using Saga.Dungeon.Enemies;
using Saga.Assets;
using Saga.Dungeon.Rooms.Room_Objects;

namespace Saga.Dungeon.Rooms
{
    public abstract class RoomBase
    {
        public string RoomName { set; get; } = "";
        public string Description { set; get; } = "";
        public string EntranceDescription { set; get; } = "";
        public List<Exit> Exits { set; get; } = [];
        public int MaxExits { set; get; } = 3;
        public bool Visited { set; get; } = false;
        public List<IInteractable> Interactables { set; get; } = [];
        public EnemyBase? Enemy { set; get; } = null;
        public bool EnemySpawned { set; get; } = false;
        public bool Cleared { set; get; } = false;
        public string CorpseDescription { set; get; } = "";

        public abstract void LoadRoom();
        public virtual void IdleInRoom() {
            string exit = "";
            HUDTools.RoomHUD();
            while (exit == "") {
                exit = TextInput.SelectPlayerAction(0);
            }
            Program.RoomController.ChangeRoom(exit);
        }
        public virtual void RandomCombatEncounter() {
            Program.SoundController.Play("kamp");
            Enemy = SpawnManager.SpawnEnemyInDungeon(Program.CurrentPlayer, Program.RoomController.CurrentDungeonInstance.DungeonName);
            EnemySpawned = true;
            switch (Program.Rand.Next(0, 2)) {
                case int x when x == 0:
                    EntranceDescription = $" You turn a corner and there you see a {Enemy.Name}...";
                    break;
                case int x when x == 1:
                    EntranceDescription = $" You break down a door and find a {Enemy.Name} inside!";
                    break;
            }
            HUDTools.RoomHUD(true);
            HUDTools.ClearLastLine(1);
            TextInput.PressToContinue();
            new CombatController(Program.CurrentPlayer, Enemy).Combat();
        }
    }
}