namespace DrsBasicDI;

public class Dependency
{
    internal Dependency()
    {
    }

    public required Type DependencyType
    {
        get;
        init;
    }

    public Func<object>? Factory
    {
        get;
        init;
    }

    public required DependencyLifetime Lifetime
    {
        get;
        init;
    }

    public required Type ResolvingType
    {
        get;
        init;
    }
}