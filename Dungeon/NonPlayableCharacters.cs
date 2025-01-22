using Saga.assets;
using System;
using System.Collections.Generic;

namespace Saga.Dungeon {
    [Serializable]
    public class NonPlayableCharacters {
        public string Name { get; set; }
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
                        npc.Dialogue[parts[2]] = parts[3];                     
                    }
                }
            }
        }
        //Metode til at vise alle dialog valg mulighederne.
        public static void LoadDialogueOptions(int index) {
            Console.Clear();
            NonPlayableCharacters talkto = Program.CurrentPlayer.NpcsInCamp[index];
            if (talkto.Name == "Gheed") {
                HUDTools.Print($"Ahh you are back again... What's that, you want to ask something?\t(b)ack", 20);
            } else if (talkto.Name == "Flemsha") {
                HUDTools.Print($"Thanks again {Program.CurrentPlayer.Name}, how can I be of assistance?", 20);
            } else if (talkto == null) { }

            List<string> questions = new List<string>();
            List<string> answers = new List<string>();
            foreach (KeyValuePair<string, string> option in talkto.Dialogue) {
                questions.Add(option.Key);
                answers.Add(option.Value);
            }
            while (true) {
                foreach(string question in questions) {
                    HUDTools.Print($"{questions.IndexOf(question)}:\t{question}");
                }
                string input = HUDTools.PlayerPrompt();
                
                if (input == "b" || input == "back") {
                    break;
                } else if (int.TryParse(input, out int n) && n >= 0 && n <= questions.Count-1) {
                    HUDTools.Print($"{answers[n]}\t(b)ack", 20);
                    questions.RemoveAt(n);
                    answers.RemoveAt(n);
                } else {
                    HUDTools.Print("Please select a number from the list or (b)ack");
                }
            }
        }

        //Characters:
        public static NonPlayableCharacters Flemsha = new NonPlayableCharacters() {
            Name = "Flemsha",
            Dialogue = new Dictionary<string, string>(),
        };
        public static NonPlayableCharacters Gheed = new NonPlayableCharacters() {
            Name = "Gheed",
            Dialogue = new Dictionary<string, string>(),
        };
    }
}
