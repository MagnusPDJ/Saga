using Saga.Assets;
using Saga.Dungeon.Quests;
using System.Text.Json;

namespace Saga.Dungeon.People 
{
    public class NonPlayableCharacters {
        public required string Name { get; set; }
        public required string Greeting { get; set; }
        public List<Quest> AvailableQuests { get; set; } = [];
        public Dictionary<string, string> Dialogue { get; set; } = [];
        //Funktion som kaldes under campen når spilleren skal snakke med de tilstedeværende personer.
        public static void TalkToNpc() {
            HUDTools.TalkToNpcHUD();
            while (true) {
                string input = TextInput.PlayerPrompt();
                if (int.TryParse(input, out int n) && n <= Program.CurrentPlayer.NpcsInCamp.Count && n >= 1) {
                    LoadDialogueOptions(int.Parse(input) - 1);
                    HUDTools.TalkToNpcHUD();
                }
                else if (input == "b" || input == "back") {
                    break;
                }
                else {
                    HUDTools.Print("Not a valid input...");
                    TextInput.PressToContinue();
                    HUDTools.ClearLastLine(3);
                }
            }
        }
        //Funktion til at tilføje en NPC til campen som kan snakkes med.
        public static void AddNpcToCamp(string name) {
            var allNpcs = JsonSerializer.Deserialize<List<NonPlayableCharacters>>(HUDTools.ReadAllResourceText("Saga.Dungeon.People.NpcsDatabase.json"), Program.Options) ?? [];
            var npcToAdd = allNpcs.FirstOrDefault(x => x.Name.Equals(name));
            if (npcToAdd != null) {
                npcToAdd.Greeting = npcToAdd.Greeting.Replace("playername", Program.CurrentPlayer.Name);
                Program.CurrentPlayer.NpcsInCamp.Add(npcToAdd);
                UpdateDialogueOptions(name);
                HUDTools.Print($" \u001b[35m{name} has joined your cause!\u001b[0m", 20);
            }
        }
        //Metode til at opdatere dialog mulighederne baseret på valg taget igennem spillet.
        public static void UpdateDialogueOptions(string instance) {
            List<string> lines = HUDTools.ReadAllResourceLines("Saga.Dungeon.People.Dialogue.txt");
            lines.RemoveAt(0);

            foreach (NonPlayableCharacters npc in Program.CurrentPlayer.NpcsInCamp) {
                foreach(string line in lines) {
                    string[] parts = line.Split(';');
                    if (parts[0] == npc.Name && parts[1] == instance) {
                        parts[3] = parts[3].Replace("\\n", "\n");
                        npc.Dialogue[$"{parts[2]}"] = $"{parts[3]}";                     
                    }
                }
            }
        }
        //Metode til at vise alle dialog valg mulighederne.
        public static void LoadDialogueOptions(int index) {
            NonPlayableCharacters talkto = Program.CurrentPlayer.NpcsInCamp[index];
            List<string> questions = [];
            List<string> answers = [];
            foreach (KeyValuePair<string, string> option in talkto.Dialogue) {
                questions.Add($"{option.Key}");
                answers.Add($"{option.Value}");
            }
            bool donetalking = false;
            while (donetalking == false) {
                Console.Clear();
                HUDTools.Print($"{talkto.Greeting}\t(b)ack", 20);
                foreach (string question in questions) {
                    HUDTools.Print($"{questions.IndexOf(question) + 1}:\t{question}", 15);
                }
                //Quest dialogue options:               
                if (talkto.Name == "Flemsha" && talkto.AvailableQuests.Count == 0 && Program.CurrentPlayer.Level < 15) {
                    talkto.AvailableQuests.Add(Act1Quest.CreateRandomQuest());
                    HUDTools.Print($"{questions.Count + 1}:\t\u001b[96mDo you have any work?\u001b[0m", 15);
                } else if (talkto.Name == "Flemsha" && talkto.AvailableQuests.Count == 0 && Program.CurrentPlayer.Level >= 15) { 
                    //No quests made
                } else if (talkto.Name == "Flemsha" && talkto.AvailableQuests[0].Accepted == false) {
                    HUDTools.Print($"{questions.Count + 1}:\t\u001b[96mDo you have any work?\u001b[0m", 15);
                } else if (talkto.Name == "Flemsha" && Program.CurrentPlayer.QuestLog.Exists(quest => quest.Name == talkto.AvailableQuests[0].Name && quest.Completed == true)) {
                    HUDTools.Print($"{questions.Count + 1}:\t\u001b[96mI have your items.\u001b[0m", 15);
                }
                if (talkto.Name == "Gheed" && Program.CurrentPlayer.QuestLog.Exists(quest => quest.Giver == talkto.Name && quest.Completed == true)) {
                    HUDTools.Print($"{questions.Count +1}:\t\u001b[96mI did what you asked.\u001b[0m", 15);
                }
                
                while (true) {
                    (int, int) startCursor = Console.GetCursorPosition();
                    string input = TextInput.PlayerPrompt();
                    if (input == "b" || input == "back") {
                        donetalking = true;
                        Quest.UpdateQuestLog(Program.CurrentPlayer);
                        break;
                    } else if (int.TryParse(input, out int n) && n >= 1 && n <= questions.Count) {
                        n--;
                        Console.SetCursorPosition(0, 1 + n);
                        HUDTools.Print($"\u001b[90m{n + 1}:\t{questions[n]}\u001b[0m", 0);
                        Console.SetCursorPosition(0, startCursor.Item2+1);
                        HUDTools.Print($"{questions[n]}", 0);
                        HUDTools.Print($"{answers[n]}\n", 20);
                        TextInput.PressToContinue();
                        HUDTools.ClearLastText(startCursor);

                    } else if (int.TryParse(input, out int n1) && n1 <= questions.Count + 1) {
                        n1--;

                        //Flemsha Quest Dialogue:
                        if(talkto.Name == "Flemsha") {
                            if (Program.CurrentPlayer.QuestLog.Exists(quest => quest.Name == talkto.AvailableQuests[0].Name && quest.Completed == true)) {
                                HUDTools.Print($"I have your items.", 0);
                                HUDTools.Print($"Thanks alot {Program.CurrentPlayer.Name}", 20);
                                var questToAdd = Program.CurrentPlayer.QuestLog.Find(quest => quest.Name == talkto.AvailableQuests[0].Name);
                                if (questToAdd != null) {
                                    Quest.TurnInQuest(Program.CurrentPlayer, questToAdd);
                                }
                                talkto.AvailableQuests.RemoveAt(0);
                                TextInput.PressToContinue();
                                HUDTools.ClearLastText((startCursor.Item1, startCursor.Item2 - 1));
                            }
                            else if (talkto.AvailableQuests.Count != 0 && talkto.AvailableQuests[0].Accepted == false) {
                                HUDTools.Print($"Do you have any work?", 0);
                                HUDTools.Print($"Yes, if you could go and {talkto.AvailableQuests[0].Name}, I will make it worth your while.\n(Y)es to accept (n)o to decline.", 15);
                                while (true) {
                                    string input1 = TextInput.PlayerPrompt();
                                    if (input1 == "y") {
                                        Program.CurrentPlayer.QuestLog.Add(talkto.AvailableQuests[0]);
                                        talkto.AvailableQuests[0].Accepted = true;
                                        HUDTools.Print($"\u001b[96mYou've gained a quest: {talkto.AvailableQuests[0].Name}\u001b[0m!", 20);
                                        TextInput.PressToContinue();
                                        HUDTools.ClearLastText((startCursor.Item1, startCursor.Item2 - 1));
                                        break;
                                    }
                                    else if (input1 == "n") {
                                        talkto.AvailableQuests.RemoveAt(0);
                                        if (Program.CurrentPlayer.Level < 15) {
                                            talkto.AvailableQuests.Add(Act1Quest.CreateRandomQuest());
                                        }
                                        else if (Program.CurrentPlayer.Level >= 15) {
                                            //No quests made
                                        }
                                        HUDTools.ClearLastText(startCursor);
                                        break;
                                    }
                                    else {
                                        HUDTools.Print("Please select (Y)es or (N)o.", 15);
                                        TextInput.PressToContinue();
                                        HUDTools.ClearLastLine(3);
                                    }
                                }
                            } else {
                                HUDTools.Print("Please select a number from the list or (b)ack.", 15);
                                TextInput.PressToContinue();
                                HUDTools.ClearLastLine(3);
                            }
                        } else if (talkto.Name == "Gheed") {
                            //Gheed Quest Dialogue:
                            Quest? found;
                            if ((found = Program.CurrentPlayer.QuestLog.Find(quest => quest.Giver == "Gheed" && quest.Completed == true)) != null) {
                                HUDTools.Print($"I did what you asked.", 0);
                                HUDTools.Print($"Magnificent {Program.CurrentPlayer.Name}!", 20);
                                Quest.TurnInQuest(Program.CurrentPlayer, found);
                                TextInput.PressToContinue();
                                HUDTools.ClearLastText((startCursor.Item1, startCursor.Item2 - 1));
                            } else {
                                HUDTools.Print("Please select a number from the list or (b)ack.", 15);
                                TextInput.PressToContinue();
                                HUDTools.ClearLastLine(3);
                            }
                        }
                    } else {
                        HUDTools.Print("Please select a number from the list or (b)ack.", 15);
                        TextInput.PressToContinue();
                        HUDTools.ClearLastLine(3);
                    }
                }
            }
        }
    }
}
