using Saga.Character;
using Saga.Dungeon;
using System;
using Saga.Items.Loot.WeaponLootTable;

namespace Saga.Items
{
    public abstract class WeaponBase : ItemBase, IWeapon
    {
        public abstract WeaponCategory WeaponCategory { get; }
        public abstract WeaponAttributes WeaponAttributes { get; }
        public abstract WeaponAttributes GetWeaponAttributes(int level);
        public abstract int Attack(Enemy Monster);
        public override int CalculateItemPrice() {
            ItemPrice = Convert.ToInt32(ItemLevel * 100 + (WeaponAttributes.MaxDamage * 100 + WeaponAttributes.MinDamage*50)*(1 + 1 /(WeaponAttributes.MaxDamage-WeaponAttributes.MinDamage)) );
            return ItemPrice;
        }
        public static string RandomWeaponName(WeaponBase weapon) {
            string name1 = "Sturdy";
            string name2 = "Stick";
            int rand = Program.Rand.Next(10);
            switch(weapon) {
                case Axe:
                    name2 = "axe";
                    name1 = WeaponNameList(weapon, rand);
                    break;
                case Sword:
                    name2 = "sword";
                    name1 = WeaponNameList(weapon, rand);
                    break;
                case Hammer:
                    name2 = "hammer";
                    name1 = WeaponNameList(weapon, rand);
                    break;
                case Bow:
                    name2 = "bow";
                    name1 = WeaponNameList(weapon, rand);
                    break;
                case Dagger:
                    name2 = "dagger";
                    name1 = WeaponNameList(weapon, rand);
                    break;
                case Crossbow:
                    name2 = "crossbow";
                    name1 = WeaponNameList(weapon, rand);
                    break;
                case Staff:
                    name2 = "staff";
                    name1 = WeaponNameList(weapon, rand);
                    break;
                case Wand:
                    name2 = "wand";
                    name1 = WeaponNameList(weapon, rand);
                    break;
                case Tome:
                    name2 = "tome";
                    name1 = WeaponNameList(weapon, rand);
                    break;
            }
            return $"{name1}{name2}";
        }
        public static string WeaponNameList(WeaponBase weapon, int rand) {
            string name = "";
            switch (weapon) {
                default:
                case Axe:
                    switch (rand) {
                        case 0:
                            name = "Hand ";
                            break;
                        case 1:
                            name = "Broad ";
                            break;
                        case 2:
                            name = "Splitting ";
                            break;
                        case 3:
                            name = "Felling ";
                            break;
                        case 4:
                            name = "Battle ";
                            break;
                        case 5:
                            name = "Dane ";
                            break;
                        case 6:
                            name = "Double ";
                            break;
                        case 7:
                            name = "Cleaving ";
                            break;
                        case 8:
                            name = "Dagger-";
                            break;
                        case 9:
                            name = "Balanced ";
                            break;
                    }
                    return name;
                case Sword:
                    switch (rand) {
                        case 0:
                            name = "Short ";
                            break;
                        case 1:
                            name = "Long";
                            break;
                        case 2:
                            name = "Bastard ";
                            break;
                        case 3:
                            name = "Arming ";
                            break;
                        case 4:
                            name = "Great";
                            break;
                        case 5:
                            name = "Two-Handed ";
                            break;
                        case 6:
                            name = "Sabre ";
                            break;
                        case 7:
                            name = "Falchion ";
                            break;
                        case 8:
                            name = "Broad";
                            break;
                        case 9:
                            name = "Dueling ";
                            break;
                    }
                    return name;
                case Hammer:
                    switch (rand) {
                        case 0:
                            name = "War";
                            break;
                        case 1:
                            name = "Crushing ";
                            break;
                        case 2:
                            name = "Maul ";
                            break;
                        case 3:
                            name = "Piercing ";
                            break;
                        case 4:
                            name = "Blunt ";
                            break;
                        case 5:
                            name = "Club ";
                            break;
                        case 6:
                            name = "Mace ";
                            break;
                        case 7:
                            name = "Light ";
                            break;
                        case 8:
                            name = "Heavy ";
                            break;
                        case 9:
                            name = "Balanced ";
                            break;
                    }
                    return name;
                case Bow:
                    switch (rand) {
                        case 0:
                            name = "Short";
                            break;
                        case 1:
                            name = "Long";
                            break;
                        case 2:
                            name = "Recurve ";
                            break;
                        case 3:
                            name = "Straight ";
                            break;
                        case 4:
                            name = "Reflex ";
                            break;
                        case 5:
                            name = "Composite ";
                            break;
                        case 6:
                            name = "Hunting ";
                            break;
                        case 7:
                            name = "Flat";
                            break;
                        case 8:
                            name = "Hickory ";
                            break;
                        case 9:
                            name = "Ash ";
                            break;
                    }
                    return name;
                case Dagger:
                    switch (rand) {
                        case 0:
                            name = "Sharp ";
                            break;
                        case 1:
                            name = "Piercing ";
                            break;
                        case 2:
                            name = "Rondel ";
                            break;
                        case 3:
                            name = "Dirk ";
                            break;
                        case 4:
                            name = "Stiletto ";
                            break;
                        case 5:
                            name = "Poignard ";
                            break;
                        case 6:
                            name = "Parrying ";
                            break;
                        case 7:
                            name = "Bollock ";
                            break;
                        case 8:
                            name = "Hunting ";
                            break;
                        case 9:
                            name = "Seax ";
                            break;
                    }
                    return name;
                case Crossbow:
                    switch (rand) {
                        case 0:
                            name = "Recurve ";
                            break;
                        case 1:
                            name = "Light ";
                            break;
                        case 2:
                            name = "Siege ";
                            break;
                        case 3:
                            name = "Compound ";
                            break;
                        case 4:
                            name = "Repeating ";
                            break;
                        case 5:
                            name = "Quickdraw ";
                            break;
                        case 6:
                            name = "Penetrator ";
                            break;
                        case 7:
                            name = "Hunting ";
                            break;
                        case 8:
                            name = "Pulley ";
                            break;
                        case 9:
                            name = "Heavy ";
                            break;
                    }
                    return name;
                case Staff:
                    switch (rand) {
                        case 0:
                            name = "Quarter";
                            break;
                        case 1:
                            name = "Short ";
                            break;
                        case 2:
                            name = "Long ";
                            break;
                        case 3:
                            name = "Composite ";
                            break;
                        case 4:
                            name = "War ";
                            break;
                        case 5:
                            name = "Battle ";
                            break;
                        case 6:
                            name = "Runic ";
                            break;
                        case 7:
                            name = "Ancient ";
                            break;
                        case 8:
                            name = "Lunar ";
                            break;
                        case 9:
                            name = "Sun ";
                            break;
                    }
                    return name;
                case Wand:
                    switch (rand) {
                        case 0:
                            name = "Ashwood ";
                            break;
                        case 1:
                            name = "Elmwood ";
                            break;
                        case 2:
                            name = "Juniperwood ";
                            break;
                        case 3:
                            name = "Oakwood ";
                            break;
                        case 4:
                            name = "Walnut ";
                            break;
                        case 5:
                            name = "Beechwood ";
                            break;
                        case 6:
                            name = "Cedarwood ";
                            break;
                        case 7:
                            name = "Chestnut ";
                            break;
                        case 8:
                            name = "Yew ";
                            break;
                        case 9:
                            name = "Willow ";
                            break;
                    }
                    return name;
                case Tome:
                    switch (rand) {
                        case 0:
                            name = "Big ";
                            break;
                        case 1:
                            name = "Heavy ";
                            break;
                        case 2:
                            name = "Old ";
                            break;
                        case 3:
                            name = "Ancient ";
                            break;
                        case 4:
                            name = "Rune ";
                            break;
                        case 5:
                            name = "Mage's ";
                            break;
                        case 6:
                            name = "Spell";
                            break;
                        case 7:
                            name = "Magic ";
                            break;
                        case 8:
                            name = "Cursed ";
                            break;
                        case 9:
                            name = "Power ";
                            break;
                    }
                    return name;
            }
        }
    }
}
