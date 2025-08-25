
namespace Saga.Items
{
    public class QuestItem : ItemBase
    {
        public int Amount { get; set; } = 1;
        public override int CalculateItemPrice() {
            return 0;
        }
    }
}
