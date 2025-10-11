using Saga.Assets;
using Saga.Dungeon.Enemies;

namespace Saga.Dungeon.Rooms
{
    public class WizardRoom : RoomBase
    {
        public WizardRoom() {
            RoomName = "Black library";
            Description = " There are vast amount of black tomes on tall shelves. Every book is written in pictograms and\n scribbles sharing no secrets. The whole room is emitting a fell aura.";
            MaxExits = 1;
        }
        public override void LoadRoom() {
            if (!Visited) Visited = true;

            if (!EnemySpawned && !Cleared) {
                EnemySpawned = true;
                if (Enemy == null) {
                    WizardEncounter();
                    if (Program.RoomController.Ran == true) {
                        Program.RoomController.Ran = false;
                        Program.RoomController.ChangeRoom(Exits[0].keyString);
                    } else {
                        Cleared = true;
                        CorpseDescription = Enemy!.EnemyCorpseDescription;
                        Enemy = null;
                    }
                } else if (Enemy != null) {
                    HUDTools.RoomHUD();
                    HUDTools.ClearLastLine(1);
                    HUDTools.Print($" You return to the room where you left the {Enemy.Name}...", 10);
                    TextInput.PressToContinue();
                    new CombatController(Program.CurrentPlayer, Enemy).Combat();
                    if (Program.RoomController.Ran == true) {
                        Program.RoomController.Ran = false;
                        Program.RoomController.ChangeRoom(Exits[0].keyString);
                    } else {
                        Cleared = true;
                        CorpseDescription = Enemy!.EnemyCorpseDescription;
                        Enemy = null;
                    }
                }
            }

            IdleInRoom();
        }
        public void WizardEncounter() {
            Console.Clear();
            Program.SoundController.Play("laugh");
            HUDTools.RoomHUD();
            HUDTools.Print(" The door slowly creaks open as you peer into the dark room. You see a tall man with a", 20);
            Program.SoundController.Play("troldmandskamp");
            HUDTools.Print(" long beard and pointy hat, looking at a large tome.", 20);
            TextInput.PressToContinue();

            Enemy = EnemyFactory.CreateByName("Dark Wizard");
            new CombatController(Program.CurrentPlayer, Enemy).Combat();
        }
    }
}
