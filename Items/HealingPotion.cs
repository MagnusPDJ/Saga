using Saga.Assets;
using Saga.Character.DmgLogic;
using System;

namespace Saga.Items
{
    [Discriminator("healingPotion")]
    public class HealingPotion : IConsumable, IEquipable, IMagical
    {
        public string ItemName { get; set; }
        public int ItemLevel { get; set; }
        public int ItemPrice { get; set; }
        public string ItemDescription { get; init; }
        public int PotionPotency { get; set; }
        public int PotionQuantity { get; set; }
        public PotionType PotionType => PotionType.Healing;
        public Slot ItemSlot => Slot.Potion;
        public MagicalType MagicalType => MagicalType.Life;

        public HealingPotion() {
            PotionPotency = 5;
            PotionQuantity = 0;
            ItemName = "Healing Potion";
            ItemDescription = "They have a metallic taste and are somewhat sweet, but they reek of sulphur.";
            ItemPrice = CalculateItemPrice();
        }

        public int CalculateItemPrice() {
            return 20 + 10 * PotionPotency;
        }
        public string Equip() {
            Program.CurrentPlayer.Equipment.SetSlot(ItemSlot, this);
            return "New potion equipped!";
        }
        public string UnEquip() {
            int index = Array.FindIndex(Program.CurrentPlayer.Inventory, i => i == null || Program.CurrentPlayer.Inventory.Length == 0);
            Program.CurrentPlayer.Inventory.SetValue(this, index);
            Program.CurrentPlayer.Equipment.SetSlot(ItemSlot, null);
            return "Potion unequipped!";
        }
        public void Consume() {
            if (PotionQuantity == 0) {
                HUDTools.Print("No potions left!", 5);
            } else if (Program.CurrentPlayer.CurrentClass == "Mage") {
                int mageBonus = 1 + Program.CurrentPlayer.Level * 2;
                HUDTools.Print("You use a healing potion amplified by your magic", 10);
                PotionQuantity--;
                Program.CurrentPlayer.Health += PotionPotency + mageBonus;
                if (Program.CurrentPlayer.Health > Program.CurrentPlayer.DerivedStats.MaxHealth) {
                    Program.CurrentPlayer.Health = Program.CurrentPlayer.DerivedStats.MaxHealth;
                }
                if (Program.CurrentPlayer.Health == Program.CurrentPlayer.DerivedStats.MaxHealth) {
                    HUDTools.Print("You heal to max health!", 10);
                } else {
                    HUDTools.Print($"You gain {PotionPotency + mageBonus} health", 10);
                }
            } else {
                HUDTools.Print("You use a healing potion", 10);
                PotionQuantity--;
                Program.CurrentPlayer.Health += PotionPotency;
                if (Program.CurrentPlayer.Health > Program.CurrentPlayer.DerivedStats.MaxHealth) {
                    Program.CurrentPlayer.Health = Program.CurrentPlayer.DerivedStats.MaxHealth;
                }
                if (Program.CurrentPlayer.Health == Program.CurrentPlayer.DerivedStats.MaxHealth) {
                    HUDTools.Print("You heal to max health!", 10);
                } else {
                    HUDTools.Print($"You gain {PotionPotency} health", 10);
                }
            }
        }
    }
}
