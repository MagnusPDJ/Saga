using System;


namespace Saga.Items
{
    [Serializable]
    public class QuestItem : Item
    {
        public string ItemDescription { get; set; }
        public (int, int) Amount { get; set; } = (1, 1);
        public override int CalculateItemPrice() {
            return 0;
        }
    }
}
