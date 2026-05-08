
namespace Saga.Items.Loot
{
    public class LootEntry
    {
        public string ItemId { get; set; } = string.Empty;
        public double DropChance { get; set; } // 0.0 - 1.0
        public string Class { get; set; } = string.Empty;
        public int MaxQuantity { get; set; } = 1;
        public int MinQuantity { get; set; } = 0;
        public double Modifier { get; set; } = 1.0;
    }
}
