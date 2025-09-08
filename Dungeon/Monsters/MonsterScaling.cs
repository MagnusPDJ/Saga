using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Saga.Dungeon.Monsters
{
    public class MonsterScaling
    {
        public double HealthMultiplier { get; set; } = 1.0;
        public double AttackMultiplier { get; set; } = 1.0;
        public double ArmorMultiplier { get; set; } = 1.0;
        public double PhysicalResistancesMultiplier {  get; set; } = 1.0;
        public double ElementalResistancesMultiplier { get; set; } = 1.0;
        public double MagicalResistancesMultiplier { get; set; } = 1.0;

        public int FlatHealthBonus { get; set; } = 0;
        public int FlatAttackBonus { get; set; } = 0;
        public int FlatArmorBonus { get; set; } = 0;
        public int FlatPhysicalResistanceBonus { get; set; } = 0;
        public int FlatElementalResistanceBonus { get; set; } = 0;
        public int FlatMagicalResistanceBonus { get; set; } = 0;

        public int HealthPerLevel { get; set; } = 0;
        public int AttackPerLevel { get; set; } = 0;
        public int ArmorPerLevel { get; set; } = 0;
        public int PhysicalResistancesPerLevel { get; set; } = 0;
        public int ElementalResistancesPerLevel { get; set; } = 0;
        public int MagicalResistancesPerLevel { get; set; } = 0;
    }
}
