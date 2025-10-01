using Saga.Assets;
using Saga.Dungeon.Enemies;

namespace Saga.Dungeon.Rooms
{
    public class SecondRoom : RoomBase
    {
        public SecondRoom() {
            roomName = "Hallway";
            description = "";
            exits = [new Exit() { keyString = "deeper", exitDescription = $"The hallway continues \u001b[96mdeeper\u001b[0m into the dark", valueRoom = RoomController.Camp }];
        }
        public override void LoadRoom() {
            string exit = "";
            SecondEncounter();
            if (Program.RoomController.ran == true) {
                Program.RoomController.ran = false;
                Program.RoomController.ChangeRoom(exits[0].keyString);
            } else {
                HUDTools.RoomHUD();
                while (exit == "") {
                    exit = TextInput.PlayerPrompt(true);
                }
                Program.RoomController.ChangeRoom(exit);
            }
        }
        public static void SecondEncounter() {
            Console.Clear();
            Program.SoundController.Play("kamp");
            HUDTools.RoomHUD();
            HUDTools.ClearLastLine(1);
            HUDTools.Print($"The big door creaks and you continue down the gloomy hallway. You Spot a pair of red glowing eyes\nin the darkness, but before you could react the beastly dog engages you.");
            TextInput.PressToContinue();


            EnemyBase dog = EnemyFactory.CreateByName("Feral dog");
            new CombatController(Program.CurrentPlayer, dog).Combat();
        }

    }
}
