
namespace Saga.Items
{
    public interface IItem
    {
        string ItemId { get; init; }
        string ItemName { get; set; }
        int ItemLevel { get; init; }
        int ItemPrice { get; set; }
        string ItemDescription { get; init; }
        int CalculateItemPrice();
    }
}
