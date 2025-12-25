
namespace Saga.Dungeon.Rooms.Room_Objects
{
    public interface IExaminable : IInteractable
    {
        bool Examined { get; set; }
        void Examine();
    }
}
