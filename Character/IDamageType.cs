
namespace Saga.Character
{
    public interface IDamageType
    {
        public enum DamageType {
            Physical,
            Magical,
            Elemental,
        }
        DamageType DmgType { get; }
    }
}
