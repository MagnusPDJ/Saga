using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
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
            HUDTools.Print("You grope around in the darkness until you find a door handle. You feel some resistance as");
            HUDTools.Print("you turn the handle, but the rusty lock breaks with little effort. You see your captor");
            HUDTools.Print("standing with his back to you outside the door.");
            HUDTools.Print($"You throw open the door, grabbing a {Program.CurrentPlayer.Equipment[Slot.Weapon].ItemName} then {(Program.CurrentPlayer.CurrentClass == "Mage" ? "preparing an incantation" : "")}{(Program.CurrentPlayer.CurrentClass == "Warrior" ? "charging toward your captor" : "")}{(Program.CurrentPlayer.CurrentClass == "Archer" ? "nocking an arrow" : "")}.");
            Program.SoundController.Stop();
            Program.SoundController.Play("taunt");
            Program.SoundController.Play("kamp");
            HUDTools.Print("He turns...");
            TextInput.PressToContinue();
            Enemy FirstEncounter = new Act1Enemy("Human Captor", Tribe.Human) { 
                Health = 5,
                Power = 2,
            };
            AdvancedCombat(FirstEncounter);
        }
        public static void SecondEncounter() {
            Console.Clear();
            Program.SoundController.Play("kamp");
            switch (Program.Rand.Next(0, 2)) {
                case int x when (x == 0):
                    HUDTools.Print($"You turn a corner and there you see a Feral Dog...", 10);
                    break;
                case int x when (x == 1):
                    HUDTools.Print($"You break down a door and find a Feral Dog inside!", 10);
                    break;
            }
            Enemy SecondEncounter = new Act1Enemy("Feral Dog", Tribe.Beast) { 
            Health=6,
            Power=3,
            };
            TextInput.PressToContinue();
            AdvancedCombat(SecondEncounter);
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
            Console.Clear();
            HUDTools.Print("'If you \u001b[96mgo\u001b[0m and clear some of the other rooms, I will look for my wares in these crates.'");
            HUDTools.Print("'Then come back to me, I will then have been able to set up a shop where you can spend ");
            HUDTools.Print("some of that gold you are bound to have found,' he chuckles and rubs his hands at the thought.");
            HUDTools.Print($"You nod and prepare your {Program.CurrentPlayer.Equipment[Slot.Weapon].ItemName}. You should start by \u001b[96mlooking around\u001b[0m...");
            AddNpcToCamp("Gheed");
            TextInput.PressToContinue();
            Program.SoundController.Stop();
        }
        //Encounter som køres for at introducere Camp
        public static void FirstCamp() {
            Console.Clear();
            Program.SoundController.Play("typewriter");
            HUDTools.Print("After taking what few scraps you could find, you explore your surroundings.");
            HUDTools.Print("The dark and cold dungeon walls seem to creep closer, you feel claustrophobic.");
            TextInput.PressToContinue();
            Program.SoundController.Stop();
            Program.SoundController.Play("campfire");
            HUDTools.Print("You hastily gather some old wood scattered about and make a campfire. The");
            HUDTools.Print("shadows retract and you feel at ease again. Although you are not out of danger,");
            HUDTools.Print("you can stay for a while and rest.");
            TextInput.PressToContinue();
        }


        //Story or NPC Encounters:

        //Alchemist trader/Quest giver
        public static void MeetFlemsha() {
            Console.Clear();
            Program.SoundController.Play("typewriter");
            HUDTools.SmallCharacterInfo();
            HUDTools.Print("You enter a dimly lit room, stone slab walls and iron bar grates make up some cells on either side\nof the room, there is also a desk which probably belonged to the long gone warden.",30);
            bool examined = false;
            bool searched = false;
            bool leftForDead = false;
            while (true) {
                while (!examined || !searched) {
                    Console.Clear();
                    HUDTools.SmallCharacterInfo();
                    HUDTools.Print("You enter a dimly lit room, stone slab walls and iron bar grates make up some cells on either\nside of the room, there is also a desk which probably belonged to the long gone warden.", 0);
                    HUDTools.Print($"\nDo you {(!examined? "(1)examine desk? " : "")}{(!examined && !searched? "Or " : "")}{(!searched?"(2) search the prison cells?":"")}", 20);
                    string input = TextInput.PlayerPrompt(true);
                    if (input == "1" && !examined) {
                        examined = true;
                        HUDTools.Print("You rummage through dusty documents and moldy records illegible or in unknown languages,\nbut in a drawer you find some gold and a key.", 20);
                        Program.CurrentPlayer.Loot.GetQuestLoot(1,0,"MeetFlemsha");                          
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
                            input = TextInput.PlayerPrompt(true);
                            if (input == "y") {
                                Program.CurrentPlayer.QuestLog.Add(Act1Quest.FreeFlemsha);
                                HUDTools.Print($"\u001b[96mYou've gained a quest: {Act1Quest.FreeFlemsha.Name}!\u001b[0m");
                                Program.CurrentPlayer.UpdateQuestLog();
                                break;
                            } else if (input == "n") {
                                HUDTools.Print("A locked up prisoner doesn't seem that useful to you. So you decide to leave him behind");
                                int a = Array.IndexOf(Program.CurrentPlayer.Inventory, QuestLootTable.OldKey);
                                if (a != -1) {
                                    Program.CurrentPlayer.Inventory.SetValue(null, a);
                                }
                                Program.CurrentPlayer.FailedQuests.Add(Act1Quest.FreeFlemsha);
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
                    HUDTools.Print("You close the door to the prison ward and continue on, never to see the prisoner again.");
                    TextInput.PressToContinue();
                    break;
                }

                if (Program.CurrentPlayer.QuestLog.Exists(quest => quest.Name == "Free Flemsha" && quest.Completed == true)) {
                    Console.Clear();
                    HUDTools.SmallCharacterInfo();
                    HUDTools.Print("You return to Flemsha and try the key. With some resistance you turn the mechanism and\nthe door slides open.", 20);
                    HUDTools.Print("He thanks you very much and you tell him how he can find your camp, where Gheed is too.", 20);
                    Program.CurrentPlayer.CompleteAndTurnInQuest(Program.CurrentPlayer.QuestLog.Find(quest=> quest.Name == "Free Flemsha"));
                    AddNpcToCamp("Flemsha");
                    TextInput.PressToContinue();
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
            switch (Program.Rand.Next(0,2)) {
                case int x when (x == 0):
                    HUDTools.Print($"You turn a corner and there you see a {RandomEnemy.Name}...", 10);
                    break;
                case int x when (x == 1):
                    HUDTools.Print($"You break down a door and find a {RandomEnemy.Name} inside!", 10);
                    break;
            }
            TextInput.PressToContinue();
            AdvancedCombat(RandomEnemy);
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
                Health = 3 + Program.CurrentPlayer.Level * (Program.CurrentPlayer.Level < 5 ? 2 : 4),
                Power = 6 + Program.CurrentPlayer.Level * (Program.CurrentPlayer.Level < 10 ? 2 : 4),
                ExpModifier = 3,
                Awareness = 5,
            };
            AdvancedCombat(WizardEncounter);
        }
        //Encounter der "spawner" en Mimic som skal dræbes.
        public static void MimicEncounter() {
            string input;
            Console.Clear();
            Program.SoundController.Play("dooropen");
            HUDTools.Print("You open a door and find a treasure chest inside!");
            HUDTools.Print("Do you want to try and open it?\n(Y/N)");
            Console.Clear();
            do {
                Console.WriteLine("You open a door and find a treasure chest inside!");
                Console.WriteLine("Do you want to try and open it?\n(Y/N)");
                input = TextInput.PlayerPrompt(true);
                if (input == "n") {
                    Program.SoundController.Play("doorclose");
                    HUDTools.Print("You slowly back out of the room and continue...", 20);
                    TextInput.PressToContinue();
                    RandomBasicCombatEncounter();
                    break;
                } else if (input == "y") {
                    Program.SoundController.Play("mimic");
                    HUDTools.Print("As you touch the frame of the chest, it springs open splashing you with saliva!");
                    Program.SoundController.Play("troldmandskamp");
                    HUDTools.Print("Inside are multiple rows of sharp teeth and a swirling tongue that reaches for you.",15);
                    HUDTools.Print($"You ready your {Program.CurrentPlayer.Equipment[Slot.Weapon].ItemName}!",15);
                    TextInput.PressToContinue();
                    Enemy MimicEncounter = new Act1Enemy("Mimic", Tribe.Mythical) {
                        Health = 10 + Program.CurrentPlayer.Level * (Program.CurrentPlayer.Level < 10 ? 3 : 6),
                        Power = 5 + Program.CurrentPlayer.Level * (Program.CurrentPlayer.Level < 5 ? 1 : 3),
                        GoldModifier = 3,
                    };
                    AdvancedCombat(MimicEncounter); 
                    break;
                } else {
                    HUDTools.Print("Invalid input");
                    TextInput.PressToContinue();
                    Console.Clear();
                }
            } while (input != "42");
        }
        //Encounter der "spawner" en treasure chest.
        public static void TreasureEncounter() {
            string input;
            Console.Clear();
            Program.SoundController.Play("dooropen");            
            HUDTools.Print("You open a door and find a treasure chest inside!");
            HUDTools.Print("Do you want to try and open it?\n(Y/N)");
            Console.Clear();
            do {
                Console.WriteLine("You open a door and find a treasure chest inside!");
                Console.WriteLine("Do you want to try and open it?\n(Y/N)");
                input = TextInput.PlayerPrompt(true);
                if (input == "n") {
                    Program.SoundController.Play("doorclose");
                    HUDTools.Print("You slowly back out of the room and continue...",20);
                    TextInput.PressToContinue();
                    RandomBasicCombatEncounter();
                    break;
                } else if (input == "y") {
                    Program.SoundController.Play("treasure");
                    HUDTools.Print("You release the metal latch and grab both sides of the chest and peer inside.");
                    TextInput.PressToContinue();
                    Program.SoundController.Play("win");
                    Program.CurrentPlayer.Loot.GetTreasureChestLoot();
                    break;
                } else {
                    HUDTools.Print("Invalid input");
                    TextInput.PressToContinue();
                    Console.Clear();
                }
            } while (input != "42");
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
            HUDTools.SmallCharacterInfo();
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
                HUDTools.SmallCharacterInfo();
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
                string input = TextInput.PlayerPrompt(true);
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
            Program.SoundController.Stop();
            Program.SoundController.Play("win");
            Program.CurrentPlayer.Loot.GetExp(2, 50*Program.CurrentPlayer.Level);
            TextInput.PressToContinue();
            RandomBasicCombatEncounter();
        }


      //Encounter Tools:

        //Metode til at vælge tilfældigt mellem encounters.
        public static void RandomEncounter() {
            //0, 125+1
            switch (Program.Rand.Next(0, 125 + 1)) {
                default:
                    RandomBasicCombatEncounter();
                    break;
                case int n when n <= 10 && Program.CurrentPlayer.Level > 1:
                    WizardEncounter();
                    break;
                case int n when 10 < n && n <=20 && Program.CurrentPlayer.Level > 1:
                    MimicEncounter();
                    break;
                case int n when 20 < n && n <=30 && Program.CurrentPlayer.Level > 1:
                    TreasureEncounter();
                    break;
                case int n when 30 < n && n <= 40:
                    PuzzleOneEncounter();
                    break;
            }
        }
        //Metode til at lave en pool af random encounters af tilfældig dybde/længde/antal op til 5 (default, bestemt antal gives som parametre).
        public static void RandomEncounterPool(int dybde = 0) {
            if (dybde == 0) {
                dybde = Program.Rand.Next(1, 5 +1);
            }
            for (int i=0; i <= dybde; i++) {
                RandomEncounter();
            }
        }
        //Metode til at vælge imellem story/NPC encounters, den bruges efter et sæt af randomencounters under Camp().
        public static void ProgressTheStory() {
            if (!Program.CurrentPlayer.FailedQuests.Exists(quest => quest.Name == "Free Flemsha") && !Program.CurrentPlayer.CompletedQuests.Exists(quest => quest.Name == "Free Flemsha")) {
                MeetFlemsha();
            }
        }
        //Metode til at køre Camp hvor spilleren kan reste/shoppe/heale
        public static void Camp() {
            Program.SoundController.Play("campfire");
            Program.SoundController.Play("campmusic");
            //Hver gang spilleren returnere til Camp refresher shoppen:
            Shop shop = Shop.SetForsale();
            HUDTools.TopCampHUD();
            while (true) {
                HUDTools.FullCampHUD();
                string input = TextInput.PlayerPrompt(true);
                //Explore, måden man progresser sin karakter:
                if (input == "e" || input == "explore") {                    
                    HUDTools.Print("You venture deeper...", 5);
                    TextInput.PressToContinue();
                    Program.SoundController.Stop();
                    bool explore = true;
                    while (explore) {
                        RandomEncounterPool();
                        ProgressTheStory();
                        Console.Clear();
                        HUDTools.Print("You gain a moment of respite and a choice...", 30);
                        HUDTools.Print("Do you venture deeper or turn back to your camp?", 25);
                        while (explore) {
                            HUDTools.RespiteHUD();
                            input = TextInput.PlayerPrompt(true);
                            if (input == "e" || input == "explore") {
                                HUDTools.Print("You venture deeper...", 5);
                                TextInput.PressToContinue();
                                break;
                            } else if (input == "r" || input == "return") {
                                explore = false;
                                HUDTools.Print("You retrace your steps in the darkness...", 20);
                                TextInput.PressToContinue();
                                if (Program.Rand.Next(100) > 49) {
                                    RandomBasicCombatEncounter();
                                }                              
                            } else {
                                Program.CurrentPlayer.BasicActions(input);
                            }
                        }
                    }
                    break;
                }
                //Gemmer spillet:
                else if (input == "s" || input == "sleep" || input == "quit" || input == "quit game") {
                    Program.Save();
                    HUDTools.Print("Game saved!");
                    TextInput.PressToContinue();                    
                }
                //Gheed's shop:
                else if (input == "g" || input == "gheed" || input == "gheed's shop" || input == "shop") {             
                    Program.SoundController.Stop();
                    Shop.Loadshop(Program.CurrentPlayer, shop);
                    Program.SoundController.Play("campfire");
                    Program.SoundController.Play("campmusic");
                }
                //Quit and/or save the game:
                else if (input == "q" || input == "quit") {
                    Program.Quit();
                }
                //Tale med NPC'er mens man er tilbage i campen.
                else if (input == "t" || input == "talk") {
                    TalkToNpc();
                }
                //Kalder metode til at tjekke input for, inventory, character, heale eller questloggen:
                else {
                    Program.CurrentPlayer.BasicActions(input);
                }
            }
        }
        //Funktion som kaldes under campen når spilleren skal snakke med de tilstedeværende personer.
        public static void TalkToNpc() {
            HUDTools.TalkToNpcHUD();
            while (true) {
                string input = TextInput.PlayerPrompt(true);
                if (int.TryParse(input, out int n) && n <= Program.CurrentPlayer.NpcsInCamp.Count && n >= 1) {
                    NonPlayableCharacters.LoadDialogueOptions(int.Parse(input) - 1);
                    HUDTools.TalkToNpcHUD();
                } else if (input == "b" || input == "back") {
                    break;
                } else {
                    HUDTools.Print("Not a valid input...");
                    TextInput.PressToContinue();
                    HUDTools.ClearLastLine(3);
                }
            }
        }
        //Funktion til at tilføje en NPC til campen som kan snakkes med.
        public static void AddNpcToCamp(string name) {
            var allNpcs = JsonSerializer.Deserialize<List<NonPlayableCharacters>>(HUDTools.ReadAllResourceText("Saga.Dungeon.Npcs.json"));
            var npcToAdd = allNpcs.Where(x => x.Name.Equals(name)).FirstOrDefault();
            npcToAdd.Greeting = npcToAdd.Greeting.Replace("playername", Program.CurrentPlayer.Name);
            Program.CurrentPlayer.NpcsInCamp.Add(npcToAdd);          
            NonPlayableCharacters.UpdateDialogueOptions(name);
            HUDTools.Print($"\u001b[35m{name} has joined your cause!\u001b[0m",20);
        }
        //Metode til at køre kamp
        public static void AdvancedCombat(Enemy Monster) {
            HUDTools.ClearLog();
            //Starter en tur tæller:
            Encounters TurnTimer = new();
            HUDTools.TopCombatHUD(Monster, TurnTimer);
            //Tjekker hvem starter if(spilleren starter), else (Fjenden starter):
            if (Program.CurrentPlayer.TotalSecondaryAttributes.Awareness > Monster.Awareness) {
                while (Monster.Health > 0 && TurnTimer.Ran == false) {
                    HUDTools.FullCombatHUD(Monster, TurnTimer);
                    Program.CurrentPlayer.CombatActions(Monster, TurnTimer);
                    if (TurnTimer.Ran == false) {
                        Monster.MonsterActions(TurnTimer);
                    }                   
                    Program.CurrentPlayer.CheckForDeath($"As the {Monster.Name} menacingly comes down to strike, you are slain by the mighty {Monster.Name}.");
                }
            }  else {
                while (Monster.Health > 0 && TurnTimer.Ran == false) {
                    HUDTools.FullCombatHUD(Monster, TurnTimer);
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
