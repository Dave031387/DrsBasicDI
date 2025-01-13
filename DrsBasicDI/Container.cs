namespace DrsBasicDI;

public class Container
{
    internal readonly Dictionary<Type, Dependency> _dependencies = [];

    internal Container()
    {
    }
}