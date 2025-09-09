using Saga.Dungeon.Enemies.Interfaces;

namespace Saga.Dungeon.Enemies.Behaviour
{
    [EnemyTag("Human", typeof(HumanWrapper))]
    public class HumanWrapper(EnemyBase inner) : EnemyWrapperBase(inner), IHuman 
    {
        //add behaviour here
    }
}
