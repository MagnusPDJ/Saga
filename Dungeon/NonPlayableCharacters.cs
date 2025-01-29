using System;
using System.Collections.Generic;
using Saga.assets;

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
            List<string> questions = new List<string>();
            List<string> answers = new List<string>();
            foreach (KeyValuePair<string, string> option in talkto.Dialogue) {
                questions.Add($"{option.Key}");
                answers.Add($"{option.Value}");
            }           
            while (true) {
                Console.Clear();
                HUDTools.Print($"{talkto.Greeting}\t(b)ack", 20);
                foreach (string question in questions) {
                    HUDTools.Print($"{questions.IndexOf(question)}:\t{question}", 15);
                }
                if (talkto.Name == "Flemsha" && Program.CurrentPlayer.QuestLog.Exists(quest => quest.Name == talkto.AvailableQuests[0].Name && quest.Completed == true)) {
                    Console.ForegroundColor = ConsoleColor.Cyan;
                    HUDTools.Print($"{questions.Count}:\tI have your items.", 20);
                    Console.ResetColor();
                }
                if (talkto.Name == "Flemsha" && talkto.AvailableQuests.Count == 0 && Program.CurrentPlayer.Level < 15) {
                    talkto.AvailableQuests.Add(Act1Quest.CreateRandomQuest());
                    Console.ForegroundColor = ConsoleColor.Cyan;
                    HUDTools.Print($"{questions.Count}:\tDo you have any work?", 20);
                    Console.ResetColor();
                } else if (talkto.Name == "Flemsha" && talkto.AvailableQuests.Count == 0 && Program.CurrentPlayer.Level >= 15) { 
                //No quests made
                } else if (talkto.Name == "Flemsha" && talkto.AvailableQuests[0].Accepted == false) {
                    Console.ForegroundColor = ConsoleColor.Cyan;
                    HUDTools.Print($"{questions.Count}:\tDo you have any work?", 20);
                    Console.ResetColor();
                }
                string input = HUDTools.PlayerPrompt();
                if (input == "b" || input == "back") {
                    break;
                } else if (int.TryParse(input, out int n) && n >= 0 && n <= questions.Count - 1) {
                    HUDTools.Print($"{questions[n]}", 0);
                    HUDTools.Print($"{answers[n]}\n", 20);
                    HUDTools.Print("Press to continue...", 5);
                    HUDTools.PlayerPrompt();
                    questions.RemoveAt(n);
                    answers.RemoveAt(n);
                    if (questions.Count == 0) {
                        break;
                    }
                } else if (int.TryParse(input, out int n1) && n1 >= 0 && n1 <= questions.Count) {
                    if (Program.CurrentPlayer.QuestLog.Exists(quest => quest.Name == talkto.AvailableQuests[0].Name && quest.Completed == true)) {
                        HUDTools.Print($"I have your items.", 0);
                        HUDTools.Print($"Thanks alot {Program.CurrentPlayer.Name}", 20);
                        Program.CurrentPlayer.CompleteAndTurnInQuest(Program.CurrentPlayer.QuestLog.Find(quest => quest.Name == talkto.AvailableQuests[0].Name));
                        talkto.AvailableQuests.RemoveAt(0);
                        HUDTools.PlayerPrompt();
                    } else if (talkto.AvailableQuests[0].Accepted == false){
                        HUDTools.Print($"Do you have any work?", 0);
                        HUDTools.Print($"Yes, if you could go and {talkto.AvailableQuests[0].Name}, I will make it worth your while.\n(Y)es to accept (n)o to decline.", 15);
                        while (true) {
                            string input1 = HUDTools.PlayerPrompt();
                            if (input1 == "y") {
                                Program.CurrentPlayer.QuestLog.Add(talkto.AvailableQuests[0]);
                                talkto.AvailableQuests[0].Accepted = true;
                                Console.ForegroundColor = ConsoleColor.Cyan;
                                HUDTools.Print($"You've gained a quest: {talkto.AvailableQuests[0].Name}!");
                                Console.ResetColor();
                                HUDTools.PlayerPrompt();
                                break;
                            } else if (input1 == "n") {
                                talkto.AvailableQuests.RemoveAt(0);
                                break;
                            } else {
                                Console.Clear();
                                HUDTools.Print($"Do you have any work?", 0);
                                HUDTools.Print($"Yes, if you could go and {talkto.AvailableQuests[0].Name}, I will make it worth your while.\n(Y)es to accept (n)o to decline.", 15);
                            }
                        }
                    }

                } else {
                    HUDTools.Print("Please select a number from the list or (b)ack", 15);
                    HUDTools.PlayerPrompt();
                }
            }
        }
        ////Characters:
        //public static NonPlayableCharacters Flemsha = new NonPlayableCharacters() {
        //    Name = "Flemsha",
        //    Greeting = $"Thanks again {(Program.CurrentPlayer.Name ?? "")}, how can I be of assistance?",
        //    Dialogue = new Dictionary<string, string>(),
        //    AvailableQuests = new List<Quest>(),
        //};
        //public static NonPlayableCharacters Gheed = new NonPlayableCharacters() {
        //    Name = "Gheed",
        //    Greeting = "Ahh you are back again... What's that, you want to ask something?",
        //    Dialogue = new Dictionary<string, string>(),
        //};
    }
}
