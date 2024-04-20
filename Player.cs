using System;
using System.Collections.Generic;
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
        public int maxHealth = 10;
        public int health = 10;
        public int damage = 1;
        public int armorValue = 0;
        public int potion = 5;
        public int weaponValue = 0;
        public int potionValue = 5;
        public int mods = 0;

        //Monster liv skaleret på spilleren.
        public int GetHealth() {
            int upper = (2 * mods+5);
            int lower = (mods+2);
            return Program.rand.Next(lower, upper);
        }

        //Monster skade skaleret på spilleren.
        public int GetPower() {
            int upper = (2 * mods + 3);
            int lower = (mods + 1);
            return Program.rand.Next(lower, upper);
        }

        //Guld drop skaleret på spilleren.
        public int GetGold() {
            int upper = (15 * mods + 50);
            int lower = (10 * mods + 10);
            return Program.rand.Next(lower, upper);
        }
    }
}
