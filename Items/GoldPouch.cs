
namespace Saga.Items
{
    public class GoldPouch : IEquipable
    {
        public string ItemId { get; init; }
        public string ItemName { get; set; }
        public int ItemLevel { get; init; }
        public int ItemPrice { get; set; }
        public string ItemDescription { get; init; }
        public Slot ItemSlot => Slot.Pouch;
        public int GoldAmount { get; set; }
        public GoldPouch() { 
            ItemId = "goldpouch";
            ItemName = "Gold Pouch";
            ItemDescription = "A small leathery pouch to hold your gold.";
            GoldAmount = 0;
        }

        public int CalculateItemPrice() {
            return 0;
        }
        public string Equip() {
            return "";
        }
        public string UnEquip() {
            return "";
        }
    }
}
