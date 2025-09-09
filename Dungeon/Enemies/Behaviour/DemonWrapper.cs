using Saga.Dungeon.Enemies.Interfaces;

namespace Saga.Dungeon.Enemies.Behaviour
{
    [EnemyTag("Demon", typeof(DemonWrapper))]
    public class DemonWrapper(EnemyBase inner) : EnemyWrapperBase(inner), IDemon 
    {
        //add behaviour here
    }
}
