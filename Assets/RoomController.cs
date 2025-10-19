using Saga.Dungeon;
using Saga.Dungeon.Rooms;

namespace Saga.Assets
{
    public class RoomController
    {
        public static readonly CampRoom Camp = new();
        public static readonly StartRoom StartRoom = new();

        public InputAction[] InputRoomActions = [
            new Go("go"),
            new Use("use"),
            new Look("look"),
            new DrinkHealingPotion("drink", "d"),
            new SeeCharacterScreen("character", "c"),
            new SeeInventory("inventory", "i"),
            new SeeQuestLog("questlog", "l"),
            new SeeSkillTree("skilltree", "k")
        ];
        public InputAction[] InputInvActions = [
            new Examine("examine"),
            new Equip("equip"),
            new UnEquip("unequip"),
            new Back("back", "b"),
        ];
        public RoomBase CurrentRoom { get; set; } = Camp;
        public DungeonInstance CurrentDungeonInstance { get; set; } = new();
        public bool Ran { get; set; } = false;
        

        public void ChangeRoom(string keystring, RoomBase? room = null) {
            bool foundRoom = false;
            RoomBase previousRoom = new DungeonRoom("", "");
            if (room != null) {
                CurrentRoom = room;
                foundRoom = true;
            }
            if (keystring == "home") {
                CurrentRoom = Camp;
                foundRoom = true;
            }
            if (!foundRoom) {
                previousRoom = CurrentRoom;
                foreach (Exit exit in CurrentRoom.Exits) {
                    if (exit.keyString == keystring) {
                        CurrentRoom = exit.valueRoom;
                        foundRoom = true;
                        break;
                    }
                }
            }
            if (foundRoom) {
                Program.CurrentPlayer.RegainMana();

                //Update exit descriptions for all exits in current room
                foreach (var ex in CurrentRoom.Exits) {
                    if (ex.valueRoom != null && ex.ExitTemplateDescription != null) {
                        if (ex.valueRoom == previousRoom) {
                            string destName = ex.valueRoom.RoomName;
                            ex.exitDescription = $"[\u001b[96mback\u001b[0m] {ex.ExitTemplateDescription.Replace("{0}", destName)}";
                            ex.hasPreviousRoom = true;
                        } else {
                            string destName = ex.valueRoom.Visited ? ex.valueRoom.RoomName : "UNKNOWN";
                            ex.exitDescription = $"[\u001b[96m{ex.keyString}\u001b[0m] {ex.ExitTemplateDescription.Replace("{0}", destName)}";
                        }
                    }
                }

                CurrentRoom.LoadRoom();
            }
        }

        public void ExploreDungeon() {
            Program.RoomController.CurrentDungeonInstance = DungeonGenerator.GenerateDungeon();
            Program.CurrentPlayer.TimesExplored++;
            ChangeRoom("", CurrentDungeonInstance.Rooms[0]);
        }
    }
}
