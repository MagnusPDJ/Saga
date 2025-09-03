
namespace Saga.Character.Skills
{
    public interface IActiveSkill : ISkill
    {
        int ActionPointCost { get; set; }
    }
}
