
using NAudio.CoreAudioApi;
using Saga.Character;
using System;

namespace Saga.Items
{
    public class ArmorBase : ItemBase, IArmor, IEquipable
    {
        public ArmorType ArmorType { get; set; }
        public Slot ItemSlot { get; set; }
        public PrimaryAttributes PrimaryAttributes { get; set; }
        public SecondaryAttributes SecondaryAttributes { get; set; }

        public ArmorBase() {
            SetPrimaryAttributes();
            SetSecondaryAttributes();
            SetItemPrice();
        }

        public void SetPrimaryAttributes() => PrimaryAttributes = CalculatePrimaryAttributes(ItemLevel);
        public void SetSecondaryAttributes() => SecondaryAttributes = CalculateSecondaryAttributes(ItemLevel);
        public PrimaryAttributes CalculatePrimaryAttributes(int level) {
            int constitution = 0;
            int strength = 0;
            int dexterity = 0;
            int intellect = 0;
            int willpower = 0;
            
            //Roll for primary stats:
            for (int i = 0; i < 5; i++) {

                int roll = Program.Rand.Next(1, 100 + 1);

                if (i == 0 && roll <= 10) {
                    constitution = Program.Rand.Next(Math.Max(1, Program.CurrentPlayer.Level + level));
                }
                if (i == 1 && roll <= 10) {
                    strength = Program.Rand.Next(Math.Max(1, Program.CurrentPlayer.Level + level));
                }
                if (i == 2 && roll <= 10) {
                    dexterity = Program.Rand.Next(Math.Max(1, Program.CurrentPlayer.Level + level));
                }
                if (i == 3 && roll <= 10) {
                    intellect = Program.Rand.Next(Math.Max(1, Program.CurrentPlayer.Level + level));
                }
                if (i == 4 && roll <= 10) {
                    willpower = Program.Rand.Next(Math.Max(1, Program.CurrentPlayer.Level + level));
                }

            }
            return new PrimaryAttributes() { Constitution = constitution, Strength = strength, Dexterity = dexterity, Intellect = intellect, WillPower = willpower };
        }
        public SecondaryAttributes CalculateSecondaryAttributes(int level) {
            int maxHealth = 0;
            int maxMana = 0;
            int awareness = 0;
            int armorRating = 1;
            int elementalResistance = 0;

            //Roll for secondary stats:
            for (int i = 0; i < 5; i++) {

                int roll = Program.Rand.Next(1, 100 + 1);

                if (i == 0 && roll <= -1) {
                    maxHealth = Program.Rand.Next(Math.Max(1, Program.CurrentPlayer.Level + level));
                }
                if (i == 1 && roll <= -1) {
                    maxMana = Program.Rand.Next(Math.Max(1, Program.CurrentPlayer.Level + level));
                }
                if (i == 2 && roll <= -1) {
                    awareness = Program.Rand.Next(Math.Max(1, Program.CurrentPlayer.Level + level));
                }
                if (i == 3 && roll <= 10) {
                    armorRating = Program.Rand.Next(Program.CurrentPlayer.Level / 2, 2 + Math.Max(3, Program.CurrentPlayer.Level + level));
                }
                if (i == 4 && roll <= -1) {
                    elementalResistance = Program.Rand.Next(Math.Max(1, Program.CurrentPlayer.Level + level));
                }
            }
            return new SecondaryAttributes() { MaxHealth = maxHealth, MaxMana = maxMana, Awareness = awareness, ArmorRating = armorRating, ElementalResistance = elementalResistance };
        }
        public override int CalculateItemPrice() {
            throw new System.NotImplementedException();
        }
        public string Equip() { 
            throw new System.NotImplementedException(); 
        }
        public string UnEquip() { 
            throw new System.NotImplementedException(); 
        }

    }
}
