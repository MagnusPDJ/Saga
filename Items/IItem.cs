
namespace Saga.Items
{
    public interface IItem
    {
        string ItemName { get; set; }
        int ItemLevel { get; set; }
        int ItemPrice { get; set; }
        string ItemDescription { get; init; }
        int CalculateItemPrice();
    }
}
