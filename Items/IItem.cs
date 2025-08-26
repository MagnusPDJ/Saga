
namespace Saga.Items
{
    internal interface IItem
    {
        string ItemName { get; set; }
        int ItemLevel { get; set; }
        int ItemPrice { get; }
        string ItemDescription { get; init; }
        int CalculateItemPrice();
    }
}
