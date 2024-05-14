using Saga.assets;
using Saga.Character;
using Saga.Items;
using System;
using System.Threading;

namespace Saga.Dungeon
{
    public class Act1Enemy : Enemy
    {
        public Act1Enemy(string name="Worm", int health=1, int power = 1) {
            Name = name;
            Health = health;
            Power = power;
        }
        public static Enemy CreateRandomAct1Enemy() {
            Enemy monster;
            string name = GetName();
            monster = new Act1Enemy() {
                Name = name,
            };
            monster.Health = monster.GetHealth(name);
            monster.Power = monster.GetPower(name);
            return monster;
        }
        public override void GetExp() {
            int upper = (20 * Program.CurrentPlayer.Level + 31);
            int lower = (10 * Program.CurrentPlayer.Level);
            int x = Program.rand.Next(lower, upper + 1)*ExpModifier;
            if (x > 0) {
                Console.ForegroundColor = ConsoleColor.DarkGreen;
                HUDTools.Print($"You've gained {x} experience points!", 10);
                Program.CurrentPlayer.Exp += x;
            }
            Program.CurrentPlayer.CheckForLevelUp();
        }
        //Monster type låst efter level
        public static string GetName() {
            if (Program.CurrentPlayer.Level < 3) {
                switch (Program.rand.Next(0, 10 + 1)) {
                    case int n when n < 4:
                        return "Giant Rat";
                    case int n when 4 <= n && n < 8:
                        return "Giant Bat";
                    case int n when 8 <= n:
                        return "Grave Robber";
                }
            } else if (Program.CurrentPlayer.Level <= 5) {
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
            } else if (5 < Program.CurrentPlayer.Level && Program.CurrentPlayer.Level <= 15) {
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
                default:
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
        }

        //Monster liv skaleret på spilleren.
        public override int GetHealth(string name) {
            int baseModUp = 3 + 2 * Program.CurrentPlayer.Level;
            int baseModLower = 4 + Program.CurrentPlayer.Level;
            switch (name) {
                case "Vampire":
                    int upper6 = (45 + baseModUp + Program.CurrentPlayer.Level / 2);
                    int lower6 = (29 + baseModLower + Program.CurrentPlayer.Level);
                    return Program.rand.Next(lower6, upper6 + 1);
                case "Werewolf":
                    int upper7 = (23 + baseModUp + Program.CurrentPlayer.Level / 2);
                    int lower7 = (27 + baseModLower + Program.CurrentPlayer.Level / 3);
                    return Program.rand.Next(lower7, upper7 + 1);
                case "Dire Wolf":
                    int upper8 = (22 + baseModUp);
                    int lower8 = (21 + baseModLower);
                    return Program.rand.Next(lower8, upper8 + 1);
                case "Human Cultist":
                    int upper2 = (12 + baseModUp);
                    int lower2 = (11 + baseModLower);
                    return Program.rand.Next(lower2, upper2 + 1);
                case "Human Rogue":
                    int upper5 = (2 + baseModUp);
                    int lower5 = (1 + baseModLower);
                    return Program.rand.Next(lower5, upper5 + 1);
                case "Bandit":
                    int upper9 = (6 + baseModUp);
                    int lower9 = (6 + baseModLower);
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
        public override int GetPower(string name) {
            int baseModUp = 3 + 2 * Program.CurrentPlayer.Level;
            int baseModLower = 4 + Program.CurrentPlayer.Level;
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
                    int lower = (baseModLower + Program.CurrentPlayer.Level - Program.CurrentPlayer.Level / 3);
                    return Program.rand.Next(lower, upper + 1);
                case "Zombie":
                    int upper1 = (baseModUp);
                    int lower1 = (baseModLower);
                    return Program.rand.Next(lower1, upper1 + 1);
                case "Grave Robber":
                    int upper3 = (baseModUp - Program.CurrentPlayer.Level / 4);
                    int lower3 = (baseModLower - Program.CurrentPlayer.Level / 4);
                    return Program.rand.Next(lower3 - 1, upper3 + 1);
                case "Giant Bat":
                    int upper4 = (baseModUp - 1 - Program.CurrentPlayer.Level / 2 - Program.CurrentPlayer.Level / 5);
                    int lower4 = (baseModLower - 1 - Program.CurrentPlayer.Level / 2);
                    return Program.rand.Next(lower4 - 1, upper4 + 1);
                case "Giant Rat":
                    int upper0 = (baseModUp - 3 - Program.CurrentPlayer.Level / 2 - Program.CurrentPlayer.Level / 5);
                    int lower0 = (baseModLower - 3 - Program.CurrentPlayer.Level / 2);
                    return Program.rand.Next(lower0, upper0 + 2);
            }
            return 0;
        }

        public override void MonsterActions(Enemy Monster, Encounters TurnTimer) {
            if (Program.CurrentPlayer.TotalSecondaryAttributes.Awareness > 0) {
                if (Monster.EnemyTurn < TurnTimer.TurnTimer && Monster.Health > 0) {
                    int attack = Monster.Power;
                    if (Monster.AttackDebuff > 0) {
                        attack /= Math.Max(2, Program.CurrentPlayer.TotalSecondaryAttributes.ArmorRating);
                        Program.CurrentPlayer.Health -= attack;
                        Monster.AttackDebuff--;
                        if (Monster.AttackDebuff == 0) {
                            HUDTools.Print("You are no longer defended!", 5);
                        }
                    } else {
                        attack -= Program.CurrentPlayer.TotalSecondaryAttributes.ArmorRating;
                        if (attack < 0) {
                            attack = 0;
                        }
                        Program.CurrentPlayer.Health -= attack;
                    }
                    Monster.EnemyTurn++;
                    HUDTools.Print($"The Enemy Attacked and dealt {attack} damage!\n", 10);
                    HUDTools.WriteCombatLog("enemysecond", TurnTimer, attack, 0, Monster);
                    Console.ReadKey(true);
                }
            } else {
                if (Monster.EnemyTurn == TurnTimer.TurnTimer && Monster.Health > 0) {
                    Console.ReadKey(true);
                    int attack = Monster.Power;
                    if (Monster.AttackDebuff > 0) {
                        attack /= Math.Max(2, Program.CurrentPlayer.TotalSecondaryAttributes.ArmorRating);
                        Program.CurrentPlayer.Health -= attack;
                        Monster.AttackDebuff--;
                    } else {
                        attack -= Program.CurrentPlayer.TotalSecondaryAttributes.ArmorRating;
                        if (attack < 0) {
                            attack = 0;
                        }
                        Program.CurrentPlayer.Health -= attack;
                    }
                    Monster.EnemyTurn++;
                    HUDTools.Print($"The Enemy Attacked and dealt {attack} damage!\n", 10);
                    HUDTools.WriteCombatLog("enemyfirst", TurnTimer, attack, 0, Monster);

                }
            }
        }
    }
}
