namespace Saga.Character.DmgLogic
{
    public static class DamageHelper
    {
        public static string Describe(IDamageType damage) {
            return damage switch {
                IPhysical physical => $"Physical damage of type {physical.PhysicalType}.",
                IElemental elemental => $"Elemental damage of type {elemental.ElementalType}.",
                IMagical magical => $"Magical damage of type {magical.MagicalType}.",
                _ => "This has no damage type."
            };
        }
    }
}
