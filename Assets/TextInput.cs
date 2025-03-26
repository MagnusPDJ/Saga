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
         ///Waits for and reads the player's input through the console and calls the correct function based on input.
         ///</summary>
         ///<param name = "readkey" >True if one letter input </param>
         ///<returns>Success if a function was found. Failed if no function was found.</returns>
        public static string PlayerPrompt(bool readkey) {
            if (readkey) {
                string userInput = Console.ReadKey().KeyChar.ToString().ToLower();
                Console.WriteLine("");
                foreach(InputAction action in Program.CurrentPlayer.InputActions) {
                    if (action.AbrKeyWord == userInput) {
                        action.RespondToInput(Program.CurrentPlayer);
                        return "Success";
                    }
                }

                //!!!Remove this when input actions are made!!!
                return userInput;


            } else {
                string userInput = Console.ReadLine().ToLower();
                char[] delimiterCharacters = [' '];
                string[] separatedInputWords = userInput.Split(delimiterCharacters);
                foreach(InputAction action in Program.CurrentPlayer.InputActions) {                  
                    if (action.KeyWord == separatedInputWords[0]) {
                        action.RespondToInput(Program.CurrentPlayer, separatedInputWords);
                        return "Success";
                    }
                }
                return "Failed";
            }
        }
    }
}
