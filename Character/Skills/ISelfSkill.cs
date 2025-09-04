
namespace Saga.Character.Skills
{
    public interface ISelfSkill : IActiveSkill
    {
        bool Activate(Player player);
    }
}
