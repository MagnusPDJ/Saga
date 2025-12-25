using Saga.Assets;
using Saga.Dungeon.People;
using Saga.Dungeon.Quests;
using Saga.Dungeon.Rooms.Room_Objects;

namespace Saga.Dungeon.Rooms
{
    public class OldJailCells : RoomBase
    {
        public OldJailCells() {
            RoomName = "Old jail cells";
            EntranceDescription = " You enter a dimly lit room, stone slab walls and iron bar grates make up some prison cells on\n" +
                                  " either side of the room, there is also a desk which probably belonged to the long gone warden.";
            Description = " You look around the old jail.";
            MaxExits = 1;
            Interactables = [new PrisonCells(), new Desk()];
        }
        public override void LoadRoom() {
            if (!Visited) Visited = true;

            if (!Cleared) {
                MeetFlemsha();
                if (Program.RoomController.Ran == true) {
                    Program.RoomController.Ran = false;
                    Program.RoomController.ChangeRoom(Exits[0].keyString);
                }
            }

            IdleInRoom();
        }
        public void MeetFlemsha() {
            Program.SoundController.Play("typewriter");
            HUDTools.RoomHUD(true);
            while (true) {
                
                string input = TextInput.SelectPlayerAction(0);           
                
                if (Program.CurrentPlayer.FailedQuests.Exists(q => q.Name == "Free Flemsha")) {
                    HUDTools.Print(" You close the door to the prison ward and continue on, never to see the prisoner again.");
                    TextInput.PressToContinue();
                    Description = " You look around the old jail. There is nothing of value. The rambling man has vanished without a\n trace.";
                    Program.RoomController.Ran = true;
                    break;
                }

                if (Program.CurrentPlayer.QuestLog.Exists(quest => quest.Name == "Free Flemsha" && quest.Completed == true)) {
                    HUDTools.RoomHUD();
                    HUDTools.ClearLastLine(1);
                    HUDTools.Print(" You return to Flemsha and try the key. With some resistance you turn the mechanism and\n" +
                                   " the door slides open.\n" +
                                   " He thanks you very much and you tell him about your camp, where Gheed is too.", 20);
                    var quest = Program.CurrentPlayer.QuestLog.Find(quest => quest.Name == "Free Flemsha");
                    if (quest != null) {
                        Quest.TurnInQuest(Program.CurrentPlayer, quest);
                    }
                    NonPlayableCharacters.AddNpcToCamp("Flemsha");
                    Description = " You look around the old jail. There is nothing of value. Flemsha seems to want you to leave first.";
                    TextInput.PressToContinue();
                    HUDTools.ClearLastLine(8);                 
                    break;
                }
            }
            Cleared = true;
        }
        public override void IdleInRoom() {
            string exit = "";
            HUDTools.RoomHUD();
            while (exit == "") {
                exit = TextInput.SelectPlayerAction(0);
            }
            Description = " You look around the old jail. There is nothing of value.";
            Program.RoomController.ChangeRoom(exit);
        }
    }
}
