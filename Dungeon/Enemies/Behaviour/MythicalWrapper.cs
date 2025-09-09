using Saga.Dungeon.Enemies.Interfaces;

namespace Saga.Dungeon.Enemies.Behaviour
{
    [EnemyTag("Mythical", typeof(MythicalWrapper))]
    public class MythicalWrapper(EnemyBase inner) : EnemyWrapperBase(inner), IMythical 
    {
        //add behaviour here
    }
}
