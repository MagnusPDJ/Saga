
namespace Saga.Dungeon.Rooms.Room_Objects
{
    public interface ISearchable : IInteractable
    {
        bool Searched { get; set; }
        void Search();
    }
}
