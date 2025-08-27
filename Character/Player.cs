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
        public PrimaryAttributes BasePrimaryAttributes { get; set; }
        public PrimaryAttributes TotalPrimaryAttributes { get; set; }
        public SecondaryAttributes BaseSecondaryAttributes { get; set; }
        public SecondaryAttributes TotalSecondaryAttributes { get; set; }
        public Dictionary<Slot, ItemBase> Equipment { get; set; }
        public ItemBase[] Inventory { get; set; }
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
            Inventory = new ItemBase[10];
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
        //Metode til at sætte start udstyr.
        public abstract void SetStartingGear();
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

            bool hasHeadArmor = Equipment.TryGetValue(Slot.Headgear, out ItemBase headArmor);
            bool hasBodyArmor = Equipment.TryGetValue(Slot.Torso, out ItemBase bodyArmor);
            bool hasLegsArmor = Equipment.TryGetValue(Slot.Legs, out ItemBase legsArmor);
            bool hasFeetArmor = Equipment.TryGetValue(Slot.Feet, out ItemBase FeetArmor);
            bool hasArmsArmor = Equipment.TryGetValue(Slot.Bracers, out ItemBase ArmsArmor);
            bool hasShouldersArmor = Equipment.TryGetValue(Slot.Shoulders, out ItemBase ShouldersArmor);
            bool hasBeltArmor = Equipment.TryGetValue(Slot.Belt, out ItemBase BeltArmor);
            bool hasCapeArmor = Equipment.TryGetValue(Slot.Cape, out ItemBase CapeArmor);
            bool hasGlovesArmor = Equipment.TryGetValue(Slot.Gloves, out ItemBase GlovesArmor);
            bool hasAmuletArmor = Equipment.TryGetValue(Slot.Amulet, out ItemBase AmuletArmor);
            bool hasRing1Armor = Equipment.TryGetValue(Slot.Finger_1, out ItemBase Ring1Armor);
            bool hasRing2Armor = Equipment.TryGetValue(Slot.Finger_2, out ItemBase Ring2Armor);
            bool hasCrestArmor = Equipment.TryGetValue(Slot.Crest, out ItemBase CrestArmor);
            bool hasTrinketArmor = Equipment.TryGetValue(Slot.Trinket, out ItemBase TrinketArmor);

            if (hasHeadArmor) {
                ArmorBase a = (ArmorBase)headArmor;
                armorBonusValues += new PrimaryAttributes() { Strength = a.PrimaryAttributes.Strength, Dexterity = a.PrimaryAttributes.Dexterity, Intellect = a.PrimaryAttributes.Intellect, Constitution = a.PrimaryAttributes.Constitution, WillPower = a.PrimaryAttributes.WillPower };
            }
            if (hasBodyArmor) {
                ArmorBase a = (ArmorBase)bodyArmor;
                armorBonusValues += new PrimaryAttributes() { Strength = a.PrimaryAttributes.Strength, Dexterity = a.PrimaryAttributes.Dexterity, Intellect = a.PrimaryAttributes.Intellect, Constitution = a.PrimaryAttributes.Constitution, WillPower = a.PrimaryAttributes.WillPower };
            }
            if (hasLegsArmor) {
                ArmorBase a = (ArmorBase)legsArmor;
                armorBonusValues += new PrimaryAttributes() {Strength = a.PrimaryAttributes.Strength, Dexterity = a.PrimaryAttributes.Dexterity, Intellect = a.PrimaryAttributes.Intellect, Constitution = a.PrimaryAttributes.Constitution, WillPower = a.PrimaryAttributes.WillPower };
            }
            if (hasFeetArmor) {
                ArmorBase a = (ArmorBase)FeetArmor;
                armorBonusValues += new PrimaryAttributes() {Strength = a.PrimaryAttributes.Strength, Dexterity = a.PrimaryAttributes.Dexterity, Intellect = a.PrimaryAttributes.Intellect, Constitution = a.PrimaryAttributes.Constitution, WillPower = a.PrimaryAttributes.WillPower };
            }
            if (hasArmsArmor) {
                ArmorBase a = (ArmorBase)ArmsArmor;
                armorBonusValues += new PrimaryAttributes() {Strength = a.PrimaryAttributes.Strength, Dexterity = a.PrimaryAttributes.Dexterity, Intellect = a.PrimaryAttributes.Intellect, Constitution = a.PrimaryAttributes.Constitution, WillPower = a.PrimaryAttributes.WillPower };
            }
            if (hasShouldersArmor) {
                ArmorBase a = (ArmorBase)ShouldersArmor;
                armorBonusValues += new PrimaryAttributes() {Strength = a.PrimaryAttributes.Strength, Dexterity = a.PrimaryAttributes.Dexterity, Intellect = a.PrimaryAttributes.Intellect, Constitution = a.PrimaryAttributes.Constitution, WillPower = a.PrimaryAttributes.WillPower };
            }
            if (hasBeltArmor) {
                ArmorBase a = (ArmorBase)BeltArmor;
                armorBonusValues += new PrimaryAttributes() {Strength = a.PrimaryAttributes.Strength, Dexterity = a.PrimaryAttributes.Dexterity, Intellect = a.PrimaryAttributes.Intellect, Constitution = a.PrimaryAttributes.Constitution, WillPower = a.PrimaryAttributes.WillPower };
            }
            if (hasCapeArmor) {
                ArmorBase a = (ArmorBase)CapeArmor;
                armorBonusValues += new PrimaryAttributes() {Strength = a.PrimaryAttributes.Strength, Dexterity = a.PrimaryAttributes.Dexterity, Intellect = a.PrimaryAttributes.Intellect, Constitution = a.PrimaryAttributes.Constitution, WillPower = a.PrimaryAttributes.WillPower };
            }
            if (hasGlovesArmor) {
                ArmorBase a = (ArmorBase)GlovesArmor;
                armorBonusValues += new PrimaryAttributes() {Strength = a.PrimaryAttributes.Strength, Dexterity = a.PrimaryAttributes.Dexterity, Intellect = a.PrimaryAttributes.Intellect, Constitution = a.PrimaryAttributes.Constitution, WillPower = a.PrimaryAttributes.WillPower };
            }
            if (hasAmuletArmor) {
                ArmorBase a = (ArmorBase)AmuletArmor;
                armorBonusValues += new PrimaryAttributes() {Strength = a.PrimaryAttributes.Strength, Dexterity = a.PrimaryAttributes.Dexterity, Intellect = a.PrimaryAttributes.Intellect, Constitution = a.PrimaryAttributes.Constitution, WillPower = a.PrimaryAttributes.WillPower };
            }
            if (hasRing1Armor) {
                ArmorBase a = (ArmorBase)Ring1Armor;
                armorBonusValues += new PrimaryAttributes() {Strength = a.PrimaryAttributes.Strength, Dexterity = a.PrimaryAttributes.Dexterity, Intellect = a.PrimaryAttributes.Intellect, Constitution = a.PrimaryAttributes.Constitution, WillPower = a.PrimaryAttributes.WillPower };
            }
            if (hasRing2Armor) {
                ArmorBase a = (ArmorBase)Ring2Armor;
                armorBonusValues += new PrimaryAttributes() {Strength = a.PrimaryAttributes.Strength, Dexterity = a.PrimaryAttributes.Dexterity, Intellect = a.PrimaryAttributes.Intellect, Constitution = a.PrimaryAttributes.Constitution, WillPower = a.PrimaryAttributes.WillPower };
            }
            if (hasCrestArmor) {
                ArmorBase a = (ArmorBase)CrestArmor;
                armorBonusValues += new PrimaryAttributes() {Strength = a.PrimaryAttributes.Strength, Dexterity = a.PrimaryAttributes.Dexterity, Intellect = a.PrimaryAttributes.Intellect, Constitution = a.PrimaryAttributes.Constitution, WillPower = a.PrimaryAttributes.WillPower };
            }
            if (hasTrinketArmor) {
                ArmorBase a = (ArmorBase)TrinketArmor;
                armorBonusValues += new PrimaryAttributes() {Strength = a.PrimaryAttributes.Strength, Dexterity = a.PrimaryAttributes.Dexterity, Intellect = a.PrimaryAttributes.Intellect, Constitution = a.PrimaryAttributes.Constitution, WillPower = a.PrimaryAttributes.WillPower };
            }
            return BasePrimaryAttributes + armorBonusValues;
        }
        public SecondaryAttributes CalculateSecondaryArmorBonus() {
            SecondaryAttributes armorBonusValues = new() { ArmorRating = 0, MaxHealth = 0, MaxMana = 0, Awareness = 0, ElementalResistance = 0 };

            bool hasHeadArmor = Equipment.TryGetValue(Slot.Headgear, out ItemBase headArmor);
            bool hasBodyArmor = Equipment.TryGetValue(Slot.Torso, out ItemBase bodyArmor);
            bool hasLegsArmor = Equipment.TryGetValue(Slot.Legs, out ItemBase legsArmor);
            bool hasFeetArmor = Equipment.TryGetValue(Slot.Feet, out ItemBase FeetArmor);
            bool hasArmsArmor = Equipment.TryGetValue(Slot.Bracers, out ItemBase ArmsArmor);
            bool hasShouldersArmor = Equipment.TryGetValue(Slot.Shoulders, out ItemBase ShouldersArmor);
            bool hasBeltArmor = Equipment.TryGetValue(Slot.Belt, out ItemBase BeltArmor);
            bool hasCapeArmor = Equipment.TryGetValue(Slot.Cape, out ItemBase CapeArmor);
            bool hasGlovesArmor = Equipment.TryGetValue(Slot.Gloves, out ItemBase GlovesArmor);
            bool hasShield = Equipment.TryGetValue(Slot.Left_Hand, out ItemBase ShieldArmor);
            bool hasAmuletArmor = Equipment.TryGetValue(Slot.Amulet, out ItemBase AmuletArmor);
            bool hasRing1Armor = Equipment.TryGetValue(Slot.Finger_1, out ItemBase Ring1Armor);
            bool hasRing2Armor = Equipment.TryGetValue(Slot.Finger_2, out ItemBase Ring2Armor);
            bool hasCrestArmor = Equipment.TryGetValue(Slot.Crest, out ItemBase CrestArmor);
            bool hasTrinketArmor = Equipment.TryGetValue(Slot.Trinket, out ItemBase TrinketArmor);

            if (hasHeadArmor) {
                ArmorBase a = (ArmorBase)headArmor;
                armorBonusValues += new SecondaryAttributes() { ArmorRating = a.SecondaryAttributes.ArmorRating, MaxHealth = a.SecondaryAttributes.MaxHealth, MaxMana = a.SecondaryAttributes.MaxMana, Awareness = a.SecondaryAttributes.Awareness, ElementalResistance = a.SecondaryAttributes.ElementalResistance };
            }
            if (hasBodyArmor) {
                ArmorBase a = (ArmorBase)bodyArmor;
                armorBonusValues += new SecondaryAttributes() { ArmorRating = a.SecondaryAttributes.ArmorRating, MaxHealth = a.SecondaryAttributes.MaxHealth, MaxMana = a.SecondaryAttributes.MaxMana, Awareness = a.SecondaryAttributes.Awareness, ElementalResistance = a.SecondaryAttributes.ElementalResistance };
            }
            if (hasLegsArmor) {
                ArmorBase a = (ArmorBase)legsArmor;
                armorBonusValues += new SecondaryAttributes() { ArmorRating = a.SecondaryAttributes.ArmorRating, MaxHealth = a.SecondaryAttributes.MaxHealth, MaxMana = a.SecondaryAttributes.MaxMana, Awareness = a.SecondaryAttributes.Awareness, ElementalResistance = a.SecondaryAttributes.ElementalResistance };
            }
            if (hasFeetArmor) {
                ArmorBase a = (ArmorBase)FeetArmor;
                armorBonusValues += new SecondaryAttributes() { ArmorRating = a.SecondaryAttributes.ArmorRating, MaxHealth = a.SecondaryAttributes.MaxHealth, MaxMana = a.SecondaryAttributes.MaxMana, Awareness = a.SecondaryAttributes.Awareness, ElementalResistance = a.SecondaryAttributes.ElementalResistance };
            }
            if (hasArmsArmor) {
                ArmorBase a = (ArmorBase)ArmsArmor;
                armorBonusValues += new SecondaryAttributes() { ArmorRating = a.SecondaryAttributes.ArmorRating, MaxHealth = a.SecondaryAttributes.MaxHealth, MaxMana = a.SecondaryAttributes.MaxMana, Awareness = a.SecondaryAttributes.Awareness, ElementalResistance = a.SecondaryAttributes.ElementalResistance };
            }
            if (hasShouldersArmor) {
                ArmorBase a = (ArmorBase)ShouldersArmor;
                armorBonusValues += new SecondaryAttributes() { ArmorRating = a.SecondaryAttributes.ArmorRating, MaxHealth = a.SecondaryAttributes.MaxHealth, MaxMana = a.SecondaryAttributes.MaxMana, Awareness = a.SecondaryAttributes.Awareness, ElementalResistance = a.SecondaryAttributes.ElementalResistance };
            }
            if (hasBeltArmor) {
                ArmorBase a = (ArmorBase)BeltArmor;
                armorBonusValues += new SecondaryAttributes() { ArmorRating = a.SecondaryAttributes.ArmorRating, MaxHealth = a.SecondaryAttributes.MaxHealth, MaxMana = a.SecondaryAttributes.MaxMana, Awareness = a.SecondaryAttributes.Awareness, ElementalResistance = a.SecondaryAttributes.ElementalResistance };
            }
            if (hasCapeArmor) {
                ArmorBase a = (ArmorBase)CapeArmor;
                armorBonusValues += new SecondaryAttributes() { ArmorRating = a.SecondaryAttributes.ArmorRating, MaxHealth = a.SecondaryAttributes.MaxHealth, MaxMana = a.SecondaryAttributes.MaxMana, Awareness = a.SecondaryAttributes.Awareness, ElementalResistance = a.SecondaryAttributes.ElementalResistance };
            }
            if (hasGlovesArmor) {
                ArmorBase a = (ArmorBase)GlovesArmor;
                armorBonusValues += new SecondaryAttributes() { ArmorRating = a.SecondaryAttributes.ArmorRating, MaxHealth = a.SecondaryAttributes.MaxHealth, MaxMana = a.SecondaryAttributes.MaxMana, Awareness = a.SecondaryAttributes.Awareness, ElementalResistance = a.SecondaryAttributes.ElementalResistance };
            }
            if (hasShield && !((ITwoHanded)ShieldArmor).IsTwohanded) {
                ArmorBase a = (ArmorBase)ShieldArmor;
                armorBonusValues += new SecondaryAttributes() { ArmorRating = a.SecondaryAttributes.ArmorRating, MaxHealth = a.SecondaryAttributes.MaxHealth, MaxMana = a.SecondaryAttributes.MaxMana, Awareness = a.SecondaryAttributes.Awareness, ElementalResistance = a.SecondaryAttributes.ElementalResistance };
            }
            if (hasAmuletArmor) {
                ArmorBase a = (ArmorBase)AmuletArmor;
                armorBonusValues += new SecondaryAttributes() { ArmorRating = a.SecondaryAttributes.ArmorRating, MaxHealth = a.SecondaryAttributes.MaxHealth, MaxMana = a.SecondaryAttributes.MaxMana, Awareness = a.SecondaryAttributes.Awareness, ElementalResistance = a.SecondaryAttributes.ElementalResistance };
            }
            if (hasRing1Armor) {
                ArmorBase a = (ArmorBase)Ring1Armor;
                armorBonusValues += new SecondaryAttributes() { ArmorRating = a.SecondaryAttributes.ArmorRating, MaxHealth = a.SecondaryAttributes.MaxHealth, MaxMana = a.SecondaryAttributes.MaxMana, Awareness = a.SecondaryAttributes.Awareness, ElementalResistance = a.SecondaryAttributes.ElementalResistance };
            }
            if (hasRing2Armor) {
                ArmorBase a = (ArmorBase)Ring2Armor;
                armorBonusValues += new SecondaryAttributes() { ArmorRating = a.SecondaryAttributes.ArmorRating, MaxHealth = a.SecondaryAttributes.MaxHealth, MaxMana = a.SecondaryAttributes.MaxMana, Awareness = a.SecondaryAttributes.Awareness, ElementalResistance = a.SecondaryAttributes.ElementalResistance };
            }
            if (hasCrestArmor) {
                ArmorBase a = (ArmorBase)CrestArmor;
                armorBonusValues += new SecondaryAttributes() { ArmorRating = a.SecondaryAttributes.ArmorRating, MaxHealth = a.SecondaryAttributes.MaxHealth, MaxMana = a.SecondaryAttributes.MaxMana, Awareness = a.SecondaryAttributes.Awareness, ElementalResistance = a.SecondaryAttributes.ElementalResistance };
            }
            if (hasTrinketArmor) {
                ArmorBase a = (ArmorBase)TrinketArmor;
                armorBonusValues += new SecondaryAttributes() { ArmorRating = a.SecondaryAttributes.ArmorRating, MaxHealth = a.SecondaryAttributes.MaxHealth, MaxMana = a.SecondaryAttributes.MaxMana, Awareness = a.SecondaryAttributes.Awareness, ElementalResistance = a.SecondaryAttributes.ElementalResistance };
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
                ElementalResistance = TotalPrimaryAttributes.Intellect
            };
        }
        // Calculates a weapons damage per turn.
        public (int, int) CalculateWeaponDPT() {
            bool hasWeapon = Equipment.TryGetValue(Slot.Right_Hand, out ItemBase equippedWeapon);
            if (hasWeapon) {
                return (((IWeapon)equippedWeapon).WeaponAttributes.MinDamage, ((IWeapon)equippedWeapon).WeaponAttributes.MaxDamage);
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
                HUDTools.ClearLastLine(1);
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
                ((IConsumable)Equipment[Slot.Potion]).Consume();
                TextInput.PressToContinue();
            } else if (input == "c" || input == "character" || input == "character screen") {
                HUDTools.CharacterScreen();
                TextInput.PressToContinue();
            } else if (input == "i" || input == "inventory") {
                while (true) {
                    HUDTools.InventoryScreen();
                    string input2 = TextInput.PlayerPrompt(false);
                    if (input2 == "back") {
                        break;
                    }
                }
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
            int a = Array.FindIndex(Inventory, item => item != null && item.ItemName == "Old Key");
            if (a != -1) {
                Inventory.SetValue(null, a);
            }
            int b = Array.FindIndex(Inventory, item => item != null && item.ItemName == "Rat Tail");
            if (b != -1) {
                Inventory.SetValue(null, b);
            }
            int c = Array.FindIndex(Inventory, item => item != null && item.ItemName == "Bat Wings");
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
            string input = TextInput.PlayerPrompt();
            if (input == "a" || input == "attack") {
                //Attack
                int damage = ((IWeapon)Program.CurrentPlayer.Equipment[Slot.Right_Hand]).Attack(Monster); ;
                Monster.TakeDamage(damage);
                HUDTools.WriteCombatLog("attack", TurnTimer, 0, damage, Monster);
                TurnTimer.TurnTimer++;
            }  else if (input == "r" || input == "run") {
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
                ((IConsumable)Equipment[Slot.Potion]).Consume();
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
            HUDTools.ClearLastLine(1);
        }
    }
}
