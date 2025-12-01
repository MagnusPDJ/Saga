using Saga.Assets;
using Saga.Character;
using Saga.Dungeon.People;
using Saga.Dungeon.Quests;
using Saga.Items.Loot;

namespace Saga.Dungeon.Rooms
{
    public class CampRoom : RoomBase
    {
        public CampRoom() {
            RoomName = "Camp";
            Description = "";
        }
        public override void LoadRoom() {
            if (Program.CurrentPlayer.CurrentAct == Act.Start) {
                FirstCamp();
            }
            if (Program.CurrentPlayer.TimesExplored == 1) {
                FirstReturn();
            }
            string choice = RunCamp();
            Program.SoundController.Stop();
            if (choice == "quit") {
                Program.CurrentPlayer = new Warrior("Adventurer");
                Program.MainMenu();
            } else if (choice == "explore") {
                Program.RoomController.ExploreDungeon();
            }
        }
        public static string RunCamp() {
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
                    TextInput.PlayerPrompt("EventActions", input);
                    HUDTools.FullCampHUD();
                }
            }
            return choice;
        }
        public static void FirstCamp() {
            Console.Clear();
            Program.SoundController.Play("typewriter");
            HUDTools.Print(" You venture deeper and deeper, but while you explore your surroundings, you get queasy.");
            HUDTools.Print(" The dark and cold dungeon walls seem to creep closer, you feel claustrophobic.");
            TextInput.PressToContinue();
            HUDTools.ClearLastLine(1);
            Program.SoundController.Stop();
            Program.SoundController.Play("campfire");
            HUDTools.Print(" You hastily gather some old wood scattered about and make a campfire. The shadows retract and\n" +
                           " you feel at ease again. Although you are not out of danger, you can stay for a while and rest.");
            Program.CurrentPlayer.CurrentAct = Act.Act1;
            TextInput.PressToContinue();
        }
        public static void FirstReturn() {
            Console.Clear();
            Program.SoundController.Play("typewriter");
            Program.SoundController.Play("labyrinthchange");
            HUDTools.Print($" As you enter the camp and close the door behind you, everything shakes and there are loud\n" +
                           $" sounds of stone grinding against each other. Sand and pebbles fall from the ceiling and you\n" +
                           $" collapse to the floor from the vibrations.");
            HUDTools.Print($" After a few moments, you regain your composure and you check on Gheed.");
            TextInput.PressToContinue();
            HUDTools.ClearLastLine(1);
            HUDTools.Print($" 'What was that?', you ask, 'it sounded like an earthquake'.\n" +
                           $" 'Indeed', Gheed answers, 'Although, I suspect it wasn't destructive in nature. That is what makes\n" +
                           $" this labyrinth a prison for those who enter. When you open that door again, you will find that\n" +
                           $" all the rooms have changed.'");
            TextInput.PressToContinue();
            Program.SoundController.Stop();
        }
    }
}
