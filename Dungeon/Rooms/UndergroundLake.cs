
namespace Saga.Dungeon.Rooms
{
    public class UndergroundLake : RoomBase
    {
        public UndergroundLake() {
            RoomName = "Underground Lake";
            Description = " A lake so vast it vanishes into the darkness. Above, silkworm colonies glow with a faint blue\n light, resembling a starry sky. The water lies still and pitch-black; when you submerge your hand,\n it disappears completely beneath the surface. It could be unfathomably deep.";
        }
        public override void LoadRoom() {
            throw new NotImplementedException();
        }
    }
}
