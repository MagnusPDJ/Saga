using Saga.Assets;
using Saga.Dungeon.Quests;
using Saga.Items.Loot;

namespace Saga.Dungeon.Rooms
{
    public class OldJailCells : RoomBase
    {
        public OldJailCells() {
            roomName = "Old jail cells";
            description = " You look around the old jail. There is nothing of value. Flemsha seems to want you to leave first.";
        }
        public override void LoadRoom() {
            string exit = "";
            if (!Cleared) {
                MeetFlemsha();
            }
            
            if (Program.RoomController.Ran == true) {
                Program.RoomController.Ran = false;
                Program.RoomController.ChangeRoom(exits[0].keyString);
            } else {
                HUDTools.RoomHUD();
                while (exit == "") {
                    exit = TextInput.PlayerPrompt(true);
                }
                description = " You look around the old jail. There is nothing of value.";
                Program.RoomController.ChangeRoom(exit);
            }
        }
        public void MeetFlemsha() {
            Console.Clear();
            Program.SoundController.Play("typewriter");
            HUDTools.RoomHUD();
            HUDTools.ClearLastLine(1);
            HUDTools.Print(" You enter a dimly lit room, stone slab walls and iron bar grates make up some cells on either side\n of the room, there is also a desk which probably belonged to the long gone warden.", 30);
            bool examined = false;
            bool searched = false;
            bool leftForDead = false;
            while (true) {
                while (!examined || !searched) {
                    Console.Clear();
                    HUDTools.RoomHUD();
                    HUDTools.ClearLastLine(1);
                    HUDTools.Print(" You enter a dimly lit room, stone slab walls and iron bar grates make up some cells on either side\n of the room, there is also a desk which probably belonged to the long gone warden.", 0);
                    HUDTools.Print($"\n Do you {(!examined ? "(1)examine desk? " : "")}{(!examined && !searched ? "Or " : "")}{(!searched ? "(2) search the prison cells?" : "")}", 20);
                    string input = TextInput.PlayerPrompt();
                    if (input == "1" && !examined) {
                        examined = true;
                        HUDTools.Print(" You rummage through dusty documents and moldy records illegible or in unknown languages,\n but in a drawer you find some gold and a key.", 20);
                        LootSystem.GetQuestLoot(1, 0, "MeetFlemsha");
                        TextInput.PressToContinue();
                        break;
                    } else if (input == "2" && !searched) {
                        HUDTools.Print(
                            " You search the prison cells and in one of them, you find a man laying on the stone floor rambling\n" +
                            " to himself. As you approach the iron grate he comes to his senses,\n" +
                            " 'You must help me get out!' he exclaims, 'My name is Flemsha, I'm an alchemist, I can be of help'",
                            20);
                        HUDTools.Print("\n Do you want to help the man? (Y/N)", 10);
                        while (true) {
                            input = TextInput.PlayerPrompt();
                            if (input == "y") {
                                Act1Quest.AddQuest("Free Flemsha");
                                Program.CurrentPlayer.UpdateQuestLog();
                                break;
                            } else if (input == "n") {
                                HUDTools.Print(" A locked up prisoner doesn't seem that useful to you. So you decide to leave him behind");
                                int a = Array.FindIndex(Program.CurrentPlayer.Inventory, x => x != null && x.ItemId == "oldkey");
                                if (a != -1) {
                                    Program.CurrentPlayer.Inventory.SetValue(null, a);
                                }
                                Act1Quest.FailQuest("Free Flemsha");
                                NonPlayableCharacters.UpdateDialogueOptions("Deadflemsha");
                                leftForDead = true;
                                break;
                            } else {
                                HUDTools.Print("Invalid input.", 5);
                                TextInput.PressToContinue();
                                HUDTools.ClearLastLine(3);
                            }
                        }
                        searched = true;
                        TextInput.PressToContinue();
                        break;
                    } else {
                        Program.CurrentPlayer.BasicActions(input);
                    }
                }
                if (leftForDead) {
                    HUDTools.ClearLastLine(1);
                    HUDTools.Print(" You close the door to the prison ward and continue on, never to see the prisoner again.");
                    TextInput.PressToContinue();
                    description = " You look around the old jail. There is nothing of value. The rambling man has vanished without a\n trace.";
                    Program.RoomController.Ran = true;
                    Cleared = true;
                    break;
                }

                if (Program.CurrentPlayer.QuestLog.Exists(quest => quest.Name == "Free Flemsha" && quest.Completed == true)) {
                    Console.Clear();
                    HUDTools.RoomHUD();
                    HUDTools.Print(" You return to Flemsha and try the key. With some resistance you turn the mechanism and\n the door slides open.", 20);
                    HUDTools.Print(" He thanks you very much and you tell him about your camp, where Gheed is too.", 20);
                    var quest = Program.CurrentPlayer.QuestLog.Find(quest => quest.Name == "Free Flemsha");
                    if (quest != null) {
                        Program.CurrentPlayer.CompleteAndTurnInQuest(quest);
                    }
                    NonPlayableCharacters.AddNpcToCamp("Flemsha");
                    TextInput.PressToContinue();
                    HUDTools.ClearLastLine(8);
                    Cleared = true;
                    break;
                }
            }
        }
    }
}
