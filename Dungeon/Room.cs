using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Saga.Dungeon
{
    public class Exit
    {
        public string keyString;
        public string exitDescription;
        public Room valueRoom;
    }
    public class Room
    {
        public string description;
        public string roomName;
        public Exit[] exits;
    }
}
