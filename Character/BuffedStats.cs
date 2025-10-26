using Saga.Character.Buffs;
using Saga.Character.DmgLogic;
using Saga.Items;
using System.Text.Json.Serialization;

namespace Saga.Character
{
    public class BuffedStats
    {
        private Player? _player;

        public List<IBuff> ActiveBuffs { get; set; } = [];

        public event Action? BuffsChanged;

        public int BuffStr { get; private set; }
        public int BuffDex { get; private set; }
        public int BuffInt { get; private set; }
        public int BuffCon { get; private set; }
        public int BuffWP { get; private set; }
        public int BuffAwa { get; private set; }
        public int BuffVirtue { get; private set; }
        public Dictionary<PhysicalType, int> BuffPhysicalRes { get; private set; } = new() {
            { PhysicalType.Normal, 0 },
            { PhysicalType.Piercing, 0 },
            { PhysicalType.Crushing, 0 }
        };
        public Dictionary<ElementalType, int> BuffElementRes { get; private set; } = new() {
            { ElementalType.Frost, 0 },
            { ElementalType.Fire, 0 },
            { ElementalType.Poison, 0 },
            { ElementalType.Lightning, 0 }
        };
        public Dictionary<MagicalType, int> BuffMagicRes { get; private set; } = new() {
            { MagicalType.Arcane, 0 },
            { MagicalType.Chaos, 0 },
            { MagicalType.Void, 0 },
            { MagicalType.Nature, 0 },
            { MagicalType.Life, 0 },
            { MagicalType.Death, 0 }
        };
        public int BuffInitiative { get; private set; }
        public int BuffHealth { get; private set; }
        public int BuffMana { get; private set; }
        public int BuffManaRegenRate { get; private set; }
        public int BuffActionPoints { get; private set; }
        public int BuffAttackSpeed { get; private set; }
        public int BuffCastingSpeed { get; private set; }
        public int BuffArmorRating { get; private set; }

        public BuffedStats(Player player) {
            AttachToPlayer(player);
        }

        [JsonConstructor]
        private BuffedStats() { }

        public void AttachToPlayer(Player player) {
            _player = player;
            _player.PlayerChanged += RecalculateBuffedStats;
            RecalculateBuffedStats();
        }

        public void RecalculateBuffedStats() {
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

            foreach (var buff in ActiveBuffs) {
                foreach (var buffedStat in buff.AsEnumerable()) {
                    switch (buffedStat.Key) {
                        case "Strength":
                            bonusStr += buffedStat.Value;
                            break;
                        case "Dexterity":
                            bonusDex += buffedStat.Value;
                            break;
                        case "Intellect":
                            bonusInt += buffedStat.Value;
                            break;
                        case "Constitution":
                            bonusCon += buffedStat.Value;
                            break;
                        case "WillPower":
                            bonusWP += buffedStat.Value;
                            break;
                        case "Awareness":
                            bonusAwa += buffedStat.Value;
                            break;
                        case "Virtue":
                            bonusVirtue += buffedStat.Value;
                            break;
                        case "Initiative":
                            bonusInitiative += buffedStat.Value;
                            break;
                        case "MaxHealth":
                            bonusHealth += buffedStat.Value;
                            break;
                        case "MaxMana":
                            bonusMana += buffedStat.Value;
                            break;
                        case "ManaRegenRate":
                            bonusManaRegenRate += buffedStat.Value;
                            break;
                        case "ActionPoints":
                            bonusActionPoints += buffedStat.Value;
                            break;
                        case "ArmorRating":
                            armorRating += buffedStat.Value;
                            break;
                        case "AttackSpeed":
                            bonusAttackSpeed += buffedStat.Value;
                            break;
                        case "CastingSpeed":
                            bonusCastingSpeed += buffedStat.Value;
                            break;
                    }
                }
                foreach (var kv in buff.PhysicalRes) {
                    if (bonusPhysicalRes.ContainsKey(kv.Key))
                        bonusPhysicalRes[kv.Key] += kv.Value;
                    else
                        bonusPhysicalRes[kv.Key] = kv.Value;
                }
                foreach (var kv in buff.ElementalRes) {
                    if (bonusElementRes.ContainsKey(kv.Key))
                        bonusElementRes[kv.Key] += kv.Value;
                    else
                        bonusElementRes[kv.Key] = kv.Value;
                }
                foreach (var kv in buff.MagicalRes) {
                    if (bonusMagicRes.ContainsKey(kv.Key))
                        bonusMagicRes[kv.Key] += kv.Value;
                    else
                        bonusMagicRes[kv.Key] = kv.Value;
                }
            }
            //Pri affix
            BuffStr = bonusStr;
            BuffDex = bonusDex;
            BuffInt = bonusInt;
            BuffCon = bonusCon;
            BuffWP = bonusWP;
            BuffAwa = bonusAwa;
            BuffVirtue = bonusVirtue;
            //Second affix
            BuffPhysicalRes = bonusPhysicalRes;
            BuffElementRes = bonusElementRes;
            BuffMagicRes = bonusMagicRes;
            BuffInitiative = bonusInitiative;
            BuffHealth = bonusHealth;
            BuffMana = bonusMana;
            BuffManaRegenRate = bonusManaRegenRate;
            BuffActionPoints = bonusActionPoints;
            BuffArmorRating = armorRating;
            //W affix
            BuffAttackSpeed = bonusAttackSpeed;
            BuffCastingSpeed = bonusCastingSpeed;
        }

        public void AddBuff(IBuff buff) {
            var existing = ActiveBuffs.Find(b => b.Name == buff.Name);
            if (existing != null) {
                existing.RemainingTurns = Math.Max(existing.RemainingTurns, buff.Duration);
            } else {
                ActiveBuffs.Add(buff);
                buff.OnApply(_player!);
            }
            BuffsChanged?.Invoke();
        }

        public void ClearAllBuffs() {
            ActiveBuffs.Clear();
            BuffsChanged?.Invoke();
        }

        public void TickBuffs() {
            for (int i = ActiveBuffs.Count - 1; i >= 0; i--) {
                var buff = ActiveBuffs[i];
                buff.RemainingTurns--;
                if (buff.RemainingTurns <= 0) {
                    buff.OnRemove(_player!);
                    ActiveBuffs.RemoveAt(i);
                    BuffsChanged?.Invoke();
                }
            }
        }
    }
}
