using Saga.Assets;

namespace Saga.Items
{
    [Discriminator("questItem")]
    public class QuestItemBase : IQuestItem
    {
        public string ItemId { get; init; } = string.Empty;
        public string ItemName { get; set; } = string.Empty;
        public int ItemLevel { get; init; }
        public int ItemPrice { get; set; }
        public string ItemDescription { get; init; } = string.Empty;
        public int Amount { get; set; } = 1;

        public int CalculateItemPrice() {
            return 0;
        }
    }
}
