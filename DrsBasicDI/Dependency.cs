namespace DrsBasicDI;

/// <summary>
/// The <see cref="Dependency" /> record represents a single dependency in an application for which
/// we want to use dependency injection.
/// </summary>
/// <param name="DependencyType">
/// Gets the <see langword="Type" /> of this <see cref="Dependency" /> instance.
/// </param>
/// <param name="Factory">
/// Gets the factory method used for creating instances of the resolving type.
/// </param>
/// <param name="Lifetime">
/// Gets the <see cref="DependencyLifetime" /> enumeration value representing the dependency
/// lifetime.
/// </param>
/// <param name="ResolvingType">
/// Gets the resolving type that is mapped to the dependency type.
/// </param>
/// <remarks>
/// This class is immutable.
/// </remarks>
public sealed record Dependency(Type? DependencyType,
                                Func<object>? Factory,
                                DependencyLifetime Lifetime,
                                Type? ResolvingType)
{
    /// <summary>
    /// Constructor for the <see cref="Dependency" /> record.
    /// </summary>
    /// <remarks>
    /// This constructor is declared <see langword="internal" /> to force the user to use the
    /// <see cref="DependencyBuilder" /> object for constructing new <see cref="Dependency" />
    /// objects.
    /// </remarks>
    internal Dependency() : this(default, null, default, default)
    {
    }
}