using Saga.Dungeon.Enemies.Interfaces;

namespace Saga.Dungeon.Enemies.Behaviour
{
    [EnemyTag("Beast", typeof(BeastWrapper))]
    public class BeastWrapper(EnemyBase inner) : EnemyWrapperBase(inner), IBeast 
    {
        //add behaviour here
    }
}
