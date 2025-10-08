
using Saga.Assets;

namespace Saga.Dungeon.Rooms
{
    public class BlacksmithRoom : RoomBase
    {
        public BlacksmithRoom() {
            RoomName = "Mine blacksmith";
            Description = " There are a forge with various sized anvils, hammers and tongs. Here tools could be repaired and\n new hooks, pitons and fittings crafted used during mining.";
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
