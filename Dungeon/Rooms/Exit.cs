using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Saga.Dungeon.Rooms
{
    public class Exit : IExit
    {
        public string keyString = "";
        public string exitDescription = "";
        public required RoomBase valueRoom;
        public bool hasPreviousRoom = false;
        public bool IsOneWay = false;
        public string ExitTemplateDescription { get; set; } = "";
    }
}
