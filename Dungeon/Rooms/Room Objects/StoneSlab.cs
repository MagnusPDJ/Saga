
using Saga.Assets;
using Saga.Items;
using Saga.Items.Loot;

namespace Saga.Dungeon.Rooms.Room_Objects
{
    public class StoneSlab : ISearchable
    {
        public string Name => "stone slabs";
        public string DescriptionBeforeInteracted => " You spot some loose \u001b[96mstone slabs\u001b[0m on the floor that look like you could search under them.";
        public string DescriptionAfterInteracted => " The \u001b[90mstone slabs\u001b[0m you flipped over.";
        public string LookDescription { get; set; } = string.Empty;
        public bool Searched { get; set; } = false;

        public StoneSlab() { 
            LookDescription = DescriptionBeforeInteracted; 
        }

        public void Search() { 
            if (!Searched) {
                Searched = true;
                LootTable table = new()
                {
                    Items = [
                        new LootEntry() {ItemId = "rattail", DropChance = 1},
                        new LootEntry() {ItemId = "greenslime", DropChance = 1},
                        new LootEntry() {ItemId = "batwings", DropChance = 1},
                        new LootEntry() {ItemId = "candle", DropChance = 1}
                    ]
                };
                var loot = table.Items[Program.Rand.Next(table.Items.Count)];
                var item = ItemDatabase.GetByItemId(loot.ItemId);
                if (item is ICraftingItem cItem) {
                    int index = Array.FindIndex(Program.CurrentPlayer.Inventory, i => i is not null && i.ItemName == cItem.ItemName);
                    if (index != -1) {
                        cItem.Amount++;
                    } else {
                        index = Array.FindIndex(Program.CurrentPlayer.Inventory, i => i == null || Program.CurrentPlayer.Inventory.Length == 0);
                        Program.CurrentPlayer.Inventory.SetValue(item, index);
                    }
                }
                HUDTools.Print($" You search under the {Name} and you find some {item?.ItemName}.", 20);
                LookDescription = DescriptionAfterInteracted;
                TextInput.PressToContinue();
                HUDTools.ClearLastLine(3);
            } else {
                HUDTools.Print($" You have already searched the {Name}.", 20);
                TextInput.PressToContinue();
                HUDTools.ClearLastLine(3);
            }
        }
    }
}
