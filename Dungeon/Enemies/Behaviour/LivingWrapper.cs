using Saga.Dungeon.Enemies.Interfaces;

namespace Saga.Dungeon.Enemies.Behaviour
{
    [EnemyTag("Living", typeof(LivingWrapper))]
    public class LivingWrapper(EnemyBase inner) : EnemyWrapperBase(inner), ILiving 
    {
        //add behaviour here
    }
}
