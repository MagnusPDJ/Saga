using Saga.Dungeon.Enemies.Interfaces;

namespace Saga.Dungeon.Enemies.Behaviour
{
    [EnemyTag("Greenskin", typeof(GreenskinWrapper))]
    public class GreenskinWrapper(EnemyBase inner) : EnemyWrapperBase(inner), IGreenskin 
    {
        //add behaviour here
    }
}
