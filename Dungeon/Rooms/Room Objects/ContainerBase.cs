using Saga.Assets;
using Saga.Items.Loot;

namespace Saga.Dungeon.Rooms.Room_Objects
{
    [Discriminator("containerBase")]
    public class ContainerBase : IExaminable, ILootable
    {
        public string Name { get; set; } = string.Empty;
        public string LootableId { get; init; } = string.Empty;
        public string DescriptionBeforeInteracted { get; set; } = string.Empty;
        public string DescriptionAfterInteracted { get; set; } = string.Empty;
        public string LookDescription { get; set; } = string.Empty;
        public bool Examined { get; set; } = false;

        public LootTable LootTable { get; set; } = new LootTable();

        public void Examine() {
            if (!Examined) {
                Examined = true;
                HUDTools.Print($" You rummage through the {Name}:", 20);
                LootSystem.GetLootFromTable(LootTable);
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
