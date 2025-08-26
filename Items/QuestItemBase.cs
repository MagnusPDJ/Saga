
namespace Saga.Items
{
    public class QuestItemBase : ItemBase, IQuestItem
    {
        public int Amount { get; set; } = 1;

        public QuestItemBase() {
            SetItemPrice();
        }

        public override int CalculateItemPrice() {
            return 0;
        }
    }
}
