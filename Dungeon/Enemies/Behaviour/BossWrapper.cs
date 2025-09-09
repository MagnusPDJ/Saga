using Saga.Dungeon.Enemies.Interfaces;

namespace Saga.Dungeon.Enemies.Behaviour
{
    [EnemyTag("Boss", typeof(BossWrapper))]
    public class BossWrapper(EnemyBase inner) : EnemyWrapperBase(inner), IBoss 
    {
        //add behaviour here
    }
}
