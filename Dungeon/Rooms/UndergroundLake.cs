
using Saga.Assets;

namespace Saga.Dungeon.Rooms
{
    public class UndergroundLake : RoomBase
    {
        public UndergroundLake() {
            RoomName = "Underground lake";
            Description = " A lake so vast it vanishes into the darkness. Above, silkworm colonies glow with a faint blue\n light, resembling a starry sky. The water lies still and pitch-black; when you submerge your hand,\n it disappears completely beneath the surface. It could be unfathomably deep.";
            MaxExits = 1;
        }
        public override void LoadRoom() {
            if (!Visited) Visited = true;

            IdleInRoom();
        }
    }
}
