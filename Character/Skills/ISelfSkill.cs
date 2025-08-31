
namespace Saga.Character.Skills
{
    public interface ISelfSkill : IActiveSkill
    {
        void Activate(Player player);
    }
}
