
namespace Saga.Assets
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
    public class DiscriminatorAttribute(string name) : Attribute
    {
        public string Name { get; } = name;
    }
}
