
using Saga.Assets;

namespace Saga.Items.Loot
{
    public static class LootSystem
    {
        //Metode til at få en tilfældig mængde guld:
        public static void GetGold(float modifier = 1) {
            int upper = (26 * Program.CurrentPlayer.Level + 61);
            int lower = (5 * Program.CurrentPlayer.Level);
            int g = (int)Math.Floor(Program.Rand.Next(lower, upper + 1) * modifier);
            if (g > 0) {
                HUDTools.Print($"\u001b[33mYou loot {g} gold coins.\u001b[0m", 15);
                Program.CurrentPlayer.Gold += g;
            }
        }
        //Metode til at få en bestemt mængde guld:
        public static void GetFixedGold(int g) {
            if (g > 0) {
                HUDTools.Print($"\u001b[33mYou loot {g} gold coins.\u001b[0m", 15);
                Program.CurrentPlayer.Gold += g;
            }
        }
        //Metode til at få healing potions, default random 0-2:
        public static void GetPotions(int amount = 0) {
            int p;
            if (amount == 0) {
                int[] numbers = [0, 0, 0, 0, 0, 0, 0, 1, 1, 1, 2];
                var picked = Program.Rand.Next(0, numbers.Length);
                p = numbers[picked];
            } else {
                p = amount;
            }
            if (p > 0) {
                var potion = Array.Find(Program.CurrentPlayer.Equipment.Potion, p => p is IItem { ItemName: "Healing Potion" });
                if (potion is not null) {
                    HUDTools.Print($"\u001b[90mYou loot {p} healing potions\u001b[0m", 20);
                    potion.PotionQuantity += p;
                }
            }
        }
        //Metode til at få en tilfældig mængde exp eller en bestemt mængde:
        public static void GetExp(int expModifier, int flatExp = 0) {
            int upper = (20 * Program.CurrentPlayer.Level + 21);
            int lower = (2 * Program.CurrentPlayer.Level);
            int x = Program.Rand.Next(lower, upper + 1) * expModifier + flatExp;
            if (x > 0) {
                HUDTools.Print($"\u001b[32mYou've gained {x} experience points!\u001b[0m", 10);
                Program.CurrentPlayer.Exp += x;
            }
            Program.CurrentPlayer.CheckForLevelUp();
        }
    }
}
