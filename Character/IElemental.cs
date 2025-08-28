
namespace Saga.Character
{
    public enum ElementalType
    {
        Frost,
        Fire,
        Poison,
        Lightning
    }
    public interface IElemental : IDamageType
    {
        DamageType IDamageType.DmgType => DamageType.Elemental;
        ElementalType ElementalType { get; }
    }
}
