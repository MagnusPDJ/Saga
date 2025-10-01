
using Saga.Assets;
using Saga.Dungeon.Rooms;

namespace Saga.Dungeon
{
    public static class DungeonGenerator
    {
        public static DungeonInstance GenerateDungeon(int minRooms = 6, int maxRooms = 10)
        {
            // Pick the dungeon:
            List<string> dungeonNames = [.. DungeonDatabase.GetDungeons().Keys];
            string dungeonName = dungeonNames[Program.Rand.Next(dungeonNames.Count)];
            
            // Pick number of rooms
            int roomCount = Program.Rand.Next(minRooms, maxRooms + 1);

            // Give each room a name and description:
            var roomNamesAndDesc = DungeonDatabase.GetDungeons()[dungeonName];
            var rooms = new List<RoomBase>(roomCount);
            for (int i = 0; i < roomCount; i++)
            {
                int index = Program.Rand.Next(roomNamesAndDesc.Length);
                rooms.Add(new DungeonRoom(roomNamesAndDesc[index][0], roomNamesAndDesc[index][1]));
            }

            // Spanning tree: connect each new node to a random connected node, respecting max 3 exits
            var connected = new HashSet<int> { 0 };
            var remaining = Enumerable.Range(1, roomCount - 1).ToList();

            while (remaining.Count > 0)
            {
                int idx = remaining[Program.Rand.Next(remaining.Count)];
                var candidates = connected.Where(c => rooms[c].exits.Count < 3).ToList();
                if (candidates.Count == 0) break;
                int connectTo = candidates[Program.Rand.Next(candidates.Count)];
                if (rooms[idx].exits.Count < 3)
                {
                    ConnectRoomsBidirectional(rooms[idx], rooms[connectTo], dungeonName);
                    connected.Add(idx);
                    remaining.Remove(idx);
                }
                else
                {
                    remaining.Remove(idx);
                }
            }

            // Add one-way exits between rooms with less than 3 exits
            int countToAdd = Program.Rand.Next(3);
            int numberAdded = 0;
                for (int a = 0; a < rooms.Count; a++) {
                    if (countToAdd == numberAdded) break;
                    if (rooms[a].exits.Count >= 3) continue;
                    for (int b = 0; b < rooms.Count; b++) {
                        if (a == b) continue;
                        if (rooms[b].exits.Count >= 3) continue;
                        // Skip if already connected in either direction
                        if (rooms[a].exits.Any(x => x.valueRoom == rooms[b])) continue;
                        if (rooms[b].exits.Any(x => x.valueRoom == rooms[a])) continue;

                        // Add a one-way exit from a to b
                        ConnectRoomsOneDirectional(rooms[a], rooms[b], dungeonName);
                        numberAdded++;
                        // breaking so it only adds one per room
                        if (rooms[a].exits.Count >= 3) break;                        
                    }
                }           
            // Add "[camp]" exit to the first room (always visible, not numbered)
            rooms[0].exits.Add(new Exit
            {
                keyString = "camp",
                exitDescription = "[camp] A passage leads back to camp.",
                valueRoom = RoomController.Camp,
                ExitTemplateDescription = "A passage leads back to camp."
            });

            // Assign numeric keys and format exit descriptions (skip camp exit)
            foreach (var room in rooms)
            {
                int idx = 1;
                foreach (var exit in room.exits)
                {
                    if (exit.keyString != "camp")
                    {
                        exit.keyString = idx.ToString();
                        exit.exitDescription = $"[{exit.keyString}] {exit.ExitTemplateDescription.Replace("{0}", exit.valueRoom.roomName)}";
                        idx++;
                    }
                }
            }

            var dungeon = new DungeonInstance
            {
                DungeonName = dungeonName,
                Rooms = rooms
            };

            return dungeon;
        }

        private static void ConnectRoomsBidirectional(RoomBase a, RoomBase b, string dungeonName)
        {
            var exits = DungeonDatabase.GetExits()[dungeonName];
            string exitA = exits[0][Program.Rand.Next(exits[0].Count)];
            string exitB = exits[0][Program.Rand.Next(exits[0].Count)];
            
            a.exits.Add(new Exit() { valueRoom = b, ExitTemplateDescription = exitA });
        
            string backExitTemplate = exitB;
            b.exits.Add(new Exit() { valueRoom = a, ExitTemplateDescription = backExitTemplate });
        }
        private static void ConnectRoomsOneDirectional(RoomBase a, RoomBase b, string dungeonName) {
            var exits = DungeonDatabase.GetExits()[dungeonName];
            string exitTemplate = exits[1][Program.Rand.Next(exits[1].Count)];
            a.exits.Add(new Exit { valueRoom = b, ExitTemplateDescription = exitTemplate,
                IsOneWay = true
            });
        }
    }
}