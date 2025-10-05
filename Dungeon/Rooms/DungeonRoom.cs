using Saga.Assets;
using Saga.Dungeon.Enemies;

namespace Saga.Dungeon.Rooms
{
    public class DungeonRoom : RoomBase
    {
        private const double DefaultSpawnChance = 0.75;
        public DungeonRoom(string name, string desc) {
            roomName = name;
            description = desc;
            exits = [];
        }

        public override void LoadRoom() {
            if (!Visited) Visited = true;

            //Spawn Enemy
            if (!EnemySpawned && !Cleared) {
                EnemySpawned = true;
                if (Program.Rand.NextDouble() < DefaultSpawnChance && enemy == null) {
                    RandomBasicCombatEncounter();
                    if (Program.RoomController.Ran == true) {
                        Program.RoomController.Ran = false;
                        Program.RoomController.ChangeRoom(exits[0].keyString);
                    } else {
                        Cleared = true;
                        corpseDescription = enemy!.EnemyCorpseDescription;
                        enemy = null;
                    }
                } else if (enemy != null) {
                    HUDTools.RoomHUD();
                    HUDTools.ClearLastLine(1);
                    HUDTools.Print($"You return to the room where you left the {enemy.Name}...", 10);
                    TextInput.PressToContinue();
                    new CombatController(Program.CurrentPlayer, enemy).Combat();
                    if (Program.RoomController.Ran == true) {
                        Program.RoomController.Ran = false;
                        Program.RoomController.ChangeRoom(exits[0].keyString);
                    } else {
                        Cleared = true;
                        corpseDescription = enemy!.EnemyCorpseDescription;
                        enemy = null;
                    }
                }
            }

            string exit = "";
            HUDTools.RoomHUD();
            while (exit == "") {
                exit = TextInput.PlayerPrompt(true);
            }
            Program.RoomController.ChangeRoom(exit);
        }
        public void RandomBasicCombatEncounter() {
            Program.SoundController.Play("kamp");
            enemy = SpawnManager.SpawnEnemyInDungeon(Program.CurrentPlayer, Program.RoomController.CurrentDungeonInstance.DungeonName);
            HUDTools.RoomHUD();
            HUDTools.ClearLastLine(1);
            switch (Program.Rand.Next(0, 2)) {
                case int x when x == 0:
                    HUDTools.Print($"You turn a corner and there you see a {enemy.Name}...", 10);
                    break;
                case int x when x == 1:
                    HUDTools.Print($"You break down a door and find a {enemy.Name} inside!", 10);
                    break;
            }
            TextInput.PressToContinue();
            new CombatController(Program.CurrentPlayer, enemy).Combat();
        }
    }
}
