using Saga.Items;
using System.Text.Json.Serialization;
using System.Diagnostics.CodeAnalysis;

namespace Saga.Character
{  
    public class Equipment
    {
        private Player? _player;

        public IEquipable? Headgear { get; set; }
        public IEquipable? Torso { get; set; }
        public IEquipable? Legs { get; set; }
        public IEquipable? Feet { get; set; }
        public IEquipable? Bracers { get; set; }
        public IEquipable? Shoulders { get; set; }
        public IEquipable? Belt {  get; set; }
        public IEquipable? Cape {  get; set; }
        public IEquipable? Gloves { get; set; }
        public IEquipable? Right_Hand { get; set; }
        public IEquipable? Left_Hand { get; set; }
        public IEquipable? Amulet { get; set; }
        public IEquipable? Finger_1 { get; set; }
        public IEquipable? Finger_2 { get; set; }
        public IEquipable? Crest { get; set; }
        public IEquipable? Trinket { get; set; }
        public IConsumable[] Potion { get; set; } = new IConsumable[4];

        public event Action? EquipmentChanged;

        public int BonusStr { get; private set; }
        public int BonusDex { get; private set; }
        public int BonusInt { get; private set; }
        public int BonusCon { get; private set; }
        public int BonusWP { get; private set; }
        public int BonusAwa { get; private set; }
        public int BonusVirtue { get; private set; }
        public int BonusDamageReduction { get; private set; }
        public int BonusElementRes { get; private set; }
        public int BonusMagicRes { get; private set; }
        public int BonusInitiative { get; private set; }
        public int BonusHealth { get; private set; }
        public int BonusMana { get; private set; }
        public int BonusManaRegenRate { get; private set; }
        public int BonusActionPoints { get; private set; }
        public int BonusAttackSpeed { get; private set; }
        public int BonusCastingSpeed { get; private set; }
        public int ArmorRating { get; private set; }

        public Equipment(Player player) {
            AttachToPlayer(player);
        }

        [JsonConstructor]
        private Equipment() { }
        public void AttachToPlayer(Player player) {
            _player = player;
            _player.PlayerChanged += RecalculateArmorStats;
            RecalculateArmorStats();
        }

        /// <summary>
        /// Enumerates all slots as key/value pairs.
        /// </summary>
        public IEnumerable<KeyValuePair<string, IEquipable?>> AsEnumerable() {
            yield return new(nameof(Headgear), Headgear);
            yield return new(nameof(Torso), Torso);
            yield return new(nameof(Legs), Legs);
            yield return new(nameof(Feet), Feet);
            yield return new(nameof(Bracers), Bracers);
            yield return new(nameof(Shoulders), Shoulders);
            yield return new(nameof(Belt), Belt);
            yield return new(nameof(Cape), Cape);
            yield return new(nameof(Gloves), Gloves);
            yield return new(nameof(Right_Hand), Right_Hand);
            yield return new(nameof(Left_Hand), Left_Hand);
            yield return new(nameof(Amulet), Amulet);
            yield return new(nameof(Finger_1), Finger_1);
            yield return new(nameof(Finger_2), Finger_2);
            yield return new(nameof(Crest), Crest);
            yield return new(nameof(Trinket), Trinket);
        }

        /// <summary>
        /// Tries to get an item in a slot by enum.
        /// </summary>
        public bool TryGetSlot(Slot slot, [NotNullWhen(true)] out IEquipable? value) {
            var prop = GetType().GetProperty(slot.ToString());
            value = (IEquipable?)prop?.GetValue(this);
            return value is not null;
        }

        /// <summary>
        /// Sets an item into a slot by enum.
        /// </summary>
        public void SetSlot(Slot slot, IEquipable? item) {
            var prop = GetType().GetProperty(slot.ToString());
            if (prop != null && typeof(IEquipable).IsAssignableFrom(prop.PropertyType)) {
                prop.SetValue(this, item);
            } else {
                throw new ArgumentException($"Invalid slot {slot}");
            }
            EquipmentChanged?.Invoke();
        }

        /// <summary>
        /// Convenience iterator to use foreach if desired.
        /// </summary>
        public IEnumerable<KeyValuePair<string, IEquipable?>> GetSlots() => AsEnumerable();

        private void RecalculateArmorStats() {
            //Pri affix
            int bonusStr = 0;
            int bonusDex = 0;
            int bonusInt = 0;
            int bonusCon = 0;
            int bonusWP = 0;
            int bonusAwa = 0;
            int bonusVirtue = 0;
            //Second affix
            int armorRating = 0;
            int bonusDamageReduction = 0;
            int bonusElementRes = 0;
            int bonusMagicRes = 0;
            int bonusInitiative = 0;
            int bonusHealth = 0;
            int bonusMana = 0;          
            int bonusActionPoints = 0;
            int bonusManaRegenRate = 0;
            //W affix
            int bonusAttackSpeed = 0;
            int bonusCastingSpeed = 0;

            foreach (var slot in AsEnumerable()) {
                if (slot.Value is not null && slot.Value is IArmor armor) {
                    foreach (var pAffix in armor.PrimaryAffixes.AsEnumerable()) {
                        if (pAffix.Key == "Strength") {
                            bonusStr += pAffix.Value;
                        } else if (pAffix.Key == "Dexterity") {
                            bonusDex += pAffix.Value;
                        } else if (pAffix.Key == "Intellect") {
                            bonusInt += pAffix.Value;
                        } else if (pAffix.Key == "Constitution") {
                            bonusCon += pAffix.Value;
                        } else if (pAffix.Key == "WillPower") {
                            bonusWP += pAffix.Value;
                        } else if (pAffix.Key == "Awareness") {
                            bonusAwa += pAffix.Value;
                        } else if (pAffix.Key == "Virtue") {
                            bonusVirtue += pAffix.Value;
                        } 
                    }
                    foreach (var sAffix in armor.SecondaryAffixes.AsEnumerable()) {
                        if (sAffix.Key == "DamageReduction") {
                            bonusDamageReduction += sAffix.Value;
                        } else if (sAffix.Key == "ElementalResistance") {
                            bonusElementRes += sAffix.Value;
                        } else if (sAffix.Key == "MagicalResistance") {
                            bonusMagicRes += sAffix.Value;
                        } else if (sAffix.Key == "Initiative") {
                            bonusInitiative += sAffix.Value;
                        } else if (sAffix.Key == "MaxHealth") {
                            bonusHealth += sAffix.Value;
                        } else if (sAffix.Key == "MaxMana") {
                            bonusMana += sAffix.Value;
                        } else if (sAffix.Key == "ManaRegenRate") {
                            bonusManaRegenRate += sAffix.Value;
                        } else if (sAffix.Key == "ActionPoints") {
                            bonusActionPoints += sAffix.Value;
                        } else if (sAffix.Key == "ArmorRating") {
                            armorRating += sAffix.Value;
                        }
                    }
                } else if (slot.Value is not null && slot.Value is IWeapon weapon) {
                    foreach (var waffix in weapon.WeaponAttributes.AsEnumerable()) {
                        if (waffix.Key == "AttackSpeed") {
                            bonusAttackSpeed += waffix.Value;
                            bonusCastingSpeed += waffix.Value;
                        }
                    }
                }
            }
            //Pri affix
            BonusStr = bonusStr;
            BonusDex = bonusDex;
            BonusInt = bonusInt;
            BonusCon = bonusCon;
            BonusWP = bonusWP;
            BonusAwa = bonusAwa;
            BonusVirtue = bonusVirtue;
            //Second affix
            BonusDamageReduction = bonusDamageReduction;
            BonusElementRes = bonusElementRes;
            BonusMagicRes = bonusMagicRes;
            BonusInitiative = bonusInitiative;
            BonusHealth = bonusHealth;
            BonusMana = bonusMana;
            BonusManaRegenRate = bonusManaRegenRate;
            BonusActionPoints = bonusActionPoints;
            ArmorRating = armorRating;
            //W affix
            BonusAttackSpeed = bonusAttackSpeed;
            BonusCastingSpeed = bonusCastingSpeed;
        }
    }
}
