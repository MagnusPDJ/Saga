using Saga.Dungeon.Enemies.Interfaces;

namespace Saga.Dungeon.Enemies.Behaviour
{
    [EnemyTag("Undead", typeof(UndeadWrapper))]
    public class UndeadWrapper(EnemyBase inner) : EnemyWrapperBase(inner), IUndead 
    {
        //add behaviour here
    }
}
