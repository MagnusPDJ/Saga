using System;
using System.Collections.Generic;
using System.Reflection.Emit;
using Saga.assets;
using Saga.Dungeon;
using Saga.Items;

namespace Saga.Character
{
    [Serializable]
    public abstract class Player
    {
        public string currentClass;
        public string Name { get; set; }
        public int Id { get; set; }
        public int Level { get; set; }
        public int Exp { get; set; }
        public int Gold { get; set; }
        public int Health { get; set; }
        public int Mana { get; set; }
        public int FreeAttributePoints { get; set; }
        public PrimaryAttributes BasePrimaryAttributes { get; set; }
        public PrimaryAttributes TotalPrimaryAttributes { get; set; }
        public SecondaryAttributes BaseSecondaryAttributes { get; set; }
        public Dictionary<Slot, Item> Equipment { get; set; }
        public Item[] Inventory { get; set; }
        public (int, int) DPT { get; set; }

        public Player(string name, int id, int strength, int dexterity, int intellect, int constitution, int willpower) {
            Name = name;
            Id = id;
            Level = 1; 
            Equipment = new Dictionary<Slot, Item>();
            Inventory = new Item[10];
            Exp = 0;
            Gold = 0;
            FreeAttributePoints = 0;
            BasePrimaryAttributes = new PrimaryAttributes() { Strength = strength, Dexterity = dexterity, Intellect = intellect, Constitution = constitution, WillPower = willpower };
            CalculateTotalStats();
            Health = BaseSecondaryAttributes.MaxHealth;
            Mana = BaseSecondaryAttributes.MaxMana; 
        }
        // <param name="levels">Number of levels to level up</param>
        public abstract void LevelUp();

        //Metode til udregning af det exp det koster at level op.
        public int GetLevelUpValue() {
            return Convert.ToInt32(5000000 / (1 + 10000 * Math.Pow(1.2, 1 - Program.CurrentPlayer.Level)));
        }

        //Metode til at tjekke om lvl op er muligt
        public bool CanLevelUp() {
            if (Program.CurrentPlayer.Exp >= GetLevelUpValue()) {
                return true;
            }
            else {
                return false;
            }
        }

        // <returns> An integer with damage per turn</returns>
        public abstract (int, int) CalculateDPT();
        // Equips a weapon.
        public abstract string Equip(Weapon weapon);
        // Equips armor.
        public abstract string Equip(Armor armor);
        //Equips a potion.
        public abstract string Equip(Potion potion);
        //Unequips an item and places it into the inventory.
        public abstract string UnEquip(Slot slot, Item item);
        //Metode til at sætte start udstyr
        public abstract void SetStartingGear();

        //Metode til at vælge mellem character actions/skills/items
        public abstract void PlayerActions(Enemy Monster, Encounters TurnTimer);

        public abstract void Heal();

        // Calculates and outputs hero stats.
        public void DisplayStats() {
            CalculateTotalStats();
            HUDTools.WriteStatsToConsole(Name, Level, TotalPrimaryAttributes, BaseSecondaryAttributes, DPT);
        }

        // Calculates total stats based on base stats and equipped items.
        public void CalculateTotalStats() {
            TotalPrimaryAttributes = CalculateArmorBonus();
            BaseSecondaryAttributes = CalculateSecondaryStats();
            DPT = CalculateDPT();
        }

        // Calculates armor bonus
        public PrimaryAttributes CalculateArmorBonus() {
            PrimaryAttributes armorBonusValues = new PrimaryAttributes() { Strength = 0, Dexterity = 0, Intellect = 0, Constitution = 0, WillPower = 0 };

            bool hasHeadArmor = Equipment.TryGetValue(Slot.Headgear, out Item headArmor);
            bool hasBodyArmor = Equipment.TryGetValue(Slot.Torso, out Item bodyArmor);
            bool hasLegsArmor = Equipment.TryGetValue(Slot.Legs, out Item legsArmor);
            bool hasFeetArmor = Equipment.TryGetValue(Slot.Feet, out Item FeetArmor);
            bool hasArmsArmor = Equipment.TryGetValue(Slot.Bracers, out Item ArmsArmor);
            bool hasShouldersArmor = Equipment.TryGetValue(Slot.Shoulders, out Item ShouldersArmor);
            bool hasBeltArmor = Equipment.TryGetValue(Slot.Belt, out Item BeltArmor);
            bool hasCapeArmor = Equipment.TryGetValue(Slot.Cape, out Item CapeArmor);
            bool hasGlovesArmor = Equipment.TryGetValue(Slot.Gloves, out Item GlovesArmor);
            bool hasAmuletArmor = Equipment.TryGetValue(Slot.Amulet, out Item AmuletArmor);
            bool hasRing1Armor = Equipment.TryGetValue(Slot.SLOT_RING1, out Item Ring1Armor);
            bool hasRing2Armor = Equipment.TryGetValue(Slot.SLOT_RING2, out Item Ring2Armor);
            bool hasCrestArmor = Equipment.TryGetValue(Slot.SLOT_CREST, out Item CrestArmor);
            bool hasTrinketArmor = Equipment.TryGetValue(Slot.SLOT_TRINKET, out Item TrinketArmor);
            bool hasOffhandArmor = Equipment.TryGetValue(Slot.SLOT_OFFHAND, out Item OffhandArmor);

            if (hasHeadArmor) {
                Armor a = (Armor)headArmor;
                armorBonusValues += new PrimaryAttributes() { Strength = a.Attributes.Strength, Dexterity = a.Attributes.Dexterity, Intellect = a.Attributes.Intellect, Constitution = a.Attributes.Constitution, WillPower = a.Attributes.WillPower };
            }
            if (hasBodyArmor) {
                Armor a = (Armor)bodyArmor;
                armorBonusValues += new PrimaryAttributes() { Strength = a.Attributes.Strength, Dexterity = a.Attributes.Dexterity, Intellect = a.Attributes.Intellect, Constitution = a.Attributes.Constitution, WillPower = a.Attributes.WillPower };
            }
            if (hasLegsArmor) {
                Armor a = (Armor)legsArmor;
                armorBonusValues += new PrimaryAttributes() { Strength = a.Attributes.Strength, Dexterity = a.Attributes.Dexterity, Intellect = a.Attributes.Intellect, Constitution = a.Attributes.Constitution, WillPower = a.Attributes.WillPower };
            }
            if (hasFeetArmor) {
                Armor a = (Armor)FeetArmor;
                armorBonusValues += new PrimaryAttributes() { Strength = a.Attributes.Strength, Dexterity = a.Attributes.Dexterity, Intellect = a.Attributes.Intellect, Constitution = a.Attributes.Constitution, WillPower = a.Attributes.WillPower };
            }
            if (hasArmsArmor) {
                Armor a = (Armor)ArmsArmor;
                armorBonusValues += new PrimaryAttributes() { Strength = a.Attributes.Strength, Dexterity = a.Attributes.Dexterity, Intellect = a.Attributes.Intellect, Constitution = a.Attributes.Constitution, WillPower = a.Attributes.WillPower };
            }
            if (hasShouldersArmor) {
                Armor a = (Armor)ShouldersArmor;
                armorBonusValues += new PrimaryAttributes() { Strength = a.Attributes.Strength, Dexterity = a.Attributes.Dexterity, Intellect = a.Attributes.Intellect, Constitution = a.Attributes.Constitution, WillPower = a.Attributes.WillPower };
            }
            if (hasBeltArmor) {
                Armor a = (Armor)BeltArmor;
                armorBonusValues += new PrimaryAttributes() { Strength = a.Attributes.Strength, Dexterity = a.Attributes.Dexterity, Intellect = a.Attributes.Intellect, Constitution = a.Attributes.Constitution, WillPower = a.Attributes.WillPower };
            }
            if (hasCapeArmor) {
                Armor a = (Armor)CapeArmor;
                armorBonusValues += new PrimaryAttributes() { Strength = a.Attributes.Strength, Dexterity = a.Attributes.Dexterity, Intellect = a.Attributes.Intellect, Constitution = a.Attributes.Constitution, WillPower = a.Attributes.WillPower };
            }
            if (hasGlovesArmor) {
                Armor a = (Armor)GlovesArmor;
                armorBonusValues += new PrimaryAttributes() { Strength = a.Attributes.Strength, Dexterity = a.Attributes.Dexterity, Intellect = a.Attributes.Intellect, Constitution = a.Attributes.Constitution, WillPower = a.Attributes.WillPower };
            }
            if (hasAmuletArmor) {
                Armor a = (Armor)AmuletArmor;
                armorBonusValues += new PrimaryAttributes() { Strength = a.Attributes.Strength, Dexterity = a.Attributes.Dexterity, Intellect = a.Attributes.Intellect, Constitution = a.Attributes.Constitution, WillPower = a.Attributes.WillPower };
            }
            if (hasRing1Armor) {
                Armor a = (Armor)Ring1Armor;
                armorBonusValues += new PrimaryAttributes() { Strength = a.Attributes.Strength, Dexterity = a.Attributes.Dexterity, Intellect = a.Attributes.Intellect, Constitution = a.Attributes.Constitution, WillPower = a.Attributes.WillPower };
            }
            if (hasRing2Armor) {
                Armor a = (Armor)Ring2Armor;
                armorBonusValues += new PrimaryAttributes() { Strength = a.Attributes.Strength, Dexterity = a.Attributes.Dexterity, Intellect = a.Attributes.Intellect, Constitution = a.Attributes.Constitution, WillPower = a.Attributes.WillPower };
            }
            if (hasCrestArmor) {
                Armor a = (Armor)CrestArmor;
                armorBonusValues += new PrimaryAttributes() { Strength = a.Attributes.Strength, Dexterity = a.Attributes.Dexterity, Intellect = a.Attributes.Intellect, Constitution = a.Attributes.Constitution, WillPower = a.Attributes.WillPower };
            }
            if (hasTrinketArmor) {
                Armor a = (Armor)TrinketArmor;
                armorBonusValues += new PrimaryAttributes() { Strength = a.Attributes.Strength, Dexterity = a.Attributes.Dexterity, Intellect = a.Attributes.Intellect, Constitution = a.Attributes.Constitution, WillPower = a.Attributes.WillPower };
            }
            if (hasOffhandArmor) {
                Armor a = (Armor)OffhandArmor;
                armorBonusValues += new PrimaryAttributes() { Strength = a.Attributes.Strength, Dexterity = a.Attributes.Dexterity, Intellect = a.Attributes.Intellect, Constitution = a.Attributes.Constitution, WillPower = a.Attributes.WillPower };
            }
            return BasePrimaryAttributes + armorBonusValues;
        }

        //Calculates secondaryAttributes
        public SecondaryAttributes CalculateSecondaryStats() {
            return new SecondaryAttributes()
            {
                MaxHealth = 5 + TotalPrimaryAttributes.Constitution * 5,
                MaxMana = 5 + TotalPrimaryAttributes.WillPower * 5,
                Awareness = TotalPrimaryAttributes.Dexterity,
                ArmorRating = TotalPrimaryAttributes.Strength + TotalPrimaryAttributes.Dexterity,
                ElementalResistence = TotalPrimaryAttributes.Intellect
            };
        }

        // Calculates a weapons damage per turn.
        public (int, int) CalculateWeaponDPT() {
            bool hasWeapon = Equipment.TryGetValue(Slot.Weapon, out Item equippedWeapon);
            if (hasWeapon) {
                Weapon w = (Weapon)equippedWeapon;
                return (w.WeaponAttributes.MinDamage, w.WeaponAttributes.MaxDamage);
            }
            else {
                return (1, 1);
            }
        }

        //Metode til at checke for om spilleren dør som kan kaldes hver gang spilleren tager skade.
        public static void DeathCode(string message) {
            AudioManager.soundGameOver.Play();
            Console.ForegroundColor = ConsoleColor.DarkRed;
            HUDTools.Print(message, 20);
            HUDTools.Print("Press to go back to main menu...", 5);
            Console.ResetColor();
            HUDTools.PlayerPrompt();
            Program.MainMenu();
        }

    }
}
