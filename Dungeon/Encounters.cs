using System;
using System.Collections.Generic;
using Saga.Assets;
using Saga.Items;
using Saga.Items.Loot;

namespace Saga.Dungeon
{
    public class Encounters {
        public int TurnTimer { get; set; } = 1;
        public bool Ran { get; set; } = false;

      //Encounters:

        //Tutorial encounters:

        //To Encounters som køres når en ny karakter startes for at introducere kamp.
        public static void FirstEncounter() {
            Console.Clear();
            Program.SoundController.Play("typewriter");
            HUDTools.Print("You awake in a cold and dark room. You feel dazed and are having trouble remembering");
            HUDTools.Print("anything about your past.");
            if (string.IsNullOrWhiteSpace(Program.CurrentPlayer.Name) == true) {
                HUDTools.Print("You can't even remember your own name...");
                Program.CurrentPlayer.Name = "Adventurer";
            }
            else {
                HUDTools.Print($"You know your name is {Program.CurrentPlayer.Name}.");
            }
            TextInput.PressToContinue();
            HUDTools.ClearLastLine(1);
            HUDTools.Print("You grope around in the darkness until you find a door handle. You feel some resistance as");
            HUDTools.Print("you turn the handle, but the rusty lock breaks with little effort. You see your captor");
            HUDTools.Print("standing with his back to you outside the door.");
            HUDTools.Print($"You throw open the door, grabbing a {Program.CurrentPlayer.Equipment[Slot.Right_Hand].ItemName} then {(Program.CurrentPlayer.CurrentClass == "Mage" ? "preparing an incantation" : "")}{(Program.CurrentPlayer.CurrentClass == "Warrior" ? "charging toward your captor" : "")}{(Program.CurrentPlayer.CurrentClass == "Archer" ? "nocking an arrow" : "")}.", 30);
            Program.SoundController.Stop();
            Program.SoundController.Play("taunt");
            Program.SoundController.Play("kamp");
            HUDTools.Print("He turns...");
            TextInput.PressToContinue();
            Enemy FirstEncounter = new Act1Enemy("Human Captor", Tribe.Human) { 
                MaxHealth = 5,
                Power = 2,
            };
            Combat(FirstEncounter);
        }
        public static void SecondEncounter() {
            Console.Clear();
            Program.SoundController.Play("kamp");
            HUDTools.RoomHUD();
            HUDTools.ClearLastLine(1);
            HUDTools.Print($"The big door creaks and you continue down the gloomy hallway. You Spot a pair of red glowing eyes\nin the darkness, but before you could react the beastly dog engages you.");
            TextInput.PressToContinue();
            Enemy SecondEncounter = new Act1Enemy("Feral Dog", Tribe.Beast) { 
                MaxHealth = 6,
                Power = 3,
            };
            Combat(SecondEncounter);
        }
        //Encounter som køres for at introducere shopkeeperen Gheed.
        public static void MeetGheed() {
            Console.Clear();
            Program.SoundController.Play("shop");
            Program.SoundController.Play("typewriter");
            if (Program.CurrentPlayer.CurrentClass == "Mage") {
                HUDTools.Print($"After dusting off your {Program.CurrentPlayer.Equipment[Slot.Torso].ItemName} and tucking in your new wand, you find someone else captured.");
            } else if (Program.CurrentPlayer.CurrentClass == "Archer") {
                HUDTools.Print("After retrieving the last arrow from your captor's corpse, you find someone else captured.");
            } else if (Program.CurrentPlayer.CurrentClass == "Warrior") {
                HUDTools.Print("After cleaning the blood from your captor off your new sword, you find someone else captured.");
            }
            HUDTools.Print("Freeing him from his shackles, he thanks you and gets up.");
            HUDTools.Print("'Gheed is the name and trade is my game', he gives a wink.");
            TextInput.PressToContinue();
            HUDTools.ClearLastLine(1);
            HUDTools.Print("'If you \u001b[96mgo\u001b[0m and clear some of the other rooms, I will look for my wares in these crates.'");
            Act1Quest.AddQuest("Clear some rooms");
            HUDTools.Print("'Then come back to me, I will then have been able to set up a shop where you can spend ");
            HUDTools.Print("some of that gold you are bound to have found,' he chuckles and rubs his hands at the thought.");
            NonPlayableCharacters.AddNpcToCamp("Gheed");
            HUDTools.Print($"You nod and prepare your {Program.CurrentPlayer.Equipment[Slot.Right_Hand].ItemName}. You should start by \u001b[96mlooking around\u001b[0m...");     
            TextInput.PressToContinue();
            Program.SoundController.Stop();
        }
        //Encounter som køres for at introducere Camp
        public static void FirstCamp() {
            Console.Clear();
            Program.SoundController.Play("typewriter");
            HUDTools.Print("You venture deeper and deeper, but while you explore your surroundings, you get queasy.");
            HUDTools.Print("The dark and cold dungeon walls seem to creep closer, you feel claustrophobic.");
            TextInput.PressToContinue();
            HUDTools.ClearLastLine(1);
            Program.SoundController.Stop();
            Program.SoundController.Play("campfire");
            HUDTools.Print("You hastily gather some old wood scattered about and make a campfire. The shadows retract and\nyou feel at ease again. Although you are not out of danger, you can stay for a while and rest.");
            Program.CurrentPlayer.CurrentAct = Character.Act.Act1;
            TextInput.PressToContinue();
        }
        //Encounter som køres første gang en spiller vender tilbage til Camp for at introducerer roguelike
        public static void FirstReturn() {
            Console.Clear();
            Program.SoundController.Play("typewriter");
            Program.SoundController.Play("labyrinthchange");
            HUDTools.Print($"As you enter the camp and close the door behind you, everything shakes and there are loud\nsounds of stone grinding against each other. Sand and pebbles fall from the ceiling and you\ncollapse to the floor from the vibrations.");
            HUDTools.Print($"After a few moments, you regain your composure and you check on Gheed.");
            TextInput.PressToContinue();
            HUDTools.ClearLastLine(1);
            HUDTools.Print($"'What was that?', you ask, 'it sounded like an earthquake'.\n'Indeed', Gheed answers, 'Although, I suspect it wasn't destructive in nature. That is what makes\nthis labyrinth a prison for those who enter. When you open that door again, you will find that\nall the rooms have changed.'");
            TextInput.PressToContinue();
            Program.SoundController.Stop();
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
                        Program.CurrentPlayer.Loot.GetQuestLoot(1,0,"MeetFlemsha");                          
                        TextInput.PressToContinue();                       
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
                    Program.CurrentPlayer.CompleteAndTurnInQuest(Program.CurrentPlayer.QuestLog.Find(quest=> quest.Name == "Free Flemsha"));
                    NonPlayableCharacters.AddNpcToCamp("Flemsha");
                    TextInput.PressToContinue();
                    HUDTools.ClearLastLine(8);
                    break;
                }
            }
        }


        //Random Encounters:

        //Encounter der "spawner" en random fjende som skal dræbes.
        public static void RandomBasicCombatEncounter() {
            Console.Clear();
            Program.SoundController.Play("kamp");
            Act1Enemy RandomEnemy = Act1Enemy.CreateRandomAct1Enemy();
            HUDTools.RoomHUD();
            HUDTools.ClearLastLine(1);
            switch (Program.Rand.Next(0,2)) {
                case int x when (x == 0):
                    HUDTools.Print($"You turn a corner and there you see a {RandomEnemy.Name}...", 10);
                    break;
                case int x when (x == 1):
                    HUDTools.Print($"You break down a door and find a {RandomEnemy.Name} inside!", 10);
                    break;
            }
            TextInput.PressToContinue();
            Combat(RandomEnemy);
        }
        //Encounter der "spawner" en Dark Wizard som skal dræbes.
        public static void WizardEncounter() {
            Console.Clear();
            Program.SoundController.Play("laugh");
            HUDTools.Print("The door slowly creaks open as you peer into the dark room. You see a tall man with a ",20);
            Program.SoundController.Play("troldmandskamp");
            HUDTools.Print("long beard and pointy hat, looking at a large tome.",20);
            TextInput.PressToContinue();
            Enemy WizardEncounter = new Act1Enemy("Dark Wizard", Tribe.Human) {
                MaxHealth = 3 + Program.CurrentPlayer.Level * (Program.CurrentPlayer.Level < 5 ? 2 : 4),
                Power = 6 + Program.CurrentPlayer.Level * (Program.CurrentPlayer.Level < 10 ? 2 : 4),
                ExpModifier = 3,
                Awareness = 5,
            };
            Combat(WizardEncounter);
            Console.Clear();
            HUDTools.RoomHUD();
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
            do {
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
                    HUDTools.Print($"You ready your {Program.CurrentPlayer.Equipment[Slot.Right_Hand].ItemName}!",15);
                    TextInput.PressToContinue();
                    Enemy MimicEncounter = new Act1Enemy("Mimic", Tribe.Mythical) {
                        MaxHealth = 10 + Program.CurrentPlayer.Level * (Program.CurrentPlayer.Level < 10 ? 3 : 6),                        
                        Power = 5 + Program.CurrentPlayer.Level * (Program.CurrentPlayer.Level < 5 ? 1 : 3),
                        GoldModifier = 3,
                    };
                    Combat(MimicEncounter); 
                    break;
                } else {
                    HUDTools.Print("Invalid input");
                    TextInput.PressToContinue();
                    HUDTools.ClearLastLine(3);
                }
            } while (input != "42");
            Console.Clear();
            HUDTools.RoomHUD();
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
            do {
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
                    Program.CurrentPlayer.Loot.GetTreasureChestLoot();
                    break;
                } else {
                    HUDTools.Print("Invalid input");
                    TextInput.PressToContinue();
                    HUDTools.ClearLastLine(3);
                }
            } while (input != "42");
            Console.Clear();
            HUDTools.RoomHUD();
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
            HUDTools.Print("Choose your path (each rune position corresponds to a number 1-4)", 10);
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
                    HUDTools.Print("Choose your path (each rune position corresponds to a number 1-4)", 0);
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
                        Program.CurrentPlayer.Health -= 2;
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
            Program.CurrentPlayer.Loot.GetExp(2, 50*Program.CurrentPlayer.Level);
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
        //Metode til at vælge imellem story/NPC encounters, den bruges efter et sæt af randomencounters under Camp().
        public static Room ProgressTheStory(Room[] rooms, int i) {
            if (!Program.CurrentPlayer.FailedQuests.Exists(quest => quest.Name == "Free Flemsha") && !Program.CurrentPlayer.CompletedQuests.Exists(quest => quest.Name == "Free Flemsha")) {
                return new MeetFlemshaRoom();
            } else {
                return Room.CreateRandomBasicCombatRoom(rooms, i);
            }
        }
        //Metode til at køre Camp hvor spilleren kan reste/shoppe/heale
        public static string Camp() {
            bool leave = false;
            string choice = "";
            Program.SoundController.Play("campfire");
            Program.SoundController.Play("campmusic");
            //Hver gang spilleren returnere til Camp refresher shoppen:
            Shop shop = Shop.SetForsale();
            HUDTools.FullCampHUD();
            while (leave == false) {              
                string input = TextInput.PlayerPrompt();
                //Explore, måden man progresser sin karakter:
                if (input == "e" || input == "explore") {
                    leave = true;
                    choice = "explore";
                }
                //Gemmer spillet:
                else if (input == "s" || input == "sleep" || input == "quit" || input == "quit game") {
                    Program.Save();
                    HUDTools.Print("Game saved!");
                    TextInput.PressToContinue();
                    HUDTools.ClearLastLine(3);
                }
                //Gheed's shop:
                else if (input == "g" || input == "gheed" || input == "gheed's shop" || input == "shop") {             
                    Program.SoundController.Stop();
                    Shop.Loadshop(Program.CurrentPlayer, shop);
                    Program.SoundController.Play("campfire");
                    Program.SoundController.Play("campmusic");
                    HUDTools.FullCampHUD();
                }
                //Quit and/or save the game:
                else if (input == "q" || input == "quit") {
                    if (Program.Quit() == "quit") {
                        leave = true;
                        choice = "quit";
                    }       
                }
                //Tale med NPC'er mens man er tilbage i campen.
                else if (input == "t" || input == "talk") {
                    NonPlayableCharacters.TalkToNpc();
                    HUDTools.FullCampHUD();
                }
                //Kalder metode til at tjekke input for, inventory, character, heale eller questloggen:
                else {
                    Program.CurrentPlayer.BasicActions(input);
                }
            }
            return choice;
        }
        //Metode til at køre kamp
        public static void Combat(Enemy Monster) {
            HUDTools.ClearLog();
            //Starter en tur tæller:
            Encounters TurnTimer = new();
            //Tjekker hvem starter if(spilleren starter), else (Fjenden starter):
            if (Program.CurrentPlayer.TotalSecondaryAttributes.Awareness > Monster.Awareness) {
                while (Monster.Health > 0 && TurnTimer.Ran == false) {
                    HUDTools.CombatHUD(Monster, TurnTimer);
                    Program.CurrentPlayer.CombatActions(Monster, TurnTimer);                   
                    if (TurnTimer.Ran == false) {
                        Monster.MonsterActions(TurnTimer);
                    }                   
                    Program.CurrentPlayer.CheckForDeath($"As the {Monster.Name} menacingly comes down to strike, you are slain by the mighty {Monster.Name}.");
                }
            }  else {
                while (Monster.Health > 0 && TurnTimer.Ran == false) {
                    HUDTools.CombatHUD(Monster, TurnTimer);
                    Monster.MonsterActions(TurnTimer);
                    Program.CurrentPlayer.CheckForDeath($"As the {Monster.Name} menacingly comes down to strike, you are slain by the mighty {Monster.Name}.");
                    Program.CurrentPlayer.CombatActions(Monster, TurnTimer);
                }
            }
            //Tjekker om monstret er besejret:
            if (Monster.Health <= 0) {
                Program.SoundController.Stop();
                HUDTools.ClearLog();
                Program.SoundController.Play("win");
                Program.CurrentPlayer.Loot.GetCombatLoot(Monster, $"You Won against the {Monster.Name} on turn {TurnTimer.TurnTimer - 1}!");
            }
        }
    }
}
