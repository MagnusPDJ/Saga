using Saga.Assets;
using Saga.Items.Loot;

namespace Saga.Dungeon.Rooms.Room_Objects
{
    public class Chest : IExaminable, ILootable
    {
        public string Name { get; set; } = "chest";
        public string DescriptionBeforeInteracted => " A \u001b[96mchest\u001b[0m you could examine.";
        public string DescriptionAfterInteracted => " An empty \u001b[90mchest\u001b[0m.";
        public string LookDescription { set; get; } = string.Empty;
        public bool Examined { get; set; } = false;

        public LootTable LootTable { get; set; } = new() {
            Items = [
                        new LootEntry() {ItemId = "rattail", DropChance = 0.1},
                        new LootEntry() {ItemId = "greenslime", DropChance = 0.1},
                        new LootEntry() {ItemId = "batwings", DropChance = 0.1},
                        new LootEntry() {ItemId = "candle", DropChance = 0.1}
                    ]
        };

        public Chest() {
            LookDescription = DescriptionBeforeInteracted;
        }

        public void Examine() {
            if (!Examined) {
                Examined = true;
                HUDTools.Print($" You rummage through the {Name}:", 20);
                LootSystem.GetRoomObjectLoot(this);
                LookDescription = DescriptionAfterInteracted;
                TextInput.PressToContinue();
                HUDTools.RoomHUD();
            } else {
                HUDTools.Print($" You have already examined the {Name}.", 20);
                TextInput.PressToContinue();
                HUDTools.ClearLastLine(3);
            }
        }
    }
}
