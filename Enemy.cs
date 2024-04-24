using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;

namespace Saga
{
    internal class Enemy
    {
        //Monster type låst efter level
        public static string GetType() {
            if (Program.currentPlayer.level < 3) {
                switch (Program.rand.Next(0, 2 + 1)) {
                    case 0:
                        return "Giant Rat";
                    case 1:
                        return "Grave Robber";
                    case 2:
                        return "Giant Bat";
                }
            }
            else if (Program.currentPlayer.level <= 5) {
                switch (Program.rand.Next(0, 4 + 1)) {
                    case 0:
                        return "Skeleton";
                    case 1:
                        return "Zombie";
                    case 2:
                        return "Giant Rat";
                    case 3:
                        return "Grave Robber";
                    case 4:
                        return "Giant Bat";
                }
            }
            else if (5 < Program.currentPlayer.level && Program.currentPlayer.level <= 15) {
                switch (Program.rand.Next(0, 8 + 1)) {
                    case 0:
                        return "Skeleton";
                    case 1:
                        return "Zombie";
                    case 2:
                        return "Human Cultist";
                    case 3:
                        return "Grave Robber";
                    case 4:
                        return "Giant Bat";
                    case 5:
                        return "Human Rogue";
                    case 6:
                        return "Giant Rat";
                    case 7:
                        return "Bandit";
                    case 8:
                        return "Dire Wolf";
                }
            }
            switch (Program.rand.Next(0, 6 + 1)) {
                case 0:
                    return "Human Cultist";
                case 1:
                    return "Skeleton";
                case 2:
                    return "Human Rogue";
                case 3:
                    return "Vampire";
                case 4:
                    return "Werewolf";
                case 5:
                    return "Dire Wolf";
                case 6:
                    return "Bandit";
            }
            return "";
        }

        //Monster liv skaleret på spilleren.
        public static int GetHealth(string name) {
            int baseModUp = 3 + 2 * Program.currentPlayer.level;
            int baseModLower = 4 + Program.currentPlayer.level;
            switch (name) {
                case "Vampire":
                    int upper6 = (7 + baseModUp);
                    int lower6 = (4 + baseModLower);
                    return Program.rand.Next(lower6, upper6 + 1);
                case "Werewolf":
                    int upper7 = (6 + baseModUp);
                    int lower7 = (4 + baseModLower);
                    return Program.rand.Next(lower7, upper7 + 1);
                case "Dire Wolf":
                    int upper8 = (5 + baseModUp);
                    int lower8 = (4 + baseModLower);
                    return Program.rand.Next(lower8, upper8 + 1);
                case "Human Cultist":
                    int upper2 = (4 + baseModUp);
                    int lower2 = (3 + baseModLower);
                    return Program.rand.Next(lower2, upper2 + 1);
                case "Human Rogue":
                    int upper5 = (3 + baseModUp);
                    int lower5 = (2 + baseModLower);
                    return Program.rand.Next(lower5, upper5 + 1);
                case "Bandit":
                    int upper9 = (3 + baseModUp + 3);
                    int lower9 = (2 + baseModLower + 2);
                    return Program.rand.Next(lower9, upper9 + 1);
                case "Skeleton":
                    int upper = (2 + baseModUp);
                    int lower = (2 + baseModLower);
                    return Program.rand.Next(lower, upper + 1);
                case "Zombie":
                    int upper1 = (2 + baseModUp + 2);
                    int lower1 = (2 + baseModLower + 1);
                    return Program.rand.Next(lower1, upper1 + 1);
                case "Grave Robber":
                    int upper3 = (1 + baseModUp);
                    int lower3 = (1 + baseModLower);
                    return Program.rand.Next(lower3, upper3 + 1);
                case "Giant Bat":
                    int upper4 = (baseModUp - 1);
                    int lower4 = (baseModLower - 2);
                    return Program.rand.Next(lower4, upper4 + 1);
                case "Giant Rat":
                    int upper0 = (baseModUp);
                    int lower0 = (baseModLower);
                    return Program.rand.Next(lower0, upper0 + 1);
            }
            return 0;
        }

        //Monster skade skaleret på spilleren.
        public static int GetPower(string name) {
            int baseModUp = 3 + 2 * Program.currentPlayer.level;
            int baseModLower = 4 + Program.currentPlayer.level;
            switch (name) {
                case "Vampire":
                    int upper6 = (7 + baseModUp);
                    int lower6 = (4 + baseModLower);
                    return Program.rand.Next(lower6, upper6 + 1);
                case "Werewolf":
                    int upper7 = (6 + baseModUp);
                    int lower7 = (4 + baseModLower);
                    return Program.rand.Next(lower7, upper7 + 1);
                case "Dire Wolf":
                    int upper8 = (5 + baseModUp);
                    int lower8 = (4 + baseModLower);
                    return Program.rand.Next(lower8, upper8 + 1);
                case "Human Cultist":
                    int upper2 = (4 + baseModUp);
                    int lower2 = (3 + baseModLower);
                    return Program.rand.Next(lower2, upper2 + 1);
                case "Human Rogue":
                    int upper5 = (2 + baseModUp + 3);
                    int lower5 = (2 + baseModLower + 1);
                    return Program.rand.Next(lower5, upper5 + 1);
                case "Bandit":
                    int upper9 = (2 + baseModUp);
                    int lower9 = (2 + baseModLower);
                    return Program.rand.Next(lower9, upper9 + 1);
                case "Skeleton":
                    int upper = (baseModUp + 2);
                    int lower = (baseModLower + Program.currentPlayer.level - Program.currentPlayer.level / 3);
                    return Program.rand.Next(lower, upper + 1);
                case "Zombie":
                    int upper1 = (baseModUp);
                    int lower1 = (baseModLower);
                    return Program.rand.Next(lower1, upper1 + 1);
                case "Grave Robber":
                    int upper3 = (baseModUp - Program.currentPlayer.level / 4);
                    int lower3 = (baseModLower - Program.currentPlayer.level / 4);
                    return Program.rand.Next(lower3, upper3 + 1);
                case "Giant Bat":
                    int upper4 = (baseModUp - 1 - Program.currentPlayer.level / 2 - Program.currentPlayer.level / 5);
                    int lower4 = (baseModLower - 1 - Program.currentPlayer.level / 2);
                    return Program.rand.Next(lower4, upper4 + 1);
                case "Giant Rat":
                    int upper0 = (baseModUp - 3 - Program.currentPlayer.level / 2 - Program.currentPlayer.level / 5);
                    int lower0 = (baseModLower - 3 - Program.currentPlayer.level / 2);
                    return Program.rand.Next(lower0, upper0 + 1);
            }
            return 0;
        }
    }
}
