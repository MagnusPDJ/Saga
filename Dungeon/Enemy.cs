using System;
using System.Reflection.Emit;
using Saga.assets;

namespace Saga.Dungeon
{
    public class Enemy
    {
        public string name;
        public int health;             
        public int power;
        public int awareness = 0;
        public int xpModifier = 1;
        public int goldModifier = 1;
        int enemyTurn = 1;
        public int attackDebuff = 0;

        public static int GetXP() {
            int upper = (20 * Program.CurrentPlayer.Level + 31);
            int lower = (10 * Program.CurrentPlayer.Level);
            return Program.rand.Next(lower, upper + 1);
        }
        //Monster type låst efter level
        public static new string GetType() {
            if (Program.CurrentPlayer.Level < 3) {
                switch (Program.rand.Next(0, 2 + 1)) {
                    case 0:
                        return "Giant Rat";
                    case 1:
                        return "Grave Robber";
                    case 2:
                        return "Giant Bat";
                }
            }
            else if (Program.CurrentPlayer.Level <= 5) {
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
            else if (5 < Program.CurrentPlayer.Level && Program.CurrentPlayer.Level <= 15) {
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
            int baseModUp = 3 + 2 * Program.CurrentPlayer.Level;
            int baseModLower = 4 + Program.CurrentPlayer.Level;
            switch (name) {
                case "Vampire":
                    int upper6 = (45 + baseModUp+Program.CurrentPlayer.Level /2);
                    int lower6 = (29 + baseModLower+Program.CurrentPlayer.Level);
                    return Program.rand.Next(lower6, upper6 + 1);
                case "Werewolf":
                    int upper7 = (23 + baseModUp+Program.CurrentPlayer.Level /2);
                    int lower7 = (27 + baseModLower+Program.CurrentPlayer.Level /3);
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
        public static int GetPower(string name) {
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
                    return Program.rand.Next(lower3, upper3 + 1);
                case "Giant Bat":
                    int upper4 = (baseModUp - 1 - Program.CurrentPlayer.Level / 2 - Program.CurrentPlayer.Level / 5);
                    int lower4 = (baseModLower - 1 - Program.CurrentPlayer.Level / 2);
                    return Program.rand.Next(lower4, upper4 + 1);
                case "Giant Rat":
                    int upper0 = (baseModUp - 3 - Program.CurrentPlayer.Level / 2 - Program.CurrentPlayer.Level / 5);
                    int lower0 = (baseModLower - 3 - Program.CurrentPlayer.Level / 2);
                    return Program.rand.Next(lower0, upper0 + 1);
            }
            return 0;
        }

        public static void MonsterActions(Enemy Monster, Encounters TurnTimer) {
            if (Program.CurrentPlayer.BaseSecondaryAttributes.Awareness > 0) {
                if (Monster.enemyTurn < TurnTimer.turnTimer && Monster.health > 0) {
                    int attack = Monster.power;
                    if (Monster.attackDebuff > 0) {
                        attack /= Program.CurrentPlayer.BaseSecondaryAttributes.ArmorRating;
                        Program.CurrentPlayer.Health -= attack;
                        Monster.attackDebuff--;
                        if (Monster.attackDebuff == 0) {
                            HUDTools.Print("You are no longer defended!",5);
                        }
                    } else {
                        attack -= Program.CurrentPlayer.BaseSecondaryAttributes.ArmorRating;
                        if (attack < 0) {
                            attack = 0;
                        }
                        Program.CurrentPlayer.Health -= attack;
                    }
                    Monster.enemyTurn++;
                    HUDTools.Print($"The Enemy Attacked and dealt {attack} damage!\n",10);
                    HUDTools.WriteCombatLog("enemysecond", TurnTimer,attack,0,Monster);
                    Console.ReadKey(true);
                }
            } else {                
                if (Monster.enemyTurn == TurnTimer.turnTimer && Monster.health > 0) {
                    Console.ReadKey(true);
                    int attack = Monster.power;
                    if (Monster.attackDebuff > 0) {
                        attack /= Program.CurrentPlayer.BaseSecondaryAttributes.ArmorRating;
                        Program.CurrentPlayer.Health -= attack;
                        Monster.attackDebuff--;
                    }
                    else {
                        attack -= Program.CurrentPlayer.BaseSecondaryAttributes.ArmorRating;
                        if (attack < 0) {
                            attack = 0;
                        }
                        Program.CurrentPlayer.Health -= attack;
                    }
                    Monster.enemyTurn++;
                    HUDTools.Print($"The Enemy Attacked and dealt {attack} damage!\n", 10);
                    HUDTools.WriteCombatLog("enemyfirst", TurnTimer,attack,0,Monster);
                    
                }
            }
        }





        ////Fra Attack
        //int damage = Monster.power - Program.currentPlayer.TotalArmorValue();
        //if (damage < 0)
        //    damage = 0;

        //HUDTools.Print($"You lose {damage} health", 10);
        //Program.currentPlayer.health -= damage;
        //HUDTools.WriteCombatLog("attack", TurnTimer, damage, attack);


        ////Fra Defend
        //int damage = (Monster.power / Program.currentPlayer.TotalArmorValue());
        //if (damage < 0)
        //    damage = 0;
        //HUDTools.Print($"You lose {damage} health and you deal {attack} damage", 20);
        //Program.currentPlayer.health -= damage;
        //HUDTools.WriteCombatLog("defend", TurnTimer, damage, attack);

        ////Fra Run
        //int damage = Monster.power - Program.currentPlayer.TotalArmorValue();
        //if (damage < 0)
        //    damage = 0;
        //HUDTools.Print($"You lose {damage} health and are unable to escape this turn.", 20);
        //Program.currentPlayer.health -= damage;
        //HUDTools.WriteCombatLog("run", TurnTimer, damage, 0);

        ////Fra Heal
        //if (combat) {
        //    if (Program.currentPlayer.potion == 0) {
        //        HUDTools.Print("No potions left!", 20);
        //        int damage = Monster.power - Program.currentPlayer.TotalArmorValue();
        //        if (damage < 0)
        //            damage = 0;
        //        HUDTools.Print($"The {Monster.name} attacks you while you fumble in your bags and lose {damage} health!", 20);
        //        Program.currentPlayer.health -= damage;
        //        HUDTools.WriteCombatLog("heal", TurnTimer, damage, 0);
        //    }
        //    else {
        //        if (Program.currentPlayer.currentClass == PlayerClass.Mage) {
        //            HUDTools.Print("You use a potion amplified by your magic", 30);
        //        }
        //        else {
        //            HUDTools.Print("You use a potion", 20);
        //        }
        //        Program.currentPlayer.health += Program.currentPlayer.potionValue + ((Program.currentPlayer.currentClass == PlayerClass.Mage) ? 3 + Program.currentPlayer.level : 0);
        //        if (Program.currentPlayer.health > Program.currentPlayer.maxHealth) {
        //            Program.currentPlayer.health = Program.currentPlayer.maxHealth;
        //        }
        //        if (Program.currentPlayer.health == Program.currentPlayer.maxHealth) {
        //            HUDTools.Print("You heal to max health!", 20);
        //        }
        //        else {
        //            HUDTools.Print($"You gain {Program.currentPlayer.potionValue + ((Program.currentPlayer.currentClass == PlayerClass.Mage) ? 3 + Program.currentPlayer.level : 0)} health", 20);
        //        }
        //        HUDTools.Print($"As you drink, the {Monster.name} strikes you.", 20);
        //        int damage = (Monster.power / 2) - Program.currentPlayer.TotalArmorValue();
        //        if (damage < 0)
        //            damage = 0;
        //        HUDTools.Print($"You lose {damage} health", 20);
        //        Program.currentPlayer.health -= damage;
        //        HUDTools.WriteCombatLog("heal", TurnTimer, damage, 0);
        //        Program.currentPlayer.potion--;
        //    }
        //}
        //else



    }
}
