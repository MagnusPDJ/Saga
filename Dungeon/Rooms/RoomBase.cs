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

        public virtual void InitializeRoom() { }
        public abstract void LoadRoom();

        public virtual void IdleInRoom() {
            string exit = "";
            HUDTools.RoomHUD();
            while (exit == "") {
                exit = TextInput.SelectPlayerAction(0);
            }
            Program.RoomController.ChangeRoom(exit);
        }

        public virtual void SpawnEnemy(double enemySpawnChance) {
            double rollForEncounter = Program.Rand.NextDouble();
            if (rollForEncounter < enemySpawnChance) {
                Enemy = SpawnManager.SpawnEnemyInDungeon(Program.CurrentPlayer, Program.RoomController.CurrentDungeonInstance.DungeonName);
                switch (Program.Rand.Next(0, 2)) {
                    case int x when x == 0:
                        EntranceDescription = $" You turn a corner and there you see a {Enemy.Name}...";
                        break;
                    case int x when x == 1:
                        EntranceDescription = $" You break down a door and find a {Enemy.Name} inside!";
                        break;
                }
            }
        }

        public virtual void SpawnInteractable(string containerToSpawn, double spawnChance) {
            double rollForContainer = Program.Rand.NextDouble();
            if (rollForContainer < spawnChance) {
                ILootable? container = ContainerDatabase.GetByLootableId(containerToSpawn);
                if (container is IInteractable interactable)
                Interactables.Add(interactable);
            }
        }

        public virtual void StartCombatEncounter(EnemyBase enemy) {
            Program.SoundController.Play("kamp");
            HUDTools.RoomHUD(true);
            HUDTools.ClearLastLine(1);
            TextInput.PressToContinue();
            new CombatController(Program.CurrentPlayer, enemy).Combat();
            if (Program.RoomController.Ran == true) {
                Program.RoomController.Ran = false;
                EntranceDescription = $" You return to the room where you left the {enemy.Name}...";
                Program.RoomController.ChangeRoom(Exits[0].keyString);
            } else {
                Cleared = true;
                CorpseDescription = enemy.EnemyCorpseDescription;
                Enemy = null;
            }
        }
    }
}