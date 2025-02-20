namespace DrsBasicDI;

/// <summary>
/// The <see cref="IDependency" /> interface defines the properties and methods for a dependency
/// object.
/// </summary>
public interface IDependency
{
    /// <summary>
    /// Gets the dependency type of this <see cref="IDependency" /> object.
    /// </summary>
    Type? DependencyType
    {
        get;
        init;
    }

    /// <summary>
    /// Gets the optional factory method used for creating instances of the resolving object for
    /// this <see cref="IDependency" /> object.
    /// </summary>
    Func<object>? Factory
    {
        get;
        init;
    }

    /// <summary>
    /// Gets the <see cref="DependencyLifetime" /> enumeration value representing the dependency's
    /// lifetime.
    /// </summary>
    DependencyLifetime Lifetime
    {
        get;
        init;
    }

    /// <summary>
    /// Gets the resolving type that is mapped to the <see cref="DependencyType" /> property.
    /// </summary>
    Type? ResolvingType
    {
        get;
        init;
    }
}