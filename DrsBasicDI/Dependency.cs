namespace DrsBasicDI;

/// <summary>
/// The <see cref="Dependency" /> class represents a single dependency in an application for which
/// we want to use dependency injection.
/// </summary>
/// <remarks>
/// This class is immutable.
/// </remarks>
public class Dependency
{
    /// <summary>
    /// Default constructor for the <see cref="Dependency" /> class.
    /// </summary>
    /// <remarks>
    /// This constructor is marked <see langword="internal" /> to force the user to use the
    /// <see cref="DependencyBuilder" /> object for constructing new <see cref="Dependency" />
    /// objects.
    /// </remarks>
    internal Dependency()
    {
    }

    /// <summary>
    /// Gets the <see langword="Type" /> of this <see cref="Dependency" /> instance.
    /// </summary>
    public required Type DependencyType
    {
        get;
        init;
    }

    /// <summary>
    /// Gets the factory method used for creating instances of the resolving type.
    /// </summary>
    public Func<object>? Factory
    {
        get;
        init;
    }

    /// <summary>
    /// Gets the enumeration representing the dependency lifetime.
    /// </summary>
    public DependencyLifetime Lifetime
    {
        get;
        init;
    }

    /// <summary>
    /// Gets the resolving type that is mapped to the dependency type.
    /// </summary>
    public required Type ResolvingType
    {
        get;
        init;
    }
}