using Saga.Assets;
using Saga.Items.Loot;

namespace Saga.Dungeon.Rooms
{
    public class HallwayRoom : RoomBase
    {
        private const double EnemySpawnChance = 0.10;
        private const double PuzzleChance = 0.75;
        public HallwayRoom(string name, string desc) {
            RoomName = name;
            Description = desc;
            EntranceDescription = " As you are walking down the dark corridors, you see that the floor is suddenly covered in runes,\n" +
                                  " so you decide to tread carefully.";
            MaxExits = 2;
        }
        public override void LoadRoom() {
            if (!Visited) Visited = true;

            if (!EnemySpawned && !Cleared) {
                double rollForEncounter = Program.Rand.NextDouble();
                if (rollForEncounter <= EnemySpawnChance) {
                    RandomCombatEncounter();
                    if (Program.RoomController.Ran == true) {
                        Program.RoomController.Ran = false;
                        EntranceDescription = $" You return to the room where you left the {Enemy!.Name}...";
                        Program.RoomController.ChangeRoom(Exits[0].keyString);
                    } else {
                        Cleared = true;
                        CorpseDescription = Enemy!.EnemyCorpseDescription;
                        Enemy = null;
                    }
                } else if (rollForEncounter <= PuzzleChance) {
                    RunePuzzleEncounter();
                } else {
                    Cleared = true;
                }
            } else if (Enemy != null) {
                HUDTools.RoomHUD(true);
                HUDTools.ClearLastLine(1);
                TextInput.PressToContinue();
                new CombatController(Program.CurrentPlayer, Enemy).Combat();
                if (Program.RoomController.Ran == true) {
                    Program.RoomController.Ran = false;
                    Program.RoomController.ChangeRoom(Exits[0].keyString);
                } else {
                    Cleared = true;
                    CorpseDescription = Enemy!.EnemyCorpseDescription;
                    Enemy = null;
                }
            }
            
            IdleInRoom();
        }
        public void RunePuzzleEncounter() {
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
                    } else if (0 < a && a < 3) {
                        row += chars[Program.Rand.Next(0, 9)] + " ";
                    } else {
                        row += endchars[Program.Rand.Next(0, 9)] + " ";
                    }
                }
                puzzle.Add(row);
            }

            //slow print:
            HUDTools.RoomHUD(true);
            HUDTools.ClearLastLine(1);     
            HUDTools.Print("    o    <- starting position", 5);
            for (int j = 0; j < 4; j++) {
                HUDTools.Print(" " + puzzle[j] + "\n", 10);
            }           
            //Player action sequence:
            string location = "";
            for (int i = 0; i < 4;) {
                HUDTools.RoomHUD(true);
                HUDTools.ClearLastLine(1);
                if (i == 0) {
                    HUDTools.Print("    o    <- starting position", 0);
                    for (int j = 0; j < 4; j++) {
                        HUDTools.Print(" " + puzzle[j] + "\n", 0);
                    }
                    HUDTools.Print(" Choose your path (each rune position corresponds to a number 1-4):", 0);
                } else {                    
                    for (int j = 0; j < 4; j++) {
                        if (i == j) {
                            Console.Write($" {location} <- Your position");
                        }
                        HUDTools.Print("\n " + puzzle[j], 0);
                    }
                    Console.WriteLine();
                    HUDTools.Print(" Choose your path (each rune position corresponds to a number 1-4):", 0);
                }                
                string input = TextInput.UserKeyInput();
                if (int.TryParse(input, out int number) && number < 5 && number > 0) {
                    if (positions[i] == number - 1) {
                        Program.SoundController.Play("footsteps");
                        HUDTools.Print($" You stepped on the {c} rune, nothing happens...", 10);
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
                        HUDTools.Print($" Darts fly out of the walls! You take 2 damage.)", 10);
                        Program.CurrentPlayer.TakeDamage(2);
                        TextInput.PressToContinue();
                        Program.CurrentPlayer.CheckForDeath(" You start to feel sick. The poison from the darts slowly kills you");
                    }
                } else if (int.TryParse(input, out _)) {
                    Console.WriteLine(" Invalid Input: Whole numbers 1-4 only");
                    TextInput.PressToContinue();
                } else {
                    TextInput.SelectPlayerAction(0, input);
                }
            }
            HUDTools.ClearLastLine(1);
            Program.SoundController.Stop();
            Program.SoundController.Play("win");
            CorpseDescription = " The runes you crossed cover the lenght of the floor. The trap seems inert now.";
            Cleared = true;
            LootSystem.GetFixedExp(70 * Program.CurrentPlayer.Level);
            TextInput.PressToContinue();
            HUDTools.ClearLastLine(16);
        }
    }
}
