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
        private int Turn { get; set; } = 1;
        private bool Ran { get; set; } = false;
        private bool UsedMana { get; set; } = false;
        private bool Endturn { get; set; } = false;
        private int RemainingAcionPoints { get; set; }

        public void Combat() {
            int manaRegen = 1;
            HUDTools.ClearLog();  
            //Tjekker hvem starter if(spilleren starter), else (Fjenden starter):
            if (_player.DerivedStats.Initiative > _enemy.Initiative) {
                while (_enemy.Health > 0 && Ran == false) {
                    HUDTools.CombatHUD(_enemy, this);
                    PlayerActions();
                    if (Ran == false) {
                        _enemy.EnemyActions(this);
                    }
                    _player.CheckForDeath($"As the {_enemy.Name} menacingly comes down to strike, you are slain by the mighty {_enemy.Name}.");
                    if (manaRegen < Turn && !UsedMana) {
                        _player.RegainMana();
                        manaRegen++;
                    } else {
                        UsedMana = false;
                    }
                }
            } else {
                while (_enemy.Health > 0 && Ran == false) {
                    HUDTools.CombatHUD(_enemy, this);
                    _enemy.EnemyActions(this);
                    _player.CheckForDeath($"As the {_enemy.Name} menacingly comes down to strike, you are slain by the mighty {_enemy.Name}.");
                    if (manaRegen < Turn && !UsedMana) {
                        _player.RegainMana();
                        manaRegen++;
                    } else {
                        UsedMana = false;
                    }
                    PlayerActions();
                }
            }
            //Tjekker om monstret er besejret:
            if (_enemy.Health <= 0) {
                Program.SoundController.Stop();
                HUDTools.ClearLog();
                Program.SoundController.Play("win");
                _player.Loot.GetCombatLoot(_enemy, $"You Won against the {_enemy.Name} on turn {Turn - 1}!");
            }
        }
        //Metode til at vælge mellem klasse skills i kamp.
        public void PlayerActions() {
            string input = TextInput.PlayerPrompt();
            if (input == "2" || input == "basic attack") {
                //Attack
                if (_player.LearnedSkills.Find(s => s.Name == "Basic Attack") is ITargetedSkill skill) {
                    skill.Activate(Program.CurrentPlayer, _enemy);
                }
                Turn++;
            } else if (input == "3" || input == "heal") {
                //Heal
                var potion = Array.Find(_player.Equipment.Potion, p => p is IItem { ItemName: "Healing Potion" });
                potion?.Consume();
                HUDTools.WriteCombatLog(action: "heal", combatController: this, monster: _enemy);
                Turn++;
            } else if (input == "4" || input == "run") {
                //Run                   
                if (_player.RunAway(_enemy)) {
                    Program.SoundController.Stop();
                    HUDTools.ClearLog();
                    Ran = true;
                } else {
                    HUDTools.WriteCombatLog(action: "run", combatController: this, monster: _enemy);
                    Turn++;
                }
            } else if (input == "5" || input == "look at skills") {
                //
            } else if (input == "1" || input == "quickcast") {
                if (_player.SkillTree.QuickCast is ITargetedSkill skill) {
                    int tempMana = _player.Mana;
                    skill.Activate(_player, _enemy);
                    if (tempMana > _player.Mana) {
                        Turn++;
                    }
                    UsedMana = true;
                } else {
                    HUDTools.Print($"You have no skill selected for quickcast!", 5);
                }

            } else if (input == "c" || input == "character screen") {
                HUDTools.CharacterScreen();
            } else if (input == "l" || input == "combat log") {
                Console.Clear();
                HUDTools.GetLog();
            } else if (input == "q" || input == "questlog") {
                HUDTools.QuestLogHUD();
            } else {
                HUDTools.Print($"There is no {input} action!", 5);
            }
            TextInput.PressToContinue();
            HUDTools.ClearLastLine(1);
        }

    }
}
