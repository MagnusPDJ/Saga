using Saga.Assets;
using Saga.Dungeon.Enemies;

namespace Saga.Dungeon.Rooms
{
    public class SecondRoom : RoomBase
    {
        public SecondRoom() {
            roomName = "Hallway";
            description = "";
            exits = [new Exit() { keyString = "1", ExitTemplateDescription = "The hallway continues deeper into the {0}.", valueRoom = RoomController.Camp }];
        }
        public override void LoadRoom() {
            string exit = "";
            EnemySpawned = true;
            SecondEncounter();
            if (Program.RoomController.Ran == true) {
                Program.RoomController.Ran = false;
                Program.RoomController.ChangeRoom(exits[0].keyString);
            } else {
                Cleared = true;
                corpseDescription = enemy!.EnemyCorpseDescription;
                enemy = null;
                HUDTools.RoomHUD();
                while (exit == "") {
                    exit = TextInput.PlayerPrompt(true);
                }
                Program.RoomController.ChangeRoom(exit);
            }
        }
        public void SecondEncounter() {
            Console.Clear();
            Program.SoundController.Play("kamp");
            HUDTools.RoomHUD();
            HUDTools.ClearLastLine(1);
            HUDTools.Print($" The big door creaks and you continue down the gloomy hallway. You Spot a pair of red glowing eyes\n in the darkness, but before you could react the beastly dog engages you.");
            TextInput.PressToContinue();

            enemy = EnemyFactory.CreateByName("Feral dog");          
            new CombatController(Program.CurrentPlayer, enemy).Combat();
        }

    }
}
