using Saga.Assets;
using Saga.Dungeon;
using Saga.Items;
using Saga.Items.Loot;
using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Saga.Character
{
    public enum Act {
        Start,
        Act1,
        Act2,
        Act3,
        Act4,
        Act5
    }

    [JsonDerivedType(typeof(Warrior), typeDiscriminator: "warrior")]
    [JsonDerivedType(typeof(Archer), typeDiscriminator: "archer")]
    [JsonDerivedType(typeof(Mage), typeDiscriminator: "mage")]
    public abstract class Player {
        public string CurrentClass { get; set; }
        public Act CurrentAct { get; set; }
        public Loot Loot { get; set; }
        public string Name { get; set; }
        public int Id { get; set; }
        public int Level { get; set; }
        public int Exp { get; set; }
        public int Gold { get; set; }
        public int Health { get; set; }
        public int Mana { get; set; }
        public int FreeAttributePoints { get; set; }
        public Potion CurrentHealingPotion { get; set; }
        public PrimaryAttributes BasePrimaryAttributes { get; set; }
        public PrimaryAttributes TotalPrimaryAttributes { get; set; }
        public SecondaryAttributes BaseSecondaryAttributes { get; set; }
        public SecondaryAttributes TotalSecondaryAttributes { get; set; }
        public Dictionary<Slot, Item> Equipment { get; set; }
        public Item[] Inventory { get; set; }
        public List<Quest> QuestLog { get; set; }
        public List<Quest> FailedQuests { get; set; }
        public List<Quest> CompletedQuests { get; set; }
        public List<NonPlayableCharacters> NpcsInCamp { get; set; }
        public (int, int) DPT { get; set; }
        public int TimesExplored { get; set; }
        public Player(string name, int id, int strength, int dexterity, int intellect, int constitution, int willpower) {
            Name = name;
            Id = id;
            Level = 1;
            CurrentAct = Act.Start;
            Loot = new Act1Loot();
            Equipment = [];
            Inventory = new Item[10];
            QuestLog = [];
            FailedQuests = [];
            CompletedQuests = [];
            NpcsInCamp = [];
            Exp = 0;
            Gold = 0;
            FreeAttributePoints = 0;
            BasePrimaryAttributes = new PrimaryAttributes() { Strength = strength, Dexterity = dexterity, Intellect = intellect, Constitution = constitution, WillPower = willpower };
            CalculateTotalStats();
            Health = TotalSecondaryAttributes.MaxHealth;
            Mana = TotalSecondaryAttributes.MaxMana;
            CurrentHealingPotion = new Potion();
            TimesExplored = 0;
        }
        
        public abstract void LevelUp();
        //Metode til udregning af det exp det koster at level op.
        public int GetLevelUpValue() {
            return Convert.ToInt32(5000000 / (1 + 10000 * Math.Pow(1.2, 1 - Level)));
        }
        //Metode til at tjekke om lvl op er muligt.
        public bool CanLevelUp() {
            if (Exp >= GetLevelUpValue()) {
                return true;
            } else {
                return false;
            }
        }
        //Metode til at level up hver gang man får exp.
        public void CheckForLevelUp() {
            if (CanLevelUp()) {
                LevelUp();
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
        //Metode til at sætte start udstyr.
        public abstract void SetStartingGear();
        //Metode til at drikke potions.
        public abstract void Heal();
        public abstract int Attack(Enemy monster);
        public abstract bool RunAway(Enemy monster);
        public abstract void Defend(Enemy monster);

        // Calculates and outputs hero stats.
        public void DisplayStats() {
            CalculateTotalStats();
            HUDTools.WriteStatsToConsole(Name, Level, TimesExplored, TotalPrimaryAttributes, TotalSecondaryAttributes, DPT);
        }
        // Calculates total stats based on base stats and equipped items.
        public void CalculateTotalStats() {
            TotalPrimaryAttributes = CalculatePrimaryArmorBonus();
            BaseSecondaryAttributes = CalculateBaseSecondaryStats();
            TotalSecondaryAttributes = CalculateSecondaryArmorBonus();
            DPT = CalculateDPT();
        }
        // Calculates armor bonus.
        public PrimaryAttributes CalculatePrimaryArmorBonus() {
            PrimaryAttributes armorBonusValues = new() { Strength = 0, Dexterity = 0, Intellect = 0, Constitution = 0, WillPower = 0 };

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
        public SecondaryAttributes CalculateSecondaryArmorBonus() {
            SecondaryAttributes armorBonusValues = new() { ArmorRating = 0, MaxHealth = 0, MaxMana = 0, Awareness = 0, ElementalResistence = 0 };

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
                armorBonusValues += new SecondaryAttributes() { ArmorRating = a.SecondaryAttributes.ArmorRating, MaxHealth = a.SecondaryAttributes.MaxHealth, MaxMana = a.SecondaryAttributes.MaxMana, Awareness = a.SecondaryAttributes.Awareness, ElementalResistence = a.SecondaryAttributes.ElementalResistence };
            }
            if (hasBodyArmor) {
                Armor a = (Armor)bodyArmor;
                armorBonusValues += new SecondaryAttributes() { ArmorRating = a.SecondaryAttributes.ArmorRating, MaxHealth = a.SecondaryAttributes.MaxHealth, MaxMana = a.SecondaryAttributes.MaxMana, Awareness = a.SecondaryAttributes.Awareness, ElementalResistence = a.SecondaryAttributes.ElementalResistence };
            }
            if (hasLegsArmor) {
                Armor a = (Armor)legsArmor;
                armorBonusValues += new SecondaryAttributes() { ArmorRating = a.SecondaryAttributes.ArmorRating, MaxHealth = a.SecondaryAttributes.MaxHealth, MaxMana = a.SecondaryAttributes.MaxMana, Awareness = a.SecondaryAttributes.Awareness, ElementalResistence = a.SecondaryAttributes.ElementalResistence };
            }
            if (hasFeetArmor) {
                Armor a = (Armor)FeetArmor;
                armorBonusValues += new SecondaryAttributes() { ArmorRating = a.SecondaryAttributes.ArmorRating, MaxHealth = a.SecondaryAttributes.MaxHealth, MaxMana = a.SecondaryAttributes.MaxMana, Awareness = a.SecondaryAttributes.Awareness, ElementalResistence = a.SecondaryAttributes.ElementalResistence };
            }
            if (hasArmsArmor) {
                Armor a = (Armor)ArmsArmor;
                armorBonusValues += new SecondaryAttributes() { ArmorRating = a.SecondaryAttributes.ArmorRating, MaxHealth = a.SecondaryAttributes.MaxHealth, MaxMana = a.SecondaryAttributes.MaxMana, Awareness = a.SecondaryAttributes.Awareness, ElementalResistence = a.SecondaryAttributes.ElementalResistence };
            }
            if (hasShouldersArmor) {
                Armor a = (Armor)ShouldersArmor;
                armorBonusValues += new SecondaryAttributes() { ArmorRating = a.SecondaryAttributes.ArmorRating, MaxHealth = a.SecondaryAttributes.MaxHealth, MaxMana = a.SecondaryAttributes.MaxMana, Awareness = a.SecondaryAttributes.Awareness, ElementalResistence = a.SecondaryAttributes.ElementalResistence };
            }
            if (hasBeltArmor) {
                Armor a = (Armor)BeltArmor;
                armorBonusValues += new SecondaryAttributes() { ArmorRating = a.SecondaryAttributes.ArmorRating, MaxHealth = a.SecondaryAttributes.MaxHealth, MaxMana = a.SecondaryAttributes.MaxMana, Awareness = a.SecondaryAttributes.Awareness, ElementalResistence = a.SecondaryAttributes.ElementalResistence };
            }
            if (hasCapeArmor) {
                Armor a = (Armor)CapeArmor;
                armorBonusValues += new SecondaryAttributes() { ArmorRating = a.SecondaryAttributes.ArmorRating, MaxHealth = a.SecondaryAttributes.MaxHealth, MaxMana = a.SecondaryAttributes.MaxMana, Awareness = a.SecondaryAttributes.Awareness, ElementalResistence = a.SecondaryAttributes.ElementalResistence };
            }
            if (hasGlovesArmor) {
                Armor a = (Armor)GlovesArmor;
                armorBonusValues += new SecondaryAttributes() { ArmorRating = a.SecondaryAttributes.ArmorRating, MaxHealth = a.SecondaryAttributes.MaxHealth, MaxMana = a.SecondaryAttributes.MaxMana, Awareness = a.SecondaryAttributes.Awareness, ElementalResistence = a.SecondaryAttributes.ElementalResistence };
            }
            if (hasAmuletArmor) {
                Armor a = (Armor)AmuletArmor;
                armorBonusValues += new SecondaryAttributes() { ArmorRating = a.SecondaryAttributes.ArmorRating, MaxHealth = a.SecondaryAttributes.MaxHealth, MaxMana = a.SecondaryAttributes.MaxMana, Awareness = a.SecondaryAttributes.Awareness, ElementalResistence = a.SecondaryAttributes.ElementalResistence };
            }
            if (hasRing1Armor) {
                Armor a = (Armor)Ring1Armor;
                armorBonusValues += new SecondaryAttributes() { ArmorRating = a.SecondaryAttributes.ArmorRating, MaxHealth = a.SecondaryAttributes.MaxHealth, MaxMana = a.SecondaryAttributes.MaxMana, Awareness = a.SecondaryAttributes.Awareness, ElementalResistence = a.SecondaryAttributes.ElementalResistence };
            }
            if (hasRing2Armor) {
                Armor a = (Armor)Ring2Armor;
                armorBonusValues += new SecondaryAttributes() { ArmorRating = a.SecondaryAttributes.ArmorRating, MaxHealth = a.SecondaryAttributes.MaxHealth, MaxMana = a.SecondaryAttributes.MaxMana, Awareness = a.SecondaryAttributes.Awareness, ElementalResistence = a.SecondaryAttributes.ElementalResistence };
            }
            if (hasCrestArmor) {
                Armor a = (Armor)CrestArmor;
                armorBonusValues += new SecondaryAttributes() { ArmorRating = a.SecondaryAttributes.ArmorRating, MaxHealth = a.SecondaryAttributes.MaxHealth, MaxMana = a.SecondaryAttributes.MaxMana, Awareness = a.SecondaryAttributes.Awareness, ElementalResistence = a.SecondaryAttributes.ElementalResistence };
            }
            if (hasTrinketArmor) {
                Armor a = (Armor)TrinketArmor;
                armorBonusValues += new SecondaryAttributes() { ArmorRating = a.SecondaryAttributes.ArmorRating, MaxHealth = a.SecondaryAttributes.MaxHealth, MaxMana = a.SecondaryAttributes.MaxMana, Awareness = a.SecondaryAttributes.Awareness, ElementalResistence = a.SecondaryAttributes.ElementalResistence };
            }
            if (hasOffhandArmor) {
                Armor a = (Armor)OffhandArmor;
                armorBonusValues += new SecondaryAttributes() { ArmorRating = a.SecondaryAttributes.ArmorRating, MaxHealth = a.SecondaryAttributes.MaxHealth, MaxMana = a.SecondaryAttributes.MaxMana, Awareness = a.SecondaryAttributes.Awareness, ElementalResistence = a.SecondaryAttributes.ElementalResistence };
            }
            return BaseSecondaryAttributes + armorBonusValues;
        }
        //Calculates secondaryAttributes.
        public SecondaryAttributes CalculateBaseSecondaryStats() {
            return new SecondaryAttributes() {
                MaxHealth = 5 + TotalPrimaryAttributes.Constitution * 5,
                MaxMana = 5 + TotalPrimaryAttributes.WillPower * 5,
                Awareness = TotalPrimaryAttributes.Dexterity,
                ArmorRating = (TotalPrimaryAttributes.Strength + TotalPrimaryAttributes.Dexterity) / 2,
                ElementalResistence = TotalPrimaryAttributes.Intellect
            };
        }
        // Calculates a weapons damage per turn.
        public (int, int) CalculateWeaponDPT() {
            bool hasWeapon = Equipment.TryGetValue(Slot.Weapon, out Item equippedWeapon);
            if (hasWeapon) {
                Weapon w = (Weapon)equippedWeapon;
                return (w.WeaponAttributes.MinDamage, w.WeaponAttributes.MaxDamage);
            } else {
                return (1, 1);
            }
        }
        //Metode til at checke for om spilleren dør som kan kaldes hver gang spilleren tager skade.
        public void CheckForDeath(string message) {
            if (this.Health <= 0) {
                Program.SoundController.Stop();
                HUDTools.ClearLog();
                Program.SoundController.Play("gameover");
                HUDTools.Print($"\u001b[31m{message}\u001b[0m", 20);
                TextInput.PressToContinue();
                Program.CurrentPlayer = null;
                Program.MainMenu();
            }
        }
        //Metode til at kalde basic actions (heal, inventory og character).
        public void BasicActions(string input) {
            if (input == "h" || input == "heal") {
                //Heal
                Heal();
                TextInput.PressToContinue();
            } else if (input == "c" || input == "character" || input == "character screen") {
                HUDTools.CharacterScreen();
                TextInput.PressToContinue();
            } else if (input == "i" || input == "inventory") {
                HUDTools.InventoryScreen();
            } else if (input == "l" || input == "questlog") {
                HUDTools.QuestLogHUD();
                TextInput.PressToContinue();
            } else {
                HUDTools.Print($"There is no {input} action...", 15);
                TextInput.PressToContinue();
                HUDTools.ClearLastLine(3);
                return;
            }
            if (Program.RoomController.currentRoom == Rooms.Camp) {
                HUDTools.FullCampHUD();
            }
        }
        //Metode til at opdatere questloggen hver gang ny quest eller item bliver added til spilleren.
        public void UpdateQuestLog() {
            foreach (Quest quest in QuestLog) {
                quest.Completed = quest.CheckRequirements();
            }
        }
        //Metode til at få alle quest rewards og opdatere questlogs.
        public void CompleteAndTurnInQuest(Quest quest)  {
            int a = Array.FindIndex(Inventory, item => item != null && item.ItemName == "Old key");
            if (a != -1) {
                Inventory.SetValue(null, a);
            }
            int b = Array.FindIndex(Inventory, item => item != null && item.ItemName == "Rat tail");
            if (b != -1) {
                Inventory.SetValue(null, b);
            }
            int c = Array.FindIndex(Inventory, item => item != null && item.ItemName == "Bat wings");
            if (c != -1) {
                Inventory.SetValue(null, c);
            }
            QuestLog.Remove(quest);
            CompletedQuests.Add(quest);
            Program.SoundController.Play("win");
            HUDTools.Print($"\u001b[96mYou've completed the quest: {quest.Name}!\u001b[0m", 15);
            Program.CurrentPlayer.Loot.GetFixedGold(quest.Gold);
            Program.CurrentPlayer.Loot.GetPotions(quest.Potions);
            Program.CurrentPlayer.Loot.GetExp(0, quest.Exp);
            if (quest.Item != null) {
                int index = Array.FindIndex(Inventory, i => i == null || Inventory.Length == 0);
                Inventory.SetValue(quest.Item, index);
                HUDTools.Print($"\u001b[35mYou've gained {quest.Item.ItemName}\u001b[0m");
            }
        }
        //Metode til at vælge mellem klasse skills i kamp.
        public void CombatActions(Enemy Monster, Encounters TurnTimer) {
            Console.WriteLine("Choose an action...");
            string input = TextInput.PlayerPrompt();
            if (input == "a" || input == "attack") {
                //Attack
                int damage = Attack(Monster);
                Monster.Health -= damage;
                HUDTools.WriteCombatLog("attack", TurnTimer, 0, damage, Monster);
                TurnTimer.TurnTimer++;
            } else if (input == "d" || input == "defend") {
                //Defend 
                Defend(Monster);
                HUDTools.WriteCombatLog("defend", TurnTimer, 0, 0, Monster);
                TurnTimer.TurnTimer++;
            } else if (input == "r" || input == "run") {
                //Run                   
                if (RunAway(Monster)) {
                    Program.SoundController.Stop();
                    HUDTools.ClearLog();
                    TurnTimer.Ran = true;
                } else {
                    HUDTools.WriteCombatLog(action: "run", TurnTimer: TurnTimer, Monster: Monster);
                    TurnTimer.TurnTimer++;
                }
            } else if (input == "h" || input == "heal") {
                //Heal
                Heal();
                HUDTools.WriteCombatLog(action: "heal", TurnTimer: TurnTimer, Monster: Monster);
                TurnTimer.TurnTimer++;
            } else if (input == "c" || input == "character" || input == "character screen") {
                HUDTools.CharacterScreen();
            } else if (input == "l" || input == "log" || input == "combat log") {
                Console.Clear();
                HUDTools.GetLog();
            } else if (input == "q" || input == "questlog") {
                HUDTools.QuestLogHUD();
            }
            TextInput.PressToContinue();
        }
    }
}
