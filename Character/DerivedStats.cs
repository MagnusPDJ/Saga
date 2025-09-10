
using Saga.Character.DmgLogic;
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
        public int Initiative { get; private set; }
        [JsonInclude]
        public Dictionary<PhysicalType, int> PhysicalResistance { get; private set; } = new() {
            { PhysicalType.Normal, 0 },
            { PhysicalType.Piercing, 0 },
            { PhysicalType.Crushing, 0 }
        };
        [JsonInclude]
        public Dictionary<ElementalType, int> ElementalResistance { get; private set; } = new() { 
            { ElementalType.Frost, 0 }, 
            { ElementalType.Fire, 0 }, 
            { ElementalType.Poison, 0 }, 
            { ElementalType.Lightning, 0 } 
        };
        [JsonInclude]
        public Dictionary<MagicalType, int> MagicalResistance { get; private set; } = new() {
            { MagicalType.Arcane, 0 },
            { MagicalType.Chaos, 0 },
            { MagicalType.Void, 0 },
            { MagicalType.Nature, 0 },
            { MagicalType.Life, 0 },      
            { MagicalType.Death, 0 }
        };
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
            int temp_MaxHealth = MaxHealth;
            int temp_MaxMana = MaxMana;
            ArmorRating = _player!.Equipment.ArmorRating;
            MaxHealth = CalculateMaxHealth() + _player!.Equipment.BonusHealth;
            MaxMana = CalculateMaxMana() + _player!.Equipment.BonusMana;
            Initiative = CalculateInitiative() + _player!.Equipment.BonusInitiative;

            Dictionary<PhysicalType, int> resultPRes = new(CalculatePhysicalResistance());
            foreach (var kv in _player!.Equipment.BonusPhysicalRes) {
                if (resultPRes.ContainsKey(kv.Key))
                    resultPRes[kv.Key] += kv.Value;
                else
                    resultPRes[kv.Key] = kv.Value;
            }
            PhysicalResistance = resultPRes;

            Dictionary<ElementalType, int> resultERes = new(CalculateElementalResistance());
            foreach (var kv in _player!.Equipment.BonusElementRes) {
                if (resultERes.ContainsKey(kv.Key))
                    resultERes[kv.Key] += kv.Value;
                else
                    resultERes[kv.Key] = kv.Value;
            }
            ElementalResistance =  resultERes;

            Dictionary<MagicalType, int> resultMRes = new(CalculateMagicalResistance());
            foreach (var kv in _player!.Equipment.BonusMagicRes) {
                if (resultMRes.ContainsKey(kv.Key))
                    resultMRes[kv.Key] += kv.Value;
                else
                    resultMRes[kv.Key] = kv.Value;
            }
            MagicalResistance = resultMRes;

            ActionPoints = CalculateActionPoints() + _player!.Equipment.BonusActionPoints;
            ManaRegenRate = CalculateManaRegenRate() + _player!.Equipment.BonusManaRegenRate;
            AttackSpeed = CalculateAttackSpeed() + _player!.Equipment.BonusAttackSpeed;
            CastingSpeed = CalculateCastingSpeed() + _player!.Equipment.BonusCastingSpeed;
            if (temp_MaxHealth < MaxHealth) {
                _player.SetHealth(MaxHealth);
            }
            if (temp_MaxMana < MaxMana) {
                _player.SetMana(MaxMana);              
            }
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
        int CalculatePhysicalResistance() {
            int basePhysicalResistance = _player!.Attributes.Dexterity / 2;
            return basePhysicalResistance;
        }
        int CalculateInitiative() {
            int baseInitiative = 1 + _player!.Attributes.Awareness / 2;
            return baseInitiative;
        }
        Dictionary<ElementalType, int> CalculateElementalResistance() {
            int baseElementalResistance = _player!.Attributes.Strength / 2;
            Dictionary<ElementalType, int> baseElementalResistances = new() {
            {ElementalType.Frost, baseElementalResistance },
            { ElementalType.Fire, baseElementalResistance },
            { ElementalType.Poison, baseElementalResistance },
            { ElementalType.Lightning, baseElementalResistance }
        };
            return baseElementalResistances;
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