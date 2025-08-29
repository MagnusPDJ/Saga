namespace Saga.Character.DmgLogic
{
    public enum PhysicalType
    {
        Normal,
        Piercing,
        Crushing
    }
    public interface IPhysical : IDamageType
    {
        DamageType IDamageType.DmgType => DamageType.Physical;
        PhysicalType PhysicalType { get; }
    }
}
