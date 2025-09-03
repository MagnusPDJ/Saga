
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
        public int ArmorRating { get; private set; }
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
            MaxHealth = CalculateMaxHealth() + _player!.Equipment.BonusHealth;
            MaxMana = CalculateMaxMana() + _player!.Equipment.BonusMana;
            ArmorRating = CalculateArmorRating() + _player!.Equipment.BonusArmorRating;
            Initiative = CalculateInitiative() + _player!.Equipment.BonusInitiative;
            ElementalResistance = CalculateElementalResistance() + _player!.Equipment.BonusElementRes;
            MagicalResistance = CalculateMagicalResistance() + _player!.Equipment.BonusMagicRes;
            ActionPoints = CalculateActionPoints() + _player!.Equipment.BonusActionPoints;
            ManaRegenRate = CalculateManaRegenRate() + _player!.Equipment.BonusManaRegenRate;
            AttackSpeed = CalculateAttackSpeed() + _player!.Equipment.BonusAttackSpeed;
        }
        int CalculateMaxHealth() {
            int baseHealth = 5 + 5 * _player!.Attributes.Constitution;
            return baseHealth;
        }
        int CalculateMaxMana() {
            int baseMana = 5 + 5 * _player!.Attributes.WillPower;
            return baseMana;
        }
        int CalculateArmorRating() {
            int baseArmorRating = _player!.Attributes.Dexterity;
            return baseArmorRating;
        }
        int CalculateInitiative() {
            int baseInitiative = _player!.Attributes.Awareness;
            return baseInitiative;
        }
        int CalculateElementalResistance() {
            int baseElementalResistance = _player!.Attributes.Strength;
            return baseElementalResistance;
        }
        int CalculateMagicalResistance() {
            int baseMagicalResistance = _player!.Attributes.Intellect;
            return baseMagicalResistance;
        }
        int CalculateActionPoints() {
            int baseActionPoints = _player!.Attributes.Virtue;
            return baseActionPoints;
        }
        int CalculateAttackSpeed() {
            int baseAttackSpeed = (_player!.Attributes.Awareness + _player.Attributes.WillPower) / 2;
            return baseAttackSpeed;
        }
        int CalculateManaRegenRate() {
            int baseManaRegenRate = 1 + (_player!.Attributes.Constitution + _player.Attributes.WillPower)/2;
            return baseManaRegenRate;
        }
    }
}