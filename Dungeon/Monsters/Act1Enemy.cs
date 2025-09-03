using Saga.Assets;

namespace Saga.Dungeon.Monsters
{
    public static class Act1EnemyExtensions
    {
        public static float GetGoldModifier(this Tribe tribe) {
            return tribe switch {
                Tribe.Undead => 0.5f,
                Tribe.Beast => 0f,
                _ => 1f,
            };
        }
        public static float GetExpModifier(this Tribe tribe) {
            return tribe switch {
                Tribe.Mythical => 2f,
                _ => 1f,
            };
        }
    }

    public class Act1Enemy : Enemy
    {
        public Act1Enemy(string name, Tribe tribe) {
            Name = name;
            EnemyTribe = tribe;
            PlayerKillDescription = $"As the {Name} strikes, you are slain!";
            ExpModifier = tribe.GetExpModifier();
            GoldModifier = tribe.GetGoldModifier();
        }

        public static Act1Enemy CreateRandomAct1Enemy() {
            (string, Tribe)getName = GetName();
            Act1Enemy monster = new(getName.Item1, getName.Item2);
            monster.MaxHealth = monster.GetHealth(getName.Item1);
            monster.Power = monster.GetPower(getName.Item1);
            return monster;
        }
        public override void GetExp() {
            int upper = 20 * Program.CurrentPlayer.Level + 31;
            int lower = 10 * Program.CurrentPlayer.Level;
            int x = (int)Math.Floor(Program.Rand.Next(lower, upper + 1) * ExpModifier);
            if (x > 0) {
                HUDTools.Print($"\u001b[32mYou've gained {x} experience points!\u001b[0m", 10);
                Program.CurrentPlayer.Exp += x;
            }
            Program.CurrentPlayer.CheckForLevelUp();
        }
        //Monster type låst efter level
        public static (string, Tribe) GetName() {
            if (Program.CurrentPlayer.Level < 3) {
                return Program.Rand.Next(1, 100 + 1) switch {
                    int n when 91 <= n           => ("Grave Robber", Tribe.Human),
                    int n when 46 <= n && n < 91 => ("Giant Bat", Tribe.Beast),
                    _ => ("Giant Rat", Tribe.Beast),
                };
            } else if (Program.CurrentPlayer.Level <= 5) {
                return Program.Rand.Next(1, 100 + 1) switch {
                    int n when 90 <= n           => ("Skeleton", Tribe.Undead),
                    int n when 80 <= n && n < 90 => ("Zombie", Tribe.Undead),
                    int n when 60 <= n && n < 80 => ("Grave Robber", Tribe.Human),
                    int n when 30 <= n && n < 60 => ("Giant Bat", Tribe.Beast),
                    _ => ("Giant Rat", Tribe.Beast),
                };
            } else if (5 < Program.CurrentPlayer.Level && Program.CurrentPlayer.Level <= 15) {
                return Program.Rand.Next(1, 100 + 1) switch {
                    int n when 15 <= n && n < 30 => ("Giant Bat", Tribe.Beast),
                    int n when 30 <= n && n < 50 => ("Grave Robber", Tribe.Human),
                    int n when 50 <= n && n < 70 => ("Zombie", Tribe.Undead),
                    int n when 70 <= n && n < 85 => ("Skeleton", Tribe.Undead),
                    int n when 85 <= n && n < 90 => ("Bandit", Tribe.Human),
                    int n when 90 <= n && n < 95 => ("Human Rogue", Tribe.Human),
                    int n when 95 <= n && n < 98 => ("Human Cultist", Tribe.Human),
                    int n when 98 <= n           => ("Dire Wolf", Tribe.Beast),
                    _ => ("Giant Rat", Tribe.Beast),
                };
            } else {
                return Program.Rand.Next(1, 100 + 1) switch {
                    int n when 25 <= n && n < 50 => ("Bandit", Tribe.Human),
                    int n when 50 <= n && n < 60 => ("Human Rogue", Tribe.Human),
                    int n when 60 <= n && n < 75 => ("Human Cultist", Tribe.Human),
                    int n when 75 <= n && n < 85 => ("Dire Wolf", Tribe.Beast),
                    int n when 85 <= n && n < 95 => ("Werewolf", Tribe.Beast),
                    int n when 95 <= n           => ("Vampire", Tribe.Undead),
                    _ => ("Skeleton", Tribe.Undead),
                };
            }
        }

        //Monster liv skaleret på spilleren.
        public override int GetHealth(string name) {
            int baseModUp = 3 + 2 * Program.CurrentPlayer.Level;
            int baseModLower = 4 + Program.CurrentPlayer.Level;
            switch (name) {
                case "Vampire":
                    int upper6 = 49 + baseModUp + Program.CurrentPlayer.Level;
                    int lower6 = 34 + baseModLower + Program.CurrentPlayer.Level/2;
                    return Program.Rand.Next(lower6, upper6 + 1);
                case "Werewolf":
                    int upper7 = 28 + baseModUp + Program.CurrentPlayer.Level / 2;
                    int lower7 = 32 + baseModLower + Program.CurrentPlayer.Level / 3;
                    return Program.Rand.Next(lower7, upper7 + 1);
                case "Dire Wolf":
                    int upper8 = 27 + baseModUp;
                    int lower8 = 26 + baseModLower;
                    return Program.Rand.Next(lower8, upper8 + 1);
                case "Human Cultist":
                    int upper2 = 17 + baseModUp;
                    int lower2 = 16 + baseModLower;
                    return Program.Rand.Next(lower2, upper2 + 1);
                case "Human Rogue":
                    int upper5 = 10 + baseModUp;
                    int lower5 = 1 + baseModLower;
                    return Program.Rand.Next(lower5, upper5 + 1);
                case "Bandit":
                    int upper9 = 6 + baseModUp;
                    int lower9 = 6 + baseModLower;
                    return Program.Rand.Next(lower9, upper9 + 1);
                case "Skeleton":
                    int upper = 2 + baseModUp;
                    int lower = 2 + baseModLower;
                    return Program.Rand.Next(lower, upper + 1);
                case "Zombie":
                    int upper1 = 2 + baseModUp + 2;
                    int lower1 = 2 + baseModLower + 1;
                    return Program.Rand.Next(lower1, upper1 + 1);
                case "Grave Robber":
                    int upper3 = 1 + baseModUp;
                    int lower3 = 1 + baseModLower;
                    return Program.Rand.Next(lower3, upper3 + 1);
                case "Giant Bat":
                    int upper4 = baseModUp - 1;
                    int lower4 = baseModLower - 2;
                    return Program.Rand.Next(lower4, upper4 + 1);
                case "Giant Rat":
                    int upper0 = baseModUp;
                    int lower0 = baseModLower;
                    return Program.Rand.Next(lower0, upper0 + 1);
            }
            return 0;
        }

        //Monster skade skaleret på spilleren.
        public override int GetPower(string name) {
            int baseModUp = 3 + 2 * Program.CurrentPlayer.Level;
            int baseModLower = 4 + Program.CurrentPlayer.Level;
            switch (name) {
                case "Vampire":
                    int upper6 = 7 + baseModUp;
                    int lower6 = 4 + baseModLower;
                    return Program.Rand.Next(lower6, upper6 + 1);
                case "Werewolf":
                    int upper7 = 6 + baseModUp;
                    int lower7 = 4 + baseModLower;
                    return Program.Rand.Next(lower7, upper7 + 1);
                case "Dire Wolf":
                    int upper8 = 5 + baseModUp;
                    int lower8 = 4 + baseModLower;
                    return Program.Rand.Next(lower8, upper8 + 1);
                case "Human Cultist":
                    int upper2 = 4 + baseModUp;
                    int lower2 = 3 + baseModLower;
                    return Program.Rand.Next(lower2, upper2 + 1);
                case "Human Rogue":
                    int upper5 = 2 + baseModUp + 3;
                    int lower5 = 2 + baseModLower + 1;
                    return Program.Rand.Next(lower5, upper5 + 1);
                case "Bandit":
                    int upper9 = 2 + baseModUp;
                    int lower9 = 2 + baseModLower;
                    return Program.Rand.Next(lower9, upper9 + 1);
                case "Skeleton":
                    int upper = baseModUp + 2;
                    int lower = baseModLower + Program.CurrentPlayer.Level - Program.CurrentPlayer.Level / 3;
                    return Program.Rand.Next(lower, upper + 1);
                case "Zombie":
                    int upper1 = baseModUp;
                    int lower1 = baseModLower;
                    return Program.Rand.Next(lower1, upper1 + 1);
                case "Grave Robber":
                    int upper3 = baseModUp - Program.CurrentPlayer.Level / 4;
                    int lower3 = baseModLower - Program.CurrentPlayer.Level / 4;
                    return Program.Rand.Next(lower3 - 1, upper3 + 1);
                case "Giant Bat":
                    int upper4 = baseModUp - 1 - Program.CurrentPlayer.Level / 2 - Program.CurrentPlayer.Level / 5;
                    int lower4 = baseModLower - 1 - Program.CurrentPlayer.Level / 2;
                    return Program.Rand.Next(lower4 - 1, upper4 + 1);
                case "Giant Rat":
                    int upper0 = baseModUp - 3 - Program.CurrentPlayer.Level / 2 - Program.CurrentPlayer.Level / 5;
                    int lower0 = baseModLower - 3 - Program.CurrentPlayer.Level / 2;
                    return Program.Rand.Next(lower0, upper0 + 2);
            }
            return 0;
        }
    }
}
