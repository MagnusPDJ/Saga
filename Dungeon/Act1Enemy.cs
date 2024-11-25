using Saga.assets;
using System;

namespace Saga.Dungeon
{
    public static class Act1EnemyExtensions
    {
        public static float GetGoldModifier(this Tribe tribe) {
            switch (tribe) {
                default: return 1;
                case Tribe.Undead: return 0.5f;
                case Tribe.Beast: return 0;
            }
        }
        public static float GetExpModifier(this Tribe tribe) {
            switch (tribe) { 
                default : return 1;
                case Tribe.Mythical: return 2;
            }
        }
    }

    public class Act1Enemy : Enemy
    {
        public Act1Enemy(string name, Tribe tribe) {
            Name = name;
            EnemyTribe = tribe;
            ExpModifier = tribe.GetExpModifier();
            GoldModifier = tribe.GetGoldModifier();
        }

        public static Act1Enemy CreateRandomAct1Enemy() {
            (string, Tribe)getName = GetName();
            Act1Enemy monster = new Act1Enemy(getName.Item1, getName.Item2);
            monster.Health = monster.GetHealth(getName.Item1);
            monster.Power = monster.GetPower(getName.Item1);
            return monster;
        }
        public override void GetExp() {
            int upper = (20 * Program.CurrentPlayer.Level + 31);
            int lower = (10 * Program.CurrentPlayer.Level);
            int x = (int)Math.Floor(Program.rand.Next(lower, upper + 1) * ExpModifier);
            if (x > 0) {
                Console.ForegroundColor = ConsoleColor.DarkGreen;
                HUDTools.Print($"You've gained {x} experience points!", 10);
                Program.CurrentPlayer.Exp += x;
            }
            Program.CurrentPlayer.CheckForLevelUp();
        }
        //Monster type låst efter level
        public static (string, Tribe) GetName() {
            if (Program.CurrentPlayer.Level < 3) {
                switch (Program.rand.Next(1, 100 + 1)) {
                    default:
                    case int n when n < 40:
                        return ("Giant Rat", Tribe.Beast);
                    case int n when 40 <= n && n < 90:
                        return ("Giant Bat", Tribe.Beast);
                    case int n when 90 <= n:
                        return ("Grave Robber", Tribe.Human);
                }
            } else if (Program.CurrentPlayer.Level <= 5) {
                switch (Program.rand.Next(1, 100 + 1)) {
                    default:
                    case int n when 90 <= n:
                        return ("Skeleton", Tribe.Undead);
                    case int n when 80 <= n && n < 90:
                        return ("Zombie", Tribe.Undead);
                    case int n when 60 <= n && n < 80:
                        return ("Grave Robber", Tribe.Human);
                    case int n when 30 <= n && n < 60:
                        return ("Giant Bat", Tribe.Beast);
                    case int n when 1 <= n && n < 30:
                        return ("Giant Rat", Tribe.Beast);
                }
            } else if (5 < Program.CurrentPlayer.Level && Program.CurrentPlayer.Level <= 15) {
                switch (Program.rand.Next(1, 100 + 1)) {
                    default:
                    case int n when 1 <= n && n < 20:
                        return ("Giant Rat", Tribe.Beast);                        
                    case int n when 20 <= n && n < 30:
                        return ("Giant Bat", Tribe.Beast);                        
                    case int n when 30 <= n && n < 60:
                        return ("Grave Robber", Tribe.Human);
                    case int n when 60 <= n && n < 75:
                        return ("Zombie", Tribe.Undead);
                    case int n when 75 <= n && n < 90:
                        return ("Skeleton", Tribe.Undead);
                    case int n when 90 <= n && n < 95:
                        return ("Bandit", Tribe.Human);
                    case int n when 95 <= n && n < 97:
                        return ("Human Rogue", Tribe.Human);
                    case int n when 97 <= n && n < 99:
                        return ("Human Cultist", Tribe.Human);                       
                    case int n when 99 <= n && n <= 100:
                        return ("Dire Wolf", Tribe.Beast);
                }
            } else {
                switch (Program.rand.Next(1, 100 + 1)) {
                    default:
                    case int n when 1 <= n && n < 30:
                        return ("Skeleton", Tribe.Undead);
                    case int n when 30 <= n && n < 50:
                        return ("Bandit", Tribe.Human);
                    case int n when 50 <= n && n < 60:
                        return ("Human Rogue", Tribe.Human);
                    case int n when 60 <= n && n < 75:
                        return ("Human Cultist", Tribe.Human);
                    case int n when 75 <= n && n < 85:
                        return ("Dire Wolf", Tribe.Beast);
                    case int n when 85 <= n && n < 95:
                        return ("Werewolf", Tribe.Beast);
                    case int n when 95 <= n && n <= 100:
                        return ("Vampire", Tribe.Undead);
                }
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

        public override void MonsterActions(Encounters TurnTimer) {
            if (Program.CurrentPlayer.TotalSecondaryAttributes.Awareness > 0) {
                if (EnemyTurn < TurnTimer.TurnTimer && Health > 0) {
                    int attack = Power;
                    if (AttackDebuff > 0) {
                        attack /= Math.Max(2, Program.CurrentPlayer.TotalSecondaryAttributes.ArmorRating);
                        Program.CurrentPlayer.Health -= attack;
                        AttackDebuff--;
                        if (AttackDebuff == 0) {
                            HUDTools.Print("You are no longer defended!", 5);
                        }
                    } else {
                        attack -= Program.CurrentPlayer.TotalSecondaryAttributes.ArmorRating;
                        if (attack < 0) {
                            attack = 0;
                        }
                        Program.CurrentPlayer.Health -= attack;
                    }
                    EnemyTurn++;
                    HUDTools.Print($"The Enemy Attacked and dealt {attack} damage!\n", 10);
                    HUDTools.WriteCombatLog("enemysecond", TurnTimer, attack, 0, this);
                    Console.ReadKey(true);
                }
            } else {
                if (EnemyTurn == TurnTimer.TurnTimer && Health > 0) {
                    Console.ReadKey(true);
                    int attack = Power;
                    if (AttackDebuff > 0) {
                        attack /= Math.Max(2, Program.CurrentPlayer.TotalSecondaryAttributes.ArmorRating);
                        Program.CurrentPlayer.Health -= attack;
                        AttackDebuff--;
                    } else {
                        attack -= Program.CurrentPlayer.TotalSecondaryAttributes.ArmorRating;
                        if (attack < 0) {
                            attack = 0;
                        }
                        Program.CurrentPlayer.Health -= attack;
                    }
                    EnemyTurn++;
                    HUDTools.Print($"The Enemy Attacked and dealt {attack} damage!\n", 10);
                    HUDTools.WriteCombatLog("enemyfirst", TurnTimer, attack, 0, this);

                }
            }
        }
    }
}
