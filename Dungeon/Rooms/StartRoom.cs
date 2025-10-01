using Saga.Assets;
using Saga.Dungeon.Enemies;
using Saga.Dungeon.Quests;
using Saga.Items;

namespace Saga.Dungeon.Rooms
{
    public class StartRoom : RoomBase
    {
        public StartRoom() {
            roomName = "Jail cells";
            description = "You look around and see Gheed rummage through big wooden crates. You hear him counting.";
            exits = [new Exit() { keyString = "door", exitDescription = $"You see a big wooden \u001b[96mdoor\u001b[0m with rusted hinges and reinforced with iron plating", valueRoom = RoomController.SecondRoom }];
        }
        public override void LoadRoom() {
            string exit = "";
            FirstEncounter();
            MeetGheed();
            HUDTools.RoomHUD();
            while (exit == "") {
                exit = TextInput.PlayerPrompt(true);
            }
            Program.RoomController.ChangeRoom(exit);

        }
        public static void FirstEncounter() {
            Console.Clear();
            Program.SoundController.Play("typewriter");
            HUDTools.Print("You awake in a cold and dark room. You feel dazed and are having trouble remembering");
            HUDTools.Print("anything about your past.");
            if (string.IsNullOrWhiteSpace(Program.CurrentPlayer.Name) == true) {
                HUDTools.Print("You can't even remember your own name...");
                Program.CurrentPlayer.Name = "Adventurer";
            } else {
                HUDTools.Print($"You know your name is {Program.CurrentPlayer.Name}.");
            }
            TextInput.PressToContinue();
            HUDTools.ClearLastLine(1);
            HUDTools.Print("You grope around in the darkness until you find a door handle. You feel some resistance as");
            HUDTools.Print("you turn the handle, but the rusty lock breaks with little effort. You see your captor");
            HUDTools.Print("standing with his back to you outside the door.");
            HUDTools.Print($"You throw open the door, grabbing a {(Program.CurrentPlayer.Equipment.Right_Hand as IItem)?.ItemName} then {(Program.CurrentPlayer.CurrentClass == "Mage" ? "preparing an incantation" : "")}{(Program.CurrentPlayer.CurrentClass == "Warrior" ? "charging toward your captor" : "")}{(Program.CurrentPlayer.CurrentClass == "Archer" ? "nocking an arrow" : "")}.", 30);
            Program.SoundController.Stop();
            Program.SoundController.Play("taunt");
            Program.SoundController.Play("kamp");
            HUDTools.Print("He turns...");
            TextInput.PressToContinue();

            EnemyBase captor = EnemyFactory.CreateByName("Human captor");
            new CombatController(Program.CurrentPlayer, captor).Combat();
        }
        public static void MeetGheed() {
            Console.Clear();
            Program.SoundController.Play("shop");
            Program.SoundController.Play("typewriter");
            if (Program.CurrentPlayer.CurrentClass == "Mage") {
                HUDTools.Print($"After dusting off your {(Program.CurrentPlayer.Equipment.Torso as IItem)?.ItemName} and tucking in your new wand, you find someone else captured.");
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
            HUDTools.Print($"You nod and prepare your {(Program.CurrentPlayer.Equipment.Right_Hand as IItem)?.ItemName}. You should start by \u001b[96mlooking around\u001b[0m...");
            TextInput.PressToContinue();
            Program.SoundController.Stop();
        }
    }
}
