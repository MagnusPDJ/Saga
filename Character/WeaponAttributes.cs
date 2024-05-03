using System;

namespace Saga.Character
{
    [Serializable]
    public class WeaponAttributes
    {
        public int MinDamage { get; set; }
        public int MaxDamage { get; set; }
        public int AttackSpeed { get; set; }
    }
}
