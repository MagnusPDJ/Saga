
namespace Saga.Dungeon.Rooms.Room_Objects
{
    public interface IInteractable
    {
        string Name { get; }
        string DescriptionBeforeInteracted { get; }
        string DescriptionAfterInteracted { get; }
        string LookDescription { get; set; }
    }
}
