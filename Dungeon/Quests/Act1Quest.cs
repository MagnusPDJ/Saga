using Saga.Assets;
using Saga.Items.Loot;
using Saga.Items;
using Saga.Dungeon.Enemies;
using System.Text.Json;

namespace Saga.Dungeon.Quests
{
    internal class Act1Quest : Quest
    {
        public static void AddQuest(string questName) {
            var allQuests = JsonSerializer.Deserialize<List<Act1Quest>>(HUDTools.ReadAllResourceText("Saga.Dungeon.Quests.Act1Quests.json"), Program.Options) ?? [];
            var questToAdd = allQuests.Where(x => x.Name.Equals(questName)).FirstOrDefault();
            if (questToAdd != null) {
                if (questToAdd.Item?.ItemName == "Random") {
                    var item = Shop.CreateRandomArmor(0, Program.CurrentPlayer.CurrentClass == "Warrior" || Program.CurrentPlayer.CurrentClass == "Archer" ? 2 : 0);
                    ((IArmor)item).SetPrimaryAffixes();
                    ((IArmor)item).SetSecondaryAffixes();
                    item.ItemPrice = item.CalculateItemPrice();
                    questToAdd.Item = item;
                }
                Program.CurrentPlayer.QuestLog.Add(questToAdd);
                HUDTools.Print($"\u001b[96m You've gained a quest: {questToAdd.Name}!\u001b[0m");
            } 
        }
        public static void FailQuest(string questName) {
            var allQuests = JsonSerializer.Deserialize<List<Act1Quest>>(HUDTools.ReadAllResourceText("Saga.Dungeon.Quests.Act1Quests.json"), Program.Options) ?? [];
            var questToAdd = allQuests.Where(x => x.Name.Equals(questName)).FirstOrDefault();
            if (questToAdd != null) {
                Program.CurrentPlayer.FailedQuests.Add(questToAdd);
            }
        }
        //Funktion til at lave random collect quests.
        public static Quest CreateRandomQuest() {
            int roll = Program.Rand.Next(4);
            string target = "";
            string name = "";
            (int,int,List<(PotionType,int)>) reward = (0, 0, []);
            int amount = 3 + Program.CurrentPlayer.Level / 2;

            switch (roll) {
                case 0:
                    target = "rattail"; name = "Collect rat tails"; reward = (20 * amount * Math.Max(1, Program.CurrentPlayer.Level/2), 50 * Program.CurrentPlayer.Level, []);break;
                case 1:
                    target = "batwings"; name = "Collect bat wings"; reward = (0, 50 * Program.CurrentPlayer.Level, [(PotionType.Healing, 3 + Program.CurrentPlayer.Level)]);break;
                case 2:
                    target = "candle"; name = "Collect candles"; reward = (30 * amount * Math.Max(1, Program.CurrentPlayer.Level / 2), 25 * Program.CurrentPlayer.Level, []);break;
                case 3:
                    target = "greenslime"; name = "Collect green slime"; reward = (0, 50 * Program.CurrentPlayer.Level, [(PotionType.Mana, 3 + Program.CurrentPlayer.Level)]);break;
            }
            
            Quest quest = new Act1Quest() {
                Name = name,
                QuestType = Type.Collect,              
                Giver = "Flemsha",
                Objective = $"Flemsha wants you to collect {amount} {target}.",
                TurnIn = $"You have the {amount} {target}. Return to Flemsha for your reward.",
                Gold = reward.Item1,
                Exp = reward.Item2,
                Potions = reward.Item3,
                Requirements = new Dictionary<string, int> { {target, amount} }
            };
            return quest;
        }
        public static void GainQuestProgress(EnemyBase enemy) {
            if (enemy != null) {
                Quest? found;
                if ((found = Program.CurrentPlayer.QuestLog.Find(x => x.QuestType == Type.Elimination && x.Target == "Enemy" && x.Completed != true)) != null) {
                    found.Amount++;
                }
            }
            Program.CurrentPlayer.UpdateQuestLog();
        } 
    }
}
