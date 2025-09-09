using Saga.Dungeon.Enemies.Interfaces;

namespace Saga.Dungeon.Enemies.Behaviour
{
    [EnemyTag("Troll", typeof(TrollWrapper))]
    public class TrollWrapper(EnemyBase inner) : EnemyWrapperBase(inner), ITroll 
    {
        //add behaviour here
    }
}
