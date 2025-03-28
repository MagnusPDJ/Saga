using System;
using Saga.Dungeon;

namespace Saga.Assets
{
    public abstract class InputAction(string keyWord, string abrKeyWord = null) {
        public string KeyWord { get; set; } = keyWord;
        public string AbrKeyWord { get; set; } = abrKeyWord;

        public abstract string RespondToInput(string[] separatedInputWords = null);
    }

    public class Go(string keyWord) : InputAction(keyWord) {
        public override string RespondToInput(string[] separatedInputWords) {
            if (separatedInputWords.Length == 1) {
                HUDTools.Print($"You go nowhere, not very productive...", 10);
                TextInput.PressToContinue();
                HUDTools.ClearLastLine(3);
                return "";
            }
            bool foundRoom = false;
            foreach (Exit exit in Program.RoomController.currentRoom.exits) {
                if (exit.keyString == separatedInputWords[1]) {
                    foundRoom = true;
                    break;
                }
            }
            if (foundRoom) {
                return separatedInputWords[1];
            } else {
                HUDTools.Print($"You cannot go {separatedInputWords[1]}, \u001b[96mlook around\u001b[0m to find places to go.", 10);
                TextInput.PressToContinue();
                HUDTools.ClearLastLine(3);
                return "";
            }
        }
    }
    public class Examine(string keyWord) : InputAction(keyWord) {
        public override string RespondToInput(string[] separatedInputWords) {
            throw new NotImplementedException();
        }
    }
    public class Equip(string keyWord) : InputAction(keyWord) {
        public override string RespondToInput(string[] separatedInputWords) {
            throw new NotImplementedException();
        }
    }
    public class Use(string keyWord) : InputAction(keyWord) {
        public override string RespondToInput(string[] separatedInputWords) {
            throw new NotImplementedException();
        }
    }
    public class Look(string keyWord) : InputAction(keyWord) {
        public override string RespondToInput(string[] separatedInputWords) {
            (int, int) startCursorPosition = Console.GetCursorPosition();
            if (separatedInputWords[1] == "around") {
                HUDTools.Print(Program.RoomController.currentRoom.description, 10);
                foreach (Exit exit in Program.RoomController.currentRoom.exits) {
                    HUDTools.Print(exit.exitDescription, 10);
                }
            } else {
                HUDTools.Print($"You try to look at \u001b[90m{separatedInputWords[1]}\u001b[0m, but you realise that would be silly.");
            }
            TextInput.PressToContinue();
            (int, int) endCursorPosition = Console.GetCursorPosition();
            HUDTools.ClearLastLine(endCursorPosition.Item2 - startCursorPosition.Item2 + 1);
            return "";
        }
    }
    public class DrinkPotion(string keyWord, string abrKeyWord) : InputAction(keyWord, abrKeyWord) {
        public override string RespondToInput(string[] separatedInputWords = null) {
            Program.CurrentPlayer.Heal();
            TextInput.PressToContinue();
            HUDTools.SmallCharacterInfo();
            return "";
        }
    }
    public class SeeCharacterScreen(string keyWord, string abrKeyWord) : InputAction(keyWord, abrKeyWord) {
        public override string RespondToInput(string[] separatedInputWords = null) {
            HUDTools.CharacterScreen();
            TextInput.PressToContinue();
            HUDTools.SmallCharacterInfo();
            return "";
        }
    }
    public class SeeInventory(string keyWord, string abrKeyWord) : InputAction(keyWord, abrKeyWord) {
        public override string RespondToInput(string[] separatedInputWords = null) {
            HUDTools.InventoryScreen();
            HUDTools.SmallCharacterInfo();
            return "";
        }
    }
    public class SeeQuestLog(string keyWord, string abrKeyWord) : InputAction(keyWord, abrKeyWord) 
    {
        public override string RespondToInput(string[] separatedInputWords = null) {
            HUDTools.QuestLogHUD();
            TextInput.PressToContinue();
            HUDTools.SmallCharacterInfo();
            return "";
        }
    }
}

