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
                exit = TextInput.PlayerPrompt(true);
            }
            Program.RoomController.ChangeRoom(exit);
        }
    }
}