
namespace Saga.Assets
{
    class TextInput
    {
        public static InputAction[] InputRoomActions = [
            new Go("go"),
            new Use("use"),
            new Look("look"),
            new ExamineObject("examine"),
            new Search("search"),
            new DrinkHealingPotion("drink", "d"),
            new SeeCharacterScreen("character", "c"),
            new SeeInventory("inventory", "i"),
            new SeeQuestLog("questlog", "l"),
            new SeeSkillTree("skilltree", "k")
            ];
        public static InputAction[] InputInvActions = [
            new ExamineItem("examine"),
            new Equip("equip"),
            new UnEquip("unequip"),
            new Back("back", "b"),
            ];
        public static InputAction[] InputSkillActions = [
            new LearnSkill("learn"),
            new InfoSkill("info"),
            new ChangeQuickCast("quickcast"),
            new Back("back", "b")
            ];
        public static List<InputAction[]> InputActions = [
            InputRoomActions,
            InputInvActions,
            InputSkillActions
            ];
        /// <summary>
        /// 'Pauses' the game and prompts the player to press any button to continue.
        /// </summary>
        public static void PressToContinue() {
            HUDTools.Print($"\u001b[90m Press to continue...\u001b[0m", 3);
            Console.ReadKey(true);
        }
        /// <summary>
        /// Waits for and reads the player's one key input through the console.
        /// </summary>
        /// <returns>The one key input</returns>
        public static string UserKeyInput() {
            string userInput = Console.ReadKey().KeyChar.ToString().ToLower();
            Console.WriteLine("");
            return userInput;
        }
        public static string UserLineInput() {
            string userInput = (Console.ReadLine() ?? "").ToLower();
            return userInput;
        }
        ///<summary>
        ///Waits for and reads the player's input through the console and calls the correct method based on input.</summary>
        ///<param name="inputActionsToUse">
        ///0: inputs used in rooms, 1: inputs used while in inventory and 2: inputs used in skilltree.</param>
        ///<returns>
        ///Empty string or an exit from the currentRoom.</returns>
        public static string SelectPlayerAction(int inputActionsToUse) {
            InputAction[] inputActions = InputActions[inputActionsToUse];            
            string userInput = UserLineInput();
            if (userInput.Length == 1) {
                foreach (InputAction action in inputActions) {
                    if (action.AbrKeyWord == userInput) {
                        return action.RespondToInput([]);
                    }
                }
            }
            string[] separatedInputWords = userInput.Split(' ');
            foreach (InputAction action in inputActions) {
                if (action.KeyWord == separatedInputWords[0]) {
                    return action.RespondToInput(separatedInputWords);
                }
            }
            HUDTools.Print($" There is no '{userInput}' action...", 15);
            PressToContinue();
            HUDTools.ClearLastLine(3);
            return "";
        }
        ///<summary>
        ///Takes a player input and calls the correct method based on input.</summary>
        ///<param name="inputActionsToUse">
        ///RoomActions: inputs used in rooms, InvActions: inputs used while in inventory and EventActions: inputs used during room events.</param>
        ///<param name="userInput">
        ///The string inputted by the player.</param>
        ///<returns>
        ///Empty string or an exit from the currentRoom.</returns>
        public static string SelectPlayerAction(int inputActionsToUse, string userInput) {
            InputAction[] inputActions = InputActions[inputActionsToUse];
            if (userInput.Length == 1) {
                foreach (InputAction action in inputActions) {
                    if (action.AbrKeyWord == userInput) {
                        return action.RespondToInput([]);
                    }
                }
            }
            string[] separatedInputWords = userInput.Split(' ');
            foreach (InputAction action in inputActions) {
                if (action.KeyWord == separatedInputWords[0]) {
                    return action.RespondToInput(separatedInputWords);
                }
            }
            HUDTools.Print($" There is no '{userInput}' action...", 15);
            PressToContinue();
            HUDTools.ClearLastLine(3);
            return "";
        }
    }
}
