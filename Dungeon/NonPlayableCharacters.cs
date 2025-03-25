using System;
using System.Collections.Generic;
using Saga.Assets;

namespace Saga.Dungeon 
{
    public class NonPlayableCharacters {
        public string Name { get; set; }
        public string Greeting { get; set; }
        public List<Quest> AvailableQuests { get; set; }
        public Dictionary<string, string> Dialogue { get; set; }

        //Metode til at opdatere dialog mulighederne baseret på valg taget igennem spillet.
        public static void UpdateDialogueOptions(string instance) {
            List<string> lines = HUDTools.ReadAllResourceLines("Saga.Dungeon.Dialogue.txt");
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
                if (talkto.Name == "Flemsha" && Program.CurrentPlayer.QuestLog.Exists(quest => quest.Name == talkto.AvailableQuests[0].Name && quest.Completed == true)) {
                    HUDTools.Print($"{questions.Count + 1}:\t\u001b[96mI have your items.\u001b[0m", 15);
                }
                if (talkto.Name == "Flemsha" && talkto.AvailableQuests.Count == 0 && Program.CurrentPlayer.Level < 15) {
                    talkto.AvailableQuests.Add(Act1Quest.CreateRandomQuest());
                    HUDTools.Print($"{questions.Count + 1}:\t\u001b[96mDo you have any work?\u001b[0m", 15);
                } else if (talkto.Name == "Flemsha" && talkto.AvailableQuests.Count == 0 && Program.CurrentPlayer.Level >= 15) { 
                    //No quests made
                } else if (talkto.Name == "Flemsha" && talkto.AvailableQuests[0].Accepted == false) {
                    HUDTools.Print($"{questions.Count + 1}:\t\u001b[96mDo you have any work?\u001b[0m", 15);
                }
                
                while (true) {
                    (int, int) startCursor = Console.GetCursorPosition();
                    string input = TextInput.PlayerPrompt(true);
                    if (input == "b" || input == "back") {
                        donetalking = true;
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
                        if (Program.CurrentPlayer.QuestLog.Exists(quest => quest.Name == talkto.AvailableQuests[0].Name && quest.Completed == true)) {
                            HUDTools.Print($"I have your items.", 0);
                            HUDTools.Print($"Thanks alot {Program.CurrentPlayer.Name}", 20);
                            Program.CurrentPlayer.CompleteAndTurnInQuest(Program.CurrentPlayer.QuestLog.Find(quest => quest.Name == talkto.AvailableQuests[0].Name));
                            talkto.AvailableQuests.RemoveAt(0);
                            TextInput.PressToContinue();
                            HUDTools.ClearLastText((startCursor.Item1, startCursor.Item2 - 1));
                        } else if (talkto.AvailableQuests.Count != 0 && talkto.AvailableQuests[0].Accepted == false) {                        
                            HUDTools.Print($"Do you have any work?", 0);
                            HUDTools.Print($"Yes, if you could go and {talkto.AvailableQuests[0].Name}, I will make it worth your while.\n(Y)es to accept (n)o to decline.", 15);
                            while (true) {
                                string input1 = TextInput.PlayerPrompt(true);
                                if (input1 == "y") {
                                    Program.CurrentPlayer.QuestLog.Add(talkto.AvailableQuests[0]);
                                    talkto.AvailableQuests[0].Accepted = true;
                                    HUDTools.Print($"\u001b[96mYou've gained a quest: {talkto.AvailableQuests[0].Name}\u001b[0m!", 20);
                                    TextInput.PressToContinue();
                                    HUDTools.ClearLastText((startCursor.Item1, startCursor.Item2 - 1));
                                    break;
                                } else if (input1 == "n") {
                                    talkto.AvailableQuests.RemoveAt(0);
                                    if (Program.CurrentPlayer.Level < 15) {
                                        talkto.AvailableQuests.Add(Act1Quest.CreateRandomQuest());
                                    } else if (Program.CurrentPlayer.Level >= 15) {
                                        //No quests made
                                    }
                                    HUDTools.ClearLastText(startCursor);
                                    break;
                                } else {
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
