using Saga.Assets;
using Saga.Character.DmgLogic;
using Saga.Character.Skills;
using Saga.Dungeon;
using Saga.Dungeon.Enemies;
using Saga.Dungeon.Quests;
using Saga.Items;
using Saga.Items.Loot;
using System.Text.Json.Serialization;

namespace Saga.Character
{
    [JsonDerivedType(typeof(Warrior), typeDiscriminator: "warrior")]
    [JsonDerivedType(typeof(Archer), typeDiscriminator: "archer")]
    [JsonDerivedType(typeof(Mage), typeDiscriminator: "mage")]
    public abstract class Player {
        public string CurrentClass { get; }
        public Encounters.Act CurrentAct { get; set; }
        public string Name { get; set; }
        public int Id { get; set; }
        public int Level { get; set; }
        public int Exp { get; set; }
        private int LevelUpValue { get; set; } = 265;
        public int Gold { get; set; }
        public int Health { get; private set; }
        public int Mana { get; set; }
        public int FreeAttributePoints { get; set; }
        public Attributes Attributes { get; set; }
        public DerivedStats DerivedStats { get; set; }       
        public List<ISkill> LearnedSkills { get; set; }
        public SkillTree SkillTree { get; init; }
        public int SkillPoints { get; set; }
        public Equipment Equipment { get; set; }
        public IItem[] Inventory { get; set; }
        public List<Quest> QuestLog { get; set; }
        public List<Quest> FailedQuests { get; set; }
        public List<Quest> CompletedQuests { get; set; }
        public List<NonPlayableCharacters> NpcsInCamp { get; set; }
        public int TimesExplored { get; set; }

        public event Action? PlayerChanged;

        public Player(string name, string currentClass, SkillTree skillTree, int strength, int dexterity, int intellect) {
            Name = name;
            CurrentClass = currentClass;
            SkillTree = skillTree;
            Level = 1;
            CurrentAct = Encounters.Act.Start;
            Equipment = new Equipment(this);
            Inventory = new IItem[10];
            QuestLog = [];
            FailedQuests = [];
            CompletedQuests = [];
            NpcsInCamp = [];
            Exp = 0;
            Gold = 0;
            FreeAttributePoints = 0;
            Attributes = new Attributes(this, strength, dexterity, intellect);
            DerivedStats = new DerivedStats(this);
            Health = DerivedStats.MaxHealth;
            Mana = DerivedStats.MaxMana;
            LearnedSkills = [new BasicAttack()];
            SkillPoints = 0;
            TimesExplored = 0;
            Equipment.EquipmentChanged += () => PlayerChanged?.Invoke();
            Attributes.AttributesChanged += () => PlayerChanged?.Invoke();         
        }
        public void AttachAfterLoad() {
            Equipment.AttachToPlayer(this);
            Attributes.AttachToPlayer(this);
            DerivedStats.AttachToPlayer(this);

            Equipment.EquipmentChanged += () => PlayerChanged?.Invoke();
            Attributes.AttributesChanged += () => PlayerChanged?.Invoke();            
        }
        public void LevelUp() {
            int levels = 0;
            Program.SoundController.Play("levelup");
            while (CanLevelUp()) {
                Program.CurrentPlayer.Exp -= LevelUpValue;
                Program.CurrentPlayer.Level++;
                Program.CurrentPlayer.FreeAttributePoints += 6;
                Program.CurrentPlayer.SkillPoints++;
                levels++;
            }
            SetLevelUpValue();
            Program.CurrentPlayer.RegenToFull();

            HUDTools.Print($"\u001b[34m Congratulations! You are now level {Level}! You've gained 6 attribute point and 1 skill point.\u001b[0m", 20);
        }
        //Metode til udregning af det exp det koster at level op.
        public void SetLevelUpValue() {
            int temp_lvlupvalue = LevelUpValue;
            temp_lvlupvalue += 5 * Level * Level + 230;
            LevelUpValue = temp_lvlupvalue;
        }
        public int GetLevelUpValue() {
            return LevelUpValue;
        }
        //Metode til at tjekke om lvl op er muligt.
        public bool CanLevelUp() {
            if (Exp >= LevelUpValue) {
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
        public int SpendAttributePoint(int i) {
            if (FreeAttributePoints > 0 && i != 0) {
                HUDTools.Print(" Allocate attribute point? Type the corresponding (A)ttribute abbr. to spent 1 point, else (N)o", 0);
                while (true) {
                    string input = TextInput.PlayerPrompt();
                    if (input == "s" || input == "strength") {
                        Attributes.AddValues(strength: 1);
                        FreeAttributePoints--;
                        break;
                    } else if (input == "d" || input == "dexterity") {
                        Attributes.AddValues(dexterity: 1);
                        FreeAttributePoints--;
                        break;
                    } else if (input == "i" || input == "intellect") {
                        Attributes.AddValues(intellect: 1);
                        FreeAttributePoints--;
                        break;
                    } else if (input == "c" || input == "constitution") {
                        Attributes.AddValues(constitution: 1);
                        FreeAttributePoints--;
                        break;
                    } else if (input == "w" || input == "willpower") {
                        Attributes.AddValues(willPower: 1);
                        FreeAttributePoints--;
                        break;
                    } else if (input == "a" || input == "awareness") {
                        Attributes.AddValues(awareness: 1);
                        FreeAttributePoints--;
                        break;
                    } else if (input == "n" || input == "no") {
                        return 0;
                    } else {
                        HUDTools.Print("Invalid input", 1);
                        TextInput.PressToContinue();
                        HUDTools.ClearLastLine(3);
                    }
                }               
            }
            return i;
        }
        public void SpendSkillPoint(int skillIndex) {
            var availableSkills = SkillTree.GetAvailableSkills(Level);
            if (SkillPoints > 0 && skillIndex >= 0 && skillIndex < availableSkills.Count) {
                var skill = availableSkills[skillIndex];
                skill.IsUnlocked = true;
                var tier = skill.Tier;
                tier.Min += 1;
                skill.Tier = tier;
                LearnedSkills.Add(skill);
                SkillPoints--;
                HUDTools.Print($"Unlocked skill: {skill.Name}", 15);
            } else {
                HUDTools.Print("Cannot unlock this skill.", 15);
            }
        }
        //Metode til at sætte start udstyr.
        public abstract void SetStartingGear();
        public virtual bool RunAway(EnemyBase Monster) {
            bool escaped = false;
            if (Program.Rand.Next(0, 3) == 0 || Monster.Name == "Human captor") {
                HUDTools.Print($" You try to run from the {Monster.Name}, but it knocks you down. You are unable to escape this turn", 15);
            } else {
                HUDTools.Print($" You barely manage to shake off the {Monster.Name} and you successfully escape.", 20);
                escaped = true;
            }
            return escaped;
        }
        public abstract (IDamageType, int) CalculateDamageModifiers((IDamageType, int) damage);
        //Metode til at checke for om spilleren dør som kan kaldes hver gang spilleren tager skade.
        public void CheckForDeath(string message) {
            if (Health <= 0) {
                Program.SoundController.Stop();
                Program.SoundController.Play("gameover");
                HUDTools.Print($"\u001b[31m{message}\u001b[0m", 20);
                TextInput.PressToContinue();
                Program.CurrentPlayer = new Warrior("Adventurer");
                Program.MainMenu();
            }
        }
        //Metode til at kalde basic actions (heal, inventory og character).
        public void BasicActions(string input) {
            if (input == "h" || input == "heal") {
                //Heal
                var potion = Array.Find(Equipment.Potion, p => p is IItem { ItemName: "Healing Potion" });
                potion?.Consume();
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
            } else if (input == "k" || input == "skill" || input == "skills" || input == "skilltree") {
                while (true) {
                    HUDTools.ShowSkillTree();
                    string input2 = TextInput.PlayerPrompt();
                    if (input2 == "b") {
                        break;
                    } else if (int.TryParse(input, out int choice)) {
                        Program.CurrentPlayer.SpendSkillPoint(choice - 1);
                    }
                }
            } else {
                HUDTools.Print($"There is no {input} action...", 15);
                TextInput.PressToContinue();
                HUDTools.ClearLastLine(3);
                return;
            }
            if (Program.RoomController.CurrentRoom == RoomController.Camp) {
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
            int qItem = Array.FindIndex(Inventory, item => item != null && item.ItemId == "oldkey");
            if (qItem != -1) {
                Inventory.SetValue(null, qItem);
            }
            qItem = Array.FindIndex(Inventory, item => item != null && item.ItemId == "rattail");
            if (qItem != -1) {
                Inventory.SetValue(null, qItem);
            }
            qItem = Array.FindIndex(Inventory, item => item != null && item.ItemId == "batwings");
            if (qItem != -1) {
                Inventory.SetValue(null, qItem);
            }
            QuestLog.Remove(quest);
            CompletedQuests.Add(quest);
            Program.SoundController.Play("win");
            HUDTools.Print($"\u001b[96m You've completed the quest: {quest.Name}!\u001b[0m", 15);
            LootSystem.GetFixedGold(quest.Gold);
            LootSystem.GetPotions(quest.Potions);
            LootSystem.GetFixedExp(quest.Exp);
            if (quest.Item != null) {
                int index = Array.FindIndex(Inventory, i => i == null || Inventory.Length == 0);
                Inventory.SetValue(quest.Item, index);
                HUDTools.Print($"\u001b[35m You've gained {quest.Item.ItemName}\u001b[0m");
            }
        }
        public void TakeDamage(int damage) {
            Health -= damage;
        }
        public void RegainHealth(int amount) {
            Health += amount;
            if (Health > DerivedStats.MaxHealth) {
                Health = DerivedStats.MaxHealth;
            }
        }
        public void RegenToFull() {
            Health = DerivedStats.MaxHealth;
            Mana = DerivedStats.MaxMana;
        }
        public void SetHealth(int amount) {
            Health = amount;
        }
        public void SetMana(int amount) {
            Mana = amount;
        }
        public void RegainMana(int? amount = null) {
            if (amount != null) {
                Mana += (int)amount;
            } else {
                Mana += DerivedStats.ManaRegenRate;
            }
            if (Mana > DerivedStats.MaxMana) {
                Mana = DerivedStats.MaxMana;
            }
        }
        public bool SpendMana(int cost) {
            if (Mana >= cost) {
                Mana -= cost;
                return true;
            } else { 
                return false; 
            }
        }
    }
}
