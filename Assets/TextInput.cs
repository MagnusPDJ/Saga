using Saga.Character;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Saga.Assets
{
    class TextInput
    {

        public void PlayerPrompt() {
            string input = Console.ReadLine();
            AcceptStringInput(input);
        }

        void AcceptStringInput(string userInput) {
            userInput = userInput.ToLower();

            char[] delimiterCharacters = { ' ' };
            string[] separatedInputWords = userInput.Split(delimiterCharacters);

            for (int i = 0; i < Program.CurrentPlayer.InputActions.Length; i++) {
                InputAction inputAction = controller.inputActions[i];
                if (inputAction.keyWord == separatedInputWords[0]) {
                    inputAction.RespondToInput(controller, separatedInputWords);
                }
            }

            InputComplete();

        }

        void InputComplete() {
            controller.DisplayLoggedText();
            inputField.ActivateInputField();
            inputField.text = null;
        }

    }
}
