
namespace Saga.Character
{
    public class DerivedStats
    {
        private readonly Player _player;

        public int MaxHealth { get; private set; }
        public int MaxMana { get; private set; }
        public int ArmorRating { get; private set; }
        public int Initiative { get; private set; }
        public int ElementalResistance { get; private set; }
        public int MagicalResistance { get; private set; }

        public DerivedStats(Player player) {
            _player = player;
            _player.PlayerChanged += RecalculateDerivedStats;
            RecalculateDerivedStats();
        }

        private void RecalculateDerivedStats() {
            MaxHealth = CalculateMaxHealth();
            MaxMana = CalculateMaxMana();
            ArmorRating = CalculateArmorRating();
            Initiative = CalculateInitiative();
            ElementalResistance = CalculateElementalResistance();
            MagicalResistance = CalculateMagicalResistance();
        }
        int CalculateMaxHealth() {
            int baseHealth = 5 + 5 * _player.Attributes.Constitution;
            //int equipmentBonus = _player.Equipment.BonusHealth;
            return baseHealth;
        }
        int CalculateMaxMana() {
            int baseMana = 5 + 5 * _player.Attributes.WillPower;
            return baseMana;
        }
        int CalculateArmorRating() {
            int baseArmorRating = _player.Attributes.Dexterity;
            return baseArmorRating;
        }
        int CalculateInitiative() {
            int baseInitiative = _player.Attributes.Awareness;
            return baseInitiative;
        }
        int CalculateElementalResistance() {
            int baseElementalResistance = _player.Attributes.Strength;
            return baseElementalResistance;
        }
        int CalculateMagicalResistance() {
            int baseMagicalResistance = _player.Attributes.Intellect;
            return baseMagicalResistance;
        }
    }
}