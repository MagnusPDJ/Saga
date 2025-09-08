using Saga.Assets;

namespace Saga.Dungeon.Monsters
{
    [Discriminator("mimic")]
    [MonsterTag("Mythical")]
    public class Mimic : Enemy, IMythical
    {
        public Mimic(Enemy baseMonster, MonsterScaling? scaling = null, int playerLevel = 1, DungeonTemplate? dungeon = null) {          
            CopyFrom(baseMonster);
            ApplyScaling(scaling, playerLevel, dungeon);
            Health = MaxHealth;
        }

        private void CopyFrom(Enemy m) {
            Name = m.Name;
            PlayerKillDescription = m.PlayerKillDescription;
            MaxHealth = m.MaxHealth;
            Armor = m.Armor;
            Attack = m.Attack;
            MagicalResistance = m.MagicalResistance;
            ElementalResistance = m.ElementalResistance;
            PhysicalResistance = m.PhysicalResistance;
            Tags = m.Tags;
            LootTable = m.LootTable;
            SpawnWeight = m.SpawnWeight;
            Scaling = m.Scaling;
            ExpGain = m.ExpGain;
            GoldModifier = m.GoldModifier;
        }

        private void ApplyScaling(MonsterScaling? scaling, int playerLevel, DungeonTemplate? dungeon) {
            if (scaling == null) return;
            double d = dungeon?.DifficultyMultiplier ?? 1.0;

            MaxHealth = (int)((MaxHealth * scaling.HealthMultiplier + scaling.FlatHealthBonus) * d);
            Attack = (int)((Attack + scaling.FlatAttackBonus) * d);
            Armor = (int)((Armor + scaling.FlatArmorBonus) * d);
            foreach (var kv in PhysicalResistance) {
                PhysicalResistance[kv.Key] = (int)((kv.Value * scaling.PhysicalResistancesMultiplier + scaling.FlatPhysicalResistanceBonus) * d);
            }
            foreach (var kv in ElementalResistance) {
                ElementalResistance[kv.Key] = (int)((kv.Value * scaling.ElementalResistancesMultiplier + scaling.FlatElementalResistanceBonus) * d);
            }
            foreach (var kv in MagicalResistance) {
                MagicalResistance[kv.Key] = (int)((kv.Value * scaling.MagicalResistancesMultiplier + scaling.FlatMagicalResistanceBonus) * d);
            }

            MaxHealth += (int)(scaling.HealthPerLevel * playerLevel * d);
            Attack += (int)(scaling.AttackPerLevel * playerLevel * d);
            Armor += (int)(scaling.ArmorPerLevel * playerLevel * d);
            foreach (var kv in PhysicalResistance) {
                PhysicalResistance[kv.Key] = kv.Value + (int)(scaling.PhysicalResistancesPerLevel * playerLevel * d);
            }
            foreach (var kv in ElementalResistance) {
                ElementalResistance[kv.Key] = kv.Value + (int)(scaling.ElementalResistancesPerLevel * playerLevel * d);
            }
            foreach (var kv in MagicalResistance) {
                MagicalResistance[kv.Key] = kv.Value + (int)(scaling.MagicalResistancesPerLevel * playerLevel * d);
            }
        }
    }
}
