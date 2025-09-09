using Saga.Dungeon.Enemies.Interfaces;

namespace Saga.Dungeon.Enemies.Behaviour
{
    [EnemyTag("Dragon", typeof(DragonWrapper))]
    public class DragonWrapper(EnemyBase inner) : EnemyWrapperBase(inner), IDragon 
    {
        //add behaviour here
    }
}
