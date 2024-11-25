using System;
using System.Collections.Generic;
using System.Linq;
using Saga.assets;
using Saga.Items;
using Saga.Character;
using System.Runtime.InteropServices;
using System.Runtime.CompilerServices;

namespace Saga.Dungeon
{
    public class Encounters {
        public int TurnTimer { get; set; } = 1;
        public bool Ran { get; set; } = false;

      //Encounters:

        //De Encounters som køres når en ny karakter startes for at introducere kamp.
        public static void FirstEncounter() {
            Console.Clear();
            AudioManager.soundTypeWriter.Play();
            HUDTools.Print("You grope around in the darkness until you find a door handle. You feel some resistance as");
            HUDTools.Print("you turn the handle, but the rusty lock breaks with little effort. You see your captor");
            HUDTools.Print("standing with his back to you outside the door.");
            HUDTools.Print($"You throw open the door, grabbing a {Program.CurrentPlayer.Equipment[Slot.Weapon].ItemName} then {(Program.CurrentPlayer.CurrentClass == "Mage" ? "preparing an incantation" : "")}{(Program.CurrentPlayer.CurrentClass == "Warrior" ? "charging toward your captor" : "")}{(Program.CurrentPlayer.CurrentClass == "Archer" ? "nocking an arrow" : "")}.");
            AudioManager.soundMainMenu.Stop();
            AudioManager.soundTaunt.Play();
            AudioManager.soundKamp.Play();
            HUDTools.Print("He turns...");
            HUDTools.PlayerPrompt();
            Enemy FirstEncounter = new Act1Enemy("Human Captor", Tribe.Human) { 
                Health = 5,
                Power = 2,
            };
            AdvancedCombat(FirstEncounter);
        }
        public static void SecondEncounter() {
            Console.Clear();
            AudioManager.soundKamp.Play();
            switch (Program.rand.Next(0, 2)) {
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
            HUDTools.PlayerPrompt();
            AdvancedCombat(SecondEncounter);
        }
        //Encounter som køres der introducere shopkeeperen
        public static void FirstShopEncounter() {
            Console.Clear();
            AudioManager.soundShop.Play();
            AudioManager.soundTypeWriter.Play();
            if (Program.CurrentPlayer.CurrentClass == "Mage") {
                HUDTools.Print($"After dusting off your {Program.CurrentPlayer.Equipment[Slot.Torso].ItemName} and tucking in your new wand, you find someone else captured.");
            } else if (Program.CurrentPlayer.CurrentClass == "Archer") {
                HUDTools.Print("After retrieving the last arrow from your captor's corpse, you find someone else captured.");
            } else if (Program.CurrentPlayer.CurrentClass == "Warrior") {
                HUDTools.Print("After cleaning the blood from your captor off your new sword, you find someone else captured.");
            }
            HUDTools.Print("Freeing him from his shackles, he thanks you and gets up.");
            HUDTools.Print("'Gheed is the name and trade is my game', he gives a wink.");
            HUDTools.PlayerPrompt();
            Console.Clear();
            AudioManager.soundTypeWriter.Play();
            HUDTools.Print("'If you go and clear some of the other rooms, I will look for my wares in these crates.'");
            HUDTools.Print("'Then come back to me, I will then have been able to set up a shop where you can spend ");
            HUDTools.Print("some of that gold you are bound to have found,' he chuckles and rubs his hands at the thought.");
            HUDTools.Print($"You nod and prepare your {Program.CurrentPlayer.Equipment[Slot.Weapon].ItemName}, then you start walking down a dark corridor...");
            HUDTools.PlayerPrompt();
            AudioManager.soundShop.Stop();
        }
        //Encounter som køres der introducere Camp
        public static void FirstCamp() {
            Console.Clear();
            AudioManager.soundTypeWriter.Play();
            HUDTools.Print("After taking what few scraps you could find, you explore your surroundings.");
            HUDTools.Print("The dark and cold dungeon walls seem to creep closer, you feel claustrophobic.");
            Console.ReadKey(true);
            AudioManager.soundCampFire.Play();
            HUDTools.Print("You hastily gather some old wood scattered about and make a campfire. The");
            HUDTools.Print("shadows retract and you feel at ease again. Although you are not out of danger,");
            HUDTools.Print("you can stay for a while and rest.");
            HUDTools.PlayerPrompt();
        }


        //Story or NPC Encounters

        //Alchemist trader/Quest giver
        public static void MeetFlemsha() {
            Console.Clear();
            AudioManager.soundTypeWriter.Play();
            HUDTools.Print("You enter a dimly lit room, stone slab walls and iron bar grates make up some cells on either side of the room, there is also a desk which probably belonged to the long gone warden.",30);            
            bool examined = false;
            bool searched = false;
            while (true) {
                while (true) {
                    HUDTools.Print($"Do you {(!examined? "(1)examine desk? " : "")}{(!examined && !searched? "Or " : "")}{(!searched?"(2) search the prison cells?":"")}", 20);
                    string input = HUDTools.PlayerPrompt();
                    if (input == "1" && !examined) {
                        HUDTools.Print("You rummage through dusty documents and moldy records illegible or in unknown languages, but in a drawer you find some gold and a key.", 20);
                        Program.CurrentLoot.GetQuestLoot(1,0,"MeetFlemsha");
                        break;
                    } else if (input == "2" && !searched) {

                        break;
                    } else {
                        Program.CurrentPlayer.BasicActions(input);
                    }
                }
            }
        }


        //Random Encounters

        //Encounter der "spawner" en random fjende som skal dræbes.
        public static void RandomBasicCombatEncounter() {
            Console.Clear();
            AudioManager.soundKamp.Play();
            Act1Enemy RandomEnemy = Act1Enemy.CreateRandomAct1Enemy();
            switch (Program.rand.Next(0,2)) {
                case int x when (x == 0):
                    HUDTools.Print($"You turn a corner and there you see a {RandomEnemy.Name}...", 10);
                    break;
                case int x when (x == 1):
                    HUDTools.Print($"You break down a door and find a {RandomEnemy.Name} inside!", 10);
                    break;
            }
            HUDTools.PlayerPrompt();
            AdvancedCombat(RandomEnemy);
        }
        //Encounter der "spawner" en Dark Wizard som skal dræbes.
        public static void WizardEncounter() {
            Console.Clear();
            AudioManager.soundLaugh.Play();
            HUDTools.Print("The door slowly creaks open as you peer into the dark room. You see a tall man with a ",20);
            AudioManager.soundBossKamp.Play();
            HUDTools.Print("long beard and pointy hat, looking at a large tome.");
            HUDTools.PlayerPrompt();
            Enemy WizardEncounter = new Act1Enemy("Dark Wizard", Tribe.Human) {
                Health = 3 + 2 * Program.CurrentPlayer.Level + Program.CurrentPlayer.Level / 3,
                Power = 6 + 2 * Program.CurrentPlayer.Level,
                ExpModifier = 3,
            };
            AdvancedCombat(WizardEncounter);
        }
        //Encounter der "spawner" en Mimic som skal dræbes.
        public static void MimicEncounter() {
            string input;
            Console.Clear();
            AudioManager.soundDoorOpen.Play();
            HUDTools.Print("You open a door and find a treasure chest inside!");
            HUDTools.Print("Do you want to try and open it?\n(Y/N)");
            Console.Clear();
            do {
                Console.WriteLine("You open a door and find a treasure chest inside!");
                Console.WriteLine("Do you want to try and open it?\n(Y/N)");
                input = HUDTools.PlayerPrompt().ToLower();
                if (input == "n") {
                    AudioManager.soundDoorClose.Play();
                    HUDTools.Print("You slowly back out of the room and continue...", 20);
                    Console.ReadKey(true);
                    RandomBasicCombatEncounter();
                    break;
                } else if (input == "y") {
                    AudioManager.soundMimic.Play();
                    HUDTools.Print("As you touch the frame of the chest, it springs open splashing you with saliva!");
                    AudioManager.soundBossKamp.Play();
                    HUDTools.Print("Inside are multiple rows of sharp teeth and a swirling tongue that reaches for you.",15);
                    HUDTools.Print($"You ready your {Program.CurrentPlayer.Equipment[Slot.Weapon].ItemName}!",15);
                    HUDTools.PlayerPrompt();
                    Enemy MimicEncounter = new Act1Enemy("Mimic", Tribe.Mythical) {
                        Health = 10 + 2 * Program.CurrentPlayer.Level + Program.CurrentPlayer.Level / 3,
                        Power = 5 + Program.CurrentPlayer.Level + Program.CurrentPlayer.Level / 3,
                        GoldModifier = 3,
                    };
                    AdvancedCombat(MimicEncounter); 
                    break;
                } else {
                    HUDTools.Print("Invalid input");
                    HUDTools.PlayerPrompt();
                    Console.Clear();
                }
            } while (input != "42");
        }
        //Encounter der "spawner" en treasure chest.
        public static void TreasureEncounter() {
            string input;
            Console.Clear();
            AudioManager.soundDoorOpen.Play();            
            HUDTools.Print("You open a door and find a treasure chest inside!");
            HUDTools.Print("Do you want to try and open it?\n(Y/N)");
            Console.Clear();
            do {
                Console.WriteLine("You open a door and find a treasure chest inside!");
                Console.WriteLine("Do you want to try and open it?\n(Y/N)");
                input = HUDTools.PlayerPrompt().ToLower();
                if (input == "n") {
                    AudioManager.soundDoorClose.Play();
                    HUDTools.Print("You slowly back out of the room and continue...",20);
                    Console.ReadKey(true);
                    RandomBasicCombatEncounter();
                    break;
                } else if (input == "y") {
                    AudioManager.soundTreasure.Play();
                    HUDTools.Print("You release the metal latch and grab both sides of the chest and peer inside.");
                    HUDTools.PlayerPrompt();
                    AudioManager.soundWin.Play();
                    Program.CurrentLoot.GetTreasureChestLoot();
                    break;
                } else {
                    HUDTools.Print("Invalid input");
                    HUDTools.PlayerPrompt();
                    Console.Clear();
                }
            } while (input != "42");
        }
        //Encounter der starter en trap med runer hvor den rigtige rune skal vælges for at kunne exit
        public static void PuzzleOneEncounter() {
            Console.Clear();
            AudioManager.soundFootsteps.Play();
            AudioManager.soundRuneTrap.Play();

            //runer
            List<char> chars = new char[] { '\u0925', '\u0931', '\u09fa', '\u1bd1', '\u1bfe', '\u0166','\u017f','\u018d','\u0195','\u01a7' }.ToList();
            List<char> endchars = new char[] { '\u00fe', '\u00f5', '\u00d0', '\u0141', '\u014a', '\u1c07', '\u1c1c', '\u1c59', '\u1c6c', '\u1cbe' }.ToList();
            List<int> positions = new List<int>();
            char c = chars[Program.rand.Next(0, 10)];
            chars.Remove(c);
            
            //Rune template
            List<string> puzzle = new List<string>();

            for (int a = 0; a < 4; a++) {
                int pos = Program.rand.Next(0, 4);
                positions.Add(pos);
                string row = "";
                for (int b = 0; b < 4; b++) {
                    if (b == pos) {
                        row += c + " ";
                    } else if (0<a && a<3){
                        row += chars[Program.rand.Next(0, 9)] + " ";
                    } else {
                        row += endchars[Program.rand.Next(0, 9)] + " ";
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
                string input = HUDTools.PlayerPrompt();
                if (int.TryParse(input, out int number) && number < 5 && number > 0) {
                    if (positions[i] == number - 1) {
                        AudioManager.soundFootsteps.Play();
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
                        Console.ReadKey(true);
                    } else {
                        AudioManager.soundDarts.Play();
                        HUDTools.Print($"Darts fly out of the walls! You take 2 damage.)", 10);
                        Program.CurrentPlayer.Health -= 2;
                        Console.ReadKey(true);
                        if (Program.CurrentPlayer.Health <= 0) {
                            Program.CurrentPlayer.DeathCode("You start to feel sick. The poison from the darts slowly kills you");
                        }
                    }
                } else if (int.TryParse(input, out _)) {
                    Console.WriteLine("Invalid Input: Whole numbers 1-4 only");
                    Console.ReadKey(true);
                } else {
                    Program.CurrentPlayer.BasicActions(input);
                }               
            }
            AudioManager.soundRuneTrap.Stop();
            AudioManager.soundWin.Play();
            Program.CurrentLoot.GetExp(2, 50*Program.CurrentPlayer.Level);
            Console.ResetColor();
            HUDTools.PlayerPrompt();
            RandomBasicCombatEncounter();
        }




      //Encounter Tools:

        //Metode til at vælge tilfældigt mellem encounters.
        public static void RandomEncounter() {
            //0, 125+1
            switch (Program.rand.Next(0, 125 + 1)) {
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
     
        //Metode til at køre Camp hvor spilleren kan reste/shoppe/heale
        public static void Camp() {
            AudioManager.soundCampFire.Play();
            AudioManager.soundCampMusic.Play();
            Shop shop = Shop.SetForsale();
            HUDTools.TopCampHUD();
            while (true) {
                HUDTools.InstantCampHUD();
                string input = HUDTools.PlayerPrompt();
                if (input == "e" || input == "explore") {
                    //Explore
                    HUDTools.Print("You venture deeper...", 5);
                    Console.ReadKey(true);
                    AudioManager.soundCampFire.Stop();
                    AudioManager.soundCampMusic.Stop();
                    bool stay = true;
                    while (stay) {
                        int dybde = Program.rand.Next(0, 4);
                        switch (dybde) {
                            case 0:
                                for (int i = 0; i < 2; i++) {
                                    RandomEncounter();
                                }
                                break;
                            case 1:
                                for (int i = 0; i < 3; i++) {
                                    RandomEncounter();
                                }
                                break;
                            case 2:
                                for (int i = 0; i < 4; i++) {
                                    RandomEncounter();
                                }
                                break;
                            case 3:
                                for (int i = 0; i < 5; i++) {
                                    RandomEncounter();
                                }
                                break;
                        }
                        Console.Clear();
                        HUDTools.Print("You gain a moment of respite and a choice...", 30);
                        HUDTools.Print("Do you venture deeper or turn back to your camp?", 25);
                        while (stay) {
                            HUDTools.RespiteHUD();
                            input = HUDTools.PlayerPrompt();
                            if (input == "e" || input == "explore") {
                                HUDTools.Print("You venture deeper...", 5);
                                HUDTools.PlayerPrompt();
                                break;
                            } else if (input == "r" || input == "return") {
                                stay = false;
                                HUDTools.Print("You retrace your steps in the darkness...", 20);
                                HUDTools.PlayerPrompt();
                            } else {
                                Program.CurrentPlayer.BasicActions(input);
                            }
                        }
                    }
                    break;
                } else if (input == "s" || input == "sleep" || input == "quit" || input == "quit game") {
                    //Sleep/save Game
                    Program.Save();
                    HUDTools.Print("Game saved!");
                    HUDTools.PlayerPrompt();
                } else if (input == "g" || input == "gheed" || input == "gheed's shop" || input == "shop") {
                    //Gheed's shop
                    AudioManager.soundCampFire.Stop();
                    AudioManager.soundCampMusic.Stop();
                    Shop.Loadshop(Program.CurrentPlayer, shop);
                    AudioManager.soundCampFire.Play();
                    AudioManager.soundCampMusic.Play();
                } else if (input == "q" || input == "quit") {
                    Program.Quit();
                } else {
                    Program.CurrentPlayer.BasicActions(input);
                }
            }
        }

        //Metode til at køre kamp
        public static void AdvancedCombat(Enemy Monster) {
            HUDTools.ClearLog();
            Encounters TurnTimer = new Encounters();
            HUDTools.TopCombatHUD(Monster, TurnTimer);
            if (Program.CurrentPlayer.TotalSecondaryAttributes.Awareness > 0) {
                while (Monster.Health > 0 && TurnTimer.Ran == false) {
                    HUDTools.FullCombatHUD(Monster, TurnTimer);
                    Program.CurrentPlayer.CombatActions(Monster, TurnTimer);
                    if (TurnTimer.Ran == false) {
                        Monster.MonsterActions(TurnTimer);
                    }                   
                    if (Program.CurrentPlayer.Health <= 0) {
                        HUDTools.ClearLog();
                        AudioManager.soundKamp.Stop();
                        AudioManager.soundBossKamp.Stop();
                        Program.CurrentPlayer.DeathCode($"As the {Monster.Name} menacingly comes down to strike, you are slain by the mighty {Monster.Name}.");
                        break;
                    }
                }
            }  else {
                while (Monster.Health > 0 && TurnTimer.Ran == false) {
                    HUDTools.FullCombatHUD(Monster, TurnTimer);
                    Monster.MonsterActions(TurnTimer);
                    if (Program.CurrentPlayer.Health <= 0) {
                        HUDTools.ClearLog();
                        AudioManager.soundKamp.Stop();
                        AudioManager.soundBossKamp.Stop();
                        Program.CurrentPlayer.DeathCode($"As the {Monster.Name} menacingly comes down to strike, you are slain by the mighty {Monster.Name}.");
                        break;
                    }
                    Program.CurrentPlayer.CombatActions(Monster, TurnTimer);
                }
            }
            if (Monster.Health <= 0) {
                AudioManager.soundKamp.Stop();
                AudioManager.soundBossKamp.Stop();
                HUDTools.ClearLog();
                AudioManager.soundWin.Play();
                Program.CurrentLoot.GetCombatLoot(Monster, $"You Won against the {Monster.Name} on turn {TurnTimer.TurnTimer - 1}!");
            }
        }
    }
}
