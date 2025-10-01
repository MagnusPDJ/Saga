using Saga.Assets;
using Saga.Dungeon.Enemies;
using Saga.Dungeon.Quests;
using Saga.Dungeon.Rooms;
using Saga.Items;
using Saga.Items.Loot;

namespace Saga.Dungeon
{
    public static class Encounters 
    {
        public enum Act
        {
            Start,
            Act1,
            Act2,
            Act3,
            Act4,
            Act5
        }

        //Story or NPC Encounters:

        //Alchemist trader/Quest giver
        public static void MeetFlemsha() {
            Console.Clear();
            Program.SoundController.Play("typewriter");
            HUDTools.RoomHUD();
            HUDTools.ClearLastLine(1);
            HUDTools.Print("You enter a dimly lit room, stone slab walls and iron bar grates make up some cells on either side\nof the room, there is also a desk which probably belonged to the long gone warden.",30);
            bool examined = false;
            bool searched = false;
            bool leftForDead = false;
            while (true) {
                while (!examined || !searched) {
                    Console.Clear();
                    HUDTools.RoomHUD();
                    HUDTools.ClearLastLine(1);
                    HUDTools.Print("You enter a dimly lit room, stone slab walls and iron bar grates make up some cells on either\nside of the room, there is also a desk which probably belonged to the long gone warden.", 0);
                    HUDTools.Print($"\nDo you {(!examined? "(1)examine desk? " : "")}{(!examined && !searched? "Or " : "")}{(!searched?"(2) search the prison cells?":"")}", 20);
                    string input = TextInput.PlayerPrompt();
                    if (input == "1" && !examined) {
                        examined = true;
                        HUDTools.Print("You rummage through dusty documents and moldy records illegible or in unknown languages,\nbut in a drawer you find some gold and a key.", 20);
                        LootSystem.GetQuestLoot(1,0,"MeetFlemsha");                          
                        TextInput.PressToContinue();
                        break;
                    } else if (input == "2" && !searched) {
                        HUDTools.Print(
                            "You search the prison cells and in one of them, you find a man laying on the stone floor rambling to\n" +
                            "himself. As you approach the iron grate he comes to his senses,\n" +
                            "'You must help me get out!' he exclaims, 'My name is Flemsha, I'm an alchemist, I can be of help'",
                            20);
                        HUDTools.Print("\nDo you want to help the man? (Y/N)", 10);                        
                        while(true) {
                            input = TextInput.PlayerPrompt();
                            if (input == "y") {
                                Act1Quest.AddQuest("Free Flemsha");
                                Program.CurrentPlayer.UpdateQuestLog();
                                break;
                            } else if (input == "n") {
                                HUDTools.Print("A locked up prisoner doesn't seem that useful to you. So you decide to leave him behind");
                                int a = Array.FindIndex(Program.CurrentPlayer.Inventory, x => x != null && x.ItemName == "Old Key");
                                if (a != -1) {
                                    Program.CurrentPlayer.Inventory.SetValue(null, a);
                                }
                                Act1Quest.FailQuest("Free Flemsha");
                                NonPlayableCharacters.UpdateDialogueOptions("Deadflemsha");
                                leftForDead = true;
                                break;
                            } else {
                                HUDTools.Print("Invalid input.", 5);
                                TextInput.PressToContinue();
                                HUDTools.ClearLastLine(3);
                            }
                        }
                        searched = true;
                        TextInput.PressToContinue();
                        break;
                    } else {
                        Program.CurrentPlayer.BasicActions(input);
                    }
                }
                if (leftForDead) {
                    HUDTools.ClearLastLine(1);
                    HUDTools.Print("You close the door to the prison ward and continue on, never to see the prisoner again.");
                    TextInput.PressToContinue();
                    Program.RoomController.ran = true;
                    break;
                }

                if (Program.CurrentPlayer.QuestLog.Exists(quest => quest.Name == "Free Flemsha" && quest.Completed == true)) {
                    Console.Clear();
                    HUDTools.RoomHUD();
                    HUDTools.Print("You return to Flemsha and try the key. With some resistance you turn the mechanism and\nthe door slides open.", 20);
                    HUDTools.Print("He thanks you very much and you tell him about your camp, where Gheed is too.", 20);
                    var quest = Program.CurrentPlayer.QuestLog.Find(quest => quest.Name == "Free Flemsha");
                    if (quest != null) {
                        Program.CurrentPlayer.CompleteAndTurnInQuest(quest);
                    }
                    NonPlayableCharacters.AddNpcToCamp("Flemsha");
                    TextInput.PressToContinue();
                    HUDTools.ClearLastLine(8);
                    break;
                }
            }
        }


        //Bestemte Encounters:

        //Encounter der "spawner" en Dark Wizard som skal dræbes.
        public static void WizardEncounter() {
            Console.Clear();
            Program.SoundController.Play("laugh");
            HUDTools.RoomHUD();
            HUDTools.Print("The door slowly creaks open as you peer into the dark room. You see a tall man with a ",20);
            Program.SoundController.Play("troldmandskamp");
            HUDTools.Print("long beard and pointy hat, looking at a large tome.",20);
            TextInput.PressToContinue();

            EnemyBase lich = EnemyFactory.CreateByName("Ancient Lich");
            //Enemy WizardEncounter = new Act1Enemy("Dark Wizard", Tribe.Human) {
            //    MaxHealth = 3 + Program.CurrentPlayer.Level * (Program.CurrentPlayer.Level < 5 ? 2 : 4),
            //    Power = 6 + Program.CurrentPlayer.Level * (Program.CurrentPlayer.Level < 10 ? 2 : 4),
            //    ExpModifier = 3,
            //    Initiative = 5,
            //};
            new CombatController(Program.CurrentPlayer, lich).Combat();
        }
        //Encounter der "spawner" en Mimic som skal dræbes.
        public static void MimicEncounter() {
            string input;
            Console.Clear();
            Program.SoundController.Play("dooropen");
            HUDTools.RoomHUD();
            HUDTools.ClearLastLine(1);
            HUDTools.Print("You open a door and find a treasure chest inside!");
            HUDTools.Print("Do you want to try and open it?\n(Y/N)");
            while(true) {
                input = TextInput.PlayerPrompt();
                if (input == "n") {
                    Program.SoundController.Play("doorclose");
                    HUDTools.Print("You slowly back out of the room and continue...", 20);
                    TextInput.PressToContinue();
                    Program.RoomController.ran = true;
                    break;
                } else if (input == "y") {
                    Program.SoundController.Play("mimic");
                    HUDTools.Print("As you touch the frame of the chest, it springs open splashing you with saliva!");
                    Program.SoundController.Play("troldmandskamp");
                    HUDTools.Print("Inside are multiple rows of sharp teeth and a swirling tongue that reaches for you.",15);
                    HUDTools.Print($"You ready your {(Program.CurrentPlayer.Equipment.Right_Hand as IItem)?.ItemName}!",15);
                    TextInput.PressToContinue();

                    EnemyBase lich = EnemyFactory.CreateByName("Ancient Lich");
                    new CombatController(Program.CurrentPlayer, lich).Combat();

                    break;
                } else {
                    HUDTools.Print("Invalid input");
                    TextInput.PressToContinue();
                    HUDTools.ClearLastLine(3);
                }
            }
        }
        //Encounter der "spawner" en treasure chest.
        public static void TreasureEncounter() {
            string input;
            Console.Clear();
            Program.SoundController.Play("dooropen");
            HUDTools.RoomHUD();
            HUDTools.ClearLastLine(1);
            HUDTools.Print("You open a door and find a treasure chest inside!");
            HUDTools.Print("Do you want to try and open it?\n(Y/N)");
            while(true) {
                input = TextInput.PlayerPrompt();
                if (input == "n") {
                    Program.SoundController.Play("doorclose");
                    HUDTools.Print("You slowly back out of the room and continue...",20);
                    TextInput.PressToContinue();
                    Program.RoomController.ran = true;
                    break;
                } else if (input == "y") {
                    Program.SoundController.Play("treasure");
                    HUDTools.Print("You release the metal latch and grab both sides of the chest and peer inside.");
                    TextInput.PressToContinue();
                    HUDTools.ClearLastLine(1);
                    Program.SoundController.Play("win");
                    LootSystem.GetTreasureChestLoot();
                    break;
                } else {
                    HUDTools.Print("Invalid input");
                    TextInput.PressToContinue();
                    HUDTools.ClearLastLine(3);
                }
            }
        }
        //Encounter der starter en trap med runer hvor den rigtige rune skal vælges for at kunne exit
        public static void PuzzleOneEncounter() {
            Console.Clear();
            Program.SoundController.Play("footsteps");
            Program.SoundController.Play("runetrap");

            //runer
            List<char> chars = ['\u0925', '\u0931', '\u09fa', '\u1805', '\u1873', '\u0166', '\u017f', '\u018d', '\u0195', '\u01a7'];
            List<char> endchars = ['\u00fe', '\u00f5', '\u00d0', '\u0141', '\u014a', '\u047b', '\u046b', '\u1c59', '\u1c6c', '\u1cbe'];
            List<int> positions = [];
            char c = chars[Program.Rand.Next(0, 10)];
            chars.Remove(c);
            
            //Rune template
            List<string> puzzle = [];

            for (int a = 0; a < 4; a++) {
                int pos = Program.Rand.Next(0, 4);
                positions.Add(pos);
                string row = "";
                for (int b = 0; b < 4; b++) {
                    if (b == pos) {
                        row += c + " ";
                    } else if (0<a && a<3){
                        row += chars[Program.Rand.Next(0, 9)] + " ";
                    } else {
                        row += endchars[Program.Rand.Next(0, 9)] + " ";
                    }             
                }
                puzzle.Add(row);
            }

            //slow print:
            Console.Clear();
            HUDTools.RoomHUD();
            HUDTools.Print("As you are walking down the dark corridors, you see that the floor is suddenly covered in runes,\nso you decide to tread carefully.", 30);
            HUDTools.Print("Choose your path (each rune position corresponds to a number 1-4):", 10);
            HUDTools.Print("   o    <- starting position", 5);
            for (int j = 0; j < 4; j++) {
                HUDTools.Print(puzzle[j] + "\n", 10);
            }

            //Player action sequence:
            string location = "";
            for (int i = 0; i < 4;) {
                Console.Clear();               
                HUDTools.RoomHUD();
                if (i == 0) {
                    HUDTools.Print("As you are walking down the dark corridors, you see that the floor is suddenly covered in runes,\nso you decide to tread carefully.", 0);
                    HUDTools.Print("Choose your path (each rune position corresponds to a number 1-4):", 0);
                    HUDTools.Print("   o    <- starting position", 0);
                    for (int j = 0; j < 4; j++) {
                        HUDTools.Print(puzzle[j]+"\n", 0);
                    }                  
                } else {
                    HUDTools.Print("As you are walking down the dark corridors, you see that the floor is suddenly covered in runes,\nso you decide to tread carefully.", 0);
                    HUDTools.Print("Choose your path (each rune position corresponds to a number 1-4):", 0);
                    for (int j = 0; j < 4; j++) {
                        if (i == j) {
                            Console.Write($"{location} <- Your position");
                        }
                        HUDTools.Print("\n"+puzzle[j], 0);                        
                    }
                    Console.WriteLine();
                }              
                string input = TextInput.PlayerPrompt();
                if (int.TryParse(input, out int number) && number < 5 && number > 0) {
                    if (positions[i] == number - 1) {
                        Program.SoundController.Play("footsteps");
                        HUDTools.Print($"You stepped on the {c} rune, nothing happens...", 10);
                        location = "";
                        i++;
                        for (int j = 1; j < number; j++) {
                            location += "  ";
                        }
                        location += "o";
                        for (int j = 4; j > number; j--) {
                            location += "  ";
                        }
                        TextInput.PressToContinue();
                    } else {
                        Program.SoundController.Play("darts");
                        HUDTools.Print($"Darts fly out of the walls! You take 2 damage.)", 10);
                        Program.CurrentPlayer.TakeDamage(2);
                        TextInput.PressToContinue();
                        Program.CurrentPlayer.CheckForDeath("You start to feel sick. The poison from the darts slowly kills you");
                    }
                } else if (int.TryParse(input, out _)) {
                    Console.WriteLine("Invalid Input: Whole numbers 1-4 only");
                    TextInput.PressToContinue();
                } else {
                    Program.CurrentPlayer.BasicActions(input);
                }               
            }
            HUDTools.ClearLastLine(1);
            Program.SoundController.Stop();
            Program.SoundController.Play("win");
            LootSystem.GetFixedExp(75*Program.CurrentPlayer.Level);
            TextInput.PressToContinue();
            HUDTools.ClearLastLine(16);
        }


      //Encounter Tools:

        //Metode til at vælge tilfældigt mellem encounters.
        public static void RandomEncounter() {
            //1, 30+1
            switch (Program.Rand.Next(1, 30+1)) {
                case int n when n <= 10 && Program.CurrentPlayer.Level > 1:
                    WizardEncounter();
                    break;
                case int n when 10 < n && n <=20 && Program.CurrentPlayer.Level > 1:
                    MimicEncounter();
                    break;
                case int n when 20 < n && n <=30 && Program.CurrentPlayer.Level > 1:
                    TreasureEncounter();
                    break;
            }
        }
    }
}
