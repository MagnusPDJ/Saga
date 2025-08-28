
using System.Configuration;

namespace Saga.Character
{
    public enum MagicalType
    {
        Arcane,
        Chaos,
        Void,
        Nature,
        Life,
        Death
    }
    public interface IMagical : IDamageType
    {
        DamageType IDamageType.DmgType => DamageType.Magical;
        MagicalType MagicalType { get; }
    }
}
