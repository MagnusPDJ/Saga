using Saga.Assets;
using Saga.Items.Loot;

namespace Saga.Dungeon.Rooms.Room_Objects
{
    public class Crate : IExaminable
    {
        public string Name => "crate";
        public string LookDescription => " There is a wooden \u001b[96mcrate\u001b[0m you could examine.";
        public bool Examined { set; get; } = false;
        public void Examine() {
            if (!Examined) {
                Examined = true;
                HUDTools.Print($" You rummage through the {Name} and you find some gold.", 20);
                LootSystem.GetGold(1);
                TextInput.PressToContinue();
                HUDTools.ClearLastLine(4);
            } else {
                HUDTools.Print($" You have already examined the {Name}.", 20);
                TextInput.PressToContinue();
                HUDTools.ClearLastLine(3);
            }
        }
    }
}
