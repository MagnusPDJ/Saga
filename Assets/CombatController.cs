using Saga.Character;
using Saga.Character.Skills;
using Saga.Dungeon.Monsters;
using Saga.Items;
using System.Configuration;
using System.Reflection;
using System.Text.RegularExpressions;

namespace Saga.Assets
{
    public class CombatController(Player player, Enemy enemy)
    {
        private readonly Player _player = player;
        private readonly Enemy _enemy = enemy;
        private readonly Dictionary<string, int> _counts = [];
        private static bool AutoEndturn => Convert.ToBoolean(ConfigurationManager.AppSettings.Get("toggleAutoEndturn"));
        public int Turn { get; private set; } = 0;
        private bool Ran { get; set; } = false;
        private bool UsedMana { get; set; } = false;
        private bool Endturn { get; set; } = false;
        private int RemainingActionPoints { get; set; } = 0;       

        public void Combat() {
            Action[] states = _player.DerivedStats.Initiative >= _enemy.Initiative
                ? [PlayerTurn, EnemyTurn]
                : [EnemyTurn, PlayerTurn];

            while (_enemy.Health > 0 && _player.Health > 0 && Ran == false) {
                Turn++;
                Reset();
                HUDTools.CombatHUD(_enemy, this);                
                foreach (var state in states) {
                    state();
                }
            }
            //Tjekker om monstret er besejret:
            if (_enemy.Health <= 0) {
                HUDTools.CombatHUD(_enemy, this);
                Program.SoundController.Stop();
                Program.SoundController.Play("win");
                _player.Loot.GetCombatLoot(_enemy, $"You Won against the {_enemy.Name} on turn {Turn}!");
            }
            _player.CheckForDeath(_enemy.PlayerKillDescription);
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
                    PlayerActions();
                }
                if (!UsedMana && _enemy.Health > 0) {
                    _player.RegainMana();
                } else {
                    UsedMana = false;
                }
                HUDTools.Print("Your turn ended.", 5);
                TextInput.PressToContinue();
                HUDTools.ClearLastLine(1);
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
                    HUDTools.Print($"There is no {input} action!", 5);
                    TextInput.PressToContinue();
                    HUDTools.ClearLastLine(3);
                break;
                case "1":
                    if (_player.SkillTree.QuickCast is ITargetedSkill skill) {
                        if (CanUseAction(skill)) {
                            bool usedAP = skill.Activate(_player, _enemy);
                            if (usedAP) {
                                SpendActionPoints(usedAP, skill);
                                UsedMana = true;
                            }
                        }
                    } else if (_player.SkillTree.QuickCast is ISelfSkill skill1) {
                        if (CanUseAction(skill1)) {
                            bool usedAP = skill1.Activate(_player);
                            if (usedAP) {
                                SpendActionPoints(usedAP, skill1);
                                UsedMana = true;                              
                            }
                        }
                    } else {
                        HUDTools.Print($"You have no skill selected for quickcast!", 3);
                        TextInput.PressToContinue();
                        HUDTools.ClearLastLine(3);
                    }
                break;
                case "2":
                    //Attack
                    if (_player.LearnedSkills.Find(s => s.Name == "Basic Attack") is ITargetedSkill skill2) {
                        if (CanUseAction(skill2)) {
                            bool usedAP = skill2.Activate(Program.CurrentPlayer, _enemy);
                            SpendActionPoints(usedAP, skill2);
                        }
                    }
                break;
                case "3":
                    //Heal
                    var potion = Array.Find(_player.Equipment.Potion, p => p is IItem { ItemName: "Healing Potion" });
                    if (potion is not null) {
                        if (CanUseAction(potion)) {
                            bool usedAP = potion.Consume();
                            SpendActionPoints(usedAP, potion);
                        }
                    }
                break;
                case "4":
                    //Run                   
                    if (_player.RunAway(_enemy)) {
                        Program.SoundController.Stop();
                        Ran = true;
                    }
                break;
                case "5":
                    //Skill tree logic
                    HUDTools.Print($"*Not implemented*", 3);
                    TextInput.PressToContinue();
                    HUDTools.ClearLastLine(3);
                    break;
                case "c":
                    HUDTools.CharacterScreen();
                    TextInput.PressToContinue();
                    HUDTools.CombatHUD(_enemy, this);
                break;
                case "q":
                    HUDTools.QuestLogHUD();
                    TextInput.PressToContinue();
                    HUDTools.CombatHUD(_enemy, this);
                break;
                case "e":
                    Endturn = true;
                break;
            }
        }

        void EnemyActions() {
            int attack = _enemy.Power;
            attack -= Program.CurrentPlayer.DerivedStats.ArmorRating;
            if (attack <= 0) {
                attack = 1;
            }
            Program.CurrentPlayer.TakeDamage(attack);
            HUDTools.Print($"The Enemy Attacked and dealt {attack} damage!\n", 10);
            TextInput.PressToContinue();                               
        }
        bool CanUseAction(IAction action) {
            if (action.ActionPointCost > RemainingActionPoints) {
                HUDTools.Print("Not enough AP for that action");
                TextInput.PressToContinue();
                HUDTools.ClearLastLine(3);
                return false;
            } else {
                if (action is IActiveSkill skill && skill.Name == "Basic Attack" && GetUsedActionCount(skill.Name) == _player.DerivedStats.AttackSpeed) {
                    HUDTools.Print($"You have already attacked this turn. (Times attacked: {GetUsedActionCount(skill.Name)}, AttackSpeed {_player.DerivedStats.AttackSpeed})");
                    TextInput.PressToContinue();
                    HUDTools.ClearLastLine(3);
                    return false;
                } else if (action is IActiveSkill skill1 && GetUsedActionCount(skill1.Name) == _player.DerivedStats.CastingSpeed) {
                    HUDTools.Print($"You have already cast spells this turn. (Spells used: {GetUsedActionCount(skill1.Name)}, Casting Speed {_player.DerivedStats.CastingSpeed})");
                    TextInput.PressToContinue();
                    HUDTools.ClearLastLine(3);
                    return false;
                } else {
                    return true;
                }                  
            }
        }
        void SpendActionPoints(bool usedAP, IAction action) {
            if (usedAP) {
                RemainingActionPoints -= action.ActionPointCost;
                if (action is IItem item) {
                    CountUsedAction(item.ItemName);
                } else if (action is ISkill skill) {
                    CountUsedAction(skill.Name);
                }           
            }          
        }
        void Reset() => _counts.Clear();
        void CountUsedAction(string name) {
            // Count call
            _counts[name] = _counts.GetValueOrDefault(name) + 1;
        }
        int GetUsedActionCount(string name) {
            if (_counts.TryGetValue(name, out int value)) {
                return value;
            }
            return 0; 
        }
    }
}
