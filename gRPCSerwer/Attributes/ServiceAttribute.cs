namespace Serwer.Attributes
{
    [AttributeUsage(AttributeTargets.Class, Inherited = false)]
    public class ServiceAttribute(string name) : Attribute, INamed
    {
        public string Name { get; } = name;
    }
}
