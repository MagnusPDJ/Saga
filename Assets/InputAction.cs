using Saga.Dungeon;
using Saga.Items;
using System;
using System.Linq;

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
            string itemToSearchFor = String.Join(" ", separatedInputWords.Skip(1));
            (int, int) startCursor = Console.GetCursorPosition();
            var wat = Program.CurrentPlayer.Equipment.FirstOrDefault(x => x.Value.ItemName.Equals(itemToSearchFor, StringComparison.CurrentCultureIgnoreCase));
            var item = Program.CurrentPlayer.Inventory.FirstOrDefault(x => x?.ItemName.ToLower() == itemToSearchFor);
            if (itemToSearchFor == "healing potion" || itemToSearchFor == "potion" || itemToSearchFor == "potions" || itemToSearchFor == "healing potions") {
                HUDTools.Print($"\n{Program.CurrentPlayer.Equipment[Slot.Potion].ItemDescription.Replace("\\n", "\n")}", 3);
            }
            else if (wat.Value == null && item == null) {
                Console.WriteLine("\nNo such item exists...");
            }
            else if (wat.Value != null) {
                if (wat.Value is IQuestItem) {
                    HUDTools.Print($"\n{wat.Value.ItemDescription.Replace("\\n", "\n")}", 3);
                }
                else if (wat.Value is IWeapon weapon) {
                        var wat1 = Program.CurrentPlayer.Equipment[Slot.Right_Hand];
                        HUDTools.Print($"\nThis is a {weapon.WeaponCategory} weapon.\n{wat1.ItemDescription.Replace("\\n", "\n")}", 3);
                }
                else {
                    HUDTools.Print($"\nThis is an armor of type {((ArmorBase)wat.Value).ArmorType}.\n{wat.Value.ItemDescription.Replace("\\n", "\n")}", 3);
                }
            }
            else {
                if (item is IQuestItem) {
                    HUDTools.Print($"\n{item.ItemDescription.Replace("\\n", "\n")}", 3);
                }
                else if (item is IWeapon weapon) {
                    HUDTools.Print($"\nThis is a {weapon.WeaponCategory} weapon.\n{item.ItemDescription.Replace("\\n", "\n")}", 3);
                }
                else {
                    HUDTools.Print($"\nThis is an armor of type {((ArmorBase)item).ArmorType}.\n{item.ItemDescription.Replace("\\n", "\n")}", 3);
                }
            }
            TextInput.PressToContinue();
            HUDTools.ClearLastText((startCursor.Item1, startCursor.Item2 - 1));
            return "";
        }
    }
    public class Equip(string keyWord) : InputAction(keyWord) {
        public override string RespondToInput(string[] separatedInputWords) {
            string itemToSearchFor = String.Join(" ", separatedInputWords.Skip(1));
            (int, int) startCursor = Console.GetCursorPosition();
            if (Program.CurrentPlayer.Inventory.All(x => x == null)) {
                Console.WriteLine("\nNo items in inventory...");
            }
            else {
                var item = Program.CurrentPlayer.Inventory.FirstOrDefault(x => x?.ItemName.ToLower() == itemToSearchFor);
                if (item != null) {
                    if (item is not IEquipable) {
                        HUDTools.Print("\nYou cannot equip this item...", 3);
                    }else {
                        HUDTools.Print($"\n{((IEquipable)item).Equip()}", 3);
                    }
                }
                else {
                    Console.WriteLine("\nNo such item in inventory...");
                }
            }
            TextInput.PressToContinue();
            HUDTools.ClearLastText((startCursor.Item1, startCursor.Item2 - 1));
            return "";
        }
    }
    public class UnEquip(string keyWord) : InputAction(keyWord) {
        public override string RespondToInput(string[] separatedInputWords = null) {
            string itemToSearchFor = String.Join(" ", separatedInputWords.Skip(1));
            (int, int) startCursor = Console.GetCursorPosition();
            var wat = Program.CurrentPlayer.Equipment.FirstOrDefault(x => x.Value.ItemName.Equals(itemToSearchFor, StringComparison.CurrentCultureIgnoreCase));
            if (wat.Value == null) {
                Console.WriteLine("\nNo such item equipped...");
            }
            else {
                HUDTools.Print($"\n{((IEquipable)wat.Value).UnEquip()}", 3);
            }
            TextInput.PressToContinue();
            HUDTools.ClearLastText((startCursor.Item1, startCursor.Item2 - 1));
            return "";
        }
    }
    public class Use(string keyWord) : InputAction(keyWord) {
        public override string RespondToInput(string[] separatedInputWords = null) {
            throw new NotImplementedException();
        }
    }
    public class Back(string keyWord, string abrKeyWord) : InputAction(keyWord, abrKeyWord)
    {
        public override string RespondToInput(string[] separatedInputWords) {
            return "back";
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
            ((IConsumable)Program.CurrentPlayer.Equipment[Slot.Potion]).Consume();
            TextInput.PressToContinue();
            HUDTools.RoomHUD();
            return "";
        }
    }
    public class SeeCharacterScreen(string keyWord, string abrKeyWord) : InputAction(keyWord, abrKeyWord) {
        public override string RespondToInput(string[] separatedInputWords = null) {
            HUDTools.CharacterScreen();
            TextInput.PressToContinue();
            HUDTools.RoomHUD();
            return "";
        }
    }
    public class SeeInventory(string keyWord, string abrKeyWord) : InputAction(keyWord, abrKeyWord) {
        public override string RespondToInput(string[] separatedInputWords = null) {        
            while (true) {
                HUDTools.InventoryScreen();
                string input = TextInput.PlayerPrompt(false);
                if (input == "back") {
                    break;
                }
            }
            HUDTools.RoomHUD();
            return "";
        }
    }
    public class SeeQuestLog(string keyWord, string abrKeyWord) : InputAction(keyWord, abrKeyWord) 
    {
        public override string RespondToInput(string[] separatedInputWords = null) {
            HUDTools.QuestLogHUD();
            TextInput.PressToContinue();
            HUDTools.RoomHUD();
            return "";
        }
    }
}

