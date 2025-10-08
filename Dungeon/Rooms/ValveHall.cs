
using Saga.Assets;

namespace Saga.Dungeon.Rooms
{
    public class ValveHall : RoomBase
    {
        public ValveHall() {
            RoomName = "Valve hall";
            Description = " Massive wheels and rusted levers line the walls, probably used to control the waterflows in the\n sewer. The air tastes of old metal and oil, and when the silence breaks, it's inly from the slow\n drip of water echoing through the gears of forgotten machinery.";
            MaxExits = 1;
        }
        public override void LoadRoom() {
            string exit = "";
            HUDTools.RoomHUD();
            while (exit == "") {
                exit = TextInput.PlayerPrompt(true);
            }
            Program.RoomController.ChangeRoom(exit);
        }
    }
}
