
using System.Text.Json.Serialization;

namespace Saga.Character
{
    public class DerivedStats
    {
        private Player? _player;
        [JsonInclude]
        public int MaxHealth { get; private set; }
        [JsonInclude]
        public int MaxMana { get; private set; }
        [JsonInclude]
        public int DamageReduction { get; private set; }
        [JsonInclude]
        public int Initiative { get; private set; }
        [JsonInclude]
        public int ElementalResistance { get; private set; }
        [JsonInclude]
        public int MagicalResistance { get; private set; }
        [JsonInclude]
        public int ManaRegenRate { get; private set; }
        [JsonInclude]
        public int ActionPoints {  get; private set; }
        [JsonInclude]
        public int AttackSpeed { get; private set; }
        [JsonInclude]
        public int CastingSpeed { get; private set; }
        [JsonInclude]
        public int ArmorRating { get; private set; }

        public DerivedStats(Player player) {
            AttachToPlayer(player);
        }

        [JsonConstructor]
        private DerivedStats() { }
        public void AttachToPlayer(Player player) {
            _player = player;
            _player.PlayerChanged += RecalculateDerivedStats;
            RecalculateDerivedStats();
        }
        private void RecalculateDerivedStats() {
            ArmorRating = _player!.Equipment.ArmorRating;
            MaxHealth = CalculateMaxHealth() + _player!.Equipment.BonusHealth;
            MaxMana = CalculateMaxMana() + _player!.Equipment.BonusMana;
            DamageReduction = CalculateDamageReduction() + _player!.Equipment.BonusDamageReduction;
            Initiative = CalculateInitiative() + _player!.Equipment.BonusInitiative;
            ElementalResistance = CalculateElementalResistance() + _player!.Equipment.BonusElementRes;
            MagicalResistance = CalculateMagicalResistance() + _player!.Equipment.BonusMagicRes;
            ActionPoints = CalculateActionPoints() + _player!.Equipment.BonusActionPoints;
            ManaRegenRate = CalculateManaRegenRate() + _player!.Equipment.BonusManaRegenRate;
            AttackSpeed = CalculateAttackSpeed() + _player!.Equipment.BonusAttackSpeed;
            CastingSpeed = CalculateCastingSpeed() + _player!.Equipment.BonusCastingSpeed;
        }
        int CalculateMaxHealth() {
            int baseHealth = 5;
            if (_player!.CurrentClass == "Warrior") baseHealth += 6 * _player!.Attributes.Constitution;
            if (_player!.CurrentClass == "Mage") baseHealth += 4 * _player!.Attributes.Constitution;
            if (_player!.CurrentClass == "Archer") baseHealth += 5 * _player!.Attributes.Constitution;
            return baseHealth;
        }
        int CalculateMaxMana() {
            int baseMana = 5;
            if (_player!.CurrentClass == "Warrior") baseMana += 4 * _player!.Attributes.WillPower;
            if (_player!.CurrentClass == "Mage") baseMana += 6 * _player!.Attributes.WillPower;
            if (_player!.CurrentClass == "Archer") baseMana += 5 * _player!.Attributes.WillPower;
            return baseMana;
        }
        int CalculateDamageReduction() {
            int baseArmorRating = _player!.Attributes.Dexterity / 2;
            return baseArmorRating;
        }
        int CalculateInitiative() {
            int baseInitiative = _player!.Attributes.Awareness / 2;
            return baseInitiative;
        }
        int CalculateElementalResistance() {
            int baseElementalResistance = _player!.Attributes.Strength / 2;
            return baseElementalResistance;
        }
        int CalculateMagicalResistance() {
            int baseMagicalResistance = _player!.Attributes.Intellect / 2;
            return baseMagicalResistance;
        }
        int CalculateActionPoints() {
            int baseActionPoints = 10 + _player!.Attributes.Virtue / 20;
            return baseActionPoints;
        }
        int CalculateAttackSpeed() {
            int baseAttackSpeed = 1 + (_player!.Attributes.Awareness + _player.Attributes.Constitution) / 50;
            return baseAttackSpeed;
        }
        int CalculateManaRegenRate() {
            int baseManaRegenRate = 2 + (_player!.Attributes.Constitution + _player.Attributes.WillPower) / 4;
            return baseManaRegenRate;
        }
        int CalculateCastingSpeed() {
            int baseCastingSpeed = 1 + (_player!.Attributes.Awareness + _player.Attributes.WillPower) / 50;
            return baseCastingSpeed;
        }
    }
}