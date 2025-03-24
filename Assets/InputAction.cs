using Saga.Character;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Saga.Assets
{
    public abstract class InputAction(string keyWord)
    {
        public string keyWord = keyWord;

        public abstract void RespondToInput(Player player, string[] separatedInputWords);
    }

    public class Go(string keyWord) : InputAction(keyWord)
    {
        public override void RespondToInput(Player player, string[] separatedInputWords) {
            // controller.roomNavigation.AttemptToChangeRooms(separatedInputWords[1]);
        }
    }
}

