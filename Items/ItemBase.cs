
namespace Saga.Items
{
    public abstract class ItemBase : IItem
    {
        public string ItemName { get; set; }
        public int ItemLevel { get; set; }
        private int _itemPrice;
        public int ItemPrice => _itemPrice;
        public string ItemDescription { get; init; }
        
        public void SetItemPrice() => _itemPrice = CalculateItemPrice();
        public abstract int CalculateItemPrice();
    }
}
