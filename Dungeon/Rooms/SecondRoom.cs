using Saga.Assets;
using Saga.Dungeon.Enemies;
using Saga.Dungeon.Rooms.Room_Objects;

namespace Saga.Dungeon.Rooms
{
    public class SecondRoom : RoomBase
    {
        public SecondRoom() {
            RoomName = "Hallway";
            Description = " Nothing noteworthy.";
            EntranceDescription = " The big door creaks and you continue down the gloomy hallway. You Spot a pair of red glowing eyes\n" +
                                  " in the darkness, but before you could react the beastly dog engages you.";
            Exits = [new Exit() { keyString = "1", ExitTemplateDescription = "The hallway continues deeper into the {0}.", valueRoom = RoomController.Camp }];
            Interactables = [new StoneSlab()];
        }
        public override void LoadRoom() {
            if (!Visited) Visited = true;
                  
            SecondEncounter();
            if (Program.RoomController.Ran == true) {
                Program.RoomController.Ran = false;
                EntranceDescription = $" You return to the room where you left the {Enemy!.Name}...";
                Program.RoomController.ChangeRoom(Exits[0].keyString);
            } else {
                Cleared = true;
                CorpseDescription = Enemy!.EnemyCorpseDescription;
                Enemy = null;
            }

            IdleInRoom();
        }
        public void SecondEncounter() {
            Program.SoundController.Play("kamp");
            HUDTools.RoomHUD(true);
            HUDTools.ClearLastLine(1);
            TextInput.PressToContinue();

            Enemy = EnemyFactory.CreateByName("Feral dog");
            EnemySpawned = true;
            new CombatController(Program.CurrentPlayer, Enemy).Combat();
        }

    }
}
