using Saga.Assets;
using Saga.Character.DmgLogic;

namespace Saga.Items
{
    [Discriminator("manaPotion")]
    public class ManaPotion : IConsumable, IEquipable, IMagical
    {
        public string ItemId { get; init; }
        public string ItemName { get; set; }
        public int ItemLevel { get; init; }
        public int ItemPrice { get; set; }
        public string ItemDescription { get; init; }
        public int PotionPotency { get; set; }
        public int PotionQuantity { get; set; }
        public int ActionPointCost { get; set; } = 1;
        public PotionType PotionType => PotionType.Mana;
        public Slot ItemSlot => Slot.Potion;
        public MagicalType MagicalType => MagicalType.Arcane;

        public ManaPotion() {
            PotionPotency = 5;
            PotionQuantity = 0;
            ItemId = "manapotion";
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
        public bool Consume() {
            if (PotionQuantity == 0) {
                HUDTools.Print("No potions left!", 5);
                TextInput.PressToContinue();
                HUDTools.ClearLastLine(3);
                return false;
            } else if (Program.CurrentPlayer.Mana == Program.CurrentPlayer.DerivedStats.MaxMana) {
                HUDTools.Print("You are already at max mana...", 5);
                TextInput.PressToContinue();
                HUDTools.ClearLastLine(3);
                return false;
            } else {
                HUDTools.Print("You use a mana potion", 10);
                PotionQuantity--;
                Program.CurrentPlayer.RegainMana(PotionPotency);
                if (Program.CurrentPlayer.Mana == Program.CurrentPlayer.DerivedStats.MaxMana) {
                    HUDTools.Print("You go to max mana!", 10);
                } else {
                    HUDTools.Print($"You gain {PotionPotency} mana", 10);
                }
                TextInput.PressToContinue();
                HUDTools.ClearLastLine(4);
                return true;
            }
        }
    }
}
