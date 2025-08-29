using Saga.Assets;

namespace Saga.Items
{
    [Discriminator("questItem")]
    public class QuestItemBase : IQuestItem
    {
        public string ItemName { get; set; }
        public int ItemLevel { get; set; }
        public int ItemPrice { get; set; }
        public string ItemDescription { get; init; }
        public int Amount { get; set; } = 1;

        public QuestItemBase() {
            ItemPrice = CalculateItemPrice();
        }

        public int CalculateItemPrice() {
            return 0;
        }
    }
}
