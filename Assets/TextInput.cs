using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Saga.Assets
{
    class TextInput
    {

         ///<summary>
         ///Reads the player input through the console and calls the correct function based on input.
         ///</summary>
         ///<param name = "readkey" >True if one letter input </param>
         ///<returns>The player input</returns>
        public static string PlayerPrompt(bool readkey = false) {
            if (readkey) {
                string x = Console.ReadKey().KeyChar.ToString().ToLower();
                Console.WriteLine("");
                return x;
            } else {
                string userInput = Console.ReadLine().ToLower();
                char[] delimiterCharacters = [' '];
                string[] separatedInputWords = userInput.Split(delimiterCharacters);
                foreach(InputAction action in Program.CurrentPlayer.InputActions) {                  
                    if (action.keyWord == separatedInputWords[0]) {
                        action.RespondToInput(Program.CurrentPlayer, separatedInputWords);
                        return "Success";
                    }
                }
                return "Failed";
            }
        }
    }
}
