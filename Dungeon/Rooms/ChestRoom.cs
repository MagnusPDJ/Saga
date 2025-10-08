using Saga.Assets;
using Saga.Dungeon.Enemies;
using Saga.Items;
using Saga.Items.Loot;

namespace Saga.Dungeon.Rooms
{
    public class ChestRoom : RoomBase
    {
        private bool chestOpened = false;
        public ChestRoom(string name, string desc) {
            RoomName = name;
            Description = desc;
        }
        public override void LoadRoom() {
            if (!Visited) Visited = true;

            //Spawn Enemy
            if (!EnemySpawned && !Cleared) {               
                if (!chestOpened && Enemy == null) {
                    Encounter();
                    if (Program.RoomController.Ran == true) {
                        Program.RoomController.Ran = false;
                        Program.RoomController.ChangeRoom(Exits[0].keyString);
                    } else {
                        Cleared = true;
                        if (Enemy != null) {
                            CorpseDescription = Enemy!.EnemyCorpseDescription;
                            Enemy = null;
                        } else {
                            CorpseDescription = " The treasure chest stands with an open lid and looted for anything valuable.";
                        }
                        
                    }
                } else if (Enemy != null) {
                    HUDTools.RoomHUD();
                    HUDTools.ClearLastLine(1);
                    HUDTools.Print($" You return to the room where you left the {Enemy.Name}...", 10);
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
            }

            string exit = "";
            HUDTools.RoomHUD();
            while (exit == "") {
                exit = TextInput.PlayerPrompt(true);
            }
            Program.RoomController.ChangeRoom(exit);
        }
        //Encounter der "spawner" en Mimic som skal dræbes.
        public void Encounter() {
            Console.Clear();
            Program.SoundController.Play("dooropen");
            HUDTools.RoomHUD();
            HUDTools.ClearLastLine(1);
            HUDTools.Print(" You open a door and find a treasure chest inside!");
            HUDTools.Print(" Do you want to try and open it?\n(Y/N)");
            while (true) {
                string input = TextInput.PlayerPrompt();
                if (input == "n") {
                    HUDTools.Print(" You decide to leave the chest for now...", 20);
                    TextInput.PressToContinue();
                    
                    break;
                } else if (input == "y") {
                    chestOpened = true;
                    if (Program.Rand.NextDouble() < 0.5f) {
                        EnemySpawned = true;
                        Program.SoundController.Play("mimic");
                        HUDTools.Print(" As you touch the frame of the chest, it springs open splashing you with saliva!");
                        Program.SoundController.Play("troldmandskamp");
                        HUDTools.Print(" Inside are multiple rows of sharp teeth and a swirling tongue that reaches for you.", 15);
                        HUDTools.Print($" You ready your {(Program.CurrentPlayer.Equipment.Right_Hand as IItem)?.ItemName}!", 15);
                        TextInput.PressToContinue();

                        Enemy = EnemyFactory.CreateByName("Mimic");
                        new CombatController(Program.CurrentPlayer, Enemy).Combat();
                        break;
                    } else {
                        Program.SoundController.Play("treasure");
                        HUDTools.Print(" You release the metal latch and grab both sides of the chest and peer inside.");
                        TextInput.PressToContinue();
                        HUDTools.ClearLastLine(1);
                        Program.SoundController.Play("win");
                        LootSystem.GetTreasureChestLoot();
                        break;
                    }
                } else {
                    HUDTools.Print("Invalid input");
                    TextInput.PressToContinue();
                    HUDTools.ClearLastLine(3);
                }
            }
        }
    }
}
