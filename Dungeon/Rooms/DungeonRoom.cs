using Saga.Assets;
using Saga.Dungeon.Enemies;

namespace Saga.Dungeon.Rooms
{
    public class DungeonRoom : RoomBase
    {
        private const double EnemySpawnChance = 0.75;
        public DungeonRoom(string name, string desc) {
            RoomName = name;
            Description = desc;
        }

        public override void LoadRoom() {
            if (!Visited) Visited = true;

            if (!EnemySpawned && !Cleared) {
                double rollForEncounter = Program.Rand.NextDouble();
                if (rollForEncounter < EnemySpawnChance && Enemy == null) {
                    RandomCombatEncounter();
                    if (Program.RoomController.Ran == true) {
                        Program.RoomController.Ran = false;
                        EntranceDescription = $" You return to the room where you left the {Enemy!.Name}...";
                        Program.RoomController.ChangeRoom(Exits[0].keyString);
                    } else {
                        Cleared = true;
                        CorpseDescription = Enemy!.EnemyCorpseDescription;
                        Enemy = null;
                    }
                } else {
                    Cleared = true;
                }
            } else if (Enemy != null) {
                HUDTools.RoomHUD(true);
                HUDTools.ClearLastLine(1);
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

            IdleInRoom();
        }
    }
}
