using System;


namespace Saga.Items
{
    [Serializable]
    public class QuestItem : Item
    {
        public string ItemDescription { get; set; }
        public int Amount { get; set; } = 1;
        public override int CalculateItemPrice() {
            return 0;
        }
    }
}
