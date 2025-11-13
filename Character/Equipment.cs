using Saga.Items;
using System.Text.Json.Serialization;
using System.Diagnostics.CodeAnalysis;
using Saga.Character.DmgLogic;
using Saga.Assets;

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
        public IEquipable Pouch { get; set; } = new GoldPouch();
        public IConsumable[] Potions { get; set; } = new IConsumable[4];

        public event Action? EquipmentChanged;

        public int BonusStr { get; private set; }
        public int BonusDex { get; private set; }
        public int BonusInt { get; private set; }
        public int BonusCon { get; private set; }
        public int BonusWP { get; private set; }
        public int BonusAwa { get; private set; }
        public int BonusVirtue { get; private set; }
        public Dictionary<PhysicalType, int> BonusPhysicalRes { get; private set; } = new() {
            { PhysicalType.Normal, 0 },
            { PhysicalType.Piercing, 0 },
            { PhysicalType.Crushing, 0 }
        };
        public Dictionary<ElementalType, int> BonusElementRes { get; private set; } = new() {
            { ElementalType.Frost, 0 },
            { ElementalType.Fire, 0 },
            { ElementalType.Poison, 0 },
            { ElementalType.Lightning, 0 }
        };
        public Dictionary<MagicalType, int> BonusMagicRes { get; private set; } = new() {
            { MagicalType.Arcane, 0 },
            { MagicalType.Chaos, 0 },
            { MagicalType.Void, 0 },
            { MagicalType.Nature, 0 },
            { MagicalType.Life, 0 },      
            { MagicalType.Death, 0 }
        };
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
            yield return new(nameof(Pouch), Pouch);
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
            Dictionary<PhysicalType, int> bonusPhysicalRes = new() {
            { PhysicalType.Normal, 0 },
            { PhysicalType.Piercing, 0 },
            { PhysicalType.Crushing, 0 }
            };
            Dictionary<ElementalType, int> bonusElementRes = new() {
            { ElementalType.Frost, 0 },
            { ElementalType.Fire, 0 },
            { ElementalType.Poison, 0 },
            { ElementalType.Lightning, 0 }
            };
            Dictionary<MagicalType, int> bonusMagicRes = new() {
            { MagicalType.Arcane, 0 },
            { MagicalType.Chaos, 0 },
            { MagicalType.Void, 0 },
            { MagicalType.Nature, 0 },
            { MagicalType.Life, 0 },
            { MagicalType.Death, 0 }
            };
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
                        switch (pAffix.Key) {
                            case "Strength":
                                bonusStr += pAffix.Value;
                                break;
                            case "Dexterity":
                                bonusDex += pAffix.Value;
                                break;
                            case "Intellect":
                                bonusInt += pAffix.Value;
                                break;
                            case "Constitution":
                                bonusCon += pAffix.Value;
                                break;
                            case "WillPower":
                                bonusWP += pAffix.Value;
                                break;
                            case "Awareness":
                                bonusAwa += pAffix.Value;
                                break;
                            case "Virtue":
                                bonusVirtue += pAffix.Value;
                                break;
                        }
                    }
                    foreach (var sAffix in armor.SecondaryAffixes.AsEnumerable()) {
                        switch (sAffix.Key) {
                            case "Initiative":
                                bonusInitiative += sAffix.Value;
                                break;
                            case "MaxHealth":
                                bonusHealth += sAffix.Value;
                                break;
                            case "MaxMana":
                                bonusMana += sAffix.Value;
                                break;
                            case "ManaRegenRate":
                                bonusManaRegenRate += sAffix.Value;
                                break;
                            case "ActionPoints":
                                bonusActionPoints += sAffix.Value;
                                break;
                            case "ArmorRating":
                                armorRating += sAffix.Value;
                                break;
                        }
                    }
                    foreach (var kv in armor.SecondaryAffixes.PhysicalResistance) {
                        if (bonusPhysicalRes.ContainsKey(kv.Key))
                            bonusPhysicalRes[kv.Key] += kv.Value;
                        else
                            bonusPhysicalRes[kv.Key] = kv.Value;                    
                    }
                    foreach (var kv in armor.SecondaryAffixes.ElementalResistance) {
                        if (bonusElementRes.ContainsKey(kv.Key))
                            bonusElementRes[kv.Key] += kv.Value;
                        else
                            bonusElementRes[kv.Key] = kv.Value;
                    }
                    foreach (var kv in armor.SecondaryAffixes.MagicalResistance) {
                        if (bonusMagicRes.ContainsKey(kv.Key))
                            bonusMagicRes[kv.Key] += kv.Value;
                        else
                            bonusMagicRes[kv.Key] = kv.Value;
                    }
                } else if (slot.Value is not null && slot.Value is IWeapon weapon) {
                    foreach (var waffix in weapon.WeaponAttributes.AsEnumerable()) {
                        switch (waffix.Key) {
                            case "AttackSpeed":
                                bonusAttackSpeed += waffix.Value;
                                break;
                            case "CastingSpeed":
                                bonusCastingSpeed += waffix.Value;
                                break;
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
            BonusPhysicalRes = bonusPhysicalRes;
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

        public IConsumable? ChoosePotionToDrink() {
            if (Array.FindIndex(Potions, p => p != null) == -1) {
                Console.WriteLine(" No potions equipped.");
                TextInput.PressToContinue();
                HUDTools.ClearLastLine(3);
                return null;
            } else {
                Console.WriteLine(" Choose a potion to drink:");
                for (int i = 0; i < 4; i++) {
                    if (Potions[i] != null) {
                        Console.WriteLine($"   {i + 1} - {(Potions[i] as IItem)!.ItemName}");
                    }
                }
                string input = TextInput.PlayerPrompt();
                if (int.TryParse(input, out int slot) && 1 <= slot && slot <= 4) {
                    if (Potions[slot - 1] != null) {
                        return Potions[slot - 1];
                    } else {
                        HUDTools.Print(" No potion in that slot!", 3);
                        TextInput.PressToContinue();
                        HUDTools.ClearLastLine(3);
                        return null;
                    }
                } else {
                    HUDTools.Print(" Invalid input!", 3);
                    TextInput.PressToContinue();
                    HUDTools.ClearLastLine(3);
                    return null;
                }
            }
        }

        public void RemovePotion() {
            foreach (var potion in Potions) {
                if (potion != null && potion.PotionQuantity <= 0) {
                    int index = Array.IndexOf(Potions, potion);
                    Potions.SetValue(null, index);
                    HUDTools.Print($" You have used up all of your {(potion as IItem)!.ItemName}s in slot {index + 1}.", 10);
                }
            }
        }

        public void AddGold(int amount) {
            if (Pouch is GoldPouch goldPouch) {
                goldPouch.GoldAmount += amount;
            }
        }

        public void RemoveGold(int amount) {
            if (Pouch is GoldPouch goldPouch) {
                goldPouch.GoldAmount -= amount;
            }
        }

        public int GetGoldAmount() {
            if (Pouch is GoldPouch goldPouch) {
                return goldPouch.GoldAmount;
            } else {
                return 0;
            }
        }
    }
}
