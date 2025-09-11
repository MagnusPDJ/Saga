
namespace Saga.Items.Loot
{
    public class LootEntry
    {
        public string ItemId { get; set; } = string.Empty;
        public double DropChance { get; set; } // 0.0 - 1.0
    }
}
