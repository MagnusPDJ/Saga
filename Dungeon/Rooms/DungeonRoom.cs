
namespace Saga.Dungeon.Rooms
{
    public class DungeonRoom : RoomBase
    {
        private const double EnemySpawnChance = 0.75;
        private const double ChestSpawnChance = 0.3;
        public DungeonRoom(string name, string desc) {
            RoomName = name;
            Description = desc;
        }

        public override void InitializeRoom() {
            SpawnEnemy(EnemySpawnChance);
            SpawnInteractable("chest", ChestSpawnChance);
        }

        public override void LoadRoom() {
            if (!Visited) {
                Visited = true;
                InitializeRoom();
            }
            
            if (Enemy != null && !Cleared) {
                StartCombatEncounter(Enemy);
            }

            IdleInRoom();
        }
    }
}
