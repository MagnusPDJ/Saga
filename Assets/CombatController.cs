using Saga.Character;
using Saga.Character.Buffs;
using Saga.Character.Skills;
using Saga.Dungeon.Enemies;
using Saga.Dungeon.Quests;
using Saga.Items;
using Saga.Items.Loot;
using System.Configuration;

namespace Saga.Assets
{
    public class CombatController(Player player, EnemyBase enemy)
    {
        private readonly Player _player = player;
        private readonly EnemyBase _enemy = enemy;
        public static bool AutoEndturn => Convert.ToBoolean(ConfigurationManager.AppSettings.Get("toggleAutoEndturn"));
        public int Turn { get; private set; } = 0;
        private bool Ran { get; set; } = false;
        private bool UsedMana { get; set; } = false;
        private bool Endturn { get; set; } = false;
        private int RemainingActionPoints { get; set; } = 0;
        private List<ISkill> AvailableSkills { get; set; } = [];

        public void Combat() {
            Action[] states = _player.DerivedStats.Initiative >= _enemy.Initiative
                ? [PlayerTurn, EnemyTurn]
                : [EnemyTurn, PlayerTurn];
            AvailableSkills = _player.SkillTree.GetLearnedSkills();
            HUDTools.CombatHUD(_player, _enemy, this);
            while (_enemy.Health > 0 && _player.Health > 0 && Ran == false) {
                Turn++;         
                foreach (var state in states) {
                    state();
                }
            }
            //Tjekker om monstret er besejret:
            if (_enemy.Health <= 0) {
                _player.BuffedStats.ClearAllBuffs();
                foreach (var skill in AvailableSkills) {
                    if (skill.Timer > 0) skill.Timer = 0;
                }
                HUDTools.CombatHUD(_player, _enemy, this);
                Program.SoundController.Stop();
                Program.SoundController.Play("win");
                LootSystem.GetCombatLoot(_enemy, $" You Won against the {_enemy.Name} on turn {Turn}!");
                Act1Quest.GainQuestProgress(_enemy);
            }
            _player.CheckForDeath(_enemy.PlayerDeathDescription);
        }

        bool EndTurn() {
            if (AutoEndturn) {
                if (RemainingActionPoints == 0) {
                    return true;
                }
            }
            return Endturn;
        }

        void PlayerTurn() {
            if (_player.Health > 0 && _enemy.Health > 0) {             
                RemainingActionPoints = _player.DerivedStats.ActionPoints;
                while (!EndTurn() && _enemy.Health > 0) {
                    HUDTools.CombatHUD(_player, _enemy, this);
                    PlayerActions();
                }
                if (!UsedMana && _enemy.Health > 0) {
                    _player.RegainMana();
                } else {
                    UsedMana = false;
                }
                if (_enemy.Health > 0 && !Ran) {
                    HUDTools.Print(" Your turn ended.", 5);
                    TickEndOfTurn();
                }               
                Endturn = false;
            }
        }

        void EnemyTurn() {
            if (Ran == false && _enemy.Health > 0) {
                EnemyActions();
            }
        }
        
        void PlayerActions() {
            string input = TextInput.PlayerPrompt();            
            switch (input) {
                default:
                    HUDTools.Print($" There is no {input} action!", 5);
                    TextInput.PressToContinue();
                    HUDTools.ClearLastLine(1);
                    break;
                case "1":
                    if (_player.SkillTree.QuickCast != string.Empty) {
                        ISkill? quickcast = AvailableSkills.Find(skill => skill is not null && skill.Name == _player.SkillTree.QuickCast);
                        if (quickcast is ITargetedSkill tSkill && tSkill.Cooldown > tSkill.Timer) {
                            if (CanUseAction(tSkill)) {
                                bool usedAP = tSkill.Activate(_player, _enemy);
                                if (usedAP) {
                                    SpendActionPoints(usedAP, tSkill);
                                    UsedMana = true;
                                }
                            }
                        } else if (quickcast is ISelfSkill sSkill && sSkill.Cooldown > sSkill.Timer) {
                            if (CanUseAction(sSkill)) {
                                bool usedAP = sSkill.Activate(_player);
                                if (usedAP) {
                                    SpendActionPoints(usedAP, sSkill);
                                    UsedMana = true;
                                }
                            }
                        } else {
                            HUDTools.Print($" {quickcast?.Name} is still on cooldown for {quickcast?.Timer} more turn(s)!", 3);
                            TextInput.PressToContinue();
                            HUDTools.ClearLastLine(1);
                        }
                    }  else {
                        HUDTools.Print($" You have no skill selected for quickcast!", 3);
                        TextInput.PressToContinue();
                        HUDTools.ClearLastLine(1);
                    }
                break;
                case "2":
                    //Attack
                    if (AvailableSkills.Find(s => s.Name == "Basic Attack") is ITargetedSkill skill2) {
                        if (CanUseAction(skill2)) {
                            bool usedAP = skill2.Activate(Program.CurrentPlayer, _enemy);
                            SpendActionPoints(usedAP, skill2);
                        }
                    }
                break;
                case "3":
                    var potion = _player.Equipment.ChoosePotionToDrink();
                    if (potion != null && CanUseAction(potion)) {
                        bool usedAP = potion.Consume();
                        SpendActionPoints(usedAP, potion);
                    }                    
                break;
                case "5":
                    //Run                   
                    if (_player.DerivedStats.ActionPoints != RemainingActionPoints) {
                        HUDTools.Print($" You have used action points this turn and can't run away...", 3);
                        TextInput.PressToContinue();
                        HUDTools.ClearLastLine(3);
                    } else {
                        HUDTools.Print($" Are you sure, you want to try and run? (Y/N)", 3);
                        input = TextInput.PlayerPrompt();
                        if (input == "y") {
                            if (_player.RunAway(_enemy)) {
                                Program.SoundController.Stop();
                                Ran = true;
                                Program.RoomController.Ran = true;
                                Endturn = true;
                                TextInput.PressToContinue();
                            }
                            Endturn = true;
                        }
                    }
                break;
                case "4":
                    //Skill tree logic
                    HUDTools.ShowSkillsCombat(_player, this);
                    while (true) {
                        input = TextInput.PlayerPrompt();
                        if (input == "b") {
                            break;
                        } else if (input == "q") {
                            Console.WriteLine(" Enter number of the skill to rebind it for quickcast:");
                            input = TextInput.PlayerPrompt();
                            if (input == "1") {
                                HUDTools.Print($" Basic attack cannot be set as quickcast...", 3);
                                TextInput.PressToContinue();
                                HUDTools.ClearLastLine(5);
                            } else if (int.TryParse(input, out int choice)) {
                                _player.SkillTree.ChangeQuickCast(choice - 1);
                                HUDTools.ShowSkillsCombat(_player, this);
                            } else {
                                HUDTools.Print($" Wrong input...", 3);
                                TextInput.PressToContinue();
                                HUDTools.ClearLastLine(5);
                            }
                        } else if (int.TryParse(input, out int choice)) {
                            if (CanUseAction((IAction)AvailableSkills[choice - 1])) {
                                HUDTools.CombatHUD(_player, _enemy, this);
                                if (AvailableSkills[choice - 1] is ISelfSkill selfSkill) {
                                    bool usedAP = selfSkill.Activate(_player);
                                    SpendActionPoints(usedAP, selfSkill);
                                } else if (AvailableSkills[choice - 1] is ITargetedSkill targetedSkill) {
                                    bool usedAP = targetedSkill.Activate(Program.CurrentPlayer, _enemy);
                                    SpendActionPoints(usedAP, targetedSkill);
                                }
                                break;
                            }
                        } else {
                            HUDTools.Print(" Wrong input...", 5);
                            TextInput.PressToContinue();
                            HUDTools.ClearLastLine(3);
                        }
                    }
                break;
                case "l":
                    //View combat log
                    HUDTools.Print($" *Not implemented*", 3);
                    TextInput.PressToContinue();
                    HUDTools.ClearLastLine(3);
                break;
                case "c":
                    HUDTools.CharacterScreen();
                    TextInput.PressToContinue();
                break;
                case "q":
                    HUDTools.QuestLogHUD();
                    TextInput.PressToContinue();
                break;
                case "e":
                    if (RemainingActionPoints >= _player.DerivedStats.ActionPoints) {
                        HUDTools.Print($" No Action Points spent, are you sure, you want to end turn? (Y/N)", 3);
                        input = TextInput.PlayerPrompt();
                        if (input != "y") break;
                    }
                    Endturn = true;
                break;
            }
        }

        void EnemyActions() {
            int attack = _enemy.Attack;
            attack -= Program.CurrentPlayer.DerivedStats.ArmorRating;
            if (attack <= 0) {
                attack = 1;
            }
            Program.CurrentPlayer.TakeDamage(attack);
            HUDTools.Print($" The Enemy Attacked and dealt {attack} damage!\n", 10);
            TextInput.PressToContinue();
            HUDTools.ClearLastLine(3);
        }
        bool CanUseAction(IAction action) {
            if (action is IActiveSkill basicAttack && basicAttack.Name == "Basic Attack") {
                if (basicAttack.ActionPointCost / _player.DerivedStats.AttackSpeed <= RemainingActionPoints) {
                    return true;
                } else {
                    HUDTools.Print(" Not enough Action Points to use your weapon", 10);
                    TextInput.PressToContinue();
                    HUDTools.ClearLastLine(3);
                    return false;
                }
            } else if (action is IActiveSkill skill) {
                if (skill.ActionPointCost / _player.DerivedStats.CastingSpeed <= RemainingActionPoints) {
                    return true;
                } else {
                    HUDTools.Print(" Not enough Action Points to use that skill", 10);
                    TextInput.PressToContinue();
                    HUDTools.ClearLastLine(3);
                    return false;
                }             
            } else if (action is IConsumable potion) {
                int hbIndex = _player.BuffedStats.ActiveBuffs.FindIndex(buff => buff.Name == "Haste");
                bool potionDrunk;
                if (hbIndex != -1) {
                    potionDrunk = ((HasteBuff)_player.BuffedStats.ActiveBuffs[hbIndex]).PotionDrunk;
                } else {
                    potionDrunk = true;
                }
                if (!potionDrunk) {
                    ((HasteBuff)_player.BuffedStats.ActiveBuffs[hbIndex]).PotionDrunk = true;
                    return true;
                }
                if (potion.ActionPointCost <= RemainingActionPoints) {
                    return true;
                } else {
                    HUDTools.Print(" Not enough Action Points to drink that potion", 10);
                    TextInput.PressToContinue();
                    HUDTools.ClearLastLine(3);
                    return false;
                }
            }
            return false;
        }
        void SpendActionPoints(bool usedAP, IAction action) {
            if (usedAP) {
                if (action is IActiveSkill attackSkill && attackSkill.SpeedType == "Attack Speed") {
                    RemainingActionPoints -= attackSkill.ActionPointCost / _player.DerivedStats.AttackSpeed;
                } else if (action is IActiveSkill castingSkill && castingSkill.SpeedType == "Casting Speed") {
                    RemainingActionPoints -= castingSkill.ActionPointCost / _player.DerivedStats.CastingSpeed;
                } else if (action is IConsumable potion) {
                    RemainingActionPoints -= potion.ActionPointCost;
                }
                if (RemainingActionPoints < 0) {
                    RemainingActionPoints = 0;
                }
            }
        }
        public int GetRemainingAP() {
            return RemainingActionPoints;
        }

        public void TickEndOfTurn() {
            foreach (var skill in AvailableSkills) {
                if (skill.Timer > 0) skill.Timer--;
            }
            _player.BuffedStats.TickBuffs();
        }
    }
}
