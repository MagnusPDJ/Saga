using Saga.Dungeon.Enemies;
using Saga.Assets;

namespace Saga.Dungeon.Rooms
{
    public abstract class RoomBase
    {
        public string Description { set; get; } = "";
        public string RoomName { set; get; } = "";
        public List<Exit> Exits { set; get; } = [];
        public int MaxExits { set; get; } = 3;
        public bool Visited { set; get; } = false;

        public EnemyBase? Enemy { set; get; } = null;
        public bool EnemySpawned { set; get; } = false;
        public bool Cleared { set; get; } = false;
        public string CorpseDescription { set; get; } = "";

        public abstract void LoadRoom();
        public virtual void IdleInRoom() {
            string exit = "";
            HUDTools.RoomHUD();
            while (exit == "") {
                exit = TextInput.PlayerPrompt("RoomActions");
            }
            Program.RoomController.ChangeRoom(exit);
        }
        public virtual void RandomCombatEncounter() {
            Program.SoundController.Play("kamp");
            Enemy = SpawnManager.SpawnEnemyInDungeon(Program.CurrentPlayer, Program.RoomController.CurrentDungeonInstance.DungeonName);
            EnemySpawned = true;
            HUDTools.RoomHUD();
            HUDTools.ClearLastLine(1);
            switch (Program.Rand.Next(0, 2)) {
                case int x when x == 0:
                    HUDTools.Print($"You turn a corner and there you see a {Enemy.Name}...", 10);
                    break;
                case int x when x == 1:
                    HUDTools.Print($"You break down a door and find a {Enemy.Name} inside!", 10);
                    break;
            }
            TextInput.PressToContinue();
            new CombatController(Program.CurrentPlayer, Enemy).Combat();
        }
    }
}