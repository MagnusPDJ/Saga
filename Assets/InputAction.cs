using Saga.Character.DmgLogic;
using Saga.Dungeon;
using Saga.Items;
using System;
using System.Linq;

namespace Saga.Assets
{
    public abstract class InputAction(string keyWord, string? abrKeyWord = null)
    {
        public string KeyWord { get; set; } = keyWord;
        public string? AbrKeyWord { get; set; } = abrKeyWord;

        public abstract string RespondToInput(string[] separatedInputWords);
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
            var equipped = Program.CurrentPlayer.Equipment.AsEnumerable().FirstOrDefault(x => x.Value != null && x.Value.ItemName.Equals(itemToSearchFor, StringComparison.CurrentCultureIgnoreCase));
            var item = Program.CurrentPlayer.Inventory.FirstOrDefault(x => x?.ItemName.ToLower() == itemToSearchFor);
            if (equipped.Value != null) {
                if (equipped.Value is IWeapon weapon) {
                    if (equipped.Value.ItemSlot == Slot.Left_Hand) {
                        Program.CurrentPlayer.Equipment.TryGetSlot(Slot.Right_Hand, out IEquipable? equipped1);
                        HUDTools.Print($"\nThis is a {weapon.WeaponCategory} weapon. {DamageHelper.Describe((IDamageType)weapon)}\n{equipped1?.ItemDescription.Replace("\\n", "\n")}", 3);
                    }
                    HUDTools.Print($"\nThis is a {weapon.WeaponCategory} weapon. {DamageHelper.Describe((IDamageType)weapon)}\n{weapon.ItemDescription.Replace("\\n", "\n")}", 3);
                } else if (equipped.Value is IArmor armor) {
                    HUDTools.Print($"\nThis is an armor of type {armor.ArmorType}.\n{armor.ItemDescription.Replace("\\n", "\n")}", 3);
                } else {
                    HUDTools.Print($"\n{equipped.Value.ItemDescription.Replace("\\n", "\n")}", 3);
                }
            } else if (item != null) {
                if (item is IWeapon weapon) {
                    HUDTools.Print($"\nThis is a {weapon.WeaponCategory} weapon. {DamageHelper.Describe((IDamageType)weapon)}\n{weapon.ItemDescription.Replace("\\n", "\n")}", 3);
                } else if (item is IArmor armor) {
                    HUDTools.Print($"\nThis is an armor of type {armor.ArmorType}.\n{armor.ItemDescription.Replace("\\n", "\n")}", 3);
                } else {
                    HUDTools.Print($"\n{item.ItemDescription.Replace("\\n", "\n")}", 3);
                }
            } else {
                Console.WriteLine("\nNo such item exists...");
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
                    if (item is IEquipable equipable) {
                        HUDTools.Print($"\n{equipable.Equip()}", 3);                      
                    }else {
                        HUDTools.Print("\nYou cannot equip this item...", 3);
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
        public override string RespondToInput(string[] separatedInputWords) {
            string itemToSearchFor = String.Join(" ", separatedInputWords.Skip(1));
            (int, int) startCursor = Console.GetCursorPosition();
            var equipped = Program.CurrentPlayer.Equipment.AsEnumerable().FirstOrDefault(x => x.Value != null && x.Value.ItemName.Equals(itemToSearchFor, StringComparison.CurrentCultureIgnoreCase));
            if (equipped.Value == null) {
                Console.WriteLine("\nNo such item equipped...");
            }
            else {
                HUDTools.Print($"\n{equipped.Value.UnEquip()}", 3);
            }
            TextInput.PressToContinue();
            HUDTools.ClearLastText((startCursor.Item1, startCursor.Item2 - 1));
            return "";
        }
    }
    public class Use(string keyWord) : InputAction(keyWord) {
        public override string RespondToInput(string[] separatedInputWords) {
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
    public class DrinkHealingPotion(string keyWord, string abrKeyWord) : InputAction(keyWord, abrKeyWord) {
        public override string RespondToInput(string[] separatedInputWords) {
            var potion = Array.Find(Program.CurrentPlayer.Equipment.Potion, p => p is IItem { ItemName: "Healing Potion" });
            potion?.Consume();
            TextInput.PressToContinue();
            HUDTools.RoomHUD();
            return "";
        }
    }
    public class SeeCharacterScreen(string keyWord, string abrKeyWord) : InputAction(keyWord, abrKeyWord) {
        public override string RespondToInput(string[] separatedInputWords) {
            HUDTools.CharacterScreen();
            TextInput.PressToContinue();
            HUDTools.RoomHUD();
            return "";
        }
    }
    public class SeeInventory(string keyWord, string abrKeyWord) : InputAction(keyWord, abrKeyWord) {
        public override string RespondToInput(string[] separatedInputWords) {        
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
        public override string RespondToInput(string[] separatedInputWords) {
            HUDTools.QuestLogHUD();
            TextInput.PressToContinue();
            HUDTools.RoomHUD();
            return "";
        }
    }
    public class SeeSkillTree(string keyWord, string abrKeyWord) : InputAction(keyWord, abrKeyWord)
    {
        public override string RespondToInput(string[] separatedInputWords) {
            while (true) {
                HUDTools.ShowSkillTree();
                string input = TextInput.PlayerPrompt();
                if (input == "b") {
                    break;
                } else if (int.TryParse(input, out int choice)) {
                    Program.CurrentPlayer.SpendSkillPoint(choice - 1);
                }
            }
            HUDTools.RoomHUD();
            return "";
        }
    }
}

