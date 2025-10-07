
namespace Saga.Dungeon.Rooms
{
    public class BlacksmithRoom : RoomBase
    {
        public BlacksmithRoom() {
            RoomName = "Blacksmith Room";
            Description = " There are a forge with various sized anvils, hammers and tongs. Here tools could be repaired and\n new hooks, pitons and fittings crafted used during mining.";
            MaxExits = 1;
        }
        public override void LoadRoom() {
            throw new NotImplementedException();
        }
    }
}
