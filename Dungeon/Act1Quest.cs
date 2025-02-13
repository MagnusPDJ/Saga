using System.Collections.Generic;

namespace Saga.Dungeon
{
    internal class Act1Quest : Quest
    {
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
                Amount = amount,
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

        //Quests:
        public static Act1Quest FreeFlemsha = new Act1Quest() {
            Name = "Free Flemsha",
            QuestType = Type.Find,
            Target = "Old key",
            Giver = "Flemsha",
            Objective = "You've met a prisoner who calls himself Flemsha. He claims if you free him,\nhe will offer his alchemical expertise to your cause.",
            TurnIn = "You have found an old key, it is likely a prison key.\nYou should return to Flemsha's cell.",
            Gold = 100,
            Exp = 100,
            Requirements = new Dictionary<string, int> { {"Old key", 1} }
        };
    }
}
