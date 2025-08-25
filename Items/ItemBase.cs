
namespace Saga.Items
{
    public abstract class ItemBase : IItem
    {
        public abstract string ItemName { get; set; }
        public int ItemLevel { get; set; }
        public Slot ItemSlot { get; set; }
        public int ItemPrice { get; set; }
        public string ItemDescription { get; set; }
        
        public abstract int CalculateItemPrice();
    }
}
