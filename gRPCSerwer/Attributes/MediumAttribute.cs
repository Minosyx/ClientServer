namespace Serwer.Attributes
{
    [AttributeUsage(AttributeTargets.Class, Inherited = false)]
    public class MediumAttribute(string name) : Attribute, INamed
    {
        public string Name { get; } = name;
    }
}
