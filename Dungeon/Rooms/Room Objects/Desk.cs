using Saga.Assets;
using Saga.Items.Loot;

namespace Saga.Dungeon.Rooms.Room_Objects
{
    public class Desk : IExaminable
    {
        public string Name { get; set; } = "desk";
        public string LookDescription => " An old wooden \u001b[96mdesk\u001b[0m you could examine.";
        public bool Examined { get; set; } = false;
        public void Examine() {
            if (!Examined) {
                Examined = true;
                HUDTools.Print(" You rummage through dusty documents and moldy records illegible or in unknown languages,\n but in a drawer you find some gold and a key.", 20);
                LootSystem.GetQuestLoot(1, 0, "MeetFlemsha");
                TextInput.PressToContinue();
                HUDTools.ClearLastLine(6);
            } else {
                HUDTools.Print($" You have already examined the {Name}.", 20);
                TextInput.PressToContinue();
                HUDTools.ClearLastLine(3);
            }
        }
    }
}
