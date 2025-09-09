
namespace Saga.Dungeon.Enemies
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
    public class EnemyTagAttribute(string tag, Type wrapperType) : Attribute
    {
        public string Tag { get; } = tag;
        public Type WrapperType { get; } = wrapperType;
    }
}
