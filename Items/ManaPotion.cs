using Saga.Assets;
using Saga.Character.DmgLogic;

namespace Saga.Items
{
    [Discriminator("manaPotion")]
    public class ManaPotion : IConsumable, IEquipable, IMagical
    {
        public string ItemName { get; set; }
        public int ItemLevel { get; set; }
        public int ItemPrice { get; set; }
        public string ItemDescription { get; init; }
        public int PotionPotency { get; set; }
        public int PotionQuantity { get; set; }
        public PotionType PotionType => PotionType.Mana;
        public Slot ItemSlot => Slot.Potion;
        public MagicalType MagicalType => MagicalType.Arcane;

        public ManaPotion() {
            PotionPotency = 5;
            PotionQuantity = 0;
            ItemName = "Mana Potion";
            ItemDescription = "They are a bit minty but have a rancid after taste.";
            ItemPrice = CalculateItemPrice();
        }

        public int CalculateItemPrice() {
            return 20 + 10 * PotionPotency;
        }
        public string Equip() {
            int index = Array.FindIndex(Program.CurrentPlayer.Equipment.Potion, i => i == null || Program.CurrentPlayer.Equipment.Potion.Length == 0);
            Program.CurrentPlayer.Equipment.Potion.SetValue(this, index);
            return "New potion equipped!";
        }
        public string UnEquip() {
            int index1 = Array.FindIndex(Program.CurrentPlayer.Inventory, i => i == null || Program.CurrentPlayer.Inventory.Length == 0);
            Program.CurrentPlayer.Inventory.SetValue(this, index1);
            int index2 = Array.FindIndex(Program.CurrentPlayer.Equipment.Potion, i => i is IItem { ItemName: "Mana Potion" });
            Program.CurrentPlayer.Equipment.Potion.SetValue(null, index2);
            return "Potion unequipped!";
        }
        public void Consume() {
            if (PotionQuantity == 0) {
                HUDTools.Print("No potions left!", 5);
            } else if (Program.CurrentPlayer.CurrentClass == "Mage") {
                int mageBonus = 1 + Program.CurrentPlayer.Level * 2;
                HUDTools.Print("You use a healing potion amplified by your magic", 10);
                PotionQuantity--;
                Program.CurrentPlayer.RegainHealth(PotionPotency + mageBonus);
                if (Program.CurrentPlayer.Health == Program.CurrentPlayer.DerivedStats.MaxHealth) {
                    HUDTools.Print("You heal to max health!", 10);
                } else {
                    HUDTools.Print($"You gain {PotionPotency + mageBonus} health", 10);
                }
            } else {
                HUDTools.Print("You use a healing potion", 10);
                PotionQuantity--;
                Program.CurrentPlayer.RegainHealth(PotionPotency);
                if (Program.CurrentPlayer.Health == Program.CurrentPlayer.DerivedStats.MaxHealth) {
                    HUDTools.Print("You heal to max health!", 10);
                } else {
                    HUDTools.Print($"You gain {PotionPotency} health", 10);
                }
            }
        }
    }
}
