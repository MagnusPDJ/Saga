using System;

namespace Saga.Assets
{
    class TextInput
    {
        /// <summary>
        /// 'Pauses' the game and prompts the player to press any button to continue.
        /// </summary>
        public static void PressToContinue() {
            HUDTools.Print($"\u001b[90mPress to continue...\u001b[0m", 3);
            Console.ReadKey(true);
        }
        /// <summary>
        /// Waits for and reads the player's one key input through the console.
        /// </summary>
        /// <returns>The one key input</returns>
        public static string PlayerPrompt() {
            string userInput = Console.ReadKey().KeyChar.ToString().ToLower();
            Console.WriteLine("");
            return userInput;
        }
         ///<summary>
         ///Waits for and reads the player's input through the console and calls the correct method based on input.
         ///</summary>
         ///<returns>Empty string or an exit from the currentRoom.</returns>
        public static string PlayerPrompt(bool noReturn) {
            if (noReturn) {
                string userInput = Console.ReadLine().ToLower();
                if (userInput.Length == 1) {
                    foreach (InputAction action in Program.RoomController.InputActions) {
                        if (action.AbrKeyWord == userInput) {
                            return action.RespondToInput();
                        }
                    }
                }
                char[] delimiterCharacters = [' '];
                string[] separatedInputWords = userInput.Split(delimiterCharacters);
                foreach(InputAction action in Program.RoomController.InputActions) {                  
                    if (action.KeyWord == separatedInputWords[0]) {
                        return action.RespondToInput(separatedInputWords);
                    }
                }
                HUDTools.Print($"There is no '{userInput}' action...", 15);
                PressToContinue();
                HUDTools.ClearLastLine(3);
                return "";
            }
            return "";
        }
    }
}
