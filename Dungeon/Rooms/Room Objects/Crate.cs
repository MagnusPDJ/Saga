using Saga.Assets;
using Saga.Items.Loot;

namespace Saga.Dungeon.Rooms.Room_Objects
{
    public class Crate : IExaminable
    {
        public string Name => "crate";
        public string DescriptionBeforeInteracted => " There is a wooden \u001b[96mcrate\u001b[0m you could examine.";
        public string DescriptionAfterInteracted => " There is an empty wooden \u001b[90mcrate\u001b[0m.";
        public string LookDescription { set; get; } = string.Empty;
        public bool Examined { set; get; } = false;

        public Crate() { 
            LookDescription = DescriptionBeforeInteracted;
        }

        public void Examine() {
            if (!Examined) {
                Examined = true;
                HUDTools.Print($" You rummage through the {Name} and you find some gold.", 20);
                LootSystem.GetGold(1);
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
