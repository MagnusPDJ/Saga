using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Saga
{
    [Serializable]
    public class Player
    {
        //Player Stats
        public string name;
        public int id;
        public int gold = 0;
        public int level = 1;
        public int xp = 0;
        public int maxHealth = 10;
        public int health = 10;
        public int damage = 1;
        public int weaponValue = 0;
        public int armorValue = 0;
        public int potion = 5;
        public int potionValue = 5;

        public string equippedWeapon = null;
        public int equippedWeaponValue = 0;
        public string equippedArmor = null;
        public int equippedArmorValue = 0;


        //Player classes that can be picked
        public enum PlayerClass {Mage, Archer, Warrior};
        public PlayerClass currentClass = PlayerClass.Warrior;
       
        //Metode til at get total weapon.
        public int TotalWeaponValue() {
            return Program.currentPlayer.weaponValue + Program.currentPlayer.equippedWeaponValue;
        }
        //Metode til at get total armor.
        public int TotalArmorValue() {
            return Program.currentPlayer.armorValue + Program.currentPlayer.equippedArmorValue;
        }

        //Guld drop skaleret på spilleren.
        public int GetGold() {
            int upper = (30 * level+101);
            int lower = (10 * level);
            return Program.rand.Next(lower, upper+1);
        }

        //Metode til at udregne exp fåen skaleret på spilleren.
        public int GetXP() {
            int upper = (20*level + 31);
            int lower = (10*level);
            return Program.rand.Next(lower, upper+1);
        }

        //Metode til udregning af det exp det koster at level op.
        public int GetLevelUpValue() {
            return Convert.ToInt32(5000000/(1+10000*Math.Pow(1.2,1-level)));
        }

        //Metode til at tjekke om lvl op er muligt
        public bool CanLevelUp() {
            if (xp >= GetLevelUpValue()) {
                return true;
            } else {
                return false;
            }
        }

        //Metode til at lvl spilleren op.
        public void LevelUp() {
            AudioManager.soundLvlUp.Play();
            while(CanLevelUp()) {
                xp-=GetLevelUpValue();
                level++;
            }
            int h = 2+(level/3);
            maxHealth += h;
            Console.ForegroundColor = ConsoleColor.DarkBlue;
            HUDTools.Print($"Congratulations! You are now level {level}! Your max health increased by {h}.",20);
            Console.ResetColor();
            Program.currentPlayer.health = maxHealth;
            HUDTools.PlayerPrompt();
        }

        //Metode til at checke for om spilleren dør som kan kaldes hver gang spilleren tager skade.
        public static void DeathCode(string message) {
                AudioManager.soundGameOver.Play();
                Console.ForegroundColor = ConsoleColor.DarkRed;
                HUDTools.Print(message,20);
                HUDTools.Print("Press to go back to main menu...", 5); 
                Console.ResetColor();
                HUDTools.PlayerPrompt();
                Program.MainMenu();
        }

        //Metode til at genere loot
        public static void Loot(int xpModifier, int goldModifier, string name, string message) {
            int g = Program.currentPlayer.GetGold()*goldModifier;
            int x = Program.currentPlayer.GetXP() * xpModifier;
            int[] numbers = new[] { 0, 0, 0, 0, 0, 0, 0, 1, 1, 1, 2 };
            var pot = Program.rand.Next(0, numbers.Length);
            HUDTools.Print(message, 15);
            if (x > 0) {
                Console.ForegroundColor = ConsoleColor.DarkGreen;
                HUDTools.Print($"You've gained {x} experience points!", 10);
                Program.currentPlayer.xp += x;
            }
            if (g > 0) {
                Console.ForegroundColor = ConsoleColor.DarkYellow;
                HUDTools.Print($"You loot {g} gold coins.", 15);
                Program.currentPlayer.gold += g;
            }
            if (numbers[pot] != 0 && name != "Trap") {
                Console.ForegroundColor = ConsoleColor.DarkGray;
                HUDTools.Print($"You loot {numbers[pot]} healing potions", 20);
                Program.currentPlayer.potion += numbers[pot];
            }        
            if (name == "Treasure") {
                Console.ForegroundColor = ConsoleColor.DarkMagenta;
                int getTreasure = Program.rand.Next(0, 100+1);
                if (getTreasure <=10) {
                    if (Program.currentPlayer.currentClass == PlayerClass.Mage) {
                        switch (Program.currentPlayer.equippedArmor) {
                            case "Linen Rags":
                                Program.currentPlayer.equippedArmor = "Simple Robe";
                                Program.currentPlayer.equippedArmorValue = 2;
                                HUDTools.Print("You loot a Simple Robe");
                                break;
                            case "Simple Robe":
                                Program.currentPlayer.equippedArmor = "Elegant Robe";
                                Program.currentPlayer.equippedArmorValue = 5;
                                HUDTools.Print("You loot a Elegant Robe");
                                break;
                            case "Elegant Robe":
                                Program.currentPlayer.equippedArmor = "Arcanist's Robe";
                                Program.currentPlayer.equippedArmorValue = 9;
                                HUDTools.Print("You loot an Arcanist's Robe");
                                break;
                            case "Arcanist's Robe":
                                break;
                        }
                    } else if (Program.currentPlayer.currentClass == PlayerClass.Archer) {
                        switch (Program.currentPlayer.equippedArmor) {
                            case "Linen Rags":
                                Program.currentPlayer.equippedArmor = "Hide Armor";
                                Program.currentPlayer.equippedArmorValue = 3;
                                HUDTools.Print("You loot a Hide Armor");
                                break;
                            case "Hide Armor":
                                Program.currentPlayer.equippedArmor = "Leather Cuirass";
                                Program.currentPlayer.equippedArmorValue = 6;
                                HUDTools.Print("You loot a Leather Cuirass");
                                break;
                            case "Leather Cuirass":
                                Program.currentPlayer.equippedArmor = "Marksman's Brigandine";
                                Program.currentPlayer.equippedArmorValue = 10;
                                HUDTools.Print("You loot a Marksman's Brigandine");
                                break;
                            case "Marksman's Brigadine":
                                break;
                        }
                    } else if (Program.currentPlayer.currentClass == PlayerClass.Warrior) {
                        switch (Program.currentPlayer.equippedArmor) {
                            case "Linen Rags":
                                Program.currentPlayer.equippedArmor = "Mail Shirt";
                                Program.currentPlayer.equippedArmorValue = 4;
                                HUDTools.Print("You loot a Mail Shirt");
                                break;
                            case "Mail Shirt":
                                Program.currentPlayer.equippedArmor = "Breast Plate";
                                Program.currentPlayer.equippedArmorValue = 7;
                                HUDTools.Print("You loot a Breast Plate");
                                break;
                            case "Breast Plate":
                                Program.currentPlayer.equippedArmor = "Knight's Plate Armor";
                                Program.currentPlayer.equippedArmorValue = 11;
                                HUDTools.Print("You loot a Knight's Plate Armor");
                                break;
                            case "Knight's Plate Armor":
                                break;
                        }
                    }
                } else if (10 < getTreasure && getTreasure <=20) {
                    if (Program.currentPlayer.currentClass == PlayerClass.Mage) {
                        switch (Program.currentPlayer.equippedWeapon) {
                            case "Cracked Wand":
                                Program.currentPlayer.equippedWeapon = "Enchanted Wand";
                                Program.currentPlayer.equippedWeaponValue = 4;
                                HUDTools.Print("You loot an Enchanted Wand");
                                break;
                            case "Enchanted Wand":
                                Program.currentPlayer.equippedWeapon = "Gnarled Staff";
                                Program.currentPlayer.equippedWeaponValue = 7;
                                HUDTools.Print("You loot a Gnarled Staff");
                                break;
                            case "Gnarled Staff":
                                Program.currentPlayer.equippedWeapon = "Arcanist's Staff";
                                Program.currentPlayer.equippedWeaponValue = 11;
                                HUDTools.Print("You loot an Arcanist's Robe");
                                break;
                            case "Arcanist's Staff":
                                break;
                        }
                    }
                    else if (Program.currentPlayer.currentClass == PlayerClass.Archer) {
                        switch (Program.currentPlayer.equippedWeapon) {
                            case "Flimsy Bow":
                                Program.currentPlayer.equippedWeapon = "Short Bow";
                                Program.currentPlayer.equippedWeaponValue = 3;
                                HUDTools.Print("You loot a Short Bow");
                                break;
                            case "Short Bow":
                                Program.currentPlayer.equippedWeapon = "Long Bow";
                                Program.currentPlayer.equippedWeaponValue = 6;
                                HUDTools.Print("You loot a Long Bow");
                                break;
                            case "Long Bow":
                                Program.currentPlayer.equippedWeapon = "Marksman's Recurve";
                                Program.currentPlayer.equippedWeaponValue = 10;
                                HUDTools.Print("You loot a Marksman's Recurve");
                                break;
                            case "Marksman's Recurve":
                                break;
                        }
                    }
                    else if (Program.currentPlayer.currentClass == PlayerClass.Warrior) {
                        switch (Program.currentPlayer.equippedWeapon) {
                            case "Rusty Sword":
                                Program.currentPlayer.equippedWeapon = "Steel Sword";
                                Program.currentPlayer.equippedWeaponValue = 2;
                                HUDTools.Print("You loot a Steel Sword");
                                break;
                            case "Steel Sword":
                                Program.currentPlayer.equippedWeapon = "Longsword";
                                Program.currentPlayer.equippedWeaponValue = 5;
                                HUDTools.Print("You loot a Longsword");
                                break;
                            case "Longsword":
                                Program.currentPlayer.equippedWeapon = "Knight's Greatsword";
                                Program.currentPlayer.equippedWeaponValue = 9;
                                HUDTools.Print("You loot a Knight's Greatsword");
                                break;
                            case "Knight's Greatsword":
                                break;
                        }
                    }
                }
            }
            Console.ResetColor();
            HUDTools.PlayerPrompt();
        }
        public static void Heal(bool combat, string name, int power, int turn) {
            if (combat) {
                if (Program.currentPlayer.potion == 0) {
                    HUDTools.Print("No potions left!", 20);
                    int damage = power - Program.currentPlayer.TotalArmorValue();
                    if (damage < 0)
                        damage = 0;
                    HUDTools.Print($"The {name} attacks you while you fumble in your bags and lose {damage} health!", 20);
                    Program.currentPlayer.health -= damage;
                    HUDTools.WriteCombatLog("heal",turn, damage,0 );
                }
                else {
                    if (Program.currentPlayer.currentClass == Player.PlayerClass.Mage) {
                        HUDTools.Print("You use a potion amplified by your magic", 30);
                    }
                    else {
                        HUDTools.Print("You use a potion", 20);
                    }
                    Program.currentPlayer.health += Program.currentPlayer.potionValue + ((Program.currentPlayer.currentClass == Player.PlayerClass.Mage) ? 3 + Program.currentPlayer.level : 0);
                    if (Program.currentPlayer.health > Program.currentPlayer.maxHealth) {
                        Program.currentPlayer.health = Program.currentPlayer.maxHealth;
                    }                    
                    if (Program.currentPlayer.health == Program.currentPlayer.maxHealth) {
                        HUDTools.Print("You heal to max health!", 20);
                    }
                    else {
                        HUDTools.Print($"You gain {Program.currentPlayer.potionValue+ ((Program.currentPlayer.currentClass == Player.PlayerClass.Mage) ? 3 + Program.currentPlayer.level : 0)} health", 20);
                    }
                    HUDTools.Print($"As you drink, the {name} strikes you.", 20);
                    int damage = (power / 2) - Program.currentPlayer.TotalArmorValue();
                    if (damage < 0)
                        damage = 0;
                    HUDTools.Print($"You lose {damage} health", 20);
                    Program.currentPlayer.health -= damage;
                    HUDTools.WriteCombatLog("heal",turn,damage,0);
                    Program.currentPlayer.potion--;
                }
            } else {
                if (Program.currentPlayer.potion == 0) {
                    HUDTools.Print("No potions left!", 20);
                }
                else {
                    if (Program.currentPlayer.currentClass == Player.PlayerClass.Mage) {
                        HUDTools.Print("You use a potion amplified by your magic", 30);
                    }
                    else {
                        HUDTools.Print("You use a potion", 20);
                    }
                    Program.currentPlayer.health += Program.currentPlayer.potionValue + ((Program.currentPlayer.currentClass == Player.PlayerClass.Mage) ? 3 + Program.currentPlayer.level : 0);
                    if (Program.currentPlayer.health > Program.currentPlayer.maxHealth) {
                        Program.currentPlayer.health = Program.currentPlayer.maxHealth;
                    }
                    Program.currentPlayer.potion -= 1;
                    if (Program.currentPlayer.health == Program.currentPlayer.maxHealth) {
                        HUDTools.Print("You heal to max health!", 20);
                    }
                    else {
                        HUDTools.Print($"You gain {Program.currentPlayer.potionValue+((Program.currentPlayer.currentClass == Player.PlayerClass.Mage) ? 3 + Program.currentPlayer.level : 0)} health", 20);
                    }
                }
            }

            HUDTools.PlayerPrompt();
        }
        public static int Attack(string name, int power, int turn) {
            if (Program.currentPlayer.currentClass == Player.PlayerClass.Warrior) {
                HUDTools.Print($"You swing your {Program.currentPlayer.equippedWeapon} and {name} retaliates.", 15);
            }
            else if (Program.currentPlayer.currentClass == Player.PlayerClass.Mage) {
                HUDTools.Print($"You shoot an arcane missile from your {Program.currentPlayer.equippedWeapon} and {name} retaliates.", 10);
            }
            else {
                HUDTools.Print($"You fire an arrow with your {Program.currentPlayer.equippedWeapon} and {name} retaliates.", 10);
            }
            int damage = power - Program.currentPlayer.TotalArmorValue();
            if (damage < 0)
                damage = 0;
            int attack = Program.rand.Next(1 + (Program.currentPlayer.TotalWeaponValue() + ((Program.currentPlayer.currentClass == Player.PlayerClass.Warrior) ? 1 + Program.currentPlayer.level : 0)) / 2, 1 + Program.currentPlayer.TotalWeaponValue() + Program.rand.Next(0, 4) + ((Program.currentPlayer.currentClass == Player.PlayerClass.Warrior) ? 1 + Program.currentPlayer.level : 0)+1);
            HUDTools.Print($"You lose {damage} health and you deal {attack} damage", 10);
            Program.currentPlayer.health -= damage;
            HUDTools.WriteCombatLog("attack",turn,damage,attack);
            HUDTools.PlayerPrompt();
            return attack;
        }
        public static int Defend(string name, int power, int turn) {
            HUDTools.Print($"You defend the incoming attack from {name}", 20);
            int damage = (power / Program.currentPlayer.TotalArmorValue());
            if (damage < 0)
                damage = 0;
            int attack = Program.rand.Next(1 + Program.currentPlayer.TotalWeaponValue() / 3, (4 + Program.currentPlayer.TotalWeaponValue()) / 2);
            HUDTools.Print($"You lose {damage} health and you deal {attack} damage", 20);
            Program.currentPlayer.health -= damage;
            HUDTools.WriteCombatLog("defend", turn,damage,attack);
            HUDTools.PlayerPrompt();
            return attack;
        }
        public static bool RunAway(string name, int power,int turn) {
            bool escaped = false;
            if (Program.currentPlayer.currentClass != Player.PlayerClass.Archer && Program.rand.Next(0, 2) == 0 || name == "Human captor") {
                HUDTools.Print($"You try to sprint away from the {name}, it strikes and knocks you down", 20);
                int damage = power - Program.currentPlayer.TotalArmorValue();
                if (damage < 0)
                    damage = 0;
                HUDTools.Print($"You lose {damage} health and are unable to escape this turn.", 20);
                Program.currentPlayer.health -= damage;
                HUDTools.WriteCombatLog("run",turn, damage, 0);
                HUDTools.PlayerPrompt();
            }
            else {
                if (Program.currentPlayer.currentClass == Player.PlayerClass.Archer) {
                    HUDTools.Print($"You use your crazy ninja moves to evade the {name} and you successfully escape!",20);
                }
                else {
                    HUDTools.Print($"You barely manage to shake off the {name} and you successfully escape.",20);
                }
                HUDTools.PlayerPrompt();
                escaped = true;
            }
            return escaped;
        }
    }
}
