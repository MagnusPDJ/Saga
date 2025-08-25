
namespace Saga.Items
{
    internal interface IItem
    {
        string ItemName { get; set; }
        int ItemLevel { get; set; }
        int ItemPrice { get; set; }
        string ItemDescription { get; set; }
        int CalculateItemPrice();
    }
}
