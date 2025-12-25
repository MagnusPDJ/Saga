using Saga.Assets;
using Saga.Dungeon.People;
using Saga.Dungeon.Quests;

namespace Saga.Dungeon.Rooms.Room_Objects
{
    public class PrisonCells : ISearchable
    {
        public string Name => "prison cells";
        public string LookDescription => " There are several \u001b[96mprison cells\u001b[0m you could search through.";
        public bool Searched { get; set; } = false;
        public void Search()
        {
            if (!Searched) {
                (int, int) startCursorPosition = Console.GetCursorPosition();
                HUDTools.Print(
                    " You search the prison cells and in one of them, you find a man laying on the stone floor rambling\n" +
                    " to himself. As you approach the iron grate he comes to his senses,\n" +
                    " 'You must help me get out!' he exclaims, 'My name is Flemsha, I'm an alchemist, I can be of help'",
                    20);
                HUDTools.Print("\n Do you want to help the man? (Y/N)", 10);
                while (true) {
                    string input = TextInput.UserKeyInput();
                    if (input == "y") {
                        Act1Quest.AddQuest("Free Flemsha");
                        Quest.UpdateQuestLog(Program.CurrentPlayer);
                        break;
                    } else if (input == "n") {
                        HUDTools.Print(" A locked up prisoner doesn't seem that useful to you. So you decide to leave him behind");
                        int a = Array.FindIndex(Program.CurrentPlayer.Inventory, x => x != null && x.ItemId == "oldkey");
                        if (a != -1) {
                            Program.CurrentPlayer.Inventory.SetValue(null, a);
                        }
                        Act1Quest.FailQuest("Free Flemsha");
                        NonPlayableCharacters.UpdateDialogueOptions("Deadflemsha");
                        break;
                    } else {
                        HUDTools.Print("Invalid input.", 5);
                        TextInput.PressToContinue();
                        HUDTools.ClearLastLine(3);
                    }
                }
                Searched = true;
                TextInput.PressToContinue();
                HUDTools.ClearLastText((startCursorPosition.Item1, startCursorPosition.Item2 - 1));
            } else {
                HUDTools.Print($" You have already searched the {Name}.", 20);
                TextInput.PressToContinue();
                HUDTools.ClearLastLine(3);
            }
        }
    }
}
