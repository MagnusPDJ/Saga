using Saga.Character;
using Saga.Character.Skills;
using Saga.Dungeon.Monsters;
using Saga.Items;

namespace Saga.Assets
{
    public class CombatController(Player player, Enemy enemy)
    {
        private readonly Player _player = player;
        private readonly Enemy _enemy = enemy;
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

        void PlayerTurn() {
            if (_player.Health > 0 && _enemy.Health > 0) {
                RemainingActionPoints = _player.DerivedStats.ActionPoints;
                while (!Endturn && _enemy.Health > 0 && RemainingActionPoints > 0) {
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
                        int tempMana = _player.Mana;
                        skill.Activate(_player, _enemy);
                        if (tempMana > _player.Mana) {
                            RemainingActionPoints -= skill.ActionPointCost;
                            UsedMana = true;
                        }
                    } else if (_player.SkillTree.QuickCast is ISelfSkill skill1) {
                        int tempMana = _player.Mana;
                        skill1.Activate(_player);
                        if (tempMana > _player.Mana) {
                            RemainingActionPoints -= skill1.ActionPointCost;
                            UsedMana = true;
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
                        skill2.Activate(Program.CurrentPlayer, _enemy);
                        RemainingActionPoints -= skill2.ActionPointCost;
                    }
                break;
                case "3":
                    //Heal
                    var potion = Array.Find(_player.Equipment.Potion, p => p is IItem { ItemName: "Healing Potion" });
                    int tempHealth = _player.Health;
                    if (potion is not null) {
                        potion.Consume();
                        if (tempHealth < _player.Health) {
                            RemainingActionPoints -= potion.ActionPointCost;
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
    }
}
