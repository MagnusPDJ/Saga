using System;


namespace Saga.Items
{
    [Serializable]
    public class QuestItem : Item
    {
        public string ItemDescription { get; set; }

        public override int CalculateItemPrice() {
            return 0;
        }
    }
}
