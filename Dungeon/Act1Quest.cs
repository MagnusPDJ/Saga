using Saga.Assets;
using Saga.Items.Loot;
using System.Collections.Generic;
using System.Text.Json;
using System.Linq;

namespace Saga.Dungeon
{
    internal class Act1Quest : Quest
    {
        public static void AddQuest(string questName) {
            var allQuests = JsonSerializer.Deserialize<List<Act1Quest>>(HUDTools.ReadAllResourceText("Saga.Dungeon.Act1Quests.json"));
            var questToAdd = allQuests.Where(x => x.Name.Equals(questName)).FirstOrDefault();
            if (questToAdd != null && questToAdd.Item?.ItemName == "Random") {
                questToAdd.Item = ArmorLootTable.CreateRandomArmor(0, Program.CurrentPlayer.CurrentClass == "Warrior" || Program.CurrentPlayer.CurrentClass == "Archer" ? 2 : 0);
            }
            Program.CurrentPlayer.QuestLog.Add(questToAdd);
            HUDTools.Print($"\u001b[96mYou've gained a quest: {questToAdd.Name}!\u001b[0m");
        }
        public static void FailQuest(string questName) {
            var allQuests = JsonSerializer.Deserialize<List<Act1Quest>>(HUDTools.ReadAllResourceText("Saga.Dungeon.Act1Quests.json"));
            var questToAdd = allQuests.Where(x => x.Name.Equals(questName)).FirstOrDefault();
            Program.CurrentPlayer.FailedQuests.Add(questToAdd);
        }
        //Funktion til at lave random collect quests.
        public static Quest CreateRandomQuest() {
            int roll = Program.Rand.Next(2);
            string target = "";
            string name = "";
            (int,int,int) reward = (0,0,-1);
            if (roll == 0) { target = "Rat tail"; name = "Collect rat tails"; reward = (50*Program.CurrentPlayer.Level, 50 * Program.CurrentPlayer.Level, -1); } 
            else if (roll == 1) { target = "Bat wings"; name = "Collect bat wings"; reward = (0, 50 * Program.CurrentPlayer.Level, 3+Program.CurrentPlayer.Level); }
            int amount = 3 + Program.CurrentPlayer.Level/2;

            Quest quest = new Act1Quest() {
                Name = name,
                QuestType = Type.Collect,
                Target = target,
                Giver = "Flemsha",
                Objective = $"Flemsha wants you to collect {amount} {target}",
                TurnIn = $"You have the {amount} {target}. Return to Flemsha for your reward",
                Gold = reward.Item1,
                Exp = reward.Item2,
                Potions = reward.Item3,
                Requirements = new Dictionary<string, int> { {target, amount} }
            };
            return quest;
        }
    }
}
